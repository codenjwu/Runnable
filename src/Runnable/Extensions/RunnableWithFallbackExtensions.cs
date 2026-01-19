using System;
using System.Collections.Generic;
using System.Text;

namespace Runnable
{
    public static class RunnableWithFallbackExtensions
    {
        // ==================== WithFallback (Error Handling) ====================

        /// <summary>
        /// Provide a fallback runnable in case of exceptions (no input version)
        /// </summary>
        public static Runnable<TOutput> WithFallback<TOutput>(
            this IRunnable<TOutput> runnable,
            IRunnable<TOutput> fallback)
        {
            return new Runnable<TOutput>(
                () =>
                {
                    try
                    {
                        return runnable.Invoke();
                    }
                    catch
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
                    catch
                    {
                        return await fallback.InvokeAsync();
                    }
                }
            );
        }

        /// <summary>
        /// Provide a fallback runnable in case of exceptions (1 input version)
        /// </summary>
        public static Runnable<TInput, TOutput> WithFallback<TInput, TOutput>(
            this IRunnable<TInput, TOutput> runnable,
            IRunnable<TInput, TOutput> fallback)
        {
            return new Runnable<TInput, TOutput>(
                input =>
                {
                    try
                    {
                        return runnable.Invoke(input);
                    }
                    catch
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
                    catch
                    {
                        return await fallback.InvokeAsync(input);
                    }
                }
            );
        }

        // ==================== WithFallback for 2-16 parameters ====================

        public static Runnable<T1, T2, TOutput> WithFallback<T1, T2, TOutput>(
            this IRunnable<T1, T2, TOutput> runnable, IRunnable<T1, T2, TOutput> fallback) =>
            new Runnable<T1, T2, TOutput>(
                (a1, a2) => { try { return runnable.Invoke(a1, a2); } catch { return fallback.Invoke(a1, a2); } },
                async (a1, a2) => { try { return await runnable.InvokeAsync(a1, a2); } catch { return await fallback.InvokeAsync(a1, a2); } });

        public static Runnable<T1, T2, T3, TOutput> WithFallback<T1, T2, T3, TOutput>(
            this IRunnable<T1, T2, T3, TOutput> runnable, IRunnable<T1, T2, T3, TOutput> fallback) =>
            new Runnable<T1, T2, T3, TOutput>(
                (a1, a2, a3) => { try { return runnable.Invoke(a1, a2, a3); } catch { return fallback.Invoke(a1, a2, a3); } },
                async (a1, a2, a3) => { try { return await runnable.InvokeAsync(a1, a2, a3); } catch { return await fallback.InvokeAsync(a1, a2, a3); } });

        public static Runnable<T1, T2, T3, T4, TOutput> WithFallback<T1, T2, T3, T4, TOutput>(
            this IRunnable<T1, T2, T3, T4, TOutput> runnable, IRunnable<T1, T2, T3, T4, TOutput> fallback) =>
            new Runnable<T1, T2, T3, T4, TOutput>(
                (a1, a2, a3, a4) => { try { return runnable.Invoke(a1, a2, a3, a4); } catch { return fallback.Invoke(a1, a2, a3, a4); } },
                async (a1, a2, a3, a4) => { try { return await runnable.InvokeAsync(a1, a2, a3, a4); } catch { return await fallback.InvokeAsync(a1, a2, a3, a4); } });

        public static Runnable<T1, T2, T3, T4, T5, TOutput> WithFallback<T1, T2, T3, T4, T5, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, TOutput> runnable, IRunnable<T1, T2, T3, T4, T5, TOutput> fallback) =>
            new Runnable<T1, T2, T3, T4, T5, TOutput>(
                (a1, a2, a3, a4, a5) => { try { return runnable.Invoke(a1, a2, a3, a4, a5); } catch { return fallback.Invoke(a1, a2, a3, a4, a5); } },
                async (a1, a2, a3, a4, a5) => { try { return await runnable.InvokeAsync(a1, a2, a3, a4, a5); } catch { return await fallback.InvokeAsync(a1, a2, a3, a4, a5); } });

        public static Runnable<T1, T2, T3, T4, T5, T6, TOutput> WithFallback<T1, T2, T3, T4, T5, T6, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, TOutput> runnable, IRunnable<T1, T2, T3, T4, T5, T6, TOutput> fallback) =>
            new Runnable<T1, T2, T3, T4, T5, T6, TOutput>(
                (a1, a2, a3, a4, a5, a6) => { try { return runnable.Invoke(a1, a2, a3, a4, a5, a6); } catch { return fallback.Invoke(a1, a2, a3, a4, a5, a6); } },
                async (a1, a2, a3, a4, a5, a6) => { try { return await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6); } catch { return await fallback.InvokeAsync(a1, a2, a3, a4, a5, a6); } });

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, TOutput> WithFallback<T1, T2, T3, T4, T5, T6, T7, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, TOutput> runnable, IRunnable<T1, T2, T3, T4, T5, T6, T7, TOutput> fallback) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7) => { try { return runnable.Invoke(a1, a2, a3, a4, a5, a6, a7); } catch { return fallback.Invoke(a1, a2, a3, a4, a5, a6, a7); } },
                async (a1, a2, a3, a4, a5, a6, a7) => { try { return await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7); } catch { return await fallback.InvokeAsync(a1, a2, a3, a4, a5, a6, a7); } });

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, TOutput> WithFallback<T1, T2, T3, T4, T5, T6, T7, T8, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, TOutput> runnable, IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, TOutput> fallback) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8) => { try { return runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8); } catch { return fallback.Invoke(a1, a2, a3, a4, a5, a6, a7, a8); } },
                async (a1, a2, a3, a4, a5, a6, a7, a8) => { try { return await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8); } catch { return await fallback.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8); } });

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, TOutput> WithFallback<T1, T2, T3, T4, T5, T6, T7, T8, T9, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, TOutput> runnable, IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, TOutput> fallback) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9) => { try { return runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9); } catch { return fallback.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9); } },
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9) => { try { return await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9); } catch { return await fallback.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9); } });

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOutput> WithFallback<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOutput> runnable, IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOutput> fallback) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10) => { try { return runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10); } catch { return fallback.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10); } },
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10) => { try { return await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10); } catch { return await fallback.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10); } });

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOutput> WithFallback<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOutput> runnable, IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOutput> fallback) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11) => { try { return runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11); } catch { return fallback.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11); } },
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11) => { try { return await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11); } catch { return await fallback.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11); } });

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOutput> WithFallback<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOutput> runnable, IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOutput> fallback) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12) => { try { return runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12); } catch { return fallback.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12); } },
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12) => { try { return await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12); } catch { return await fallback.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12); } });

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOutput> WithFallback<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOutput> runnable, IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOutput> fallback) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13) => { try { return runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13); } catch { return fallback.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13); } },
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13) => { try { return await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13); } catch { return await fallback.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13); } });

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOutput> WithFallback<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOutput> runnable, IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOutput> fallback) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14) => { try { return runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14); } catch { return fallback.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14); } },
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14) => { try { return await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14); } catch { return await fallback.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14); } });

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOutput> WithFallback<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOutput> runnable, IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOutput> fallback) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15) => { try { return runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15); } catch { return fallback.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15); } },
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15) => { try { return await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15); } catch { return await fallback.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15); } });

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TOutput> WithFallback<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TOutput> runnable, IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TOutput> fallback) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16) => { try { return runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16); } catch { return fallback.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16); } },
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16) => { try { return await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16); } catch { return await fallback.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16); } });
    }
}
