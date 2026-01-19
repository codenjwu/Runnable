using System;
using System.Threading.Tasks;

namespace Runnable
{
    /// <summary>
    /// Retry strategies for exponential backoff and custom delay patterns
    /// </summary>
    public static class RetryStrategies
    {
        /// <summary>
        /// No delay between retries
        /// </summary>
        public static Func<int, TimeSpan> NoDelay = attempt => TimeSpan.Zero;

        /// <summary>
        /// Fixed delay between retries
        /// </summary>
        public static Func<int, TimeSpan> FixedDelay(TimeSpan delay) =>
            attempt => delay;

        /// <summary>
        /// Linear backoff: delay increases linearly (delay * attempt)
        /// </summary>
        public static Func<int, TimeSpan> LinearBackoff(TimeSpan baseDelay) =>
            attempt => TimeSpan.FromMilliseconds(baseDelay.TotalMilliseconds * attempt);

        /// <summary>
        /// Exponential backoff: delay doubles each attempt (2^attempt * baseDelay)
        /// </summary>
        public static Func<int, TimeSpan> ExponentialBackoff(TimeSpan baseDelay, TimeSpan? maxDelay = null) =>
            attempt =>
            {
                var delay = TimeSpan.FromMilliseconds(baseDelay.TotalMilliseconds * Math.Pow(2, attempt - 1));
                return maxDelay.HasValue && delay > maxDelay.Value ? maxDelay.Value : delay;
            };

        /// <summary>
        /// Exponential backoff with jitter to prevent thundering herd
        /// </summary>
        public static Func<int, TimeSpan> ExponentialBackoffWithJitter(TimeSpan baseDelay, TimeSpan? maxDelay = null)
        {
            var random = new Random();
            return attempt =>
            {
                var exponentialDelay = baseDelay.TotalMilliseconds * Math.Pow(2, attempt - 1);
                var jitter = random.NextDouble() * 0.3 * exponentialDelay;  // ¡À30% jitter
                var totalDelay = TimeSpan.FromMilliseconds(exponentialDelay + jitter);
                return maxDelay.HasValue && totalDelay > maxDelay.Value ? maxDelay.Value : totalDelay;
            };
        }

        /// <summary>
        /// Fibonacci backoff: delay follows Fibonacci sequence
        /// </summary>
        public static Func<int, TimeSpan> FibonacciBackoff(TimeSpan baseDelay)
        {
            return attempt =>
            {
                int Fibonacci(int n) => n <= 1 ? n : Fibonacci(n - 1) + Fibonacci(n - 2);
                return TimeSpan.FromMilliseconds(baseDelay.TotalMilliseconds * Fibonacci(attempt));
            };
        }
    }

    /// <summary>
    /// Extensions for retry with configurable delay strategies
    /// </summary>
    public static class RunnableWithRetryAndDelayExtensions
    {
        // ==================== WithRetryAndDelay (Retry with Backoff) ====================

        /// <summary>
        /// Retry with configurable delay strategy (no input version)
        /// </summary>
        /// <param name="runnable">The runnable to retry</param>
        /// <param name="maxAttempts">Maximum number of attempts (must be > 0)</param>
        /// <param name="delayStrategy">Function that returns delay based on attempt number (1-indexed)</param>
        public static Runnable<TOutput> WithRetryAndDelay<TOutput>(
            this IRunnable<TOutput> runnable,
            int maxAttempts,
            Func<int, TimeSpan> delayStrategy)
        {
            if (maxAttempts <= 0)
                throw new ArgumentException("maxAttempts must be greater than 0", nameof(maxAttempts));

            return new Runnable<TOutput>(
                () =>
                {
                    Exception lastException = null;
                    for (int attempt = 1; attempt <= maxAttempts; attempt++)
                    {
                        try
                        {
                            return runnable.Invoke();
                        }
                        catch (Exception ex)
                        {
                            lastException = ex;
                            if (attempt < maxAttempts)
                            {
                                var delay = delayStrategy(attempt);
                                if (delay > TimeSpan.Zero)
                                {
                                    System.Threading.Thread.Sleep(delay);
                                }
                            }
                        }
                    }
                    throw lastException;
                },
                async () =>
                {
                    Exception lastException = null;
                    for (int attempt = 1; attempt <= maxAttempts; attempt++)
                    {
                        try
                        {
                            return await runnable.InvokeAsync();
                        }
                        catch (Exception ex)
                        {
                            lastException = ex;
                            if (attempt < maxAttempts)
                            {
                                var delay = delayStrategy(attempt);
                                if (delay > TimeSpan.Zero)
                                {
                                    await Task.Delay(delay);
                                }
                            }
                        }
                    }
                    throw lastException;
                }
            );
        }

