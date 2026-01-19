using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Runnable
{
    /// <summary>
    /// Cache entry with expiration support
    /// </summary>
    internal class CacheEntry<TValue>
    {
        public TValue Value { get; set; }
        public DateTime ExpiresAt { get; set; }
        public DateTime LastAccessed { get; set; }

        public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
    }

    /// <summary>
    /// Cache eviction policies
    /// </summary>
    public enum CacheEvictionPolicy
    {
        /// <summary>
        /// Least Recently Used - evict entries that haven't been accessed recently
        /// </summary>
        LRU,

        /// <summary>
        /// First In First Out - evict oldest entries
        /// </summary>
        FIFO,

        /// <summary>
        /// Time To Live - evict based on expiration time only
        /// </summary>
        TTL
    }

    /// <summary>
    /// Configuration for cache behavior
    /// </summary>
    public class CacheOptions
    {
        /// <summary>
        /// Time-to-live for cache entries (null = never expire)
        /// </summary>
        public TimeSpan? ExpirationTime { get; set; }

        /// <summary>
        /// Maximum number of entries in cache (null = unlimited)
        /// </summary>
        public int? MaxSize { get; set; }

        /// <summary>
        /// Cache eviction policy when max size is reached
        /// </summary>
        public CacheEvictionPolicy EvictionPolicy { get; set; } = CacheEvictionPolicy.LRU;

        /// <summary>
        /// Sliding expiration - reset expiration time on access
        /// </summary>
        public bool SlidingExpiration { get; set; } = false;
    }

    /// <summary>
    /// Extensions for WithCache with expiration and size limits
    /// </summary>
    public static class RunnableWithCacheAdvancedExtensions
    {
        // ==================== Advanced Caching with Expiration ====================

        /// <summary>
        /// Cache the results with expiration and size limits (1 input version)
        /// </summary>
        public static Runnable<TInput, TOutput> WithCache<TInput, TOutput>(
            this IRunnable<TInput, TOutput> runnable,
            CacheOptions options)
        {
            var cache = new Dictionary<TInput, CacheEntry<TOutput>>();
            var insertionOrder = new Queue<TInput>();
            var lockObj = new object();

            TOutput GetOrCompute(TInput input, Func<TInput, TOutput> compute)
            {
                lock (lockObj)
                {
                    // Check if exists and not expired
                    if (cache.TryGetValue(input, out var entry))
                    {
                        if (!entry.IsExpired)
                        {
                            entry.LastAccessed = DateTime.UtcNow;
                            if (options.SlidingExpiration && options.ExpirationTime.HasValue)
                            {
                                entry.ExpiresAt = DateTime.UtcNow.Add(options.ExpirationTime.Value);
                            }
                            return entry.Value;
                        }
                        else
                        {
                            // Remove expired entry
                            cache.Remove(input);
                        }
                    }

                    // Compute new value
                    var result = compute(input);

                    // Evict if necessary
                    if (options.MaxSize.HasValue && cache.Count >= options.MaxSize.Value)
                    {
                        EvictEntry(cache, insertionOrder, options.EvictionPolicy);
                    }

                    // Add to cache
                    var newEntry = new CacheEntry<TOutput>
                    {
                        Value = result,
                        ExpiresAt = options.ExpirationTime.HasValue
                            ? DateTime.UtcNow.Add(options.ExpirationTime.Value)
                            : DateTime.MaxValue,
                        LastAccessed = DateTime.UtcNow
                    };

                    cache[input] = newEntry;
                    insertionOrder.Enqueue(input);

                    return result;
                }
            }

            async Task<TOutput> GetOrComputeAsync(TInput input, Func<TInput, Task<TOutput>> computeAsync)
            {
                // Check cache first
                lock (lockObj)
                {
                    if (cache.TryGetValue(input, out var entry) && !entry.IsExpired)
                    {
                        entry.LastAccessed = DateTime.UtcNow;
                        if (options.SlidingExpiration && options.ExpirationTime.HasValue)
                        {
                            entry.ExpiresAt = DateTime.UtcNow.Add(options.ExpirationTime.Value);
                        }
                        return entry.Value;
                    }
                }

                // Compute outside lock
                var result = await computeAsync(input);

                // Update cache
                lock (lockObj)
                {
                    // Evict if necessary
                    if (options.MaxSize.HasValue && cache.Count >= options.MaxSize.Value)
                    {
                        EvictEntry(cache, insertionOrder, options.EvictionPolicy);
                    }

                    var newEntry = new CacheEntry<TOutput>
                    {
                        Value = result,
                        ExpiresAt = options.ExpirationTime.HasValue
                            ? DateTime.UtcNow.Add(options.ExpirationTime.Value)
                            : DateTime.MaxValue,
                        LastAccessed = DateTime.UtcNow
                    };

                    cache[input] = newEntry;
                    insertionOrder.Enqueue(input);
                }

                return result;
            }

            return new Runnable<TInput, TOutput>(
                input => GetOrCompute(input, runnable.Invoke),
                input => GetOrComputeAsync(input, runnable.InvokeAsync)
            );
        }

        private static void EvictEntry<TInput, TOutput>(
            Dictionary<TInput, CacheEntry<TOutput>> cache,
            Queue<TInput> insertionOrder,
            CacheEvictionPolicy policy)
        {
            switch (policy)
            {
                case CacheEvictionPolicy.LRU:
                    // Find least recently used
                    var lruKey = cache.OrderBy(kvp => kvp.Value.LastAccessed).First().Key;
                    cache.Remove(lruKey);
                    break;

                case CacheEvictionPolicy.FIFO:
                    // Remove oldest (first in)
                    if (insertionOrder.Count > 0)
                    {
                        var oldestKey = insertionOrder.Dequeue();
                        cache.Remove(oldestKey);
                    }
                    break;

                case CacheEvictionPolicy.TTL:
                    // Remove first expired entry
                    var expiredKey = cache.FirstOrDefault(kvp => kvp.Value.IsExpired).Key;
                    if (expiredKey != null)
                    {
                        cache.Remove(expiredKey);
                    }
                    else
                    {
                        // No expired entries, fall back to FIFO
                        if (insertionOrder.Count > 0)
                        {
                            var oldestKey = insertionOrder.Dequeue();
                            cache.Remove(oldestKey);
                        }
                    }
                    break;
            }
        }

        // ==================== Convenience Methods ====================

        /// <summary>
        /// Cache with Time-To-Live expiration (1 input version)
        /// </summary>
        public static Runnable<TInput, TOutput> WithCacheTTL<TInput, TOutput>(
            this IRunnable<TInput, TOutput> runnable,
            TimeSpan ttl)
        {
            return runnable.WithCache(new CacheOptions
            {
                ExpirationTime = ttl,
                EvictionPolicy = CacheEvictionPolicy.TTL
            });
        }

        /// <summary>
        /// Cache with LRU eviction and size limit (1 input version)
        /// </summary>
        public static Runnable<TInput, TOutput> WithCacheLRU<TInput, TOutput>(
            this IRunnable<TInput, TOutput> runnable,
            int maxSize)
        {
            return runnable.WithCache(new CacheOptions
            {
                MaxSize = maxSize,
                EvictionPolicy = CacheEvictionPolicy.LRU
            });
        }

        /// <summary>
        /// Cache with TTL and size limit (1 input version)
        /// </summary>
        public static Runnable<TInput, TOutput> WithCacheTTLAndSize<TInput, TOutput>(
            this IRunnable<TInput, TOutput> runnable,
            TimeSpan ttl,
            int maxSize)
        {
            return runnable.WithCache(new CacheOptions
            {
                ExpirationTime = ttl,
                MaxSize = maxSize,
                EvictionPolicy = CacheEvictionPolicy.LRU
            });
        }
    }
}
