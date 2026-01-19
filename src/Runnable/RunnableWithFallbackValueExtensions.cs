using System;
using System.Collections.Generic;
using System.Text;

namespace Runnable
{
    public static class RunnableWithFallbackValueExtensions
    {
        /// <summary>
        /// Provide a fallback value in case of exceptions
        /// </summary>
        public static Runnable<TInput, TOutput> WithFallbackValue<TInput, TOutput>(
            this IRunnable<TInput, TOutput> runnable,
            TOutput fallbackValue)
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
                        return fallbackValue;
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
                        return fallbackValue;
                    }
                }
            );
        }

        // ==================== WithFallbackValue for 2-16 parameters ====================

        public static Runnable<T1, T2, TOutput> WithFallbackValue<T1, T2, TOutput>(
            this IRunnable<T1, T2, TOutput> runnable, TOutput fallbackValue) =>
            new Runnable<T1, T2, TOutput>(
                (a1, a2) => { try { return runnable.Invoke(a1, a2); } catch { return fallbackValue; } },
                async (a1, a2) => { try { return await runnable.InvokeAsync(a1, a2); } catch { return fallbackValue; } });

        public static Runnable<T1, T2, T3, TOutput> WithFallbackValue<T1, T2, T3, TOutput>(
            this IRunnable<T1, T2, T3, TOutput> runnable, TOutput fallbackValue) =>
            new Runnable<T1, T2, T3, TOutput>(
                (a1, a2, a3) => { try { return runnable.Invoke(a1, a2, a3); } catch { return fallbackValue; } },
                async (a1, a2, a3) => { try { return await runnable.InvokeAsync(a1, a2, a3); } catch { return fallbackValue; } });

        public static Runnable<T1, T2, T3, T4, TOutput> WithFallbackValue<T1, T2, T3, T4, TOutput>(
            this IRunnable<T1, T2, T3, T4, TOutput> runnable, TOutput fallbackValue) =>
            new Runnable<T1, T2, T3, T4, TOutput>(
                (a1, a2, a3, a4) => { try { return runnable.Invoke(a1, a2, a3, a4); } catch { return fallbackValue; } },
                async (a1, a2, a3, a4) => { try { return await runnable.InvokeAsync(a1, a2, a3, a4); } catch { return fallbackValue; } });

        public static Runnable<T1, T2, T3, T4, T5, TOutput> WithFallbackValue<T1, T2, T3, T4, T5, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, TOutput> runnable, TOutput fallbackValue) =>
            new Runnable<T1, T2, T3, T4, T5, TOutput>(
                (a1, a2, a3, a4, a5) => { try { return runnable.Invoke(a1, a2, a3, a4, a5); } catch { return fallbackValue; } },
                async (a1, a2, a3, a4, a5) => { try { return await runnable.InvokeAsync(a1, a2, a3, a4, a5); } catch { return fallbackValue; } });

        public static Runnable<T1, T2, T3, T4, T5, T6, TOutput> WithFallbackValue<T1, T2, T3, T4, T5, T6, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, TOutput> runnable, TOutput fallbackValue) =>
            new Runnable<T1, T2, T3, T4, T5, T6, TOutput>(
                (a1, a2, a3, a4, a5, a6) => { try { return runnable.Invoke(a1, a2, a3, a4, a5, a6); } catch { return fallbackValue; } },
                async (a1, a2, a3, a4, a5, a6) => { try { return await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6); } catch { return fallbackValue; } });

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, TOutput> WithFallbackValue<T1, T2, T3, T4, T5, T6, T7, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, TOutput> runnable, TOutput fallbackValue) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7) => { try { return runnable.Invoke(a1, a2, a3, a4, a5, a6, a7); } catch { return fallbackValue; } },
                async (a1, a2, a3, a4, a5, a6, a7) => { try { return await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7); } catch { return fallbackValue; } });

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, TOutput> WithFallbackValue<T1, T2, T3, T4, T5, T6, T7, T8, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, TOutput> runnable, TOutput fallbackValue) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8) => { try { return runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8); } catch { return fallbackValue; } },
                async (a1, a2, a3, a4, a5, a6, a7, a8) => { try { return await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8); } catch { return fallbackValue; } });

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, TOutput> WithFallbackValue<T1, T2, T3, T4, T5, T6, T7, T8, T9, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, TOutput> runnable, TOutput fallbackValue) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9) => { try { return runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9); } catch { return fallbackValue; } },
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9) => { try { return await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9); } catch { return fallbackValue; } });

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOutput> WithFallbackValue<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOutput> runnable, TOutput fallbackValue) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10) => { try { return runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10); } catch { return fallbackValue; } },
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10) => { try { return await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10); } catch { return fallbackValue; } });

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOutput> WithFallbackValue<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOutput> runnable, TOutput fallbackValue) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11) => { try { return runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11); } catch { return fallbackValue; } },
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11) => { try { return await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11); } catch { return fallbackValue; } });

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOutput> WithFallbackValue<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOutput> runnable, TOutput fallbackValue) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12) => { try { return runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12); } catch { return fallbackValue; } },
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12) => { try { return await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12); } catch { return fallbackValue; } });

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOutput> WithFallbackValue<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOutput> runnable, TOutput fallbackValue) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13) => { try { return runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13); } catch { return fallbackValue; } },
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13) => { try { return await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13); } catch { return fallbackValue; } });

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOutput> WithFallbackValue<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOutput> runnable, TOutput fallbackValue) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14) => { try { return runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14); } catch { return fallbackValue; } },
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14) => { try { return await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14); } catch { return fallbackValue; } });

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOutput> WithFallbackValue<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOutput> runnable, TOutput fallbackValue) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15) => { try { return runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15); } catch { return fallbackValue; } },
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15) => { try { return await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15); } catch { return fallbackValue; } });

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TOutput> WithFallbackValue<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TOutput> runnable, TOutput fallbackValue) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16) => { try { return runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16); } catch { return fallbackValue; } },
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16) => { try { return await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16); } catch { return fallbackValue; } });

    }
}
