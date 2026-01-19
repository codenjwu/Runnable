using System;
using System.Collections.Generic;
using System.Text;

namespace Runnable
{
    public static class RunnableWithCacheExtensions
    {
        // ==================== Memoization/Caching ====================

        /// <summary>
        /// Cache the results of a runnable (1 input version)
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
