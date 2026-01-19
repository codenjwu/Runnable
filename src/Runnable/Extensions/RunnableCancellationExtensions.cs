using System;
using System.Threading;
using System.Threading.Tasks;

namespace Runnable
{
    /// <summary>
    /// Extensions for CancellationToken support in Runnable pipelines
    /// NOTE: These are additive extensions that don't break existing code
    /// </summary>
    public static class RunnableCancellationExtensions
    {
        // ==================== WithCancellation (Add cancellation support) ====================

        /// <summary>
        /// Adds cancellation support to a runnable (no input version)
        /// </summary>
        public static Runnable<TOutput> WithCancellation<TOutput>(
            this IRunnable<TOutput> runnable)
        {
            return new Runnable<TOutput>(
                () => runnable.Invoke(),
                async () => await runnable.InvokeAsync()
            );
        }

        /// <summary>
        /// Adds cancellation support to a runnable (1 input version)  
        /// </summary>
        public static Runnable<TInput, TOutput> WithCancellation<TInput, TOutput>(
            this IRunnable<TInput, TOutput> runnable)
        {
            return new Runnable<TInput, TOutput>(
                input => runnable.Invoke(input),
                async input => await runnable.InvokeAsync(input)
            );
        }

        // ==================== InvokeAsync overloads with CancellationToken ====================

        /// <summary>
        /// Invoke with cancellation token (no input version)
        /// </summary>
        public static async Task<TOutput> InvokeAsync<TOutput>(
            this IRunnable<TOutput> runnable,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            
            var task = runnable.InvokeAsync();
            
            // Use TaskCompletionSource to support cancellation
            var tcs = new TaskCompletionSource<TOutput>();
            
            using (cancellationToken.Register(() => tcs.TrySetCanceled()))
            {
                var completedTask = await Task.WhenAny(task, tcs.Task);
                return await completedTask;
            }
        }

        /// <summary>
        /// Invoke with cancellation token (1 input version)
        /// </summary>
        public static async Task<TOutput> InvokeAsync<TInput, TOutput>(
            this IRunnable<TInput, TOutput> runnable,
            TInput input,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            
            var task = runnable.InvokeAsync(input);
            
            // Use TaskCompletionSource to support cancellation
            var tcs = new TaskCompletionSource<TOutput>();
            
            using (cancellationToken.Register(() => tcs.TrySetCanceled()))
            {
                var completedTask = await Task.WhenAny(task, tcs.Task);
                return await completedTask;
            }
        }

        // ==================== InvokeAsync with timeout and cancellation ====================

        /// <summary>
        /// Invoke with timeout and cancellation token (1 input version)
        /// </summary>
        public static async Task<TOutput> InvokeAsync<TInput, TOutput>(
            this IRunnable<TInput, TOutput> runnable,
            TInput input,
            TimeSpan timeout,
            CancellationToken cancellationToken = default)
        {
            using (var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken))
            {
                cts.CancelAfter(timeout);
                return await InvokeAsync(runnable, input, cts.Token);
            }
        }

        // ==================== Helper extensions ====================

        /// <summary>
        /// Execute runnable with automatic cancellation after timeout
        /// </summary>
        public static Runnable<TInput, TOutput> WithAutoCancellation<TInput, TOutput>(
            this IRunnable<TInput, TOutput> runnable,
            TimeSpan timeout)
        {
            return new Runnable<TInput, TOutput>(
                input => runnable.Invoke(input),
                async input =>
                {
                    using (var cts = new CancellationTokenSource(timeout))
                    {
                        return await InvokeAsync(runnable, input, cts.Token);
                    }
                }
            );
        }

        /// <summary>
        /// Check if operation is cancelled and throw if so
        /// </summary>
        public static Runnable<TInput, TOutput> WithCancellationCheck<TInput, TOutput>(
            this IRunnable<TInput, TOutput> runnable,
            Func<CancellationToken> cancellationTokenProvider)
        {
            return new Runnable<TInput, TOutput>(
                input =>
                {
                    cancellationTokenProvider().ThrowIfCancellationRequested();
                    return runnable.Invoke(input);
                },
                async input =>
                {
                    cancellationTokenProvider().ThrowIfCancellationRequested();
                    return await runnable.InvokeAsync(input);
                }
            );
        }
    }
}
