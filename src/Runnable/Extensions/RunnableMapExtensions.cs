using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Runnable
{
    public static class RunnableMapExtensions
    {
        // ==================== Map (Transform Output) ====================

        /// <summary>
        /// Map/transform the output of a runnable (no input version)
        /// </summary>
        public static Runnable<TNext> Map<TOutput, TNext>(
            this IRunnable<TOutput> runnable,
            Func<TOutput, TNext> mapper)
        {
            return new Runnable<TNext>(
                () => mapper(runnable.Invoke()),
                async () => mapper(await runnable.InvokeAsync())
            );
        }

        /// <summary>
        /// Map/transform the output of a runnable (1 input version)
        /// </summary>
        public static Runnable<TInput, TNext> Map<TInput, TOutput, TNext>(
            this IRunnable<TInput, TOutput> runnable,
            Func<TOutput, TNext> mapper)
        {
            return new Runnable<TInput, TNext>(
                input => mapper(runnable.Invoke(input)),
                async input => mapper(await runnable.InvokeAsync(input))
            );
        }

        // ==================== MapAsync (Async Transform Output) ====================

        /// <summary>
        /// Async map/transform the output (0 params)
        /// </summary>
        public static Runnable<TNext> MapAsync<TOutput, TNext>(
            this IRunnable<TOutput> runnable,
            Func<TOutput, Task<TNext>> asyncMapper)
        {
            return new Runnable<TNext>(
                () => asyncMapper(runnable.Invoke()).GetAwaiter().GetResult(),
                async () => await asyncMapper(await runnable.InvokeAsync())
            );
        }

        /// <summary>
        /// Async map/transform the output (1 param)
        /// </summary>
        public static Runnable<TInput, TNext> MapAsync<TInput, TOutput, TNext>(
            this IRunnable<TInput, TOutput> runnable,
            Func<TOutput, Task<TNext>> asyncMapper)
        {
            return new Runnable<TInput, TNext>(
                input => asyncMapper(runnable.Invoke(input)).GetAwaiter().GetResult(),
                async input => await asyncMapper(await runnable.InvokeAsync(input))
            );
        }

        /// <summary>
        /// Async map/transform the output (2 params)
        /// </summary>
        public static Runnable<T1, T2, TNext> MapAsync<T1, T2, TOutput, TNext>(
            this IRunnable<T1, T2, TOutput> runnable,
            Func<TOutput, Task<TNext>> asyncMapper)
        {
            return new Runnable<T1, T2, TNext>(
                (a1, a2) => asyncMapper(runnable.Invoke(a1, a2)).GetAwaiter().GetResult(),
                async (a1, a2) => await asyncMapper(await runnable.InvokeAsync(a1, a2))
            );
        }

        /// <summary>
        /// Async map/transform the output (3 params)
        /// </summary>
        public static Runnable<T1, T2, T3, TNext> MapAsync<T1, T2, T3, TOutput, TNext>(
            this IRunnable<T1, T2, T3, TOutput> runnable,
            Func<TOutput, Task<TNext>> asyncMapper)
        {
            return new Runnable<T1, T2, T3, TNext>(
                (a1, a2, a3) => asyncMapper(runnable.Invoke(a1, a2, a3)).GetAwaiter().GetResult(),
                async (a1, a2, a3) => await asyncMapper(await runnable.InvokeAsync(a1, a2, a3))
            );
        }

        /// <summary>
        /// Async map/transform the output (4 params)
        /// </summary>
        public static Runnable<T1, T2, T3, T4, TNext> MapAsync<T1, T2, T3, T4, TOutput, TNext>(
            this IRunnable<T1, T2, T3, T4, TOutput> runnable,
            Func<TOutput, Task<TNext>> asyncMapper)
        {
            return new Runnable<T1, T2, T3, T4, TNext>(
                (a1, a2, a3, a4) => asyncMapper(runnable.Invoke(a1, a2, a3, a4)).GetAwaiter().GetResult(),
                async (a1, a2, a3, a4) => await asyncMapper(await runnable.InvokeAsync(a1, a2, a3, a4))
            );
        }

        /// <summary>
        /// Async map/transform the output (5 params)
        /// </summary>
        public static Runnable<T1, T2, T3, T4, T5, TNext> MapAsync<T1, T2, T3, T4, T5, TOutput, TNext>(
            this IRunnable<T1, T2, T3, T4, T5, TOutput> runnable,
            Func<TOutput, Task<TNext>> asyncMapper)
        {
            return new Runnable<T1, T2, T3, T4, T5, TNext>(
                (a1, a2, a3, a4, a5) => asyncMapper(runnable.Invoke(a1, a2, a3, a4, a5)).GetAwaiter().GetResult(),
                async (a1, a2, a3, a4, a5) => await asyncMapper(await runnable.InvokeAsync(a1, a2, a3, a4, a5))
            );
        }

        /// <summary>
        /// Async map/transform the output (6 params)
        /// </summary>
        public static Runnable<T1, T2, T3, T4, T5, T6, TNext> MapAsync<T1, T2, T3, T4, T5, T6, TOutput, TNext>(
            this IRunnable<T1, T2, T3, T4, T5, T6, TOutput> runnable,
            Func<TOutput, Task<TNext>> asyncMapper)
        {
            return new Runnable<T1, T2, T3, T4, T5, T6, TNext>(
                (a1, a2, a3, a4, a5, a6) => asyncMapper(runnable.Invoke(a1, a2, a3, a4, a5, a6)).GetAwaiter().GetResult(),
                async (a1, a2, a3, a4, a5, a6) => await asyncMapper(await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6))
            );
        }

        /// <summary>
        /// Async map/transform the output (7 params)
        /// </summary>
        public static Runnable<T1, T2, T3, T4, T5, T6, T7, TNext> MapAsync<T1, T2, T3, T4, T5, T6, T7, TOutput, TNext>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, TOutput> runnable,
            Func<TOutput, Task<TNext>> asyncMapper)
        {
            return new Runnable<T1, T2, T3, T4, T5, T6, T7, TNext>(
                (a1, a2, a3, a4, a5, a6, a7) => asyncMapper(runnable.Invoke(a1, a2, a3, a4, a5, a6, a7)).GetAwaiter().GetResult(),
                async (a1, a2, a3, a4, a5, a6, a7) => await asyncMapper(await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7))
            );
        }

        /// <summary>
        /// Async map/transform the output (8 params)
        /// </summary>
        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, TNext> MapAsync<T1, T2, T3, T4, T5, T6, T7, T8, TOutput, TNext>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, TOutput> runnable,
            Func<TOutput, Task<TNext>> asyncMapper)
        {
            return new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, TNext>(
                (a1, a2, a3, a4, a5, a6, a7, a8) => asyncMapper(runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8)).GetAwaiter().GetResult(),
                async (a1, a2, a3, a4, a5, a6, a7, a8) => await asyncMapper(await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8))
            );
        }

        /// <summary>
        /// Async map/transform the output (9 params)
        /// </summary>
        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, TNext> MapAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, TOutput, TNext>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, TOutput> runnable,
            Func<TOutput, Task<TNext>> asyncMapper)
        {
            return new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, TNext>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9) => asyncMapper(runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9)).GetAwaiter().GetResult(),
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9) => await asyncMapper(await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9))
            );
        }

        /// <summary>
        /// Async map/transform the output (10 params)
        /// </summary>
        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TNext> MapAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOutput, TNext>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOutput> runnable,
            Func<TOutput, Task<TNext>> asyncMapper)
        {
            return new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TNext>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10) => asyncMapper(runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10)).GetAwaiter().GetResult(),
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10) => await asyncMapper(await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10))
            );
        }

        /// <summary>
        /// Async map/transform the output (11 params)
        /// </summary>
        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TNext> MapAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOutput, TNext>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOutput> runnable,
            Func<TOutput, Task<TNext>> asyncMapper)
        {
            return new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TNext>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11) => asyncMapper(runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11)).GetAwaiter().GetResult(),
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11) => await asyncMapper(await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11))
            );
        }

        /// <summary>
        /// Async map/transform the output (12 params)
        /// </summary>
        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TNext> MapAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOutput, TNext>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOutput> runnable,
            Func<TOutput, Task<TNext>> asyncMapper)
        {
            return new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TNext>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12) => asyncMapper(runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12)).GetAwaiter().GetResult(),
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12) => await asyncMapper(await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12))
            );
        }

        /// <summary>
        /// Async map/transform the output (13 params)
        /// </summary>
        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TNext> MapAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOutput, TNext>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOutput> runnable,
            Func<TOutput, Task<TNext>> asyncMapper)
        {
            return new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TNext>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13) => asyncMapper(runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13)).GetAwaiter().GetResult(),
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13) => await asyncMapper(await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13))
            );
        }

        /// <summary>
        /// Async map/transform the output (14 params)
        /// </summary>
        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TNext> MapAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOutput, TNext>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOutput> runnable,
            Func<TOutput, Task<TNext>> asyncMapper)
        {
            return new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TNext>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14) => asyncMapper(runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14)).GetAwaiter().GetResult(),
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14) => await asyncMapper(await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14))
            );
        }

        /// <summary>
        /// Async map/transform the output (15 params)
        /// </summary>
        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TNext> MapAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOutput, TNext>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOutput> runnable,
            Func<TOutput, Task<TNext>> asyncMapper)
        {
            return new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TNext>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15) => asyncMapper(runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15)).GetAwaiter().GetResult(),
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15) => await asyncMapper(await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15))
            );
        }

        /// <summary>
        /// Async map/transform the output (16 params)
        /// </summary>
        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TNext> MapAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TOutput, TNext>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TOutput> runnable,
            Func<TOutput, Task<TNext>> asyncMapper)
        {
            return new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TNext>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16) => asyncMapper(runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16)).GetAwaiter().GetResult(),
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16) => await asyncMapper(await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16))
            );
        }

        // Map for 2-16 parameters
        public static Runnable<T1, T2, TNext> Map<T1, T2, TOutput, TNext>(
            this IRunnable<T1, T2, TOutput> runnable, Func<TOutput, TNext> mapper) =>
            new Runnable<T1, T2, TNext>(
                (a1, a2) => mapper(runnable.Invoke(a1, a2)),
                async (a1, a2) => mapper(await runnable.InvokeAsync(a1, a2)));

        public static Runnable<T1, T2, T3, TNext> Map<T1, T2, T3, TOutput, TNext>(
            this IRunnable<T1, T2, T3, TOutput> runnable, Func<TOutput, TNext> mapper) =>
            new Runnable<T1, T2, T3, TNext>(
                (a1, a2, a3) => mapper(runnable.Invoke(a1, a2, a3)),
                async (a1, a2, a3) => mapper(await runnable.InvokeAsync(a1, a2, a3)));

        public static Runnable<T1, T2, T3, T4, TNext> Map<T1, T2, T3, T4, TOutput, TNext>(
            this IRunnable<T1, T2, T3, T4, TOutput> runnable, Func<TOutput, TNext> mapper) =>
            new Runnable<T1, T2, T3, T4, TNext>(
                (a1, a2, a3, a4) => mapper(runnable.Invoke(a1, a2, a3, a4)),
                async (a1, a2, a3, a4) => mapper(await runnable.InvokeAsync(a1, a2, a3, a4)));

        public static Runnable<T1, T2, T3, T4, T5, TNext> Map<T1, T2, T3, T4, T5, TOutput, TNext>(
            this IRunnable<T1, T2, T3, T4, T5, TOutput> runnable, Func<TOutput, TNext> mapper) =>
            new Runnable<T1, T2, T3, T4, T5, TNext>(
                (a1, a2, a3, a4, a5) => mapper(runnable.Invoke(a1, a2, a3, a4, a5)),
                async (a1, a2, a3, a4, a5) => mapper(await runnable.InvokeAsync(a1, a2, a3, a4, a5)));

        public static Runnable<T1, T2, T3, T4, T5, T6, TNext> Map<T1, T2, T3, T4, T5, T6, TOutput, TNext>(
            this IRunnable<T1, T2, T3, T4, T5, T6, TOutput> runnable, Func<TOutput, TNext> mapper) =>
            new Runnable<T1, T2, T3, T4, T5, T6, TNext>(
                (a1, a2, a3, a4, a5, a6) => mapper(runnable.Invoke(a1, a2, a3, a4, a5, a6)),
                async (a1, a2, a3, a4, a5, a6) => mapper(await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6)));

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, TNext> Map<T1, T2, T3, T4, T5, T6, T7, TOutput, TNext>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, TOutput> runnable, Func<TOutput, TNext> mapper) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, TNext>(
                (a1, a2, a3, a4, a5, a6, a7) => mapper(runnable.Invoke(a1, a2, a3, a4, a5, a6, a7)),
                async (a1, a2, a3, a4, a5, a6, a7) => mapper(await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7)));

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, TNext> Map<T1, T2, T3, T4, T5, T6, T7, T8, TOutput, TNext>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, TOutput> runnable, Func<TOutput, TNext> mapper) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, TNext>(
                (a1, a2, a3, a4, a5, a6, a7, a8) => mapper(runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8)),
                async (a1, a2, a3, a4, a5, a6, a7, a8) => mapper(await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8)));

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, TNext> Map<T1, T2, T3, T4, T5, T6, T7, T8, T9, TOutput, TNext>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, TOutput> runnable, Func<TOutput, TNext> mapper) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, TNext>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9) => mapper(runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9)),
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9) => mapper(await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9)));

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TNext> Map<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOutput, TNext>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOutput> runnable, Func<TOutput, TNext> mapper) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TNext>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10) => mapper(runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10)),
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10) => mapper(await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10)));

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TNext> Map<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOutput, TNext>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOutput> runnable, Func<TOutput, TNext> mapper) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TNext>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11) => mapper(runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11)),
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11) => mapper(await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11)));

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TNext> Map<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOutput, TNext>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOutput> runnable, Func<TOutput, TNext> mapper) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TNext>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12) => mapper(runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12)),
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12) => mapper(await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12)));

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TNext> Map<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOutput, TNext>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOutput> runnable, Func<TOutput, TNext> mapper) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TNext>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13) => mapper(runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13)),
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13) => mapper(await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13)));

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TNext> Map<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOutput, TNext>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOutput> runnable, Func<TOutput, TNext> mapper) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TNext>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14) => mapper(runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14)),
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14) => mapper(await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14)));

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TNext> Map<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOutput, TNext>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOutput> runnable, Func<TOutput, TNext> mapper) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TNext>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15) => mapper(runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15)),
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15) => mapper(await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15)));

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TNext> Map<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TOutput, TNext>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TOutput> runnable, Func<TOutput, TNext> mapper) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TNext>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16) => mapper(runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16)),
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16) => mapper(await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16)));

    }
}
