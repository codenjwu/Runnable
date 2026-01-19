using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Runnable
{
    public static class RunnableWithRetryExtensions
    { 
        // ==================== WithRetry ====================

        /// <summary>
        /// Retry the runnable on failure (no input version)
        /// </summary>
        public static Runnable<TOutput> WithRetry<TOutput>(
            this IRunnable<TOutput> runnable,
            int maxAttempts = 3,
            TimeSpan? delay = null)
        {
            var retryDelay = delay ?? TimeSpan.FromMilliseconds(100);

            return new Runnable<TOutput>(
                () =>
                {
                    Exception lastException = null;
                    for (int i = 0; i < maxAttempts; i++)
                    {
                        try
                        {
                            return runnable.Invoke();
                        }
                        catch (Exception ex)
                        {
                            lastException = ex;
                            if (i < maxAttempts - 1)
                            {
                                System.Threading.Thread.Sleep(retryDelay);
                            }
                        }
                    }
                    throw lastException;
                },
                async () =>
                {
                    Exception lastException = null;
                    for (int i = 0; i < maxAttempts; i++)
                    {
                        try
                        {
                            return await runnable.InvokeAsync();
                        }
                        catch (Exception ex)
                        {
                            lastException = ex;
                            if (i < maxAttempts - 1)
                            {
                                await Task.Delay(retryDelay);
                            }
                        }
                    }
                    throw lastException;
                }
            );
        }

        /// <summary>
        /// Retry the runnable on failure (1 input version)
        /// </summary>
        public static Runnable<TInput, TOutput> WithRetry<TInput, TOutput>(
            this IRunnable<TInput, TOutput> runnable,
            int maxAttempts = 3,
            TimeSpan? delay = null)
        {
            var retryDelay = delay ?? TimeSpan.FromMilliseconds(100);

            return new Runnable<TInput, TOutput>(
                input =>
                {
                    Exception lastException = null;
                    for (int i = 0; i < maxAttempts; i++)
                    {
                        try
                        {
                            return runnable.Invoke(input);
                        }
                        catch (Exception ex)
                        {
                            lastException = ex;
                            if (i < maxAttempts - 1)
                            {
                                System.Threading.Thread.Sleep(retryDelay);
                            }
                        }
                    }
                    throw lastException;
                },
                async input =>
                {
                    Exception lastException = null;
                    for (int i = 0; i < maxAttempts; i++)
                    {
                        try
                        {
                            return await runnable.InvokeAsync(input);
                        }
                        catch (Exception ex)
                        {
                            lastException = ex;
                            if (i < maxAttempts - 1)
                            {
                                await Task.Delay(retryDelay);
                            }
                        }
                    }
                    throw lastException;
                }
            );
        }

        // ==================== WithRetry for 2-16 parameters ====================

        public static Runnable<T1, T2, TOutput> WithRetry<T1, T2, TOutput>(
            this IRunnable<T1, T2, TOutput> runnable, int maxAttempts = 3, TimeSpan? delay = null)
        {
            var retryDelay = delay ?? TimeSpan.FromMilliseconds(100);
            return new Runnable<T1, T2, TOutput>(
                (a1, a2) => { Exception lastEx = null; for (int i = 0; i < maxAttempts; i++) { try { return runnable.Invoke(a1, a2); } catch (Exception ex) { lastEx = ex; if (i < maxAttempts - 1) System.Threading.Thread.Sleep(retryDelay); } } throw lastEx; },
                async (a1, a2) => { Exception lastEx = null; for (int i = 0; i < maxAttempts; i++) { try { return await runnable.InvokeAsync(a1, a2); } catch (Exception ex) { lastEx = ex; if (i < maxAttempts - 1) await Task.Delay(retryDelay); } } throw lastEx; });
        }

        public static Runnable<T1, T2, T3, TOutput> WithRetry<T1, T2, T3, TOutput>(
            this IRunnable<T1, T2, T3, TOutput> runnable, int maxAttempts = 3, TimeSpan? delay = null)
        {
            var retryDelay = delay ?? TimeSpan.FromMilliseconds(100);
            return new Runnable<T1, T2, T3, TOutput>(
                (a1, a2, a3) => { Exception lastEx = null; for (int i = 0; i < maxAttempts; i++) { try { return runnable.Invoke(a1, a2, a3); } catch (Exception ex) { lastEx = ex; if (i < maxAttempts - 1) System.Threading.Thread.Sleep(retryDelay); } } throw lastEx; },
                async (a1, a2, a3) => { Exception lastEx = null; for (int i = 0; i < maxAttempts; i++) { try { return await runnable.InvokeAsync(a1, a2, a3); } catch (Exception ex) { lastEx = ex; if (i < maxAttempts - 1) await Task.Delay(retryDelay); } } throw lastEx; });
        }

        public static Runnable<T1, T2, T3, T4, TOutput> WithRetry<T1, T2, T3, T4, TOutput>(
            this IRunnable<T1, T2, T3, T4, TOutput> runnable, int maxAttempts = 3, TimeSpan? delay = null)
        {
            var retryDelay = delay ?? TimeSpan.FromMilliseconds(100);
            return new Runnable<T1, T2, T3, T4, TOutput>(
                (a1, a2, a3, a4) => { Exception lastEx = null; for (int i = 0; i < maxAttempts; i++) { try { return runnable.Invoke(a1, a2, a3, a4); } catch (Exception ex) { lastEx = ex; if (i < maxAttempts - 1) System.Threading.Thread.Sleep(retryDelay); } } throw lastEx; },
                async (a1, a2, a3, a4) => { Exception lastEx = null; for (int i = 0; i < maxAttempts; i++) { try { return await runnable.InvokeAsync(a1, a2, a3, a4); } catch (Exception ex) { lastEx = ex; if (i < maxAttempts - 1) await Task.Delay(retryDelay); } } throw lastEx; });
        }

        public static Runnable<T1, T2, T3, T4, T5, TOutput> WithRetry<T1, T2, T3, T4, T5, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, TOutput> runnable, int maxAttempts = 3, TimeSpan? delay = null)
        {
            var retryDelay = delay ?? TimeSpan.FromMilliseconds(100);
            return new Runnable<T1, T2, T3, T4, T5, TOutput>(
                (a1, a2, a3, a4, a5) => { Exception lastEx = null; for (int i = 0; i < maxAttempts; i++) { try { return runnable.Invoke(a1, a2, a3, a4, a5); } catch (Exception ex) { lastEx = ex; if (i < maxAttempts - 1) System.Threading.Thread.Sleep(retryDelay); } } throw lastEx; },
                async (a1, a2, a3, a4, a5) => { Exception lastEx = null; for (int i = 0; i < maxAttempts; i++) { try { return await runnable.InvokeAsync(a1, a2, a3, a4, a5); } catch (Exception ex) { lastEx = ex; if (i < maxAttempts - 1) await Task.Delay(retryDelay); } } throw lastEx; });
        }

        public static Runnable<T1, T2, T3, T4, T5, T6, TOutput> WithRetry<T1, T2, T3, T4, T5, T6, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, TOutput> runnable, int maxAttempts = 3, TimeSpan? delay = null)
        {
            var retryDelay = delay ?? TimeSpan.FromMilliseconds(100);
            return new Runnable<T1, T2, T3, T4, T5, T6, TOutput>(
                (a1, a2, a3, a4, a5, a6) => { Exception lastEx = null; for (int i = 0; i < maxAttempts; i++) { try { return runnable.Invoke(a1, a2, a3, a4, a5, a6); } catch (Exception ex) { lastEx = ex; if (i < maxAttempts - 1) System.Threading.Thread.Sleep(retryDelay); } } throw lastEx; },
                async (a1, a2, a3, a4, a5, a6) => { Exception lastEx = null; for (int i = 0; i < maxAttempts; i++) { try { return await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6); } catch (Exception ex) { lastEx = ex; if (i < maxAttempts - 1) await Task.Delay(retryDelay); } } throw lastEx; });
        }

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, TOutput> WithRetry<T1, T2, T3, T4, T5, T6, T7, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, TOutput> runnable, int maxAttempts = 3, TimeSpan? delay = null)
        {
            var retryDelay = delay ?? TimeSpan.FromMilliseconds(100);
            return new Runnable<T1, T2, T3, T4, T5, T6, T7, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7) => { Exception lastEx = null; for (int i = 0; i < maxAttempts; i++) { try { return runnable.Invoke(a1, a2, a3, a4, a5, a6, a7); } catch (Exception ex) { lastEx = ex; if (i < maxAttempts - 1) System.Threading.Thread.Sleep(retryDelay); } } throw lastEx; },
                async (a1, a2, a3, a4, a5, a6, a7) => { Exception lastEx = null; for (int i = 0; i < maxAttempts; i++) { try { return await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7); } catch (Exception ex) { lastEx = ex; if (i < maxAttempts - 1) await Task.Delay(retryDelay); } } throw lastEx; });
        }

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, TOutput> WithRetry<T1, T2, T3, T4, T5, T6, T7, T8, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, TOutput> runnable, int maxAttempts = 3, TimeSpan? delay = null)
        {
            var retryDelay = delay ?? TimeSpan.FromMilliseconds(100);
            return new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8) => { Exception lastEx = null; for (int i = 0; i < maxAttempts; i++) { try { return runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8); } catch (Exception ex) { lastEx = ex; if (i < maxAttempts - 1) System.Threading.Thread.Sleep(retryDelay); } } throw lastEx; },
                async (a1, a2, a3, a4, a5, a6, a7, a8) => { Exception lastEx = null; for (int i = 0; i < maxAttempts; i++) { try { return await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8); } catch (Exception ex) { lastEx = ex; if (i < maxAttempts - 1) await Task.Delay(retryDelay); } } throw lastEx; });
        }

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, TOutput> WithRetry<T1, T2, T3, T4, T5, T6, T7, T8, T9, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, TOutput> runnable, int maxAttempts = 3, TimeSpan? delay = null)
        {
            var retryDelay = delay ?? TimeSpan.FromMilliseconds(100);
            return new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9) => { Exception lastEx = null; for (int i = 0; i < maxAttempts; i++) { try { return runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9); } catch (Exception ex) { lastEx = ex; if (i < maxAttempts - 1) System.Threading.Thread.Sleep(retryDelay); } } throw lastEx; },
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9) => { Exception lastEx = null; for (int i = 0; i < maxAttempts; i++) { try { return await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9); } catch (Exception ex) { lastEx = ex; if (i < maxAttempts - 1) await Task.Delay(retryDelay); } } throw lastEx; });
        }

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOutput> WithRetry<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOutput> runnable, int maxAttempts = 3, TimeSpan? delay = null)
        {
            var retryDelay = delay ?? TimeSpan.FromMilliseconds(100);
            return new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10) => { Exception lastEx = null; for (int i = 0; i < maxAttempts; i++) { try { return runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10); } catch (Exception ex) { lastEx = ex; if (i < maxAttempts - 1) System.Threading.Thread.Sleep(retryDelay); } } throw lastEx; },
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10) => { Exception lastEx = null; for (int i = 0; i < maxAttempts; i++) { try { return await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10); } catch (Exception ex) { lastEx = ex; if (i < maxAttempts - 1) await Task.Delay(retryDelay); } } throw lastEx; });
        }

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOutput> WithRetry<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOutput> runnable, int maxAttempts = 3, TimeSpan? delay = null)
        {
            var retryDelay = delay ?? TimeSpan.FromMilliseconds(100);
            return new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11) => { Exception lastEx = null; for (int i = 0; i < maxAttempts; i++) { try { return runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11); } catch (Exception ex) { lastEx = ex; if (i < maxAttempts - 1) System.Threading.Thread.Sleep(retryDelay); } } throw lastEx; },
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11) => { Exception lastEx = null; for (int i = 0; i < maxAttempts; i++) { try { return await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11); } catch (Exception ex) { lastEx = ex; if (i < maxAttempts - 1) await Task.Delay(retryDelay); } } throw lastEx; });
        }

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOutput> WithRetry<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOutput> runnable, int maxAttempts = 3, TimeSpan? delay = null)
        {
            var retryDelay = delay ?? TimeSpan.FromMilliseconds(100);
            return new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12) => { Exception lastEx = null; for (int i = 0; i < maxAttempts; i++) { try { return runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12); } catch (Exception ex) { lastEx = ex; if (i < maxAttempts - 1) System.Threading.Thread.Sleep(retryDelay); } } throw lastEx; },
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12) => { Exception lastEx = null; for (int i = 0; i < maxAttempts; i++) { try { return await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12); } catch (Exception ex) { lastEx = ex; if (i < maxAttempts - 1) await Task.Delay(retryDelay); } } throw lastEx; });
        }

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOutput> WithRetry<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOutput> runnable, int maxAttempts = 3, TimeSpan? delay = null)
        {
            var retryDelay = delay ?? TimeSpan.FromMilliseconds(100);
            return new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13) => { Exception lastEx = null; for (int i = 0; i < maxAttempts; i++) { try { return runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13); } catch (Exception ex) { lastEx = ex; if (i < maxAttempts - 1) System.Threading.Thread.Sleep(retryDelay); } } throw lastEx; },
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13) => { Exception lastEx = null; for (int i = 0; i < maxAttempts; i++) { try { return await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13); } catch (Exception ex) { lastEx = ex; if (i < maxAttempts - 1) await Task.Delay(retryDelay); } } throw lastEx; });
        }

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOutput> WithRetry<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOutput> runnable, int maxAttempts = 3, TimeSpan? delay = null)
        {
            var retryDelay = delay ?? TimeSpan.FromMilliseconds(100);
            return new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14) => { Exception lastEx = null; for (int i = 0; i < maxAttempts; i++) { try { return runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14); } catch (Exception ex) { lastEx = ex; if (i < maxAttempts - 1) System.Threading.Thread.Sleep(retryDelay); } } throw lastEx; },
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14) => { Exception lastEx = null; for (int i = 0; i < maxAttempts; i++) { try { return await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14); } catch (Exception ex) { lastEx = ex; if (i < maxAttempts - 1) await Task.Delay(retryDelay); } } throw lastEx; });
        }

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOutput> WithRetry<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOutput> runnable, int maxAttempts = 3, TimeSpan? delay = null)
        {
            var retryDelay = delay ?? TimeSpan.FromMilliseconds(100);
            return new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15) => { Exception lastEx = null; for (int i = 0; i < maxAttempts; i++) { try { return runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15); } catch (Exception ex) { lastEx = ex; if (i < maxAttempts - 1) System.Threading.Thread.Sleep(retryDelay); } } throw lastEx; },
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15) => { Exception lastEx = null; for (int i = 0; i < maxAttempts; i++) { try { return await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15); } catch (Exception ex) { lastEx = ex; if (i < maxAttempts - 1) await Task.Delay(retryDelay); } } throw lastEx; });
        }

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TOutput> WithRetry<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TOutput> runnable, int maxAttempts = 3, TimeSpan? delay = null)
        {
            var retryDelay = delay ?? TimeSpan.FromMilliseconds(100);
            return new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16) => { Exception lastEx = null; for (int i = 0; i < maxAttempts; i++) { try { return runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16); } catch (Exception ex) { lastEx = ex; if (i < maxAttempts - 1) System.Threading.Thread.Sleep(retryDelay); } } throw lastEx; },
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16) => { Exception lastEx = null; for (int i = 0; i < maxAttempts; i++) { try { return await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16); } catch (Exception ex) { lastEx = ex; if (i < maxAttempts - 1) await Task.Delay(retryDelay); } } throw lastEx; });
        }
    }
}
