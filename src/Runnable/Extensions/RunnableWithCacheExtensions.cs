using System;
using System.Collections.Generic;
using System.Text;

namespace Runnable
{
    public static class RunnableWithCacheExtensions
    {
        // ==================== Memoization/Caching ====================

        /// <summary>
        /// Cache the results of a runnable (1 input version).
        /// 
        /// IMPORTANT: Cache Boundaries
        /// - The cache only stores the OUTPUT of THIS runnable, not the entire pipeline
        /// - Extensions added AFTER .WithCache() will still execute (e.g., .Map(), .TapAsync(), .Pipe())
        /// - Extensions added BEFORE .WithCache() benefit from caching (e.g., .WithRetry(), expensive operations)
        /// 
        /// Example:
        ///   expensive.WithCache().Pipe(transform)  
        ///   ↑ Caches 'expensive' results, but 'transform' always executes
        /// 
        ///   expensive.Pipe(transform).WithCache()  
        ///   ↑ Caches the entire pipeline result (expensive + transform)
        /// 
        /// Thread Safety: This cache is thread-safe and uses locking for concurrent access.
        /// Memory: Cache entries never expire and grow indefinitely. Consider using WithCache with expiration for long-running services.
        /// </summary>
        public static Runnable<TInput, TOutput> WithCache<TInput, TOutput>(
            this IRunnable<TInput, TOutput> runnable)
        {
            var cache = new Dictionary<TInput, TOutput>();
            var lockObj = new object();

            return new Runnable<TInput, TOutput>(
                input =>
                {
                    lock (lockObj)
                    {
                        if (cache.TryGetValue(input, out var cached))
                        {
                            return cached;
                        }
                        var result = runnable.Invoke(input);
                        cache[input] = result;
                        return result;
                    }
                },
                async input =>
                {
                    lock (lockObj)
                    {
                        if (cache.TryGetValue(input, out var cached))
                        {
                            return cached;
                        }
                    }
                    var result = await runnable.InvokeAsync(input);
                    lock (lockObj)
                    {
                        cache[input] = result;
                    }
                    return result;
                }
            );
        }
    }
}
