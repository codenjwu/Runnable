using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Runnable
{
    /// <summary>
    /// RunnableBranch allows conditional routing of execution based on predicates.
    /// Inspired by LangChain's RunnableBranch.
    /// </summary>
    public static class RunnableBranch
    {
        // ==================== Branch for 0 parameters ====================

        /// <summary>
        /// Create a branch that routes to different runnables based on conditions (0 params)
        /// </summary>
        public static Runnable<TOutput> Create<TOutput>(
            IRunnable<TOutput> defaultBranch,
            params (Func<bool> condition, IRunnable<TOutput> runnable)[] branches)
        {
            return new Runnable<TOutput>(
                () => {
                    foreach (var (condition, runnable) in branches)
                    {
                        if (condition())
                            return runnable.Invoke();
                    }
                    return defaultBranch.Invoke();
                },
                async () => {
                    foreach (var (condition, runnable) in branches)
                    {
                        if (condition())
                            return await runnable.InvokeAsync();
                    }
                    return await defaultBranch.InvokeAsync();
                });
        }

        // ==================== Branch for 1 parameter ====================

        /// <summary>
        /// Create a branch that routes to different runnables based on input conditions
        /// </summary>
        public static Runnable<TInput, TOutput> Create<TInput, TOutput>(
            IRunnable<TInput, TOutput> defaultBranch,
            params (Func<TInput, bool> condition, IRunnable<TInput, TOutput> runnable)[] branches)
        {
            return new Runnable<TInput, TOutput>(
                input => {
                    foreach (var (condition, runnable) in branches)
                    {
                        if (condition(input))
                            return runnable.Invoke(input);
                    }
                    return defaultBranch.Invoke(input);
                },
                async input => {
                    foreach (var (condition, runnable) in branches)
                    {
                        if (condition(input))
                            return await runnable.InvokeAsync(input);
                    }
                    return await defaultBranch.InvokeAsync(input);
                });
        }

        // ==================== Branch for 2 parameters ====================

        /// <summary>
        /// Create a branch that routes to different runnables based on input conditions (2 params)
        /// </summary>
        public static Runnable<T1, T2, TOutput> Create<T1, T2, TOutput>(
            IRunnable<T1, T2, TOutput> defaultBranch,
            params (Func<T1, T2, bool> condition, IRunnable<T1, T2, TOutput> runnable)[] branches)
        {
            return new Runnable<T1, T2, TOutput>(
                (a1, a2) => {
                    foreach (var (condition, runnable) in branches)
                    {
                        if (condition(a1, a2))
                            return runnable.Invoke(a1, a2);
                    }
                    return defaultBranch.Invoke(a1, a2);
                },
                async (a1, a2) => {
                    foreach (var (condition, runnable) in branches)
                    {
                        if (condition(a1, a2))
                            return await runnable.InvokeAsync(a1, a2);
                    }
                    return await defaultBranch.InvokeAsync(a1, a2);
                });
        }

        // ==================== Branch for 3 parameters ====================

        /// <summary>
        /// Create a branch that routes to different runnables based on input conditions (3 params)
        /// </summary>
        public static Runnable<T1, T2, T3, TOutput> Create<T1, T2, T3, TOutput>(
            IRunnable<T1, T2, T3, TOutput> defaultBranch,
            params (Func<T1, T2, T3, bool> condition, IRunnable<T1, T2, T3, TOutput> runnable)[] branches)
        {
            return new Runnable<T1, T2, T3, TOutput>(
                (a1, a2, a3) => {
                    foreach (var (condition, runnable) in branches)
                    {
                        if (condition(a1, a2, a3))
                            return runnable.Invoke(a1, a2, a3);
                    }
                    return defaultBranch.Invoke(a1, a2, a3);
                },
                async (a1, a2, a3) => {
                    foreach (var (condition, runnable) in branches)
                    {
                        if (condition(a1, a2, a3))
                            return await runnable.InvokeAsync(a1, a2, a3);
                    }
                    return await defaultBranch.InvokeAsync(a1, a2, a3);
                });
        }

        // ==================== Branch for 4 parameters ====================

        /// <summary>
        /// Create a branch that routes to different runnables based on input conditions (4 params)
        /// </summary>
        public static Runnable<T1, T2, T3, T4, TOutput> Create<T1, T2, T3, T4, TOutput>(
            IRunnable<T1, T2, T3, T4, TOutput> defaultBranch,
            params (Func<T1, T2, T3, T4, bool> condition, IRunnable<T1, T2, T3, T4, TOutput> runnable)[] branches)
        {
            return new Runnable<T1, T2, T3, T4, TOutput>(
                (a1, a2, a3, a4) => {
                    foreach (var (condition, runnable) in branches)
                    {
                        if (condition(a1, a2, a3, a4))
                            return runnable.Invoke(a1, a2, a3, a4);
                    }
                    return defaultBranch.Invoke(a1, a2, a3, a4);
                },
                async (a1, a2, a3, a4) => {
                    foreach (var (condition, runnable) in branches)
                    {
                        if (condition(a1, a2, a3, a4))
                            return await runnable.InvokeAsync(a1, a2, a3, a4);
                    }
                    return await defaultBranch.InvokeAsync(a1, a2, a3, a4);
                });
        }

        // ==================== Branch for 5 parameters ====================

        /// <summary>
        /// Create a branch that routes to different runnables based on input conditions (5 params)
        /// </summary>
        public static Runnable<T1, T2, T3, T4, T5, TOutput> Create<T1, T2, T3, T4, T5, TOutput>(
            IRunnable<T1, T2, T3, T4, T5, TOutput> defaultBranch,
            params (Func<T1, T2, T3, T4, T5, bool> condition, IRunnable<T1, T2, T3, T4, T5, TOutput> runnable)[] branches)
        {
            return new Runnable<T1, T2, T3, T4, T5, TOutput>(
                (a1, a2, a3, a4, a5) => {
                    foreach (var (condition, runnable) in branches)
                    {
                        if (condition(a1, a2, a3, a4, a5))
                            return runnable.Invoke(a1, a2, a3, a4, a5);
                    }
                    return defaultBranch.Invoke(a1, a2, a3, a4, a5);
                },
                async (a1, a2, a3, a4, a5) => {
                    foreach (var (condition, runnable) in branches)
                    {
                        if (condition(a1, a2, a3, a4, a5))
                            return await runnable.InvokeAsync(a1, a2, a3, a4, a5);
                    }
                    return await defaultBranch.InvokeAsync(a1, a2, a3, a4, a5);
                });
        }

        // ==================== Branch for 6 parameters ====================

        /// <summary>
        /// Create a branch that routes to different runnables based on input conditions (6 params)
        /// </summary>
        public static Runnable<T1, T2, T3, T4, T5, T6, TOutput> Create<T1, T2, T3, T4, T5, T6, TOutput>(
            IRunnable<T1, T2, T3, T4, T5, T6, TOutput> defaultBranch,
            params (Func<T1, T2, T3, T4, T5, T6, bool> condition, IRunnable<T1, T2, T3, T4, T5, T6, TOutput> runnable)[] branches)
        {
            return new Runnable<T1, T2, T3, T4, T5, T6, TOutput>(
                (a1, a2, a3, a4, a5, a6) => {
                    foreach (var (condition, runnable) in branches)
                    {
                        if (condition(a1, a2, a3, a4, a5, a6))
                            return runnable.Invoke(a1, a2, a3, a4, a5, a6);
                    }
                    return defaultBranch.Invoke(a1, a2, a3, a4, a5, a6);
                },
                async (a1, a2, a3, a4, a5, a6) => {
                    foreach (var (condition, runnable) in branches)
                    {
                        if (condition(a1, a2, a3, a4, a5, a6))
                            return await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6);
                    }
                    return await defaultBranch.InvokeAsync(a1, a2, a3, a4, a5, a6);
                });
        }

        // ==================== Branch for 7 parameters ====================

        /// <summary>
        /// Create a branch that routes to different runnables based on input conditions (7 params)
        /// </summary>
        public static Runnable<T1, T2, T3, T4, T5, T6, T7, TOutput> Create<T1, T2, T3, T4, T5, T6, T7, TOutput>(
            IRunnable<T1, T2, T3, T4, T5, T6, T7, TOutput> defaultBranch,
            params (Func<T1, T2, T3, T4, T5, T6, T7, bool> condition, IRunnable<T1, T2, T3, T4, T5, T6, T7, TOutput> runnable)[] branches)
        {
            return new Runnable<T1, T2, T3, T4, T5, T6, T7, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7) => {
                    foreach (var (condition, runnable) in branches)
                    {
                        if (condition(a1, a2, a3, a4, a5, a6, a7))
                            return runnable.Invoke(a1, a2, a3, a4, a5, a6, a7);
                    }
                    return defaultBranch.Invoke(a1, a2, a3, a4, a5, a6, a7);
                },
                async (a1, a2, a3, a4, a5, a6, a7) => {
                    foreach (var (condition, runnable) in branches)
                    {
                        if (condition(a1, a2, a3, a4, a5, a6, a7))
                            return await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7);
                    }
                    return await defaultBranch.InvokeAsync(a1, a2, a3, a4, a5, a6, a7);
                });
        }

        // ==================== Branch for 8 parameters ====================

        /// <summary>
        /// Create a branch that routes to different runnables based on input conditions (8 params)
        /// </summary>
        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, TOutput> Create<T1, T2, T3, T4, T5, T6, T7, T8, TOutput>(
            IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, TOutput> defaultBranch,
            params (Func<T1, T2, T3, T4, T5, T6, T7, T8, bool> condition, IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, TOutput> runnable)[] branches)
        {
            return new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8) => {
                    foreach (var (condition, runnable) in branches)
                    {
                        if (condition(a1, a2, a3, a4, a5, a6, a7, a8))
                            return runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8);
                    }
                    return defaultBranch.Invoke(a1, a2, a3, a4, a5, a6, a7, a8);
                },
                async (a1, a2, a3, a4, a5, a6, a7, a8) => {
                    foreach (var (condition, runnable) in branches)
                    {
                        if (condition(a1, a2, a3, a4, a5, a6, a7, a8))
                            return await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8);
                    }
                    return await defaultBranch.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8);
                });
        }

        // ==================== Branch for 9 parameters ====================

        /// <summary>
        /// Create a branch that routes to different runnables based on input conditions (9 params)
        /// </summary>
        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, TOutput> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, TOutput>(
            IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, TOutput> defaultBranch,
            params (Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, bool> condition, IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, TOutput> runnable)[] branches)
        {
            return new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9) => {
                    foreach (var (condition, runnable) in branches)
                    {
                        if (condition(a1, a2, a3, a4, a5, a6, a7, a8, a9))
                            return runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9);
                    }
                    return defaultBranch.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9);
                },
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9) => {
                    foreach (var (condition, runnable) in branches)
                    {
                        if (condition(a1, a2, a3, a4, a5, a6, a7, a8, a9))
                            return await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9);
                    }
                    return await defaultBranch.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9);
                });
        }

        // ==================== Branch for 10 parameters ====================

        /// <summary>
        /// Create a branch that routes to different runnables based on input conditions (10 params)
        /// </summary>
        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOutput> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOutput>(
            IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOutput> defaultBranch,
            params (Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, bool> condition, IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOutput> runnable)[] branches)
        {
            return new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10) => {
                    foreach (var (condition, runnable) in branches)
                    {
                        if (condition(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10))
                            return runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10);
                    }
                    return defaultBranch.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10);
                },
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10) => {
                    foreach (var (condition, runnable) in branches)
                    {
                        if (condition(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10))
                            return await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10);
                    }
                    return await defaultBranch.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10);
                });
        }

        // ==================== Branch for 11 parameters ====================

        /// <summary>
        /// Create a branch that routes to different runnables based on input conditions (11 params)
        /// </summary>
        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOutput> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOutput>(
            IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOutput> defaultBranch,
            params (Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, bool> condition, IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOutput> runnable)[] branches)
        {
            return new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11) => {
                    foreach (var (condition, runnable) in branches)
                    {
                        if (condition(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11))
                            return runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11);
                    }
                    return defaultBranch.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11);
                },
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11) => {
                    foreach (var (condition, runnable) in branches)
                    {
                        if (condition(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11))
                            return await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11);
                    }
                    return await defaultBranch.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11);
                });
        }

        // ==================== Branch for 12 parameters ====================

        /// <summary>
        /// Create a branch that routes to different runnables based on input conditions (12 params)
        /// </summary>
        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOutput> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOutput>(
            IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOutput> defaultBranch,
            params (Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, bool> condition, IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOutput> runnable)[] branches)
        {
            return new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12) => {
                    foreach (var (condition, runnable) in branches)
                    {
                        if (condition(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12))
                            return runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12);
                    }
                    return defaultBranch.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12);
                },
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12) => {
                    foreach (var (condition, runnable) in branches)
                    {
                        if (condition(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12))
                            return await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12);
                    }
                    return await defaultBranch.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12);
                });
        }

        // ==================== Branch for 13 parameters ====================

        /// <summary>
        /// Create a branch that routes to different runnables based on input conditions (13 params)
        /// </summary>
        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOutput> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOutput>(
            IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOutput> defaultBranch,
            params (Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, bool> condition, IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOutput> runnable)[] branches)
        {
            return new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13) => {
                    foreach (var (condition, runnable) in branches)
                    {
                        if (condition(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13))
                            return runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13);
                    }
                    return defaultBranch.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13);
                },
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13) => {
                    foreach (var (condition, runnable) in branches)
                    {
                        if (condition(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13))
                            return await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13);
                    }
                    return await defaultBranch.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13);
                });
        }

        // ==================== Branch for 14 parameters ====================

        /// <summary>
        /// Create a branch that routes to different runnables based on input conditions (14 params)
        /// </summary>
        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOutput> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOutput>(
            IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOutput> defaultBranch,
            params (Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, bool> condition, IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOutput> runnable)[] branches)
        {
            return new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14) => {
                    foreach (var (condition, runnable) in branches)
                    {
                        if (condition(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14))
                            return runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14);
                    }
                    return defaultBranch.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14);
                },
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14) => {
                    foreach (var (condition, runnable) in branches)
                    {
                        if (condition(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14))
                            return await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14);
                    }
                    return await defaultBranch.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14);
                });
        }

        // ==================== Branch for 15 parameters ====================

        /// <summary>
        /// Create a branch that routes to different runnables based on input conditions (15 params)
        /// </summary>
        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOutput> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOutput>(
            IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOutput> defaultBranch,
            params (Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, bool> condition, IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOutput> runnable)[] branches)
        {
            return new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15) => {
                    foreach (var (condition, runnable) in branches)
                    {
                        if (condition(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15))
                            return runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15);
                    }
                    return defaultBranch.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15);
                },
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15) => {
                    foreach (var (condition, runnable) in branches)
                    {
                        if (condition(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15))
                            return await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15);
                    }
                    return await defaultBranch.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15);
                });
        }

        // ==================== Branch for 16 parameters ====================

        /// <summary>
        /// Create a branch that routes to different runnables based on input conditions (16 params)
        /// </summary>
        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TOutput> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TOutput>(
            IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TOutput> defaultBranch,
            params (Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, bool> condition, IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TOutput> runnable)[] branches)
        {
            return new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16) => {
                    foreach (var (condition, runnable) in branches)
                    {
                        if (condition(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16))
                            return runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16);
                    }
                    return defaultBranch.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16);
                },
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16) => {
                    foreach (var (condition, runnable) in branches)
                    {
                        if (condition(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16))
                            return await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16);
                    }
                    return await defaultBranch.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16);
                });
        }

        // ==================== CreateAsync for 0 parameters ====================

        /// <summary>
        /// Create a branch with async conditions that routes to different runnables (0 params)
        /// </summary>
        public static Runnable<TOutput> CreateAsync<TOutput>(
            IRunnable<TOutput> defaultBranch,
            params (Func<Task<bool>> asyncCondition, IRunnable<TOutput> runnable)[] branches)
        {
            return new Runnable<TOutput>(
                () => {
                    foreach (var (asyncCondition, runnable) in branches)
                    {
                        if (asyncCondition().GetAwaiter().GetResult())
                            return runnable.Invoke();
                    }
                    return defaultBranch.Invoke();
                },
                async () => {
                    foreach (var (asyncCondition, runnable) in branches)
                    {
                        if (await asyncCondition().ConfigureAwait(false))
                            return await runnable.InvokeAsync();
                    }
                    return await defaultBranch.InvokeAsync();
                });
        }

        // ==================== CreateAsync for 1 parameter ====================

        /// <summary>
        /// Create a branch with async conditions that routes to different runnables (1 param)
        /// </summary>
        public static Runnable<TInput, TOutput> CreateAsync<TInput, TOutput>(
            IRunnable<TInput, TOutput> defaultBranch,
            params (Func<TInput, Task<bool>> asyncCondition, IRunnable<TInput, TOutput> runnable)[] branches)
        {
            return new Runnable<TInput, TOutput>(
                input => {
                    foreach (var (asyncCondition, runnable) in branches)
                    {
                        if (asyncCondition(input).GetAwaiter().GetResult())
                            return runnable.Invoke(input);
                    }
                    return defaultBranch.Invoke(input);
                },
                async input => {
                    foreach (var (asyncCondition, runnable) in branches)
                    {
                        if (await asyncCondition(input).ConfigureAwait(false))
                            return await runnable.InvokeAsync(input);
                    }
                    return await defaultBranch.InvokeAsync(input);
                });
        }

        // ==================== CreateAsync for 2 parameters ====================

        /// <summary>
        /// Create a branch with async conditions that routes to different runnables (2 params)
        /// </summary>
        public static Runnable<T1, T2, TOutput> CreateAsync<T1, T2, TOutput>(
            IRunnable<T1, T2, TOutput> defaultBranch,
            params (Func<T1, T2, Task<bool>> asyncCondition, IRunnable<T1, T2, TOutput> runnable)[] branches)
        {
            return new Runnable<T1, T2, TOutput>(
                (a1, a2) => {
                    foreach (var (asyncCondition, runnable) in branches)
                    {
                        if (asyncCondition(a1, a2).GetAwaiter().GetResult())
                            return runnable.Invoke(a1, a2);
                    }
                    return defaultBranch.Invoke(a1, a2);
                },
                async (a1, a2) => {
                    foreach (var (asyncCondition, runnable) in branches)
                    {
                        if (await asyncCondition(a1, a2).ConfigureAwait(false))
                            return await runnable.InvokeAsync(a1, a2);
                    }
                    return await defaultBranch.InvokeAsync(a1, a2);
                });
        }

        // ==================== CreateAsync for 3 parameters ====================

        /// <summary>
        /// Create a branch with async conditions that routes to different runnables (3 params)
        /// </summary>
        public static Runnable<T1, T2, T3, TOutput> CreateAsync<T1, T2, T3, TOutput>(
            IRunnable<T1, T2, T3, TOutput> defaultBranch,
            params (Func<T1, T2, T3, Task<bool>> asyncCondition, IRunnable<T1, T2, T3, TOutput> runnable)[] branches)
        {
            return new Runnable<T1, T2, T3, TOutput>(
                (a1, a2, a3) => {
                    foreach (var (asyncCondition, runnable) in branches)
                    {
                        if (asyncCondition(a1, a2, a3).GetAwaiter().GetResult())
                            return runnable.Invoke(a1, a2, a3);
                    }
                    return defaultBranch.Invoke(a1, a2, a3);
                },
                async (a1, a2, a3) => {
                    foreach (var (asyncCondition, runnable) in branches)
                    {
                        if (await asyncCondition(a1, a2, a3).ConfigureAwait(false))
                            return await runnable.InvokeAsync(a1, a2, a3);
                    }
                    return await defaultBranch.InvokeAsync(a1, a2, a3);
                });
        }

        // ==================== CreateAsync for 4 parameters ====================

        /// <summary>
        /// Create a branch with async conditions that routes to different runnables (4 params)
        /// </summary>
        public static Runnable<T1, T2, T3, T4, TOutput> CreateAsync<T1, T2, T3, T4, TOutput>(
            IRunnable<T1, T2, T3, T4, TOutput> defaultBranch,
            params (Func<T1, T2, T3, T4, Task<bool>> asyncCondition, IRunnable<T1, T2, T3, T4, TOutput> runnable)[] branches)
        {
            return new Runnable<T1, T2, T3, T4, TOutput>(
                (a1, a2, a3, a4) => {
                    foreach (var (asyncCondition, runnable) in branches)
                    {
                        if (asyncCondition(a1, a2, a3, a4).GetAwaiter().GetResult())
                            return runnable.Invoke(a1, a2, a3, a4);
                    }
                    return defaultBranch.Invoke(a1, a2, a3, a4);
                },
                async (a1, a2, a3, a4) => {
                    foreach (var (asyncCondition, runnable) in branches)
                    {
                        if (await asyncCondition(a1, a2, a3, a4).ConfigureAwait(false))
                            return await runnable.InvokeAsync(a1, a2, a3, a4);
                    }
                    return await defaultBranch.InvokeAsync(a1, a2, a3, a4);
                });
        }

        // ==================== CreateAsync for 5 parameters ====================

        /// <summary>
        /// Create a branch with async conditions that routes to different runnables (5 params)
        /// </summary>
        public static Runnable<T1, T2, T3, T4, T5, TOutput> CreateAsync<T1, T2, T3, T4, T5, TOutput>(
            IRunnable<T1, T2, T3, T4, T5, TOutput> defaultBranch,
            params (Func<T1, T2, T3, T4, T5, Task<bool>> asyncCondition, IRunnable<T1, T2, T3, T4, T5, TOutput> runnable)[] branches)
        {
            return new Runnable<T1, T2, T3, T4, T5, TOutput>(
                (a1, a2, a3, a4, a5) => {
                    foreach (var (asyncCondition, runnable) in branches)
                    {
                        if (asyncCondition(a1, a2, a3, a4, a5).GetAwaiter().GetResult())
                            return runnable.Invoke(a1, a2, a3, a4, a5);
                    }
                    return defaultBranch.Invoke(a1, a2, a3, a4, a5);
                },
                async (a1, a2, a3, a4, a5) => {
                    foreach (var (asyncCondition, runnable) in branches)
                    {
                        if (await asyncCondition(a1, a2, a3, a4, a5).ConfigureAwait(false))
                            return await runnable.InvokeAsync(a1, a2, a3, a4, a5);
                    }
                    return await defaultBranch.InvokeAsync(a1, a2, a3, a4, a5);
                });
        }

        // ==================== CreateAsync for 6 parameters ====================

        /// <summary>
        /// Create a branch with async conditions that routes to different runnables (6 params)
        /// </summary>
        public static Runnable<T1, T2, T3, T4, T5, T6, TOutput> CreateAsync<T1, T2, T3, T4, T5, T6, TOutput>(
            IRunnable<T1, T2, T3, T4, T5, T6, TOutput> defaultBranch,
            params (Func<T1, T2, T3, T4, T5, T6, Task<bool>> asyncCondition, IRunnable<T1, T2, T3, T4, T5, T6, TOutput> runnable)[] branches)
        {
            return new Runnable<T1, T2, T3, T4, T5, T6, TOutput>(
                (a1, a2, a3, a4, a5, a6) => {
                    foreach (var (asyncCondition, runnable) in branches)
                    {
                        if (asyncCondition(a1, a2, a3, a4, a5, a6).GetAwaiter().GetResult())
                            return runnable.Invoke(a1, a2, a3, a4, a5, a6);
                    }
                    return defaultBranch.Invoke(a1, a2, a3, a4, a5, a6);
                },
                async (a1, a2, a3, a4, a5, a6) => {
                    foreach (var (asyncCondition, runnable) in branches)
                    {
                        if (await asyncCondition(a1, a2, a3, a4, a5, a6).ConfigureAwait(false))
                            return await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6);
                    }
                    return await defaultBranch.InvokeAsync(a1, a2, a3, a4, a5, a6);
                });
        }

        // ==================== CreateAsync for 7 parameters ====================

        /// <summary>
        /// Create a branch with async conditions that routes to different runnables (7 params)
        /// </summary>
        public static Runnable<T1, T2, T3, T4, T5, T6, T7, TOutput> CreateAsync<T1, T2, T3, T4, T5, T6, T7, TOutput>(
            IRunnable<T1, T2, T3, T4, T5, T6, T7, TOutput> defaultBranch,
            params (Func<T1, T2, T3, T4, T5, T6, T7, Task<bool>> asyncCondition, IRunnable<T1, T2, T3, T4, T5, T6, T7, TOutput> runnable)[] branches)
        {
            return new Runnable<T1, T2, T3, T4, T5, T6, T7, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7) => {
                    foreach (var (asyncCondition, runnable) in branches)
                    {
                        if (asyncCondition(a1, a2, a3, a4, a5, a6, a7).GetAwaiter().GetResult())
                            return runnable.Invoke(a1, a2, a3, a4, a5, a6, a7);
                    }
                    return defaultBranch.Invoke(a1, a2, a3, a4, a5, a6, a7);
                },
                async (a1, a2, a3, a4, a5, a6, a7) => {
                    foreach (var (asyncCondition, runnable) in branches)
                    {
                        if (await asyncCondition(a1, a2, a3, a4, a5, a6, a7).ConfigureAwait(false))
                            return await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7);
                    }
                    return await defaultBranch.InvokeAsync(a1, a2, a3, a4, a5, a6, a7);
                });
        }

        // ==================== CreateAsync for 8 parameters ====================

        /// <summary>
        /// Create a branch with async conditions that routes to different runnables (8 params)
        /// </summary>
        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, TOutput> CreateAsync<T1, T2, T3, T4, T5, T6, T7, T8, TOutput>(
            IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, TOutput> defaultBranch,
            params (Func<T1, T2, T3, T4, T5, T6, T7, T8, Task<bool>> asyncCondition, IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, TOutput> runnable)[] branches)
        {
            return new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8) => {
                    foreach (var (asyncCondition, runnable) in branches)
                    {
                        if (asyncCondition(a1, a2, a3, a4, a5, a6, a7, a8).GetAwaiter().GetResult())
                            return runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8);
                    }
                    return defaultBranch.Invoke(a1, a2, a3, a4, a5, a6, a7, a8);
                },
                async (a1, a2, a3, a4, a5, a6, a7, a8) => {
                    foreach (var (asyncCondition, runnable) in branches)
                    {
                        if (await asyncCondition(a1, a2, a3, a4, a5, a6, a7, a8).ConfigureAwait(false))
                            return await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8);
                    }
                    return await defaultBranch.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8);
                });
        }

        // ==================== CreateAsync for 9 parameters ====================

        /// <summary>
        /// Create a branch with async conditions that routes to different runnables (9 params)
        /// </summary>
        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, TOutput> CreateAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, TOutput>(
            IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, TOutput> defaultBranch,
            params (Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, Task<bool>> asyncCondition, IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, TOutput> runnable)[] branches)
        {
            return new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9) => {
                    foreach (var (asyncCondition, runnable) in branches)
                    {
                        if (asyncCondition(a1, a2, a3, a4, a5, a6, a7, a8, a9).GetAwaiter().GetResult())
                            return runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9);
                    }
                    return defaultBranch.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9);
                },
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9) => {
                    foreach (var (asyncCondition, runnable) in branches)
                    {
                        if (await asyncCondition(a1, a2, a3, a4, a5, a6, a7, a8, a9).ConfigureAwait(false))
                            return await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9);
                    }
                    return await defaultBranch.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9);
                });
        }

        // ==================== CreateAsync for 10 parameters ====================

        /// <summary>
        /// Create a branch with async conditions that routes to different runnables (10 params)
        /// </summary>
        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOutput> CreateAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOutput>(
            IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOutput> defaultBranch,
            params (Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, Task<bool>> asyncCondition, IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOutput> runnable)[] branches)
        {
            return new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10) => {
                    foreach (var (asyncCondition, runnable) in branches)
                    {
                        if (asyncCondition(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10).GetAwaiter().GetResult())
                            return runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10);
                    }
                    return defaultBranch.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10);
                },
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10) => {
                    foreach (var (asyncCondition, runnable) in branches)
                    {
                        if (await asyncCondition(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10).ConfigureAwait(false))
                            return await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10);
                    }
                    return await defaultBranch.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10);
                });
        }

        // ==================== CreateAsync for 11 parameters ====================

        /// <summary>
        /// Create a branch with async conditions that routes to different runnables (11 params)
        /// </summary>
        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOutput> CreateAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOutput>(
            IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOutput> defaultBranch,
            params (Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, Task<bool>> asyncCondition, IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOutput> runnable)[] branches)
        {
            return new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11) => {
                    foreach (var (asyncCondition, runnable) in branches)
                    {
                        if (asyncCondition(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11).GetAwaiter().GetResult())
                            return runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11);
                    }
                    return defaultBranch.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11);
                },
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11) => {
                    foreach (var (asyncCondition, runnable) in branches)
                    {
                        if (await asyncCondition(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11).ConfigureAwait(false))
                            return await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11);
                    }
                    return await defaultBranch.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11);
                });
        }

        // ==================== CreateAsync for 12 parameters ====================

        /// <summary>
        /// Create a branch with async conditions that routes to different runnables (12 params)
        /// </summary>
        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOutput> CreateAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOutput>(
            IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOutput> defaultBranch,
            params (Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, Task<bool>> asyncCondition, IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOutput> runnable)[] branches)
        {
            return new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12) => {
                    foreach (var (asyncCondition, runnable) in branches)
                    {
                        if (asyncCondition(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12).GetAwaiter().GetResult())
                            return runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12);
                    }
                    return defaultBranch.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12);
                },
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12) => {
                    foreach (var (asyncCondition, runnable) in branches)
                    {
                        if (await asyncCondition(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12).ConfigureAwait(false))
                            return await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12);
                    }
                    return await defaultBranch.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12);
                });
        }

        // ==================== CreateAsync for 13 parameters ====================

        /// <summary>
        /// Create a branch with async conditions that routes to different runnables (13 params)
        /// </summary>
        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOutput> CreateAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOutput>(
            IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOutput> defaultBranch,
            params (Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, Task<bool>> asyncCondition, IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOutput> runnable)[] branches)
        {
            return new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13) => {
                    foreach (var (asyncCondition, runnable) in branches)
                    {
                        if (asyncCondition(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13).GetAwaiter().GetResult())
                            return runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13);
                    }
                    return defaultBranch.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13);
                },
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13) => {
                    foreach (var (asyncCondition, runnable) in branches)
                    {
                        if (await asyncCondition(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13).ConfigureAwait(false))
                            return await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13);
                    }
                    return await defaultBranch.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13);
                });
        }

        // ==================== CreateAsync for 14 parameters ====================

        /// <summary>
        /// Create a branch with async conditions that routes to different runnables (14 params)
        /// </summary>
        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOutput> CreateAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOutput>(
            IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOutput> defaultBranch,
            params (Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, Task<bool>> asyncCondition, IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOutput> runnable)[] branches)
        {
            return new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14) => {
                    foreach (var (asyncCondition, runnable) in branches)
                    {
                        if (asyncCondition(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14).GetAwaiter().GetResult())
                            return runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14);
                    }
                    return defaultBranch.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14);
                },
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14) => {
                    foreach (var (asyncCondition, runnable) in branches)
                    {
                        if (await asyncCondition(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14).ConfigureAwait(false))
                            return await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14);
                    }
                    return await defaultBranch.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14);
                });
        }

        // ==================== CreateAsync for 15 parameters ====================

        /// <summary>
        /// Create a branch with async conditions that routes to different runnables (15 params)
        /// </summary>
        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOutput> CreateAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOutput>(
            IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOutput> defaultBranch,
            params (Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, Task<bool>> asyncCondition, IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOutput> runnable)[] branches)
        {
            return new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15) => {
                    foreach (var (asyncCondition, runnable) in branches)
                    {
                        if (asyncCondition(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15).GetAwaiter().GetResult())
                            return runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15);
                    }
                    return defaultBranch.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15);
                },
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15) => {
                    foreach (var (asyncCondition, runnable) in branches)
                    {
                        if (await asyncCondition(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15).ConfigureAwait(false))
                            return await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15);
                    }
                    return await defaultBranch.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15);
                });
        }

        // ==================== CreateAsync for 16 parameters ====================

        /// <summary>
        /// Create a branch with async conditions that routes to different runnables (16 params)
        /// </summary>
        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TOutput> CreateAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TOutput>(
            IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TOutput> defaultBranch,
            params (Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, Task<bool>> asyncCondition, IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TOutput> runnable)[] branches)
        {
            return new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16) => {
                    foreach (var (asyncCondition, runnable) in branches)
                    {
                        if (asyncCondition(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16).GetAwaiter().GetResult())
                            return runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16);
                    }
                    return defaultBranch.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16);
                },
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16) => {
                    foreach (var (asyncCondition, runnable) in branches)
                    {
                        if (await asyncCondition(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16).ConfigureAwait(false))
                            return await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16);
                    }
                    return await defaultBranch.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16);
                });
        }
    }
}

