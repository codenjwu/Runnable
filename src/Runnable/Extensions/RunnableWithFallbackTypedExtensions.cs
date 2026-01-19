using System;
using System.Threading.Tasks;

namespace Runnable
{
    /// <summary>
    /// Extensions for WithFallback with exception type filtering
    /// </summary>
    public static class RunnableWithFallbackTypedExtensions
    {
        // ==================== WithFallback with Exception Type Filtering ====================

        /// <summary>
        /// Provide a fallback runnable for specific exception types (no input version)
        /// </summary>
        public static Runnable<TOutput> WithFallback<TOutput, TException>(
            this IRunnable<TOutput> runnable,
            IRunnable<TOutput> fallback)
            where TException : Exception
        {
            return new Runnable<TOutput>(
                () =>
                {
                    try
                    {
                        return runnable.Invoke();
                    }
                    catch (TException)
                    {
                        return fallback.Invoke();
                    }
                },
                async () =>
                {
                    try
                    {
                        return await runnable.InvokeAsync();
                    }
                    catch (TException)
                    {
                        return await fallback.InvokeAsync();
                    }
                }
            );
        }

        /// <summary>
        /// Provide a fallback runnable for specific exception types (1 input version)
        /// </summary>
        public static Runnable<TInput, TOutput> WithFallback<TInput, TOutput, TException>(
            this IRunnable<TInput, TOutput> runnable,
            IRunnable<TInput, TOutput> fallback)
            where TException : Exception
        {
            return new Runnable<TInput, TOutput>(
                input =>
                {
                    try
                    {
                        return runnable.Invoke(input);
                    }
                    catch (TException)
                    {
                        return fallback.Invoke(input);
                    }
                },
                async input =>
                {
                    try
                    {
                        return await runnable.InvokeAsync(input);
                    }
                    catch (TException)
                    {
                        return await fallback.InvokeAsync(input);
                    }
                }
            );
        }

        /// <summary>
        /// Provide a fallback value for specific exception types (no input version)
        /// </summary>
        public static Runnable<TOutput> WithFallbackValue<TOutput, TException>(
            this IRunnable<TOutput> runnable,
            TOutput fallbackValue)
            where TException : Exception
        {
            return new Runnable<TOutput>(
                () =>
                {
                    try
                    {
                        return runnable.Invoke();
                    }
                    catch (TException)
                    {
                        return fallbackValue;
                    }
                },
                async () =>
                {
                    try
                    {
                        return await runnable.InvokeAsync();
                    }
                    catch (TException)
                    {
                        return fallbackValue;
                    }
                }
            );
        }

        /// <summary>
        /// Provide a fallback value for specific exception types (1 input version)
        /// </summary>
        public static Runnable<TInput, TOutput> WithFallbackValue<TInput, TOutput, TException>(
            this IRunnable<TInput, TOutput> runnable,
            TOutput fallbackValue)
            where TException : Exception
        {
            return new Runnable<TInput, TOutput>(
                input =>
                {
                    try
                    {
                        return runnable.Invoke(input);
                    }
                    catch (TException)
                    {
                        return fallbackValue;
                    }
                },
                async input =>
                {
                    try
                    {
                        return await runnable.InvokeAsync(input);
                    }
                    catch (TException)
                    {
                        return fallbackValue;
                    }
                }
            );
        }

        // ==================== WithFallback with Exception Predicate ====================

        /// <summary>
        /// Provide a fallback runnable with custom exception filter (1 input version)
        /// </summary>
        public static Runnable<TInput, TOutput> WithFallbackWhen<TInput, TOutput>(
            this IRunnable<TInput, TOutput> runnable,
            Func<Exception, bool> exceptionPredicate,
            IRunnable<TInput, TOutput> fallback)
        {
            return new Runnable<TInput, TOutput>(
                input =>
                {
                    try
                    {
                        return runnable.Invoke(input);
                    }
                    catch (Exception ex) when (exceptionPredicate(ex))
                    {
                        return fallback.Invoke(input);
                    }
                },
                async input =>
                {
                    try
                    {
                        return await runnable.InvokeAsync(input);
                    }
                    catch (Exception ex) when (exceptionPredicate(ex))
                    {
                        return await fallback.InvokeAsync(input);
                    }
                }
            );
        }

        /// <summary>
        /// Provide a fallback value with custom exception filter (1 input version)
        /// </summary>
        public static Runnable<TInput, TOutput> WithFallbackValueWhen<TInput, TOutput>(
            this IRunnable<TInput, TOutput> runnable,
            Func<Exception, bool> exceptionPredicate,
            TOutput fallbackValue)
        {
            return new Runnable<TInput, TOutput>(
                input =>
                {
                    try
                    {
                        return runnable.Invoke(input);
                    }
                    catch (Exception ex) when (exceptionPredicate(ex))
                    {
                        return fallbackValue;
                    }
                },
                async input =>
                {
                    try
                    {
                        return await runnable.InvokeAsync(input);
                    }
                    catch (Exception ex) when (exceptionPredicate(ex))
                    {
                        return fallbackValue;
                    }
                }
            );
        }
    }
}
