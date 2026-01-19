using System;
using System.Threading.Tasks;

namespace Runnable
{
    /// <summary>
    /// Extensions for filtering with RunnableContext access for multi-tenant and security filtering
    /// </summary>
    public static class RunnableFilterContextExtensions
    {
        // ==================== FilterContext (Filter with context) ====================

        /// <summary>
        /// Filter with context access (1 input version)
        /// </summary>
        public static Runnable<TInput, TInput> FilterContext<TInput>(
            this IRunnable<TInput, TInput> runnable,
            Func<TInput, RunnableContext, bool> predicate)
        {
            return new Runnable<TInput, TInput>(
                input =>
                {
                    if (!predicate(input, RunnableContext.Current))
                        throw new InvalidOperationException("Input filtered out by context predicate");
                    return runnable.Invoke(input);
                },
                async input =>
                {
                    if (!predicate(input, RunnableContext.Current))
                        throw new InvalidOperationException("Input filtered out by context predicate");
                    return await runnable.InvokeAsync(input);
                }
            );
        }

        /// <summary>
        /// Filter with context access, returning default on filter (1 input version)
        /// </summary>
        public static Runnable<TInput, TInput> FilterContextOrDefault<TInput>(
            this IRunnable<TInput, TInput> runnable,
            Func<TInput, RunnableContext, bool> predicate,
            TInput defaultValue = default)
        {
            return new Runnable<TInput, TInput>(
                input => predicate(input, RunnableContext.Current) ? runnable.Invoke(input) : defaultValue,
                async input => predicate(input, RunnableContext.Current) ? await runnable.InvokeAsync(input) : defaultValue
            );
        }

        /// <summary>
        /// Filter asynchronously with context access (1 input version)
        /// </summary>
        public static Runnable<TInput, TInput> FilterAsyncContext<TInput>(
            this IRunnable<TInput, TInput> runnable,
            Func<TInput, RunnableContext, Task<bool>> predicate)
        {
            return new Runnable<TInput, TInput>(
                input =>
                {
                    if (!predicate(input, RunnableContext.Current).GetAwaiter().GetResult())
                        throw new InvalidOperationException("Input filtered out by async context predicate");
                    return runnable.Invoke(input);
                },
                async input =>
                {
                    if (!await predicate(input, RunnableContext.Current))
                        throw new InvalidOperationException("Input filtered out by async context predicate");
                    return await runnable.InvokeAsync(input);
                }
            );
        }

        /// <summary>
        /// Filter asynchronously with context access, returning default on filter (1 input version)
        /// </summary>
        public static Runnable<TInput, TInput> FilterAsyncContextOrDefault<TInput>(
            this IRunnable<TInput, TInput> runnable,
            Func<TInput, RunnableContext, Task<bool>> predicate,
            TInput defaultValue = default)
        {
            return new Runnable<TInput, TInput>(
                input =>
                {
                    var allowed = predicate(input, RunnableContext.Current).GetAwaiter().GetResult();
                    return allowed ? runnable.Invoke(input) : defaultValue;
                },
                async input =>
                {
                    var allowed = await predicate(input, RunnableContext.Current);
                    return allowed ? await runnable.InvokeAsync(input) : defaultValue;
                }
            );
        }

        // ==================== FilterContext for 2-15 parameters ====================

        public static Runnable<T1, T2, T1> FilterContext<T1, T2>(
            this IRunnable<T1, T2, T1> runnable,
            Func<T1, T2, RunnableContext, bool> predicate) =>
            new Runnable<T1, T2, T1>(
                (a1, a2) =>
                {
                    if (!predicate(a1, a2, RunnableContext.Current))
                        throw new InvalidOperationException("Input filtered out by context predicate");
                    return runnable.Invoke(a1, a2);
                },
                async (a1, a2) =>
                {
                    if (!predicate(a1, a2, RunnableContext.Current))
                        throw new InvalidOperationException("Input filtered out by context predicate");
                    return await runnable.InvokeAsync(a1, a2);
                });

        public static Runnable<T1, T2, T3, T1> FilterContext<T1, T2, T3>(
            this IRunnable<T1, T2, T3, T1> runnable,
            Func<T1, T2, T3, RunnableContext, bool> predicate) =>
            new Runnable<T1, T2, T3, T1>(
                (a1, a2, a3) =>
                {
                    if (!predicate(a1, a2, a3, RunnableContext.Current))
                        throw new InvalidOperationException("Input filtered out by context predicate");
                    return runnable.Invoke(a1, a2, a3);
                },
                async (a1, a2, a3) =>
                {
                    if (!predicate(a1, a2, a3, RunnableContext.Current))
                        throw new InvalidOperationException("Input filtered out by context predicate");
                    return await runnable.InvokeAsync(a1, a2, a3);
                });

