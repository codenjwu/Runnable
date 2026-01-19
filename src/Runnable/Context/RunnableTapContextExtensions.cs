using System;
using System.Threading.Tasks;

namespace Runnable
{
    /// <summary>
    /// Extensions for tapping into RunnableContext for observability and logging
    /// </summary>
    public static class RunnableTapContextExtensions
    {
        // ==================== TapContext (Observe context) ====================

        /// <summary>
        /// Tap into context for logging/observability (no input version)
        /// </summary>
        /// <param name="runnable">The runnable to tap</param>
        /// <param name="contextAction">Action to perform with context</param>
        /// <returns>A new runnable that taps the context</returns>
        public static Runnable<TOutput> TapContext<TOutput>(
            this IRunnable<TOutput> runnable,
            Action<RunnableContext> contextAction)
        {
            return new Runnable<TOutput>(
                () =>
                {
                    contextAction(RunnableContext.Current);
                    return runnable.Invoke();
                },
                async () =>
                {
                    contextAction(RunnableContext.Current);
                    return await runnable.InvokeAsync();
                }
            );
        }

        /// <summary>
        /// Tap into context for logging/observability (1 input version)
        /// </summary>
        /// <param name="runnable">The runnable to tap</param>
        /// <param name="contextAction">Action to perform with input and context</param>
        /// <returns>A new runnable that taps the context</returns>
        public static Runnable<TInput, TOutput> TapContext<TInput, TOutput>(
            this IRunnable<TInput, TOutput> runnable,
            Action<TInput, RunnableContext> contextAction)
        {
            return new Runnable<TInput, TOutput>(
                input =>
                {
                    contextAction(input, RunnableContext.Current);
                    return runnable.Invoke(input);
                },
                async input =>
                {
                    contextAction(input, RunnableContext.Current);
                    return await runnable.InvokeAsync(input);
                }
            );
        }

        /// <summary>
        /// Tap into context asynchronously (no input version)
        /// </summary>
        /// <param name="runnable">The runnable to tap</param>
        /// <param name="contextAction">Async action to perform with context</param>
        /// <returns>A new runnable that taps the context asynchronously</returns>
        public static Runnable<TOutput> TapContextAsync<TOutput>(
            this IRunnable<TOutput> runnable,
            Func<RunnableContext, Task> contextAction)
        {
            return new Runnable<TOutput>(
                () => runnable.Invoke(),
                async () =>
                {
                    await contextAction(RunnableContext.Current);
                    return await runnable.InvokeAsync();
                }
            );
        }

        /// <summary>
        /// Tap into context asynchronously (1 input version)
        /// </summary>
        /// <param name="runnable">The runnable to tap</param>
        /// <param name="contextAction">Async action to perform with input and context</param>
        /// <returns>A new runnable that taps the context asynchronously</returns>
        public static Runnable<TInput, TOutput> TapContextAsync<TInput, TOutput>(
            this IRunnable<TInput, TOutput> runnable,
            Func<TInput, RunnableContext, Task> contextAction)
        {
            return new Runnable<TInput, TOutput>(
                input => runnable.Invoke(input),
                async input =>
                {
                    await contextAction(input, RunnableContext.Current);
                    return await runnable.InvokeAsync(input);
                }
            );
        }

        // ==================== TapContext for 2-16 parameters ====================

        public static Runnable<T1, T2, TOutput> TapContext<T1, T2, TOutput>(
            this IRunnable<T1, T2, TOutput> runnable,
            Action<T1, T2, RunnableContext> contextAction) =>
            new Runnable<T1, T2, TOutput>(
                (a1, a2) => { contextAction(a1, a2, RunnableContext.Current); return runnable.Invoke(a1, a2); },
                async (a1, a2) => { contextAction(a1, a2, RunnableContext.Current); return await runnable.InvokeAsync(a1, a2); });

        public static Runnable<T1, T2, T3, TOutput> TapContext<T1, T2, T3, TOutput>(
            this IRunnable<T1, T2, T3, TOutput> runnable,
            Action<T1, T2, T3, RunnableContext> contextAction) =>
            new Runnable<T1, T2, T3, TOutput>(
                (a1, a2, a3) => { contextAction(a1, a2, a3, RunnableContext.Current); return runnable.Invoke(a1, a2, a3); },
                async (a1, a2, a3) => { contextAction(a1, a2, a3, RunnableContext.Current); return await runnable.InvokeAsync(a1, a2, a3); });

        public static Runnable<T1, T2, T3, T4, TOutput> TapContext<T1, T2, T3, T4, TOutput>(
            this IRunnable<T1, T2, T3, T4, TOutput> runnable,
            Action<T1, T2, T3, T4, RunnableContext> contextAction) =>
            new Runnable<T1, T2, T3, T4, TOutput>(
                (a1, a2, a3, a4) => { contextAction(a1, a2, a3, a4, RunnableContext.Current); return runnable.Invoke(a1, a2, a3, a4); },
                async (a1, a2, a3, a4) => { contextAction(a1, a2, a3, a4, RunnableContext.Current); return await runnable.InvokeAsync(a1, a2, a3, a4); });

