using System;
using System.Threading.Tasks;

namespace Runnable
{
    /// <summary>
    /// Extensions for mapping with RunnableContext access for enrichment and transformation
    /// </summary>
    public static class RunnableMapContextExtensions
    {
        // ==================== MapContext (Transform with context) ====================

        /// <summary>
        /// Map with context access (no input version)
        /// </summary>
        public static Runnable<TNewOutput> MapContext<TOutput, TNewOutput>(
            this IRunnable<TOutput> runnable,
            Func<TOutput, RunnableContext, TNewOutput> mapper)
        {
            return new Runnable<TNewOutput>(
                () => mapper(runnable.Invoke(), RunnableContext.Current),
                async () => mapper(await runnable.InvokeAsync(), RunnableContext.Current)
            );
        }

        /// <summary>
        /// Map with context access (1 input version)
        /// </summary>
        public static Runnable<TInput, TNewOutput> MapContext<TInput, TOutput, TNewOutput>(
            this IRunnable<TInput, TOutput> runnable,
            Func<TOutput, RunnableContext, TNewOutput> mapper)
        {
            return new Runnable<TInput, TNewOutput>(
                input => mapper(runnable.Invoke(input), RunnableContext.Current),
                async input => mapper(await runnable.InvokeAsync(input), RunnableContext.Current)
            );
        }

        /// <summary>
        /// Map with context and input access (1 input version)
        /// </summary>
        public static Runnable<TInput, TNewOutput> MapContext<TInput, TOutput, TNewOutput>(
            this IRunnable<TInput, TOutput> runnable,
            Func<TInput, TOutput, RunnableContext, TNewOutput> mapper)
        {
            return new Runnable<TInput, TNewOutput>(
                input => mapper(input, runnable.Invoke(input), RunnableContext.Current),
                async input => mapper(input, await runnable.InvokeAsync(input), RunnableContext.Current)
            );
        }

        /// <summary>
        /// Map asynchronously with context access (no input version)
        /// </summary>
        public static Runnable<TNewOutput> MapAsyncContext<TOutput, TNewOutput>(
            this IRunnable<TOutput> runnable,
            Func<TOutput, RunnableContext, Task<TNewOutput>> mapper)
        {
            return new Runnable<TNewOutput>(
                () => mapper(runnable.Invoke(), RunnableContext.Current).GetAwaiter().GetResult(),
                async () => await mapper(await runnable.InvokeAsync(), RunnableContext.Current)
            );
        }

        /// <summary>
        /// Map asynchronously with context access (1 input version)
        /// </summary>
        public static Runnable<TInput, TNewOutput> MapAsyncContext<TInput, TOutput, TNewOutput>(
            this IRunnable<TInput, TOutput> runnable,
            Func<TOutput, RunnableContext, Task<TNewOutput>> mapper)
        {
            return new Runnable<TInput, TNewOutput>(
                input => mapper(runnable.Invoke(input), RunnableContext.Current).GetAwaiter().GetResult(),
                async input => await mapper(await runnable.InvokeAsync(input), RunnableContext.Current)
            );
        }

        /// <summary>
        /// Map asynchronously with context and input access (1 input version)
        /// </summary>
        public static Runnable<TInput, TNewOutput> MapAsyncContext<TInput, TOutput, TNewOutput>(
            this IRunnable<TInput, TOutput> runnable,
            Func<TInput, TOutput, RunnableContext, Task<TNewOutput>> mapper)
        {
            return new Runnable<TInput, TNewOutput>(
                input => mapper(input, runnable.Invoke(input), RunnableContext.Current).GetAwaiter().GetResult(),
                async input => await mapper(input, await runnable.InvokeAsync(input), RunnableContext.Current)
            );
        }

        // ==================== MapContext for 2-15 parameters ====================

        public static Runnable<T1, T2, TNewOutput> MapContext<T1, T2, TOutput, TNewOutput>(
            this IRunnable<T1, T2, TOutput> runnable,
            Func<TOutput, RunnableContext, TNewOutput> mapper) =>
            new Runnable<T1, T2, TNewOutput>(
                (a1, a2) => mapper(runnable.Invoke(a1, a2), RunnableContext.Current),
                async (a1, a2) => mapper(await runnable.InvokeAsync(a1, a2), RunnableContext.Current));

