using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Runnable
{
    /// <summary>
    /// Extensions for context-aware caching - enables per-tenant, per-user, or custom cache isolation
    /// </summary>
    public static class RunnableCacheContextExtensions
    {
        // ==================== WithCacheContext (Context-aware caching) ====================

        /// <summary>
        /// Cache with context-based key generation for multi-tenant scenarios (1 input version)
        /// Each context (tenant, user, etc.) gets its own cache space
        /// </summary>
        public static Runnable<TInput, TOutput> WithCacheContext<TInput, TOutput>(
            this IRunnable<TInput, TOutput> runnable,
            Func<RunnableContext, string> contextKeySelector)
        {
            var cache = new Dictionary<string, TOutput>();
            var lockObj = new object();

            return new Runnable<TInput, TOutput>(
                input =>
                {
                    var contextKey = contextKeySelector(RunnableContext.Current);
                    var cacheKey = $"{contextKey}:{input?.GetHashCode() ?? 0}";
                    
                    lock (lockObj)
                    {
                        if (cache.TryGetValue(cacheKey, out var cached))
                            return cached;
                        
                        var result = runnable.Invoke(input);
                        cache[cacheKey] = result;
                        return result;
                    }
                },
                async input =>
                {
                    var contextKey = contextKeySelector(RunnableContext.Current);
                    var cacheKey = $"{contextKey}:{input?.GetHashCode() ?? 0}";
                    
                    lock (lockObj)
                    {
                        if (cache.TryGetValue(cacheKey, out var cached))
                            return cached;
                    }
                    
                    var result = await runnable.InvokeAsync(input);
                    lock (lockObj)
                    {
                        cache[cacheKey] = result;
                    }
                    return result;
                }
            );
        }

        /// <summary>
        /// Cache with context-based key generation and custom input key selector (1 input version)
        /// </summary>
        public static Runnable<TInput, TOutput> WithCacheContext<TInput, TOutput>(
            this IRunnable<TInput, TOutput> runnable,
            Func<RunnableContext, string> contextKeySelector,
            Func<TInput, string> inputKeySelector)
        {
            var cache = new Dictionary<string, TOutput>();
            var lockObj = new object();

            return new Runnable<TInput, TOutput>(
                input =>
                {
                    var contextKey = contextKeySelector(RunnableContext.Current);
                    var inputKey = inputKeySelector(input);
                    var cacheKey = $"{contextKey}:{inputKey}";
                    
                    lock (lockObj)
                    {
                        if (cache.TryGetValue(cacheKey, out var cached))
                            return cached;
                        
                        var result = runnable.Invoke(input);
                        cache[cacheKey] = result;
                        return result;
                    }
                },
                async input =>
                {
                    var contextKey = contextKeySelector(RunnableContext.Current);
                    var inputKey = inputKeySelector(input);
                    var cacheKey = $"{contextKey}:{inputKey}";
                    
                    lock (lockObj)
                    {
                        if (cache.TryGetValue(cacheKey, out var cached))
                            return cached;
                    }
                    
                    var result = await runnable.InvokeAsync(input);
                    lock (lockObj)
                    {
                        cache[cacheKey] = result;
                    }
                    return result;
                }
            );
        }

        // ==================== Common caching patterns ====================

        /// <summary>
        /// Cache per tenant - each tenant gets isolated cache
        /// </summary>
        public static Runnable<TInput, TOutput> WithCachePerTenant<TInput, TOutput>(
            this IRunnable<TInput, TOutput> runnable)
        {
            return runnable.WithCacheContext(ctx => $"tenant:{ctx.TenantId ?? "default"}");
        }

        /// <summary>
        /// Cache per user - each user gets isolated cache
        /// </summary>
        public static Runnable<TInput, TOutput> WithCachePerUser<TInput, TOutput>(
            this IRunnable<TInput, TOutput> runnable)
        {
            return runnable.WithCacheContext(ctx => $"user:{ctx.UserId ?? "anonymous"}");
        }

        /// <summary>
        /// Cache per tenant and user combination
        /// </summary>
        public static Runnable<TInput, TOutput> WithCachePerTenantAndUser<TInput, TOutput>(
            this IRunnable<TInput, TOutput> runnable)
        {
            return runnable.WithCacheContext(ctx => 
                $"tenant:{ctx.TenantId ?? "default"}:user:{ctx.UserId ?? "anonymous"}");
        }

        /// <summary>
        /// Cache per correlation ID - useful for request-scoped caching
        /// </summary>
        public static Runnable<TInput, TOutput> WithCachePerCorrelation<TInput, TOutput>(
            this IRunnable<TInput, TOutput> runnable)
        {
            return runnable.WithCacheContext(ctx => $"correlation:{ctx.CorrelationId}");
        }

        /// <summary>
        /// Cache based on custom context key
        /// </summary>
        public static Runnable<TInput, TOutput> WithCacheByContextKey<TInput, TOutput>(
            this IRunnable<TInput, TOutput> runnable,
            string contextKey)
        {
            return runnable.WithCacheContext(ctx => 
                $"{contextKey}:{ctx.GetValue<string>(contextKey) ?? "default"}");
        }

        // ==================== Context-aware cache with TTL ====================

        private class CacheEntry<TValue>
        {
            public TValue Value { get; set; }
            public DateTime ExpiresAt { get; set; }
        }

        /// <summary>
        /// Cache with context-based isolation and TTL (1 input version)
        /// </summary>
        public static Runnable<TInput, TOutput> WithCacheContextTTL<TInput, TOutput>(
            this IRunnable<TInput, TOutput> runnable,
            Func<RunnableContext, string> contextKeySelector,
            TimeSpan ttl)
        {
            var cache = new Dictionary<string, CacheEntry<TOutput>>();
            var lockObj = new object();

            return new Runnable<TInput, TOutput>(
                input =>
                {
                    var contextKey = contextKeySelector(RunnableContext.Current);
                    var cacheKey = $"{contextKey}:{input?.GetHashCode() ?? 0}";
                    
                    lock (lockObj)
                    {
                        if (cache.TryGetValue(cacheKey, out var entry))
                        {
                            if (DateTime.UtcNow < entry.ExpiresAt)
                                return entry.Value;
                            
                            cache.Remove(cacheKey);
                        }
                        
                        var result = runnable.Invoke(input);
                        cache[cacheKey] = new CacheEntry<TOutput>
                        {
                            Value = result,
                            ExpiresAt = DateTime.UtcNow.Add(ttl)
                        };
                        return result;
                    }
                },
                async input =>
                {
                    var contextKey = contextKeySelector(RunnableContext.Current);
                    var cacheKey = $"{contextKey}:{input?.GetHashCode() ?? 0}";
                    
                    lock (lockObj)
                    {
                        if (cache.TryGetValue(cacheKey, out var entry))
                        {
                            if (DateTime.UtcNow < entry.ExpiresAt)
                                return entry.Value;
                            
                            cache.Remove(cacheKey);
                        }
                    }
                    
                    var result = await runnable.InvokeAsync(input);
                    lock (lockObj)
                    {
                        cache[cacheKey] = new CacheEntry<TOutput>
                        {
                            Value = result,
                            ExpiresAt = DateTime.UtcNow.Add(ttl)
                        };
                    }
                    return result;
                }
            );
        }

        /// <summary>
        /// Cache per tenant with TTL
        /// </summary>
        public static Runnable<TInput, TOutput> WithCachePerTenantTTL<TInput, TOutput>(
            this IRunnable<TInput, TOutput> runnable,
            TimeSpan ttl)
        {
            return runnable.WithCacheContextTTL(
                ctx => $"tenant:{ctx.TenantId ?? "default"}", 
                ttl);
        }

        /// <summary>
        /// Cache per user with TTL
        /// </summary>
        public static Runnable<TInput, TOutput> WithCachePerUserTTL<TInput, TOutput>(
            this IRunnable<TInput, TOutput> runnable,
            TimeSpan ttl)
        {
            return runnable.WithCacheContextTTL(
                ctx => $"user:{ctx.UserId ?? "anonymous"}", 
                ttl);
        }

        // ==================== Context-aware cache with LRU ====================

        private class LRUCache<TKey, TValue>
        {
            private readonly int _maxSize;
            private readonly Dictionary<TKey, LinkedListNode<(TKey key, TValue value)>> _cache;
            private readonly LinkedList<(TKey key, TValue value)> _lruList;
            private readonly object _lock = new object();

            public LRUCache(int maxSize)
            {
                _maxSize = maxSize;
                _cache = new Dictionary<TKey, LinkedListNode<(TKey, TValue)>>();
                _lruList = new LinkedList<(TKey, TValue)>();
            }

            public bool TryGetValue(TKey key, out TValue value)
            {
                lock (_lock)
                {
                    if (_cache.TryGetValue(key, out var node))
                    {
                        _lruList.Remove(node);
                        _lruList.AddFirst(node);
                        value = node.Value.value;
                        return true;
                    }
                    value = default;
                    return false;
                }
            }

            public void Add(TKey key, TValue value)
            {
                lock (_lock)
                {
                    if (_cache.ContainsKey(key))
                    {
                        _lruList.Remove(_cache[key]);
                        _cache.Remove(key);
                    }

                    if (_cache.Count >= _maxSize)
                    {
                        var lru = _lruList.Last;
                        _lruList.RemoveLast();
                        _cache.Remove(lru.Value.key);
                    }

                    var node = _lruList.AddFirst((key, value));
                    _cache[key] = node;
                }
            }
        }

        /// <summary>
        /// Cache with context-based isolation and LRU eviction (1 input version)
        /// </summary>
        public static Runnable<TInput, TOutput> WithCacheContextLRU<TInput, TOutput>(
            this IRunnable<TInput, TOutput> runnable,
            Func<RunnableContext, string> contextKeySelector,
            int maxSize)
        {
            var cache = new LRUCache<string, TOutput>(maxSize);

            return new Runnable<TInput, TOutput>(
                input =>
                {
                    var contextKey = contextKeySelector(RunnableContext.Current);
                    var cacheKey = $"{contextKey}:{input?.GetHashCode() ?? 0}";
                    
                    if (cache.TryGetValue(cacheKey, out var cached))
                        return cached;
                    
                    var result = runnable.Invoke(input);
                    cache.Add(cacheKey, result);
                    return result;
                },
                async input =>
                {
                    var contextKey = contextKeySelector(RunnableContext.Current);
                    var cacheKey = $"{contextKey}:{input?.GetHashCode() ?? 0}";
                    
                    if (cache.TryGetValue(cacheKey, out var cached))
                        return cached;
                    
                    var result = await runnable.InvokeAsync(input);
                    cache.Add(cacheKey, result);
                    return result;
                }
            );
        }

        /// <summary>
        /// Cache per tenant with LRU eviction
        /// </summary>
        public static Runnable<TInput, TOutput> WithCachePerTenantLRU<TInput, TOutput>(
            this IRunnable<TInput, TOutput> runnable,
            int maxSize)
        {
            return runnable.WithCacheContextLRU(
                ctx => $"tenant:{ctx.TenantId ?? "default"}", 
                maxSize);
        }

        /// <summary>
        /// Cache per user with LRU eviction
        /// </summary>
        public static Runnable<TInput, TOutput> WithCachePerUserLRU<TInput, TOutput>(
            this IRunnable<TInput, TOutput> runnable,
            int maxSize)
        {
            return runnable.WithCacheContextLRU(
                ctx => $"user:{ctx.UserId ?? "anonymous"}", 
                maxSize);
        }

        // ==================== Context-aware cache with TTL AND LRU ====================

        private class TTLLRUCache<TKey, TValue>
        {
            private readonly int _maxSize;
            private readonly TimeSpan _ttl;
            private readonly Dictionary<TKey, LinkedListNode<CacheEntry<TKey, TValue>>> _cache;
            private readonly LinkedList<CacheEntry<TKey, TValue>> _lruList;
            private readonly object _lock = new object();

            private class CacheEntry<TK, TV>
            {
                public TK Key { get; set; }
                public TV Value { get; set; }
                public DateTime ExpiresAt { get; set; }
            }

            public TTLLRUCache(int maxSize, TimeSpan ttl)
            {
                _maxSize = maxSize;
                _ttl = ttl;
                _cache = new Dictionary<TKey, LinkedListNode<CacheEntry<TKey, TValue>>>();
                _lruList = new LinkedList<CacheEntry<TKey, TValue>>();
            }

            public bool TryGetValue(TKey key, out TValue value)
            {
                lock (_lock)
                {
                    if (_cache.TryGetValue(key, out var node))
                    {
                        if (DateTime.UtcNow < node.Value.ExpiresAt)
                        {
                            _lruList.Remove(node);
                            _lruList.AddFirst(node);
                            value = node.Value.Value;
                            return true;
                        }
                        
                        _lruList.Remove(node);
                        _cache.Remove(key);
                    }
                    value = default;
                    return false;
                }
            }

            public void Add(TKey key, TValue value)
            {
                lock (_lock)
                {
                    if (_cache.ContainsKey(key))
                    {
                        _lruList.Remove(_cache[key]);
                        _cache.Remove(key);
                    }

                    if (_cache.Count >= _maxSize)
                    {
                        var lru = _lruList.Last;
                        _lruList.RemoveLast();
                        _cache.Remove(lru.Value.Key);
                    }

                    var entry = new CacheEntry<TKey, TValue>
                    {
                        Key = key,
                        Value = value,
                        ExpiresAt = DateTime.UtcNow.Add(_ttl)
                    };
                    var node = _lruList.AddFirst(entry);
                    _cache[key] = node;
                }
            }
        }

        /// <summary>
        /// Cache with context-based isolation, TTL, AND LRU eviction (1 input version)
        /// Best of both worlds: entries expire AND oldest entries are evicted when cache is full
        /// </summary>
        public static Runnable<TInput, TOutput> WithCacheContextTTLAndLRU<TInput, TOutput>(
            this IRunnable<TInput, TOutput> runnable,
            Func<RunnableContext, string> contextKeySelector,
            TimeSpan ttl,
            int maxSize)
        {
            var cache = new TTLLRUCache<string, TOutput>(maxSize, ttl);

            return new Runnable<TInput, TOutput>(
                input =>
                {
                    var contextKey = contextKeySelector(RunnableContext.Current);
                    var cacheKey = $"{contextKey}:{input?.GetHashCode() ?? 0}";
                    
                    if (cache.TryGetValue(cacheKey, out var cached))
                        return cached;
                    
                    var result = runnable.Invoke(input);
                    cache.Add(cacheKey, result);
                    return result;
                },
                async input =>
                {
                    var contextKey = contextKeySelector(RunnableContext.Current);
                    var cacheKey = $"{contextKey}:{input?.GetHashCode() ?? 0}";
                    
                    if (cache.TryGetValue(cacheKey, out var cached))
                        return cached;
                    
                    var result = await runnable.InvokeAsync(input);
                    cache.Add(cacheKey, result);
                    return result;
                }
            );
        }

        /// <summary>
        /// Cache per tenant with both TTL and LRU
        /// </summary>
        public static Runnable<TInput, TOutput> WithCachePerTenantTTLAndLRU<TInput, TOutput>(
            this IRunnable<TInput, TOutput> runnable,
            TimeSpan ttl,
            int maxSize)
        {
            return runnable.WithCacheContextTTLAndLRU(
                ctx => $"tenant:{ctx.TenantId ?? "default"}", 
                ttl, 
                maxSize);
        }

        /// <summary>
        /// Cache per user with both TTL and LRU
        /// </summary>
        public static Runnable<TInput, TOutput> WithCachePerUserTTLAndLRU<TInput, TOutput>(
            this IRunnable<TInput, TOutput> runnable,
            TimeSpan ttl,
            int maxSize)
        {
            return runnable.WithCacheContextTTLAndLRU(
                ctx => $"user:{ctx.UserId ?? "anonymous"}", 
                ttl, 
                maxSize);
        }
    }
}
