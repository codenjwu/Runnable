using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Runnable
{
    public static class RunnableTapExtensions
    {
        // ==================== Tap (Side Effects) ====================

        /// <summary>
        /// Execute a side effect without changing the output (no input version)
        /// </summary>
        public static Runnable<TOutput> Tap<TOutput>(
            this IRunnable<TOutput> runnable,
            Action<TOutput> sideEffect)
        {
            return new Runnable<TOutput>(
                () =>
                {
                    var result = runnable.Invoke();
                    sideEffect(result);
                    return result;
                },
                async () =>
                {
                    var result = await runnable.InvokeAsync();
                    sideEffect(result);
                    return result;
                }
            );
        }

        /// <summary>
        /// Execute a side effect without changing the output (1 input version)
        /// </summary>
        public static Runnable<TInput, TOutput> Tap<TInput, TOutput>(
            this IRunnable<TInput, TOutput> runnable,
            Action<TOutput> sideEffect)
        {
            return new Runnable<TInput, TOutput>(
                input =>
                {
                    var result = runnable.Invoke(input);
                    sideEffect(result);
                    return result;
                },
                async input =>
                {
                    var result = await runnable.InvokeAsync(input);
                    sideEffect(result);
                    return result;
                }
            );
        }

        // ==================== TapAsync (Async Side Effects) ====================

        /// <summary>
        /// Execute an async side effect without changing the output (0 params)
        /// </summary>
        public static Runnable<TOutput> TapAsync<TOutput>(
            this IRunnable<TOutput> runnable,
            Func<TOutput, Task> asyncSideEffect)
        {
            return new Runnable<TOutput>(
                () => {
                    var result = runnable.Invoke();
                    asyncSideEffect(result).GetAwaiter().GetResult();
                    return result;
                },
                async () =>
                {
                    var result = await runnable.InvokeAsync();
                    await asyncSideEffect(result);
                    return result;
                }
            );
        }

        /// <summary>
        /// Execute an async side effect without changing the output (1 param)
        /// </summary>
        public static Runnable<TInput, TOutput> TapAsync<TInput, TOutput>(
            this IRunnable<TInput, TOutput> runnable,
            Func<TOutput, Task> asyncSideEffect)
        {
            return new Runnable<TInput, TOutput>(
                input => {
                    var result = runnable.Invoke(input);
                    asyncSideEffect(result).GetAwaiter().GetResult();
                    return result;
                },
                async input =>
                {
                    var result = await runnable.InvokeAsync(input);
                    await asyncSideEffect(result);
                    return result;
                }
            );
        }

        /// <summary>
        /// Execute an async side effect without changing the output (2 params)
        /// </summary>
        public static Runnable<T1, T2, TOutput> TapAsync<T1, T2, TOutput>(
            this IRunnable<T1, T2, TOutput> runnable,
            Func<TOutput, Task> asyncSideEffect) =>
            new Runnable<T1, T2, TOutput>(
                (a1, a2) => { var r = runnable.Invoke(a1, a2); asyncSideEffect(r).GetAwaiter().GetResult(); return r; },
                async (a1, a2) => { var r = await runnable.InvokeAsync(a1, a2); await asyncSideEffect(r); return r; });

        /// <summary>
        /// Execute an async side effect without changing the output (3 params)
        /// </summary>
        public static Runnable<T1, T2, T3, TOutput> TapAsync<T1, T2, T3, TOutput>(
            this IRunnable<T1, T2, T3, TOutput> runnable,
            Func<TOutput, Task> asyncSideEffect) =>
            new Runnable<T1, T2, T3, TOutput>(
                (a1, a2, a3) => { var r = runnable.Invoke(a1, a2, a3); asyncSideEffect(r).GetAwaiter().GetResult(); return r; },
                async (a1, a2, a3) => { var r = await runnable.InvokeAsync(a1, a2, a3); await asyncSideEffect(r); return r; });

        /// <summary>
        /// Execute an async side effect without changing the output (4 params)
        /// </summary>
        public static Runnable<T1, T2, T3, T4, TOutput> TapAsync<T1, T2, T3, T4, TOutput>(
            this IRunnable<T1, T2, T3, T4, TOutput> runnable,
            Func<TOutput, Task> asyncSideEffect) =>
            new Runnable<T1, T2, T3, T4, TOutput>(
                (a1, a2, a3, a4) => { var r = runnable.Invoke(a1, a2, a3, a4); asyncSideEffect(r).GetAwaiter().GetResult(); return r; },
                async (a1, a2, a3, a4) => { var r = await runnable.InvokeAsync(a1, a2, a3, a4); await asyncSideEffect(r); return r; });

        /// <summary>
        /// Execute an async side effect without changing the output (5 params)
        /// </summary>
        public static Runnable<T1, T2, T3, T4, T5, TOutput> TapAsync<T1, T2, T3, T4, T5, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, TOutput> runnable,
            Func<TOutput, Task> asyncSideEffect) =>
            new Runnable<T1, T2, T3, T4, T5, TOutput>(
                (a1, a2, a3, a4, a5) => { var r = runnable.Invoke(a1, a2, a3, a4, a5); asyncSideEffect(r).GetAwaiter().GetResult(); return r; },
                async (a1, a2, a3, a4, a5) => { var r = await runnable.InvokeAsync(a1, a2, a3, a4, a5); await asyncSideEffect(r); return r; });

        /// <summary>
        /// Execute an async side effect without changing the output (6 params)
        /// </summary>
        public static Runnable<T1, T2, T3, T4, T5, T6, TOutput> TapAsync<T1, T2, T3, T4, T5, T6, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, TOutput> runnable,
            Func<TOutput, Task> asyncSideEffect) =>
            new Runnable<T1, T2, T3, T4, T5, T6, TOutput>(
                (a1, a2, a3, a4, a5, a6) => { var r = runnable.Invoke(a1, a2, a3, a4, a5, a6); asyncSideEffect(r).GetAwaiter().GetResult(); return r; },
                async (a1, a2, a3, a4, a5, a6) => { var r = await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6); await asyncSideEffect(r); return r; });

        /// <summary>
        /// Execute an async side effect without changing the output (7 params)
        /// </summary>
        public static Runnable<T1, T2, T3, T4, T5, T6, T7, TOutput> TapAsync<T1, T2, T3, T4, T5, T6, T7, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, TOutput> runnable,
            Func<TOutput, Task> asyncSideEffect) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7) => { var r = runnable.Invoke(a1, a2, a3, a4, a5, a6, a7); asyncSideEffect(r).GetAwaiter().GetResult(); return r; },
                async (a1, a2, a3, a4, a5, a6, a7) => { var r = await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7); await asyncSideEffect(r); return r; });

        /// <summary>
        /// Execute an async side effect without changing the output (8 params)
        /// </summary>
        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, TOutput> TapAsync<T1, T2, T3, T4, T5, T6, T7, T8, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, TOutput> runnable,
            Func<TOutput, Task> asyncSideEffect) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8) => { var r = runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8); asyncSideEffect(r).GetAwaiter().GetResult(); return r; },
                async (a1, a2, a3, a4, a5, a6, a7, a8) => { var r = await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8); await asyncSideEffect(r); return r; });

        /// <summary>
        /// Execute an async side effect without changing the output (9 params)
        /// </summary>
        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, TOutput> TapAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, TOutput> runnable,
            Func<TOutput, Task> asyncSideEffect) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9) => { var r = runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9); asyncSideEffect(r).GetAwaiter().GetResult(); return r; },
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9) => { var r = await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9); await asyncSideEffect(r); return r; });

        /// <summary>
        /// Execute an async side effect without changing the output (10 params)
        /// </summary>
        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOutput> TapAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOutput> runnable,
            Func<TOutput, Task> asyncSideEffect) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10) => { var r = runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10); asyncSideEffect(r).GetAwaiter().GetResult(); return r; },
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10) => { var r = await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10); await asyncSideEffect(r); return r; });

        /// <summary>
        /// Execute an async side effect without changing the output (11 params)
        /// </summary>
        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOutput> TapAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOutput> runnable,
            Func<TOutput, Task> asyncSideEffect) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11) => { var r = runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11); asyncSideEffect(r).GetAwaiter().GetResult(); return r; },
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11) => { var r = await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11); await asyncSideEffect(r); return r; });

        /// <summary>
        /// Execute an async side effect without changing the output (12 params)
        /// </summary>
        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOutput> TapAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOutput> runnable,
            Func<TOutput, Task> asyncSideEffect) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12) => { var r = runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12); asyncSideEffect(r).GetAwaiter().GetResult(); return r; },
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12) => { var r = await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12); await asyncSideEffect(r); return r; });

        /// <summary>
        /// Execute an async side effect without changing the output (13 params)
        /// </summary>
        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOutput> TapAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOutput> runnable,
            Func<TOutput, Task> asyncSideEffect) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13) => { var r = runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13); asyncSideEffect(r).GetAwaiter().GetResult(); return r; },
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13) => { var r = await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13); await asyncSideEffect(r); return r; });

        /// <summary>
        /// Execute an async side effect without changing the output (14 params)
        /// </summary>
        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOutput> TapAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOutput> runnable,
            Func<TOutput, Task> asyncSideEffect) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14) => { var r = runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14); asyncSideEffect(r).GetAwaiter().GetResult(); return r; },
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14) => { var r = await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14); await asyncSideEffect(r); return r; });

        /// <summary>
        /// Execute an async side effect without changing the output (15 params)
        /// </summary>
        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOutput> TapAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOutput> runnable,
            Func<TOutput, Task> asyncSideEffect) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15) => { var r = runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15); asyncSideEffect(r).GetAwaiter().GetResult(); return r; },
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15) => { var r = await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15); await asyncSideEffect(r); return r; });

        /// <summary>
        /// Execute an async side effect without changing the output (16 params)
        /// </summary>
        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TOutput> TapAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TOutput> runnable,
            Func<TOutput, Task> asyncSideEffect) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16) => { var r = runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16); asyncSideEffect(r).GetAwaiter().GetResult(); return r; },
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16) => { var r = await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16); await asyncSideEffect(r); return r; });

        // ==================== Tap (Sync Side Effects) ====================
        // Tap for 2-16 parameters
        public static Runnable<T1, T2, TOutput> Tap<T1, T2, TOutput>(
            this IRunnable<T1, T2, TOutput> runnable, Action<TOutput> sideEffect) =>
            new Runnable<T1, T2, TOutput>(
                (a1, a2) => { var r = runnable.Invoke(a1, a2); sideEffect(r); return r; },
                async (a1, a2) => { var r = await runnable.InvokeAsync(a1, a2); sideEffect(r); return r; });

        public static Runnable<T1, T2, T3, TOutput> Tap<T1, T2, T3, TOutput>(
            this IRunnable<T1, T2, T3, TOutput> runnable, Action<TOutput> sideEffect) =>
            new Runnable<T1, T2, T3, TOutput>(
                (a1, a2, a3) => { var r = runnable.Invoke(a1, a2, a3); sideEffect(r); return r; },
                async (a1, a2, a3) => { var r = await runnable.InvokeAsync(a1, a2, a3); sideEffect(r); return r; });

        public static Runnable<T1, T2, T3, T4, TOutput> Tap<T1, T2, T3, T4, TOutput>(
            this IRunnable<T1, T2, T3, T4, TOutput> runnable, Action<TOutput> sideEffect) =>
            new Runnable<T1, T2, T3, T4, TOutput>(
                (a1, a2, a3, a4) => { var r = runnable.Invoke(a1, a2, a3, a4); sideEffect(r); return r; },
                async (a1, a2, a3, a4) => { var r = await runnable.InvokeAsync(a1, a2, a3, a4); sideEffect(r); return r; });

        public static Runnable<T1, T2, T3, T4, T5, TOutput> Tap<T1, T2, T3, T4, T5, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, TOutput> runnable, Action<TOutput> sideEffect) =>
            new Runnable<T1, T2, T3, T4, T5, TOutput>(
                (a1, a2, a3, a4, a5) => { var r = runnable.Invoke(a1, a2, a3, a4, a5); sideEffect(r); return r; },
                async (a1, a2, a3, a4, a5) => { var r = await runnable.InvokeAsync(a1, a2, a3, a4, a5); sideEffect(r); return r; });

        public static Runnable<T1, T2, T3, T4, T5, T6, TOutput> Tap<T1, T2, T3, T4, T5, T6, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, TOutput> runnable, Action<TOutput> sideEffect) =>
            new Runnable<T1, T2, T3, T4, T5, T6, TOutput>(
                (a1, a2, a3, a4, a5, a6) => { var r = runnable.Invoke(a1, a2, a3, a4, a5, a6); sideEffect(r); return r; },
                async (a1, a2, a3, a4, a5, a6) => { var r = await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6); sideEffect(r); return r; });

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, TOutput> Tap<T1, T2, T3, T4, T5, T6, T7, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, TOutput> runnable, Action<TOutput> sideEffect) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7) => { var r = runnable.Invoke(a1, a2, a3, a4, a5, a6, a7); sideEffect(r); return r; },
                async (a1, a2, a3, a4, a5, a6, a7) => { var r = await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7); sideEffect(r); return r; });

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, TOutput> Tap<T1, T2, T3, T4, T5, T6, T7, T8, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, TOutput> runnable, Action<TOutput> sideEffect) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8) => { var r = runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8); sideEffect(r); return r; },
                async (a1, a2, a3, a4, a5, a6, a7, a8) => { var r = await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8); sideEffect(r); return r; });

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, TOutput> Tap<T1, T2, T3, T4, T5, T6, T7, T8, T9, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, TOutput> runnable, Action<TOutput> sideEffect) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9) => { var r = runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9); sideEffect(r); return r; },
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9) => { var r = await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9); sideEffect(r); return r; });

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOutput> Tap<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOutput> runnable, Action<TOutput> sideEffect) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10) => { var r = runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10); sideEffect(r); return r; },
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10) => { var r = await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10); sideEffect(r); return r; });

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOutput> Tap<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOutput> runnable, Action<TOutput> sideEffect) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11) => { var r = runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11); sideEffect(r); return r; },
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11) => { var r = await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11); sideEffect(r); return r; });

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOutput> Tap<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOutput> runnable, Action<TOutput> sideEffect) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12) => { var r = runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12); sideEffect(r); return r; },
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12) => { var r = await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12); sideEffect(r); return r; });

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOutput> Tap<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOutput> runnable, Action<TOutput> sideEffect) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13) => { var r = runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13); sideEffect(r); return r; },
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13) => { var r = await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13); sideEffect(r); return r; });

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOutput> Tap<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOutput> runnable, Action<TOutput> sideEffect) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14) => { var r = runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14); sideEffect(r); return r; },
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14) => { var r = await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14); sideEffect(r); return r; });

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOutput> Tap<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOutput> runnable, Action<TOutput> sideEffect) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15) => { var r = runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15); sideEffect(r); return r; },
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15) => { var r = await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15); sideEffect(r); return r; });

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TOutput> Tap<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TOutput> runnable, Action<TOutput> sideEffect) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16) => { var r = runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16); sideEffect(r); return r; },
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16) => { var r = await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16); sideEffect(r); return r; });

    }
}