        public static Runnable<T1, T2, T3, TNewOutput> MapContext<T1, T2, T3, TOutput, TNewOutput>(
            this IRunnable<T1, T2, T3, TOutput> runnable,
            Func<TOutput, RunnableContext, TNewOutput> mapper) =>
            new Runnable<T1, T2, T3, TNewOutput>(
                (a1, a2, a3) => mapper(runnable.Invoke(a1, a2, a3), RunnableContext.Current),
                async (a1, a2, a3) => mapper(await runnable.InvokeAsync(a1, a2, a3), RunnableContext.Current));

        public static Runnable<T1, T2, T3, T4, TNewOutput> MapContext<T1, T2, T3, T4, TOutput, TNewOutput>(
            this IRunnable<T1, T2, T3, T4, TOutput> runnable,
            Func<TOutput, RunnableContext, TNewOutput> mapper) =>
            new Runnable<T1, T2, T3, T4, TNewOutput>(
                (a1, a2, a3, a4) => mapper(runnable.Invoke(a1, a2, a3, a4), RunnableContext.Current),
                async (a1, a2, a3, a4) => mapper(await runnable.InvokeAsync(a1, a2, a3, a4), RunnableContext.Current));

        public static Runnable<T1, T2, T3, T4, T5, TNewOutput> MapContext<T1, T2, T3, T4, T5, TOutput, TNewOutput>(
            this IRunnable<T1, T2, T3, T4, T5, TOutput> runnable,
            Func<TOutput, RunnableContext, TNewOutput> mapper) =>
            new Runnable<T1, T2, T3, T4, T5, TNewOutput>(
                (a1, a2, a3, a4, a5) => mapper(runnable.Invoke(a1, a2, a3, a4, a5), RunnableContext.Current),
                async (a1, a2, a3, a4, a5) => mapper(await runnable.InvokeAsync(a1, a2, a3, a4, a5), RunnableContext.Current));

