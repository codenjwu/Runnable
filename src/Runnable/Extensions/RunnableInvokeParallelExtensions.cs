using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runnable
{
    public static class RunnableInvokeParallelExtensions
    {

        // ==================== Parallel Execution ====================

        /// <summary>
        /// Run multiple runnables in parallel and combine results
        /// </summary>
        public static async Task<TOutput[]> InvokeParallel<TInput, TOutput>(
            this IEnumerable<IRunnable<TInput, TOutput>> runnables,
            TInput input)
        {
            var tasks = runnables.Select(r => r.InvokeAsync(input));
            return await Task.WhenAll(tasks);
        }
    }
}
