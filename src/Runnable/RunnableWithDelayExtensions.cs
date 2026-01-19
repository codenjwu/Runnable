using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Runnable
{
    public static class RunnableWithDelayExtensions
    {

        // ==================== Throttle/RateLimit ====================

        /// <summary>
        /// Add delay between invocations (no input version)
        /// </summary>
        public static Runnable<TOutput> WithDelay<TOutput>(
            this IRunnable<TOutput> runnable,
            TimeSpan delay)
        {
            return new Runnable<TOutput>(
                () =>
                {
                    var result = runnable.Invoke();
                    System.Threading.Thread.Sleep(delay);
                    return result;
                },
                async () =>
                {
                    var result = await runnable.InvokeAsync();
                    await Task.Delay(delay);
                    return result;
                }
            );
        }

        /// <summary>
        /// Add delay between invocations (1 input version)
        /// </summary>
        public static Runnable<TInput, TOutput> WithDelay<TInput, TOutput>(
            this IRunnable<TInput, TOutput> runnable,
            TimeSpan delay)
        {
            return new Runnable<TInput, TOutput>(
                input =>
                {
                    var result = runnable.Invoke(input);
                    System.Threading.Thread.Sleep(delay);
                    return result;
                },
                async input =>
                {
                    var result = await runnable.InvokeAsync(input);
                    await Task.Delay(delay);
                    return result;
                }
            );
        }

        // ==================== WithDelay for 2-16 parameters ====================

        public static Runnable<T1, T2, TOutput> WithDelay<T1, T2, TOutput>(
            this IRunnable<T1, T2, TOutput> runnable, TimeSpan delay) =>
            new Runnable<T1, T2, TOutput>(
                (a1, a2) => { var r = runnable.Invoke(a1, a2); System.Threading.Thread.Sleep(delay); return r; },
                async (a1, a2) => { var r = await runnable.InvokeAsync(a1, a2); await Task.Delay(delay); return r; });

        public static Runnable<T1, T2, T3, TOutput> WithDelay<T1, T2, T3, TOutput>(
            this IRunnable<T1, T2, T3, TOutput> runnable, TimeSpan delay) =>
            new Runnable<T1, T2, T3, TOutput>(
                (a1, a2, a3) => { var r = runnable.Invoke(a1, a2, a3); System.Threading.Thread.Sleep(delay); return r; },
                async (a1, a2, a3) => { var r = await runnable.InvokeAsync(a1, a2, a3); await Task.Delay(delay); return r; });

        public static Runnable<T1, T2, T3, T4, TOutput> WithDelay<T1, T2, T3, T4, TOutput>(
            this IRunnable<T1, T2, T3, T4, TOutput> runnable, TimeSpan delay) =>
            new Runnable<T1, T2, T3, T4, TOutput>(
                (a1, a2, a3, a4) => { var r = runnable.Invoke(a1, a2, a3, a4); System.Threading.Thread.Sleep(delay); return r; },
                async (a1, a2, a3, a4) => { var r = await runnable.InvokeAsync(a1, a2, a3, a4); await Task.Delay(delay); return r; });

        public static Runnable<T1, T2, T3, T4, T5, TOutput> WithDelay<T1, T2, T3, T4, T5, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, TOutput> runnable, TimeSpan delay) =>
            new Runnable<T1, T2, T3, T4, T5, TOutput>(
                (a1, a2, a3, a4, a5) => { var r = runnable.Invoke(a1, a2, a3, a4, a5); System.Threading.Thread.Sleep(delay); return r; },
                async (a1, a2, a3, a4, a5) => { var r = await runnable.InvokeAsync(a1, a2, a3, a4, a5); await Task.Delay(delay); return r; });

        public static Runnable<T1, T2, T3, T4, T5, T6, TOutput> WithDelay<T1, T2, T3, T4, T5, T6, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, TOutput> runnable, TimeSpan delay) =>
            new Runnable<T1, T2, T3, T4, T5, T6, TOutput>(
                (a1, a2, a3, a4, a5, a6) => { var r = runnable.Invoke(a1, a2, a3, a4, a5, a6); System.Threading.Thread.Sleep(delay); return r; },
                async (a1, a2, a3, a4, a5, a6) => { var r = await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6); await Task.Delay(delay); return r; });

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, TOutput> WithDelay<T1, T2, T3, T4, T5, T6, T7, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, TOutput> runnable, TimeSpan delay) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7) => { var r = runnable.Invoke(a1, a2, a3, a4, a5, a6, a7); System.Threading.Thread.Sleep(delay); return r; },
                async (a1, a2, a3, a4, a5, a6, a7) => { var r = await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7); await Task.Delay(delay); return r; });

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, TOutput> WithDelay<T1, T2, T3, T4, T5, T6, T7, T8, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, TOutput> runnable, TimeSpan delay) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8) => { var r = runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8); System.Threading.Thread.Sleep(delay); return r; },
                async (a1, a2, a3, a4, a5, a6, a7, a8) => { var r = await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8); await Task.Delay(delay); return r; });

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, TOutput> WithDelay<T1, T2, T3, T4, T5, T6, T7, T8, T9, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, TOutput> runnable, TimeSpan delay) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9) => { var r = runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9); System.Threading.Thread.Sleep(delay); return r; },
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9) => { var r = await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9); await Task.Delay(delay); return r; });

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOutput> WithDelay<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOutput> runnable, TimeSpan delay) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10) => { var r = runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10); System.Threading.Thread.Sleep(delay); return r; },
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10) => { var r = await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10); await Task.Delay(delay); return r; });

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOutput> WithDelay<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOutput> runnable, TimeSpan delay) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11) => { var r = runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11); System.Threading.Thread.Sleep(delay); return r; },
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11) => { var r = await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11); await Task.Delay(delay); return r; });

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOutput> WithDelay<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOutput> runnable, TimeSpan delay) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12) => { var r = runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12); System.Threading.Thread.Sleep(delay); return r; },
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12) => { var r = await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12); await Task.Delay(delay); return r; });

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOutput> WithDelay<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOutput> runnable, TimeSpan delay) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13) => { var r = runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13); System.Threading.Thread.Sleep(delay); return r; },
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13) => { var r = await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13); await Task.Delay(delay); return r; });

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOutput> WithDelay<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOutput> runnable, TimeSpan delay) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14) => { var r = runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14); System.Threading.Thread.Sleep(delay); return r; },
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14) => { var r = await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14); await Task.Delay(delay); return r; });

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOutput> WithDelay<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOutput> runnable, TimeSpan delay) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15) => { var r = runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15); System.Threading.Thread.Sleep(delay); return r; },
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15) => { var r = await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15); await Task.Delay(delay); return r; });

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TOutput> WithDelay<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TOutput> runnable, TimeSpan delay) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16) => { var r = runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16); System.Threading.Thread.Sleep(delay); return r; },
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16) => { var r = await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16); await Task.Delay(delay); return r; });
    }
}