        public static Runnable<T1, T2, T3, T4, T5, TOutput> TapContext<T1, T2, T3, T4, T5, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, TOutput> runnable,
            Action<T1, T2, T3, T4, T5, RunnableContext> contextAction) =>
            new Runnable<T1, T2, T3, T4, T5, TOutput>(
                (a1, a2, a3, a4, a5) => { contextAction(a1, a2, a3, a4, a5, RunnableContext.Current); return runnable.Invoke(a1, a2, a3, a4, a5); },
                async (a1, a2, a3, a4, a5) => { contextAction(a1, a2, a3, a4, a5, RunnableContext.Current); return await runnable.InvokeAsync(a1, a2, a3, a4, a5); });

        public static Runnable<T1, T2, T3, T4, T5, T6, TOutput> TapContext<T1, T2, T3, T4, T5, T6, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, TOutput> runnable,
            Action<T1, T2, T3, T4, T5, T6, RunnableContext> contextAction) =>
            new Runnable<T1, T2, T3, T4, T5, T6, TOutput>(
                (a1, a2, a3, a4, a5, a6) => { contextAction(a1, a2, a3, a4, a5, a6, RunnableContext.Current); return runnable.Invoke(a1, a2, a3, a4, a5, a6); },
                async (a1, a2, a3, a4, a5, a6) => { contextAction(a1, a2, a3, a4, a5, a6, RunnableContext.Current); return await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6); });

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, TOutput> TapContext<T1, T2, T3, T4, T5, T6, T7, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, TOutput> runnable,
            Action<T1, T2, T3, T4, T5, T6, T7, RunnableContext> contextAction) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7) => { contextAction(a1, a2, a3, a4, a5, a6, a7, RunnableContext.Current); return runnable.Invoke(a1, a2, a3, a4, a5, a6, a7); },
                async (a1, a2, a3, a4, a5, a6, a7) => { contextAction(a1, a2, a3, a4, a5, a6, a7, RunnableContext.Current); return await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7); });

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, TOutput> TapContext<T1, T2, T3, T4, T5, T6, T7, T8, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, TOutput> runnable,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, RunnableContext> contextAction) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8) => { contextAction(a1, a2, a3, a4, a5, a6, a7, a8, RunnableContext.Current); return runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8); },
                async (a1, a2, a3, a4, a5, a6, a7, a8) => { contextAction(a1, a2, a3, a4, a5, a6, a7, a8, RunnableContext.Current); return await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8); });

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, TOutput> TapContext<T1, T2, T3, T4, T5, T6, T7, T8, T9, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, TOutput> runnable,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, RunnableContext> contextAction) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9) => { contextAction(a1, a2, a3, a4, a5, a6, a7, a8, a9, RunnableContext.Current); return runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9); },
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9) => { contextAction(a1, a2, a3, a4, a5, a6, a7, a8, a9, RunnableContext.Current); return await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9); });

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOutput> TapContext<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOutput> runnable,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, RunnableContext> contextAction) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10) => { contextAction(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, RunnableContext.Current); return runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10); },
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10) => { contextAction(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, RunnableContext.Current); return await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10); });

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOutput> TapContext<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOutput> runnable,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, RunnableContext> contextAction) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11) => { contextAction(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, RunnableContext.Current); return runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11); },
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11) => { contextAction(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, RunnableContext.Current); return await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11); });

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOutput> TapContext<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOutput> runnable,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, RunnableContext> contextAction) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12) => { contextAction(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, RunnableContext.Current); return runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12); },
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12) => { contextAction(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, RunnableContext.Current); return await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12); });

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOutput> TapContext<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOutput> runnable,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, RunnableContext> contextAction) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13) => { contextAction(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, RunnableContext.Current); return runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13); },
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13) => { contextAction(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, RunnableContext.Current); return await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13); });

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOutput> TapContext<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOutput> runnable,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, RunnableContext> contextAction) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14) => { contextAction(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, RunnableContext.Current); return runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14); },
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14) => { contextAction(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, RunnableContext.Current); return await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14); });

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOutput> TapContext<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOutput> runnable,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, RunnableContext> contextAction) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15) => { contextAction(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, RunnableContext.Current); return runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15); },
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15) => { contextAction(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, RunnableContext.Current); return await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15); });

        // Note: 16 parameters not supported due to C# Action/Func delegate limit (max 16 type parameters total, including context)

        // ==================== TapContextAsync for 2-16 parameters ====================

        public static Runnable<T1, T2, TOutput> TapContextAsync<T1, T2, TOutput>(
            this IRunnable<T1, T2, TOutput> runnable,
            Func<T1, T2, RunnableContext, Task> contextAction) =>
            new Runnable<T1, T2, TOutput>(
                (a1, a2) => runnable.Invoke(a1, a2),
                async (a1, a2) => { await contextAction(a1, a2, RunnableContext.Current); return await runnable.InvokeAsync(a1, a2); });

        public static Runnable<T1, T2, T3, TOutput> TapContextAsync<T1, T2, T3, TOutput>(
            this IRunnable<T1, T2, T3, TOutput> runnable,
            Func<T1, T2, T3, RunnableContext, Task> contextAction) =>
            new Runnable<T1, T2, T3, TOutput>(
                (a1, a2, a3) => runnable.Invoke(a1, a2, a3),
                async (a1, a2, a3) => { await contextAction(a1, a2, a3, RunnableContext.Current); return await runnable.InvokeAsync(a1, a2, a3); });

        public static Runnable<T1, T2, T3, T4, TOutput> TapContextAsync<T1, T2, T3, T4, TOutput>(
            this IRunnable<T1, T2, T3, T4, TOutput> runnable,
            Func<T1, T2, T3, T4, RunnableContext, Task> contextAction) =>
            new Runnable<T1, T2, T3, T4, TOutput>(
                (a1, a2, a3, a4) => runnable.Invoke(a1, a2, a3, a4),
                async (a1, a2, a3, a4) => { await contextAction(a1, a2, a3, a4, RunnableContext.Current); return await runnable.InvokeAsync(a1, a2, a3, a4); });

        public static Runnable<T1, T2, T3, T4, T5, TOutput> TapContextAsync<T1, T2, T3, T4, T5, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, TOutput> runnable,
            Func<T1, T2, T3, T4, T5, RunnableContext, Task> contextAction) =>
            new Runnable<T1, T2, T3, T4, T5, TOutput>(
                (a1, a2, a3, a4, a5) => runnable.Invoke(a1, a2, a3, a4, a5),
                async (a1, a2, a3, a4, a5) => { await contextAction(a1, a2, a3, a4, a5, RunnableContext.Current); return await runnable.InvokeAsync(a1, a2, a3, a4, a5); });

        public static Runnable<T1, T2, T3, T4, T5, T6, TOutput> TapContextAsync<T1, T2, T3, T4, T5, T6, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, TOutput> runnable,
            Func<T1, T2, T3, T4, T5, T6, RunnableContext, Task> contextAction) =>
            new Runnable<T1, T2, T3, T4, T5, T6, TOutput>(
                (a1, a2, a3, a4, a5, a6) => runnable.Invoke(a1, a2, a3, a4, a5, a6),
                async (a1, a2, a3, a4, a5, a6) => { await contextAction(a1, a2, a3, a4, a5, a6, RunnableContext.Current); return await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6); });

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, TOutput> TapContextAsync<T1, T2, T3, T4, T5, T6, T7, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, TOutput> runnable,
            Func<T1, T2, T3, T4, T5, T6, T7, RunnableContext, Task> contextAction) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7) => runnable.Invoke(a1, a2, a3, a4, a5, a6, a7),
                async (a1, a2, a3, a4, a5, a6, a7) => { await contextAction(a1, a2, a3, a4, a5, a6, a7, RunnableContext.Current); return await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7); });

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, TOutput> TapContextAsync<T1, T2, T3, T4, T5, T6, T7, T8, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, TOutput> runnable,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, RunnableContext, Task> contextAction) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8) => runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8),
                async (a1, a2, a3, a4, a5, a6, a7, a8) => { await contextAction(a1, a2, a3, a4, a5, a6, a7, a8, RunnableContext.Current); return await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8); });

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, TOutput> TapContextAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, TOutput> runnable,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, RunnableContext, Task> contextAction) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9) => runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9),
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9) => { await contextAction(a1, a2, a3, a4, a5, a6, a7, a8, a9, RunnableContext.Current); return await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9); });

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOutput> TapContextAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOutput> runnable,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, RunnableContext, Task> contextAction) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10) => runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10),
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10) => { await contextAction(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, RunnableContext.Current); return await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10); });

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOutput> TapContextAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOutput> runnable,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, RunnableContext, Task> contextAction) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11) => runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11),
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11) => { await contextAction(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, RunnableContext.Current); return await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11); });

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOutput> TapContextAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOutput> runnable,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, RunnableContext, Task> contextAction) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12) => runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12),
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12) => { await contextAction(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, RunnableContext.Current); return await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12); });

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOutput> TapContextAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOutput> runnable,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, RunnableContext, Task> contextAction) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13) => runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13),
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13) => { await contextAction(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, RunnableContext.Current); return await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13); });

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOutput> TapContextAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOutput> runnable,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, RunnableContext, Task> contextAction) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14) => runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14),
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14) => { await contextAction(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, RunnableContext.Current); return await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14); });

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOutput> TapContextAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOutput> runnable,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, RunnableContext, Task> contextAction) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15) => runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15),
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15) => { await contextAction(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, RunnableContext.Current); return await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15); });

        // Note: 16 parameters not supported due to C# Action/Func delegate limit (max 16 type parameters total, including context + Task)
    }
}
