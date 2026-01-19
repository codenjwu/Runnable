using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Runnable
{
    public static class RunnableWithDelayExtensions
    {

        // ==================== Throttle/RateLimit ====================

        /// <summary>
        /// Add delay between invocations
        /// </summary>
        public static Runnable<TInput, TOutput> WithDelay<TInput, TOutput>(
            this IRunnable<TInput, TOutput> runnable,
            TimeSpan delay)
        {
            return new Runnable<TInput, TOutput>(
                input =>
                {
                    var result = runnable.Invoke(input);
                    System.Threading.Thread.Sleep(delay);
                    return result;
                },
                async input =>
                {
                    var result = await runnable.InvokeAsync(input);
                    await Task.Delay(delay);
                    return result;
                }
            );
        }
    }
}
