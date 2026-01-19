using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Runnable
{
    /// <summary>
    /// RunnableMap executes multiple runnables in parallel on the same input,
    /// returning a dictionary of results. Inspired by LangChain's RunnableMap.
    /// </summary>
    public static class RunnableMap
    {
        // ==================== Map for 0 parameters ====================

        /// <summary>
        /// Execute multiple runnables in parallel and return a dictionary of results (0 params)
        /// </summary>
        public static Runnable<Dictionary<string, TOutput>> Create<TOutput>(
            params (string key, IRunnable<TOutput> runnable)[] runnables)
        {
            if (runnables == null || runnables.Length == 0)
                throw new ArgumentException("At least one runnable is required", nameof(runnables));

            return new Runnable<Dictionary<string, TOutput>>(
                () => {
                    var results = new Dictionary<string, TOutput>();
                    foreach (var (key, runnable) in runnables)
                    {
                        results[key] = runnable.Invoke();
                    }
                    return results;
                },
                async () => {
                    var tasks = runnables.Select(async r => {
                        var result = await r.runnable.InvokeAsync();
                        return (r.key, result);
                    });
                    var results = await Task.WhenAll(tasks);
                    return results.ToDictionary(r => r.key, r => r.result);
                });
        }

        // ==================== Map for 1 parameter ====================

        /// <summary>
        /// Execute multiple runnables in parallel on the same input and return a dictionary of results
        /// </summary>
        public static Runnable<TInput, Dictionary<string, TOutput>> Create<TInput, TOutput>(
            params (string key, IRunnable<TInput, TOutput> runnable)[] runnables)
        {
            if (runnables == null || runnables.Length == 0)
                throw new ArgumentException("At least one runnable is required", nameof(runnables));

            return new Runnable<TInput, Dictionary<string, TOutput>>(
                input => {
                    var results = new Dictionary<string, TOutput>();
                    foreach (var (key, runnable) in runnables)
                    {
                        results[key] = runnable.Invoke(input);
                    }
                    return results;
                },
                async input => {
                    var tasks = runnables.Select(async r => {
                        var result = await r.runnable.InvokeAsync(input);
                        return (r.key, result);
                    });
                    var results = await Task.WhenAll(tasks);
                    return results.ToDictionary(r => r.key, r => r.result);
                });
        }

        // ==================== Map for 2 parameters ====================

        /// <summary>
        /// Execute multiple runnables in parallel on the same input and return a dictionary of results (2 params)
        /// </summary>
        public static Runnable<T1, T2, Dictionary<string, TOutput>> Create<T1, T2, TOutput>(
            params (string key, IRunnable<T1, T2, TOutput> runnable)[] runnables)
        {
            if (runnables == null || runnables.Length == 0)
                throw new ArgumentException("At least one runnable is required", nameof(runnables));

            return new Runnable<T1, T2, Dictionary<string, TOutput>>(
                (a1, a2) => {
                    var results = new Dictionary<string, TOutput>();
                    foreach (var (key, runnable) in runnables)
                    {
                        results[key] = runnable.Invoke(a1, a2);
                    }
                    return results;
                },
                async (a1, a2) => {
                    var tasks = runnables.Select(async r => {
                        var result = await r.runnable.InvokeAsync(a1, a2);
                        return (r.key, result);
                    });
                    var results = await Task.WhenAll(tasks);
                    return results.ToDictionary(r => r.key, r => r.result);
                });
        }

        // ==================== Map for 3 parameters ====================

        /// <summary>
        /// Execute multiple runnables in parallel on the same input and return a dictionary of results (3 params)
        /// </summary>
        public static Runnable<T1, T2, T3, Dictionary<string, TOutput>> Create<T1, T2, T3, TOutput>(
            params (string key, IRunnable<T1, T2, T3, TOutput> runnable)[] runnables)
        {
            if (runnables == null || runnables.Length == 0)
                throw new ArgumentException("At least one runnable is required", nameof(runnables));

            return new Runnable<T1, T2, T3, Dictionary<string, TOutput>>(
                (a1, a2, a3) => {
                    var results = new Dictionary<string, TOutput>();
                    foreach (var (key, runnable) in runnables)
                    {
                        results[key] = runnable.Invoke(a1, a2, a3);
                    }
                    return results;
                },
                async (a1, a2, a3) => {
                    var tasks = runnables.Select(async r => {
                        var result = await r.runnable.InvokeAsync(a1, a2, a3);
                        return (r.key, result);
                    });
                    var results = await Task.WhenAll(tasks);
                    return results.ToDictionary(r => r.key, r => r.result);
                });
        }

        // ==================== Map for 4 parameters ====================

        /// <summary>
        /// Execute multiple runnables in parallel on the same input and return a dictionary of results (4 params)
        /// </summary>
        public static Runnable<T1, T2, T3, T4, Dictionary<string, TOutput>> Create<T1, T2, T3, T4, TOutput>(
            params (string key, IRunnable<T1, T2, T3, T4, TOutput> runnable)[] runnables)
        {
            if (runnables == null || runnables.Length == 0)
                throw new ArgumentException("At least one runnable is required", nameof(runnables));

            return new Runnable<T1, T2, T3, T4, Dictionary<string, TOutput>>(
                (a1, a2, a3, a4) => {
                    var results = new Dictionary<string, TOutput>();
                    foreach (var (key, runnable) in runnables)
                    {
                        results[key] = runnable.Invoke(a1, a2, a3, a4);
                    }
                    return results;
                },
                async (a1, a2, a3, a4) => {
                    var tasks = runnables.Select(async r => {
                        var result = await r.runnable.InvokeAsync(a1, a2, a3, a4);
                        return (r.key, result);
                    });
                    var results = await Task.WhenAll(tasks);
                    return results.ToDictionary(r => r.key, r => r.result);
                });
        }

        // ==================== Map for 5 parameters ====================

        /// <summary>
        /// Execute multiple runnables in parallel on the same input and return a dictionary of results (5 params)
        /// </summary>
        public static Runnable<T1, T2, T3, T4, T5, Dictionary<string, TOutput>> Create<T1, T2, T3, T4, T5, TOutput>(
            params (string key, IRunnable<T1, T2, T3, T4, T5, TOutput> runnable)[] runnables)
        {
            if (runnables == null || runnables.Length == 0)
                throw new ArgumentException("At least one runnable is required", nameof(runnables));

            return new Runnable<T1, T2, T3, T4, T5, Dictionary<string, TOutput>>(
                (a1, a2, a3, a4, a5) => {
                    var results = new Dictionary<string, TOutput>();
                    foreach (var (key, runnable) in runnables)
                    {
                        results[key] = runnable.Invoke(a1, a2, a3, a4, a5);
                    }
                    return results;
                },
                async (a1, a2, a3, a4, a5) => {
                    var tasks = runnables.Select(async r => {
                        var result = await r.runnable.InvokeAsync(a1, a2, a3, a4, a5);
                        return (r.key, result);
                    });
                    var results = await Task.WhenAll(tasks);
                    return results.ToDictionary(r => r.key, r => r.result);
                });
        }

        // ==================== Map for 6 parameters ====================

        /// <summary>
        /// Execute multiple runnables in parallel on the same input and return a dictionary of results (6 params)
        /// </summary>
        public static Runnable<T1, T2, T3, T4, T5, T6, Dictionary<string, TOutput>> Create<T1, T2, T3, T4, T5, T6, TOutput>(
            params (string key, IRunnable<T1, T2, T3, T4, T5, T6, TOutput> runnable)[] runnables)
        {
            if (runnables == null || runnables.Length == 0)
                throw new ArgumentException("At least one runnable is required", nameof(runnables));

            return new Runnable<T1, T2, T3, T4, T5, T6, Dictionary<string, TOutput>>(
                (a1, a2, a3, a4, a5, a6) => {
                    var results = new Dictionary<string, TOutput>();
                    foreach (var (key, runnable) in runnables)
                    {
                        results[key] = runnable.Invoke(a1, a2, a3, a4, a5, a6);
                    }
                    return results;
                },
                async (a1, a2, a3, a4, a5, a6) => {
                    var tasks = runnables.Select(async r => {
                        var result = await r.runnable.InvokeAsync(a1, a2, a3, a4, a5, a6);
                        return (r.key, result);
                    });
                    var results = await Task.WhenAll(tasks);
                    return results.ToDictionary(r => r.key, r => r.result);
                });
        }

        // ==================== Map for 7 parameters ====================

        /// <summary>
        /// Execute multiple runnables in parallel on the same input and return a dictionary of results (7 params)
        /// </summary>
        public static Runnable<T1, T2, T3, T4, T5, T6, T7, Dictionary<string, TOutput>> Create<T1, T2, T3, T4, T5, T6, T7, TOutput>(
            params (string key, IRunnable<T1, T2, T3, T4, T5, T6, T7, TOutput> runnable)[] runnables)
        {
            if (runnables == null || runnables.Length == 0)
                throw new ArgumentException("At least one runnable is required", nameof(runnables));

            return new Runnable<T1, T2, T3, T4, T5, T6, T7, Dictionary<string, TOutput>>(
                (a1, a2, a3, a4, a5, a6, a7) => {
                    var results = new Dictionary<string, TOutput>();
                    foreach (var (key, runnable) in runnables)
                    {
                        results[key] = runnable.Invoke(a1, a2, a3, a4, a5, a6, a7);
                    }
                    return results;
                },
                async (a1, a2, a3, a4, a5, a6, a7) => {
                    var tasks = runnables.Select(async r => {
                        var result = await r.runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7);
                        return (r.key, result);
                    });
                    var results = await Task.WhenAll(tasks);
                    return results.ToDictionary(r => r.key, r => r.result);
                });
        }

        // ==================== Map for 8 parameters ====================

        /// <summary>
        /// Execute multiple runnables in parallel on the same input and return a dictionary of results (8 params)
        /// </summary>
        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, Dictionary<string, TOutput>> Create<T1, T2, T3, T4, T5, T6, T7, T8, TOutput>(
            params (string key, IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, TOutput> runnable)[] runnables)
        {
            if (runnables == null || runnables.Length == 0)
                throw new ArgumentException("At least one runnable is required", nameof(runnables));

            return new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, Dictionary<string, TOutput>>(
                (a1, a2, a3, a4, a5, a6, a7, a8) => {
                    var results = new Dictionary<string, TOutput>();
                    foreach (var (key, runnable) in runnables)
                    {
                        results[key] = runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8);
                    }
                    return results;
                },
                async (a1, a2, a3, a4, a5, a6, a7, a8) => {
                    var tasks = runnables.Select(async r => {
                        var result = await r.runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8);
                        return (r.key, result);
                    });
                    var results = await Task.WhenAll(tasks);
                    return results.ToDictionary(r => r.key, r => r.result);
                });
        }

        // ==================== Map for 9 parameters ====================

        /// <summary>
        /// Execute multiple runnables in parallel on the same input and return a dictionary of results (9 params)
        /// </summary>
        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, Dictionary<string, TOutput>> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, TOutput>(
            params (string key, IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, TOutput> runnable)[] runnables)
        {
            if (runnables == null || runnables.Length == 0)
                throw new ArgumentException("At least one runnable is required", nameof(runnables));

            return new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, Dictionary<string, TOutput>>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9) => {
                    var results = new Dictionary<string, TOutput>();
                    foreach (var (key, runnable) in runnables)
                    {
                        results[key] = runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9);
                    }
                    return results;
                },
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9) => {
                    var tasks = runnables.Select(async r => {
                        var result = await r.runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9);
                        return (r.key, result);
                    });
                    var results = await Task.WhenAll(tasks);
                    return results.ToDictionary(r => r.key, r => r.result);
                });
        }

        // ==================== Map for 10 parameters ====================

        /// <summary>
        /// Execute multiple runnables in parallel on the same input and return a dictionary of results (10 params)
        /// </summary>
        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, Dictionary<string, TOutput>> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOutput>(
            params (string key, IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOutput> runnable)[] runnables)
        {
            if (runnables == null || runnables.Length == 0)
                throw new ArgumentException("At least one runnable is required", nameof(runnables));

            return new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, Dictionary<string, TOutput>>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10) => {
                    var results = new Dictionary<string, TOutput>();
                    foreach (var (key, runnable) in runnables)
                    {
                        results[key] = runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10);
                    }
                    return results;
                },
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10) => {
                    var tasks = runnables.Select(async r => {
                        var result = await r.runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10);
                        return (r.key, result);
                    });
                    var results = await Task.WhenAll(tasks);
                    return results.ToDictionary(r => r.key, r => r.result);
                });
        }

        // ==================== Map for 11 parameters ====================

        /// <summary>
        /// Execute multiple runnables in parallel on the same input and return a dictionary of results (11 params)
        /// </summary>
        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, Dictionary<string, TOutput>> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOutput>(
            params (string key, IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOutput> runnable)[] runnables)
        {
            if (runnables == null || runnables.Length == 0)
                throw new ArgumentException("At least one runnable is required", nameof(runnables));

            return new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, Dictionary<string, TOutput>>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11) => {
                    var results = new Dictionary<string, TOutput>();
                    foreach (var (key, runnable) in runnables)
                    {
                        results[key] = runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11);
                    }
                    return results;
                },
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11) => {
                    var tasks = runnables.Select(async r => {
                        var result = await r.runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11);
                        return (r.key, result);
                    });
                    var results = await Task.WhenAll(tasks);
                    return results.ToDictionary(r => r.key, r => r.result);
                });
        }

        // ==================== Map for 12 parameters ====================

        /// <summary>
        /// Execute multiple runnables in parallel on the same input and return a dictionary of results (12 params)
        /// </summary>
        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, Dictionary<string, TOutput>> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOutput>(
            params (string key, IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOutput> runnable)[] runnables)
        {
            if (runnables == null || runnables.Length == 0)
                throw new ArgumentException("At least one runnable is required", nameof(runnables));

            return new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, Dictionary<string, TOutput>>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12) => {
                    var results = new Dictionary<string, TOutput>();
                    foreach (var (key, runnable) in runnables)
                    {
                        results[key] = runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12);
                    }
                    return results;
                },
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12) => {
                    var tasks = runnables.Select(async r => {
                        var result = await r.runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12);
                        return (r.key, result);
                    });
                    var results = await Task.WhenAll(tasks);
                    return results.ToDictionary(r => r.key, r => r.result);
                });
        }

        // ==================== Map for 13 parameters ====================

        /// <summary>
        /// Execute multiple runnables in parallel on the same input and return a dictionary of results (13 params)
        /// </summary>
        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, Dictionary<string, TOutput>> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOutput>(
            params (string key, IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOutput> runnable)[] runnables)
        {
            if (runnables == null || runnables.Length == 0)
                throw new ArgumentException("At least one runnable is required", nameof(runnables));

            return new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, Dictionary<string, TOutput>>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13) => {
                    var results = new Dictionary<string, TOutput>();
                    foreach (var (key, runnable) in runnables)
                    {
                        results[key] = runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13);
                    }
                    return results;
                },
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13) => {
                    var tasks = runnables.Select(async r => {
                        var result = await r.runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13);
                        return (r.key, result);
                    });
                    var results = await Task.WhenAll(tasks);
                    return results.ToDictionary(r => r.key, r => r.result);
                });
        }

        // ==================== Map for 14 parameters ====================

        /// <summary>
        /// Execute multiple runnables in parallel on the same input and return a dictionary of results (14 params)
        /// </summary>
        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, Dictionary<string, TOutput>> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOutput>(
            params (string key, IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOutput> runnable)[] runnables)
        {
            if (runnables == null || runnables.Length == 0)
                throw new ArgumentException("At least one runnable is required", nameof(runnables));

            return new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, Dictionary<string, TOutput>>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14) => {
                    var results = new Dictionary<string, TOutput>();
                    foreach (var (key, runnable) in runnables)
                    {
                        results[key] = runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14);
                    }
                    return results;
                },
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14) => {
                    var tasks = runnables.Select(async r => {
                        var result = await r.runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14);
                        return (r.key, result);
                    });
                    var results = await Task.WhenAll(tasks);
                    return results.ToDictionary(r => r.key, r => r.result);
                });
        }

        // ==================== Map for 15 parameters ====================

        /// <summary>
        /// Execute multiple runnables in parallel on the same input and return a dictionary of results (15 params)
        /// </summary>
        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, Dictionary<string, TOutput>> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOutput>(
            params (string key, IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOutput> runnable)[] runnables)
        {
            if (runnables == null || runnables.Length == 0)
                throw new ArgumentException("At least one runnable is required", nameof(runnables));

            return new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, Dictionary<string, TOutput>>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15) => {
                    var results = new Dictionary<string, TOutput>();
                    foreach (var (key, runnable) in runnables)
                    {
                        results[key] = runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15);
                    }
                    return results;
                },
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15) => {
                    var tasks = runnables.Select(async r => {
                        var result = await r.runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15);
                        return (r.key, result);
                    });
                    var results = await Task.WhenAll(tasks);
                    return results.ToDictionary(r => r.key, r => r.result);
                });
        }

        // ==================== Map for 16 parameters ====================

        /// <summary>
        /// Execute multiple runnables in parallel on the same input and return a dictionary of results (16 params)
        /// </summary>
        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, Dictionary<string, TOutput>> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TOutput>(
            params (string key, IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TOutput> runnable)[] runnables)
        {
            if (runnables == null || runnables.Length == 0)
                throw new ArgumentException("At least one runnable is required", nameof(runnables));

            return new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, Dictionary<string, TOutput>>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16) => {
                    var results = new Dictionary<string, TOutput>();
                    foreach (var (key, runnable) in runnables)
                    {
                        results[key] = runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16);
                    }
                    return results;
                },
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16) => {
                    var tasks = runnables.Select(async r => {
                        var result = await r.runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16);
                        return (r.key, result);
                    });
                    var results = await Task.WhenAll(tasks);
                    return results.ToDictionary(r => r.key, r => r.result);
                });
        }
    }
}
