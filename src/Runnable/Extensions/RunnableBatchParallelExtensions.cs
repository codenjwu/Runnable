using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runnable
{
    public static class RunnableBatchParallelExtensions
    {

        /// <summary>
        /// Run a runnable against multiple inputs in parallel
        /// </summary>
        public static async Task<TOutput[]> BatchParallel<TInput, TOutput>(
            this IRunnable<TInput, TOutput> runnable,
            IEnumerable<TInput> inputs)
        {
            var tasks = inputs.Select(input => runnable.InvokeAsync(input));
            return await Task.WhenAll(tasks);
        }
    }
}
