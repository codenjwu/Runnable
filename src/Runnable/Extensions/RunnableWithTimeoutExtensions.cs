using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Runnable
{
    public static class RunnableWithTimeoutExtensions
    {

        // ==================== Timeout ====================

        /// <summary>
        /// Add timeout to async execution (no input version)
        /// </summary>
        public static Runnable<TOutput> WithTimeout<TOutput>(
            this IRunnable<TOutput> runnable,
            TimeSpan timeout)
        {
            return new Runnable<TOutput>(
                () => runnable.Invoke(),
                async () =>
                {
                    var task = runnable.InvokeAsync();
                    var completedTask = await Task.WhenAny(task, Task.Delay(timeout));

                    if (completedTask == task)
                    {
                        return await task;
                    }
                    throw new TimeoutException($"Operation timed out after {timeout.TotalMilliseconds}ms");
                }
            );
        }

        /// <summary>
        /// Add timeout to async execution (1 input version)
        /// </summary>
        public static Runnable<TInput, TOutput> WithTimeout<TInput, TOutput>(
            this IRunnable<TInput, TOutput> runnable,
            TimeSpan timeout)
        {
            return new Runnable<TInput, TOutput>(
                input => runnable.Invoke(input),
                async input =>
                {
                    var task = runnable.InvokeAsync(input);
                    var completedTask = await Task.WhenAny(task, Task.Delay(timeout));

                    if (completedTask == task)
                    {
                        return await task;
                    }
                    throw new TimeoutException($"Operation timed out after {timeout.TotalMilliseconds}ms");
                }
            );
        }

        // ==================== WithTimeout for 2-16 parameters ====================

        public static Runnable<T1, T2, TOutput> WithTimeout<T1, T2, TOutput>(
            this IRunnable<T1, T2, TOutput> runnable, TimeSpan timeout) =>
            new Runnable<T1, T2, TOutput>(
                (a1, a2) => runnable.Invoke(a1, a2),
                async (a1, a2) => {
                    var task = runnable.InvokeAsync(a1, a2);
                    var completedTask = await Task.WhenAny(task, Task.Delay(timeout));
                    if (completedTask == task) return await task;
                    throw new TimeoutException($"Operation timed out after {timeout.TotalMilliseconds}ms");
                });

        public static Runnable<T1, T2, T3, TOutput> WithTimeout<T1, T2, T3, TOutput>(
            this IRunnable<T1, T2, T3, TOutput> runnable, TimeSpan timeout) =>
            new Runnable<T1, T2, T3, TOutput>(
                (a1, a2, a3) => runnable.Invoke(a1, a2, a3),
                async (a1, a2, a3) => {
                    var task = runnable.InvokeAsync(a1, a2, a3);
                    var completedTask = await Task.WhenAny(task, Task.Delay(timeout));
                    if (completedTask == task) return await task;
                    throw new TimeoutException($"Operation timed out after {timeout.TotalMilliseconds}ms");
                });

        public static Runnable<T1, T2, T3, T4, TOutput> WithTimeout<T1, T2, T3, T4, TOutput>(
            this IRunnable<T1, T2, T3, T4, TOutput> runnable, TimeSpan timeout) =>
            new Runnable<T1, T2, T3, T4, TOutput>(
                (a1, a2, a3, a4) => runnable.Invoke(a1, a2, a3, a4),
                async (a1, a2, a3, a4) => {
                    var task = runnable.InvokeAsync(a1, a2, a3, a4);
                    var completedTask = await Task.WhenAny(task, Task.Delay(timeout));
                    if (completedTask == task) return await task;
                    throw new TimeoutException($"Operation timed out after {timeout.TotalMilliseconds}ms");
                });

        public static Runnable<T1, T2, T3, T4, T5, TOutput> WithTimeout<T1, T2, T3, T4, T5, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, TOutput> runnable, TimeSpan timeout) =>
            new Runnable<T1, T2, T3, T4, T5, TOutput>(
                (a1, a2, a3, a4, a5) => runnable.Invoke(a1, a2, a3, a4, a5),
                async (a1, a2, a3, a4, a5) => {
                    var task = runnable.InvokeAsync(a1, a2, a3, a4, a5);
                    var completedTask = await Task.WhenAny(task, Task.Delay(timeout));
                    if (completedTask == task) return await task;
                    throw new TimeoutException($"Operation timed out after {timeout.TotalMilliseconds}ms");
                });

        public static Runnable<T1, T2, T3, T4, T5, T6, TOutput> WithTimeout<T1, T2, T3, T4, T5, T6, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, TOutput> runnable, TimeSpan timeout) =>
            new Runnable<T1, T2, T3, T4, T5, T6, TOutput>(
                (a1, a2, a3, a4, a5, a6) => runnable.Invoke(a1, a2, a3, a4, a5, a6),
                async (a1, a2, a3, a4, a5, a6) => {
                    var task = runnable.InvokeAsync(a1, a2, a3, a4, a5, a6);
                    var completedTask = await Task.WhenAny(task, Task.Delay(timeout));
                    if (completedTask == task) return await task;
                    throw new TimeoutException($"Operation timed out after {timeout.TotalMilliseconds}ms");
                });

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, TOutput> WithTimeout<T1, T2, T3, T4, T5, T6, T7, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, TOutput> runnable, TimeSpan timeout) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7) => runnable.Invoke(a1, a2, a3, a4, a5, a6, a7),
                async (a1, a2, a3, a4, a5, a6, a7) => {
                    var task = runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7);
                    var completedTask = await Task.WhenAny(task, Task.Delay(timeout));
                    if (completedTask == task) return await task;
                    throw new TimeoutException($"Operation timed out after {timeout.TotalMilliseconds}ms");
                });

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, TOutput> WithTimeout<T1, T2, T3, T4, T5, T6, T7, T8, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, TOutput> runnable, TimeSpan timeout) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8) => runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8),
                async (a1, a2, a3, a4, a5, a6, a7, a8) => {
                    var task = runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8);
                    var completedTask = await Task.WhenAny(task, Task.Delay(timeout));
                    if (completedTask == task) return await task;
                    throw new TimeoutException($"Operation timed out after {timeout.TotalMilliseconds}ms");
                });

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, TOutput> WithTimeout<T1, T2, T3, T4, T5, T6, T7, T8, T9, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, TOutput> runnable, TimeSpan timeout) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9) => runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9),
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9) => {
                    var task = runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9);
                    var completedTask = await Task.WhenAny(task, Task.Delay(timeout));
                    if (completedTask == task) return await task;
                    throw new TimeoutException($"Operation timed out after {timeout.TotalMilliseconds}ms");
                });

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOutput> WithTimeout<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOutput> runnable, TimeSpan timeout) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10) => runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10),
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10) => {
                    var task = runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10);
                    var completedTask = await Task.WhenAny(task, Task.Delay(timeout));
                    if (completedTask == task) return await task;
                    throw new TimeoutException($"Operation timed out after {timeout.TotalMilliseconds}ms");
                });

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOutput> WithTimeout<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOutput> runnable, TimeSpan timeout) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11) => runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11),
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11) => {
                    var task = runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11);
                    var completedTask = await Task.WhenAny(task, Task.Delay(timeout));
                    if (completedTask == task) return await task;
                    throw new TimeoutException($"Operation timed out after {timeout.TotalMilliseconds}ms");
                });

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOutput> WithTimeout<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOutput> runnable, TimeSpan timeout) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12) => runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12),
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12) => {
                    var task = runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12);
                    var completedTask = await Task.WhenAny(task, Task.Delay(timeout));
                    if (completedTask == task) return await task;
                    throw new TimeoutException($"Operation timed out after {timeout.TotalMilliseconds}ms");
                });

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOutput> WithTimeout<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOutput> runnable, TimeSpan timeout) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13) => runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13),
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13) => {
                    var task = runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13);
                    var completedTask = await Task.WhenAny(task, Task.Delay(timeout));
                    if (completedTask == task) return await task;
                    throw new TimeoutException($"Operation timed out after {timeout.TotalMilliseconds}ms");
                });

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOutput> WithTimeout<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOutput> runnable, TimeSpan timeout) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14) => runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14),
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14) => {
                    var task = runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14);
                    var completedTask = await Task.WhenAny(task, Task.Delay(timeout));
                    if (completedTask == task) return await task;
                    throw new TimeoutException($"Operation timed out after {timeout.TotalMilliseconds}ms");
                });

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOutput> WithTimeout<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOutput> runnable, TimeSpan timeout) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15) => runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15),
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15) => {
                    var task = runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15);
                    var completedTask = await Task.WhenAny(task, Task.Delay(timeout));
                    if (completedTask == task) return await task;
                    throw new TimeoutException($"Operation timed out after {timeout.TotalMilliseconds}ms");
                });

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TOutput> WithTimeout<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TOutput> runnable, TimeSpan timeout) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16) => runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16),
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16) => {
                    var task = runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16);
                    var completedTask = await Task.WhenAny(task, Task.Delay(timeout));
                    if (completedTask == task) return await task;
                    throw new TimeoutException($"Operation timed out after {timeout.TotalMilliseconds}ms");
                });
    }
}