        public static Runnable<T1, T2, T3, T4, T1> FilterContext<T1, T2, T3, T4>(
            this IRunnable<T1, T2, T3, T4, T1> runnable,
            Func<T1, T2, T3, T4, RunnableContext, bool> predicate) =>
            new Runnable<T1, T2, T3, T4, T1>(
                (a1, a2, a3, a4) =>
                {
                    if (!predicate(a1, a2, a3, a4, RunnableContext.Current))
                        throw new InvalidOperationException("Input filtered out by context predicate");
                    return runnable.Invoke(a1, a2, a3, a4);
                },
                async (a1, a2, a3, a4) =>
                {
                    if (!predicate(a1, a2, a3, a4, RunnableContext.Current))
                        throw new InvalidOperationException("Input filtered out by context predicate");
                    return await runnable.InvokeAsync(a1, a2, a3, a4);
                });

        public static Runnable<T1, T2, T3, T4, T5, T1> FilterContext<T1, T2, T3, T4, T5>(
            this IRunnable<T1, T2, T3, T4, T5, T1> runnable,
            Func<T1, T2, T3, T4, T5, RunnableContext, bool> predicate) =>
            new Runnable<T1, T2, T3, T4, T5, T1>(
                (a1, a2, a3, a4, a5) =>
                {
                    if (!predicate(a1, a2, a3, a4, a5, RunnableContext.Current))
                        throw new InvalidOperationException("Input filtered out by context predicate");
                    return runnable.Invoke(a1, a2, a3, a4, a5);
                },
                async (a1, a2, a3, a4, a5) =>
                {
                    if (!predicate(a1, a2, a3, a4, a5, RunnableContext.Current))
                        throw new InvalidOperationException("Input filtered out by context predicate");
                    return await runnable.InvokeAsync(a1, a2, a3, a4, a5);
                });