        public static Runnable<T1, T2, T3, T4, T5, T6, TNewOutput> MapContext<T1, T2, T3, T4, T5, T6, TOutput, TNewOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, TOutput> runnable,
            Func<TOutput, RunnableContext, TNewOutput> mapper) =>
            new Runnable<T1, T2, T3, T4, T5, T6, TNewOutput>(
                (a1, a2, a3, a4, a5, a6) => mapper(runnable.Invoke(a1, a2, a3, a4, a5, a6), RunnableContext.Current),
                async (a1, a2, a3, a4, a5, a6) => mapper(await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6), RunnableContext.Current));

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, TNewOutput> MapContext<T1, T2, T3, T4, T5, T6, T7, TOutput, TNewOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, TOutput> runnable,
            Func<TOutput, RunnableContext, TNewOutput> mapper) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, TNewOutput>(
                (a1, a2, a3, a4, a5, a6, a7) => mapper(runnable.Invoke(a1, a2, a3, a4, a5, a6, a7), RunnableContext.Current),
                async (a1, a2, a3, a4, a5, a6, a7) => mapper(await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7), RunnableContext.Current));

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, TNewOutput> MapContext<T1, T2, T3, T4, T5, T6, T7, T8, TOutput, TNewOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, TOutput> runnable,
            Func<TOutput, RunnableContext, TNewOutput> mapper) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, TNewOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8) => mapper(runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8), RunnableContext.Current),
                async (a1, a2, a3, a4, a5, a6, a7, a8) => mapper(await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8), RunnableContext.Current));

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, TNewOutput> MapContext<T1, T2, T3, T4, T5, T6, T7, T8, T9, TOutput, TNewOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, TOutput> runnable,
            Func<TOutput, RunnableContext, TNewOutput> mapper) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, TNewOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9) => mapper(runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9), RunnableContext.Current),
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9) => mapper(await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9), RunnableContext.Current));

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TNewOutput> MapContext<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOutput, TNewOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOutput> runnable,
            Func<TOutput, RunnableContext, TNewOutput> mapper) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TNewOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10) => mapper(runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10), RunnableContext.Current),
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10) => mapper(await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10), RunnableContext.Current));

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TNewOutput> MapContext<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOutput, TNewOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOutput> runnable,
            Func<TOutput, RunnableContext, TNewOutput> mapper) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TNewOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11) => mapper(runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11), RunnableContext.Current),
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11) => mapper(await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11), RunnableContext.Current));

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TNewOutput> MapContext<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOutput, TNewOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOutput> runnable,
            Func<TOutput, RunnableContext, TNewOutput> mapper) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TNewOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12) => mapper(runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12), RunnableContext.Current),
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12) => mapper(await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12), RunnableContext.Current));

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TNewOutput> MapContext<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOutput, TNewOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOutput> runnable,
            Func<TOutput, RunnableContext, TNewOutput> mapper) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TNewOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13) => mapper(runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13), RunnableContext.Current),
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13) => mapper(await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13), RunnableContext.Current));

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TNewOutput> MapContext<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOutput, TNewOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOutput> runnable,
            Func<TOutput, RunnableContext, TNewOutput> mapper) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TNewOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14) => mapper(runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14), RunnableContext.Current),
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14) => mapper(await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14), RunnableContext.Current));

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TNewOutput> MapContext<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOutput, TNewOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOutput> runnable,
            Func<TOutput, RunnableContext, TNewOutput> mapper) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TNewOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15) => mapper(runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15), RunnableContext.Current),
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15) => mapper(await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15), RunnableContext.Current));

        // ==================== MapAsyncContext for 2-15 parameters ====================

        public static Runnable<T1, T2, TNewOutput> MapAsyncContext<T1, T2, TOutput, TNewOutput>(
            this IRunnable<T1, T2, TOutput> runnable,
            Func<TOutput, RunnableContext, Task<TNewOutput>> mapper) =>
            new Runnable<T1, T2, TNewOutput>(
                (a1, a2) => mapper(runnable.Invoke(a1, a2), RunnableContext.Current).GetAwaiter().GetResult(),
                async (a1, a2) => await mapper(await runnable.InvokeAsync(a1, a2), RunnableContext.Current));

        public static Runnable<T1, T2, T3, TNewOutput> MapAsyncContext<T1, T2, T3, TOutput, TNewOutput>(
            this IRunnable<T1, T2, T3, TOutput> runnable,
            Func<TOutput, RunnableContext, Task<TNewOutput>> mapper) =>
            new Runnable<T1, T2, T3, TNewOutput>(
                (a1, a2, a3) => mapper(runnable.Invoke(a1, a2, a3), RunnableContext.Current).GetAwaiter().GetResult(),
                async (a1, a2, a3) => await mapper(await runnable.InvokeAsync(a1, a2, a3), RunnableContext.Current));

        public static Runnable<T1, T2, T3, T4, TNewOutput> MapAsyncContext<T1, T2, T3, T4, TOutput, TNewOutput>(
            this IRunnable<T1, T2, T3, T4, TOutput> runnable,
            Func<TOutput, RunnableContext, Task<TNewOutput>> mapper) =>
            new Runnable<T1, T2, T3, T4, TNewOutput>(
                (a1, a2, a3, a4) => mapper(runnable.Invoke(a1, a2, a3, a4), RunnableContext.Current).GetAwaiter().GetResult(),
                async (a1, a2, a3, a4) => await mapper(await runnable.InvokeAsync(a1, a2, a3, a4), RunnableContext.Current));

        public static Runnable<T1, T2, T3, T4, T5, TNewOutput> MapAsyncContext<T1, T2, T3, T4, T5, TOutput, TNewOutput>(
            this IRunnable<T1, T2, T3, T4, T5, TOutput> runnable,
            Func<TOutput, RunnableContext, Task<TNewOutput>> mapper) =>
            new Runnable<T1, T2, T3, T4, T5, TNewOutput>(
                (a1, a2, a3, a4, a5) => mapper(runnable.Invoke(a1, a2, a3, a4, a5), RunnableContext.Current).GetAwaiter().GetResult(),
                async (a1, a2, a3, a4, a5) => await mapper(await runnable.InvokeAsync(a1, a2, a3, a4, a5), RunnableContext.Current));

        public static Runnable<T1, T2, T3, T4, T5, T6, TNewOutput> MapAsyncContext<T1, T2, T3, T4, T5, T6, TOutput, TNewOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, TOutput> runnable,
            Func<TOutput, RunnableContext, Task<TNewOutput>> mapper) =>
            new Runnable<T1, T2, T3, T4, T5, T6, TNewOutput>(
                (a1, a2, a3, a4, a5, a6) => mapper(runnable.Invoke(a1, a2, a3, a4, a5, a6), RunnableContext.Current).GetAwaiter().GetResult(),
                async (a1, a2, a3, a4, a5, a6) => await mapper(await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6), RunnableContext.Current));

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, TNewOutput> MapAsyncContext<T1, T2, T3, T4, T5, T6, T7, TOutput, TNewOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, TOutput> runnable,
            Func<TOutput, RunnableContext, Task<TNewOutput>> mapper) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, TNewOutput>(
                (a1, a2, a3, a4, a5, a6, a7) => mapper(runnable.Invoke(a1, a2, a3, a4, a5, a6, a7), RunnableContext.Current).GetAwaiter().GetResult(),
                async (a1, a2, a3, a4, a5, a6, a7) => await mapper(await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7), RunnableContext.Current));

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, TNewOutput> MapAsyncContext<T1, T2, T3, T4, T5, T6, T7, T8, TOutput, TNewOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, TOutput> runnable,
            Func<TOutput, RunnableContext, Task<TNewOutput>> mapper) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, TNewOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8) => mapper(runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8), RunnableContext.Current).GetAwaiter().GetResult(),
                async (a1, a2, a3, a4, a5, a6, a7, a8) => await mapper(await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8), RunnableContext.Current));

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, TNewOutput> MapAsyncContext<T1, T2, T3, T4, T5, T6, T7, T8, T9, TOutput, TNewOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, TOutput> runnable,
            Func<TOutput, RunnableContext, Task<TNewOutput>> mapper) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, TNewOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9) => mapper(runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9), RunnableContext.Current).GetAwaiter().GetResult(),
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9) => await mapper(await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9), RunnableContext.Current));

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TNewOutput> MapAsyncContext<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOutput, TNewOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOutput> runnable,
            Func<TOutput, RunnableContext, Task<TNewOutput>> mapper) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TNewOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10) => mapper(runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10), RunnableContext.Current).GetAwaiter().GetResult(),
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10) => await mapper(await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10), RunnableContext.Current));

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TNewOutput> MapAsyncContext<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOutput, TNewOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOutput> runnable,
            Func<TOutput, RunnableContext, Task<TNewOutput>> mapper) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TNewOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11) => mapper(runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11), RunnableContext.Current).GetAwaiter().GetResult(),
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11) => await mapper(await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11), RunnableContext.Current));

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TNewOutput> MapAsyncContext<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOutput, TNewOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOutput> runnable,
            Func<TOutput, RunnableContext, Task<TNewOutput>> mapper) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TNewOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12) => mapper(runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12), RunnableContext.Current).GetAwaiter().GetResult(),
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12) => await mapper(await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12), RunnableContext.Current));

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TNewOutput> MapAsyncContext<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOutput, TNewOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOutput> runnable,
            Func<TOutput, RunnableContext, Task<TNewOutput>> mapper) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TNewOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13) => mapper(runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13), RunnableContext.Current).GetAwaiter().GetResult(),
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13) => await mapper(await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13), RunnableContext.Current));

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TNewOutput> MapAsyncContext<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOutput, TNewOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOutput> runnable,
            Func<TOutput, RunnableContext, Task<TNewOutput>> mapper) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TNewOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14) => mapper(runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14), RunnableContext.Current).GetAwaiter().GetResult(),
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14) => await mapper(await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14), RunnableContext.Current));

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TNewOutput> MapAsyncContext<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOutput, TNewOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOutput> runnable,
            Func<TOutput, RunnableContext, Task<TNewOutput>> mapper) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TNewOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15) => mapper(runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15), RunnableContext.Current).GetAwaiter().GetResult(),
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15) => await mapper(await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15), RunnableContext.Current));
    }
}
