using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Runnable
{
    public static class RunnableWithTimeoutExtensions
    {

        // ==================== Timeout ====================

        /// <summary>
        /// Add timeout to async execution
        /// </summary>
        public static Runnable<TInput, TOutput> WithTimeout<TInput, TOutput>(
            this IRunnable<TInput, TOutput> runnable,
            TimeSpan timeout)
        {
            return new Runnable<TInput, TOutput>(
                input => runnable.Invoke(input),
                async input =>
                {
                    var task = runnable.InvokeAsync(input);
                    var completedTask = await Task.WhenAny(task, Task.Delay(timeout));

                    if (completedTask == task)
                    {
                        return await task;
                    }
                    throw new TimeoutException($"Operation timed out after {timeout.TotalMilliseconds}ms");
                }
            );
        }
    }
}