        public static Runnable<T1, T2, T3, T4, T5, T6, T1> FilterContext<T1, T2, T3, T4, T5, T6>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T1> runnable,
            Func<T1, T2, T3, T4, T5, T6, RunnableContext, bool> predicate) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T1>(
                (a1, a2, a3, a4, a5, a6) =>
                {
                    if (!predicate(a1, a2, a3, a4, a5, a6, RunnableContext.Current))
                        throw new InvalidOperationException("Input filtered out by context predicate");
                    return runnable.Invoke(a1, a2, a3, a4, a5, a6);
                },
                async (a1, a2, a3, a4, a5, a6) =>
                {
                    if (!predicate(a1, a2, a3, a4, a5, a6, RunnableContext.Current))
                        throw new InvalidOperationException("Input filtered out by context predicate");
                    return await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6);
                });

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T1> FilterContext<T1, T2, T3, T4, T5, T6, T7>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T1> runnable,
            Func<T1, T2, T3, T4, T5, T6, T7, RunnableContext, bool> predicate) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T1>(
                (a1, a2, a3, a4, a5, a6, a7) =>
                {
                    if (!predicate(a1, a2, a3, a4, a5, a6, a7, RunnableContext.Current))
                        throw new InvalidOperationException("Input filtered out by context predicate");
                    return runnable.Invoke(a1, a2, a3, a4, a5, a6, a7);
                },
                async (a1, a2, a3, a4, a5, a6, a7) =>
                {
                    if (!predicate(a1, a2, a3, a4, a5, a6, a7, RunnableContext.Current))
                        throw new InvalidOperationException("Input filtered out by context predicate");
                    return await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7);
                });

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T1> FilterContext<T1, T2, T3, T4, T5, T6, T7, T8>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T1> runnable,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, RunnableContext, bool> predicate) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T1>(
                (a1, a2, a3, a4, a5, a6, a7, a8) =>
                {
                    if (!predicate(a1, a2, a3, a4, a5, a6, a7, a8, RunnableContext.Current))
                        throw new InvalidOperationException("Input filtered out by context predicate");
                    return runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8);
                },
                async (a1, a2, a3, a4, a5, a6, a7, a8) =>
                {
                    if (!predicate(a1, a2, a3, a4, a5, a6, a7, a8, RunnableContext.Current))
                        throw new InvalidOperationException("Input filtered out by context predicate");
                    return await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8);
                });

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T1> FilterContext<T1, T2, T3, T4, T5, T6, T7, T8, T9>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T1> runnable,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, RunnableContext, bool> predicate) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T1>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9) =>
                {
                    if (!predicate(a1, a2, a3, a4, a5, a6, a7, a8, a9, RunnableContext.Current))
                        throw new InvalidOperationException("Input filtered out by context predicate");
                    return runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9);
                },
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9) =>
                {
                    if (!predicate(a1, a2, a3, a4, a5, a6, a7, a8, a9, RunnableContext.Current))
                        throw new InvalidOperationException("Input filtered out by context predicate");
                    return await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9);
                });

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T1> FilterContext<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T1> runnable,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, RunnableContext, bool> predicate) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T1>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10) =>
                {
                    if (!predicate(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, RunnableContext.Current))
                        throw new InvalidOperationException("Input filtered out by context predicate");
                    return runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10);
                },
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10) =>
                {
                    if (!predicate(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, RunnableContext.Current))
                        throw new InvalidOperationException("Input filtered out by context predicate");
                    return await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10);
                });

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T1> FilterContext<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T1> runnable,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, RunnableContext, bool> predicate) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T1>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11) =>
                {
                    if (!predicate(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, RunnableContext.Current))
                        throw new InvalidOperationException("Input filtered out by context predicate");
                    return runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11);
                },
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11) =>
                {
                    if (!predicate(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, RunnableContext.Current))
                        throw new InvalidOperationException("Input filtered out by context predicate");
                    return await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11);
                });

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T1> FilterContext<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T1> runnable,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, RunnableContext, bool> predicate) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T1>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12) =>
                {
                    if (!predicate(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, RunnableContext.Current))
                        throw new InvalidOperationException("Input filtered out by context predicate");
                    return runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12);
                },
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12) =>
                {
                    if (!predicate(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, RunnableContext.Current))
                        throw new InvalidOperationException("Input filtered out by context predicate");
                    return await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12);
                });

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T1> FilterContext<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T1> runnable,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, RunnableContext, bool> predicate) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T1>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13) =>
                {
                    if (!predicate(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, RunnableContext.Current))
                        throw new InvalidOperationException("Input filtered out by context predicate");
                    return runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13);
                },
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13) =>
                {
                    if (!predicate(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, RunnableContext.Current))
                        throw new InvalidOperationException("Input filtered out by context predicate");
                    return await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13);
                });

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T1> FilterContext<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T1> runnable,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, RunnableContext, bool> predicate) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T1>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14) =>
                {
                    if (!predicate(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, RunnableContext.Current))
                        throw new InvalidOperationException("Input filtered out by context predicate");
                    return runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14);
                },
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14) =>
                {
                    if (!predicate(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, RunnableContext.Current))
                        throw new InvalidOperationException("Input filtered out by context predicate");
                    return await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14);
                });

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T1> FilterContext<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T1> runnable,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, RunnableContext, bool> predicate) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T1>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15) =>
                {
                    if (!predicate(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, RunnableContext.Current))
                        throw new InvalidOperationException("Input filtered out by context predicate");
                    return runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15);
                },
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15) =>
                {
                    if (!predicate(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, RunnableContext.Current))
                        throw new InvalidOperationException("Input filtered out by context predicate");
                    return await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15);
                });

        // ==================== FilterAsyncContext for 2-15 parameters ====================

        public static Runnable<T1, T2, T1> FilterAsyncContext<T1, T2>(
            this IRunnable<T1, T2, T1> runnable,
            Func<T1, T2, RunnableContext, Task<bool>> predicate) =>
            new Runnable<T1, T2, T1>(
                (a1, a2) =>
                {
                    if (!predicate(a1, a2, RunnableContext.Current).GetAwaiter().GetResult())
                        throw new InvalidOperationException("Input filtered out by async context predicate");
                    return runnable.Invoke(a1, a2);
                },
                async (a1, a2) =>
                {
                    if (!await predicate(a1, a2, RunnableContext.Current))
                        throw new InvalidOperationException("Input filtered out by async context predicate");
                    return await runnable.InvokeAsync(a1, a2);
                });

        public static Runnable<T1, T2, T3, T1> FilterAsyncContext<T1, T2, T3>(
            this IRunnable<T1, T2, T3, T1> runnable,
            Func<T1, T2, T3, RunnableContext, Task<bool>> predicate) =>
            new Runnable<T1, T2, T3, T1>(
                (a1, a2, a3) =>
                {
                    if (!predicate(a1, a2, a3, RunnableContext.Current).GetAwaiter().GetResult())
                        throw new InvalidOperationException("Input filtered out by async context predicate");
                    return runnable.Invoke(a1, a2, a3);
                },
                async (a1, a2, a3) =>
                {
                    if (!await predicate(a1, a2, a3, RunnableContext.Current))
                        throw new InvalidOperationException("Input filtered out by async context predicate");
                    return await runnable.InvokeAsync(a1, a2, a3);
                });

        public static Runnable<T1, T2, T3, T4, T1> FilterAsyncContext<T1, T2, T3, T4>(
            this IRunnable<T1, T2, T3, T4, T1> runnable,
            Func<T1, T2, T3, T4, RunnableContext, Task<bool>> predicate) =>
            new Runnable<T1, T2, T3, T4, T1>(
                (a1, a2, a3, a4) =>
                {
                    if (!predicate(a1, a2, a3, a4, RunnableContext.Current).GetAwaiter().GetResult())
                        throw new InvalidOperationException("Input filtered out by async context predicate");
                    return runnable.Invoke(a1, a2, a3, a4);
                },
                async (a1, a2, a3, a4) =>
                {
                    if (!await predicate(a1, a2, a3, a4, RunnableContext.Current))
                        throw new InvalidOperationException("Input filtered out by async context predicate");
                    return await runnable.InvokeAsync(a1, a2, a3, a4);
                });

        public static Runnable<T1, T2, T3, T4, T5, T1> FilterAsyncContext<T1, T2, T3, T4, T5>(
            this IRunnable<T1, T2, T3, T4, T5, T1> runnable,
            Func<T1, T2, T3, T4, T5, RunnableContext, Task<bool>> predicate) =>
            new Runnable<T1, T2, T3, T4, T5, T1>(
                (a1, a2, a3, a4, a5) =>
                {
                    if (!predicate(a1, a2, a3, a4, a5, RunnableContext.Current).GetAwaiter().GetResult())
                        throw new InvalidOperationException("Input filtered out by async context predicate");
                    return runnable.Invoke(a1, a2, a3, a4, a5);
                },
                async (a1, a2, a3, a4, a5) =>
                {
                    if (!await predicate(a1, a2, a3, a4, a5, RunnableContext.Current))
                        throw new InvalidOperationException("Input filtered out by async context predicate");
                    return await runnable.InvokeAsync(a1, a2, a3, a4, a5);
                });

        public static Runnable<T1, T2, T3, T4, T5, T6, T1> FilterAsyncContext<T1, T2, T3, T4, T5, T6>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T1> runnable,
            Func<T1, T2, T3, T4, T5, T6, RunnableContext, Task<bool>> predicate) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T1>(
                (a1, a2, a3, a4, a5, a6) =>
                {
                    if (!predicate(a1, a2, a3, a4, a5, a6, RunnableContext.Current).GetAwaiter().GetResult())
                        throw new InvalidOperationException("Input filtered out by async context predicate");
                    return runnable.Invoke(a1, a2, a3, a4, a5, a6);
                },
                async (a1, a2, a3, a4, a5, a6) =>
                {
                    if (!await predicate(a1, a2, a3, a4, a5, a6, RunnableContext.Current))
                        throw new InvalidOperationException("Input filtered out by async context predicate");
                    return await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6);
                });

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T1> FilterAsyncContext<T1, T2, T3, T4, T5, T6, T7>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T1> runnable,
            Func<T1, T2, T3, T4, T5, T6, T7, RunnableContext, Task<bool>> predicate) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T1>(
                (a1, a2, a3, a4, a5, a6, a7) =>
                {
                    if (!predicate(a1, a2, a3, a4, a5, a6, a7, RunnableContext.Current).GetAwaiter().GetResult())
                        throw new InvalidOperationException("Input filtered out by async context predicate");
                    return runnable.Invoke(a1, a2, a3, a4, a5, a6, a7);
                },
                async (a1, a2, a3, a4, a5, a6, a7) =>
                {
                    if (!await predicate(a1, a2, a3, a4, a5, a6, a7, RunnableContext.Current))
                        throw new InvalidOperationException("Input filtered out by async context predicate");
                    return await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7);
                });

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T1> FilterAsyncContext<T1, T2, T3, T4, T5, T6, T7, T8>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T1> runnable,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, RunnableContext, Task<bool>> predicate) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T1>(
                (a1, a2, a3, a4, a5, a6, a7, a8) =>
                {
                    if (!predicate(a1, a2, a3, a4, a5, a6, a7, a8, RunnableContext.Current).GetAwaiter().GetResult())
                        throw new InvalidOperationException("Input filtered out by async context predicate");
                    return runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8);
                },
                async (a1, a2, a3, a4, a5, a6, a7, a8) =>
                {
                    if (!await predicate(a1, a2, a3, a4, a5, a6, a7, a8, RunnableContext.Current))
                        throw new InvalidOperationException("Input filtered out by async context predicate");
                    return await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8);
                });

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T1> FilterAsyncContext<T1, T2, T3, T4, T5, T6, T7, T8, T9>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T1> runnable,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, RunnableContext, Task<bool>> predicate) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T1>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9) =>
                {
                    if (!predicate(a1, a2, a3, a4, a5, a6, a7, a8, a9, RunnableContext.Current).GetAwaiter().GetResult())
                        throw new InvalidOperationException("Input filtered out by async context predicate");
                    return runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9);
                },
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9) =>
                {
                    if (!await predicate(a1, a2, a3, a4, a5, a6, a7, a8, a9, RunnableContext.Current))
                        throw new InvalidOperationException("Input filtered out by async context predicate");
                    return await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9);
                });

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T1> FilterAsyncContext<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T1> runnable,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, RunnableContext, Task<bool>> predicate) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T1>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10) =>
                {
                    if (!predicate(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, RunnableContext.Current).GetAwaiter().GetResult())
                        throw new InvalidOperationException("Input filtered out by async context predicate");
                    return runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10);
                },
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10) =>
                {
                    if (!await predicate(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, RunnableContext.Current))
                        throw new InvalidOperationException("Input filtered out by async context predicate");
                    return await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10);
                });

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T1> FilterAsyncContext<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T1> runnable,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, RunnableContext, Task<bool>> predicate) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T1>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11) =>
                {
                    if (!predicate(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, RunnableContext.Current).GetAwaiter().GetResult())
                        throw new InvalidOperationException("Input filtered out by async context predicate");
                    return runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11);
                },
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11) =>
                {
                    if (!await predicate(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, RunnableContext.Current))
                        throw new InvalidOperationException("Input filtered out by async context predicate");
                    return await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11);
                });

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T1> FilterAsyncContext<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T1> runnable,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, RunnableContext, Task<bool>> predicate) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T1>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12) =>
                {
                    if (!predicate(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, RunnableContext.Current).GetAwaiter().GetResult())
                        throw new InvalidOperationException("Input filtered out by async context predicate");
                    return runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12);
                },
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12) =>
                {
                    if (!await predicate(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, RunnableContext.Current))
                        throw new InvalidOperationException("Input filtered out by async context predicate");
                    return await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12);
                });

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T1> FilterAsyncContext<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T1> runnable,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, RunnableContext, Task<bool>> predicate) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T1>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13) =>
                {
                    if (!predicate(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, RunnableContext.Current).GetAwaiter().GetResult())
                        throw new InvalidOperationException("Input filtered out by async context predicate");
                    return runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13);
                },
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13) =>
                {
                    if (!await predicate(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, RunnableContext.Current))
                        throw new InvalidOperationException("Input filtered out by async context predicate");
                    return await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13);
                });

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T1> FilterAsyncContext<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T1> runnable,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, RunnableContext, Task<bool>> predicate) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T1>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14) =>
                {
                    if (!predicate(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, RunnableContext.Current).GetAwaiter().GetResult())
                        throw new InvalidOperationException("Input filtered out by async context predicate");
                    return runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14);
                },
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14) =>
                {
                    if (!await predicate(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, RunnableContext.Current))
                        throw new InvalidOperationException("Input filtered out by async context predicate");
                    return await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14);
                });

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T1> FilterAsyncContext<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T1> runnable,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, RunnableContext, Task<bool>> predicate) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T1>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15) =>
                {
                    if (!predicate(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, RunnableContext.Current).GetAwaiter().GetResult())
                        throw new InvalidOperationException("Input filtered out by async context predicate");
                    return runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15);
                },
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15) =>
                {
                    if (!await predicate(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, RunnableContext.Current))
                        throw new InvalidOperationException("Input filtered out by async context predicate");
                    return await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15);
                });
    }
}