        /// <summary>
        /// Retry with configurable delay strategy (1 input version)
        /// </summary>
        public static Runnable<TInput, TOutput> WithRetryAndDelay<TInput, TOutput>(
            this IRunnable<TInput, TOutput> runnable,
            int maxAttempts,
            Func<int, TimeSpan> delayStrategy)
        {
            if (maxAttempts <= 0)
                throw new ArgumentException("maxAttempts must be greater than 0", nameof(maxAttempts));

            return new Runnable<TInput, TOutput>(
                input =>
                {
                    Exception lastException = null;
                    for (int attempt = 1; attempt <= maxAttempts; attempt++)
                    {
                        try
                        {
                            return runnable.Invoke(input);
                        }
                        catch (Exception ex)
                        {
                            lastException = ex;
                            if (attempt < maxAttempts)
                            {
                                var delay = delayStrategy(attempt);
                                if (delay > TimeSpan.Zero)
                                {
                                    System.Threading.Thread.Sleep(delay);
                                }
                            }
                        }
                    }
                    throw lastException;
                },
                async input =>
                {
                    Exception lastException = null;
                    for (int attempt = 1; attempt <= maxAttempts; attempt++)
                    {
                        try
                        {
                            return await runnable.InvokeAsync(input);
                        }
                        catch (Exception ex)
                        {
                            lastException = ex;
                            if (attempt < maxAttempts)
                            {
                                var delay = delayStrategy(attempt);
                                if (delay > TimeSpan.Zero)
                                {
                                    await Task.Delay(delay);
                                }
                            }
                        }
                    }
                    throw lastException;
                }
            );
        }

        // ==================== Convenience Methods with Common Strategies ====================

        /// <summary>
        /// Retry with exponential backoff (no input version)
        /// </summary>
        public static Runnable<TOutput> WithExponentialBackoff<TOutput>(
            this IRunnable<TOutput> runnable,
            int maxAttempts,
            TimeSpan baseDelay,
            TimeSpan? maxDelay = null)
        {
            return runnable.WithRetryAndDelay(maxAttempts, RetryStrategies.ExponentialBackoff(baseDelay, maxDelay));
        }

        /// <summary>
        /// Retry with exponential backoff (1 input version)
        /// </summary>
        public static Runnable<TInput, TOutput> WithExponentialBackoff<TInput, TOutput>(
            this IRunnable<TInput, TOutput> runnable,
            int maxAttempts,
            TimeSpan baseDelay,
            TimeSpan? maxDelay = null)
        {
            return runnable.WithRetryAndDelay(maxAttempts, RetryStrategies.ExponentialBackoff(baseDelay, maxDelay));
        }

        /// <summary>
        /// Retry with exponential backoff and jitter (no input version)
        /// </summary>
        public static Runnable<TOutput> WithExponentialBackoffAndJitter<TOutput>(
            this IRunnable<TOutput> runnable,
            int maxAttempts,
            TimeSpan baseDelay,
            TimeSpan? maxDelay = null)
        {
            return runnable.WithRetryAndDelay(maxAttempts, RetryStrategies.ExponentialBackoffWithJitter(baseDelay, maxDelay));
        }

        /// <summary>
        /// Retry with exponential backoff and jitter (1 input version)
        /// </summary>
        public static Runnable<TInput, TOutput> WithExponentialBackoffAndJitter<TInput, TOutput>(
            this IRunnable<TInput, TOutput> runnable,
            int maxAttempts,
            TimeSpan baseDelay,
            TimeSpan? maxDelay = null)
        {
            return runnable.WithRetryAndDelay(maxAttempts, RetryStrategies.ExponentialBackoffWithJitter(baseDelay, maxDelay));
        }
    }
}
