using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Runnable
{
    /// <summary>
    /// Filter extensions for conditional execution (0-16 parameters)
    /// </summary>
    public static class RunnableFilterExtensions
    { 
        // ==================== Filter for 0 parameters ====================

        /// <summary>
        /// Only execute if predicate is true, otherwise return default value (0 params)
        /// </summary>
        public static Runnable<TOutput> Filter<TOutput>(
            this IRunnable<TOutput> runnable,
            Func<bool> predicate,
            TOutput defaultValue = default)
        {
            return new Runnable<TOutput>(
                () => predicate() ? runnable.Invoke() : defaultValue,
                async () => predicate() ? await runnable.InvokeAsync() : defaultValue
            );
        }

        // ==================== Filter for 1 parameter ====================

        /// <summary>
        /// Only execute if predicate is true, otherwise return default value
        /// </summary>
        public static Runnable<TInput, TOutput> Filter<TInput, TOutput>(
            this IRunnable<TInput, TOutput> runnable,
            Func<TInput, bool> predicate,
            TOutput defaultValue = default)
        {
            return new Runnable<TInput, TOutput>(
                input => predicate(input) ? runnable.Invoke(input) : defaultValue,
                async input => predicate(input) ? await runnable.InvokeAsync(input) : defaultValue
            );
        }

        // ==================== Filter for 2 parameters ====================

        /// <summary>
        /// Only execute if predicate is true, otherwise return default value (2 params)
        /// </summary>
        public static Runnable<T1, T2, TOutput> Filter<T1, T2, TOutput>(
            this IRunnable<T1, T2, TOutput> runnable,
            Func<T1, T2, bool> predicate,
            TOutput defaultValue = default)
        {
            return new Runnable<T1, T2, TOutput>(
                (a1, a2) => predicate(a1, a2) ? runnable.Invoke(a1, a2) : defaultValue,
                async (a1, a2) => predicate(a1, a2) ? await runnable.InvokeAsync(a1, a2) : defaultValue
            );
        }

        // ==================== Filter for 3 parameters ====================

        /// <summary>
        /// Only execute if predicate is true, otherwise return default value (3 params)
        /// </summary>
        public static Runnable<T1, T2, T3, TOutput> Filter<T1, T2, T3, TOutput>(
            this IRunnable<T1, T2, T3, TOutput> runnable,
            Func<T1, T2, T3, bool> predicate,
            TOutput defaultValue = default)
        {
            return new Runnable<T1, T2, T3, TOutput>(
                (a1, a2, a3) => predicate(a1, a2, a3) ? runnable.Invoke(a1, a2, a3) : defaultValue,
                async (a1, a2, a3) => predicate(a1, a2, a3) ? await runnable.InvokeAsync(a1, a2, a3) : defaultValue
            );
        }

        // ==================== Filter for 4 parameters ====================

        /// <summary>
        /// Only execute if predicate is true, otherwise return default value (4 params)
        /// </summary>
        public static Runnable<T1, T2, T3, T4, TOutput> Filter<T1, T2, T3, T4, TOutput>(
            this IRunnable<T1, T2, T3, T4, TOutput> runnable,
            Func<T1, T2, T3, T4, bool> predicate,
            TOutput defaultValue = default)
        {
            return new Runnable<T1, T2, T3, T4, TOutput>(
                (a1, a2, a3, a4) => predicate(a1, a2, a3, a4) ? runnable.Invoke(a1, a2, a3, a4) : defaultValue,
                async (a1, a2, a3, a4) => predicate(a1, a2, a3, a4) ? await runnable.InvokeAsync(a1, a2, a3, a4) : defaultValue
            );
        }

        // ==================== Filter for 5 parameters ====================

        /// <summary>
        /// Only execute if predicate is true, otherwise return default value (5 params)
        /// </summary>
        public static Runnable<T1, T2, T3, T4, T5, TOutput> Filter<T1, T2, T3, T4, T5, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, TOutput> runnable,
            Func<T1, T2, T3, T4, T5, bool> predicate,
            TOutput defaultValue = default)
        {
            return new Runnable<T1, T2, T3, T4, T5, TOutput>(
                (a1, a2, a3, a4, a5) => predicate(a1, a2, a3, a4, a5) ? runnable.Invoke(a1, a2, a3, a4, a5) : defaultValue,
                async (a1, a2, a3, a4, a5) => predicate(a1, a2, a3, a4, a5) ? await runnable.InvokeAsync(a1, a2, a3, a4, a5) : defaultValue
            );
        }

        // ==================== Filter for 6 parameters ====================

        /// <summary>
        /// Only execute if predicate is true, otherwise return default value (6 params)
        /// </summary>
        public static Runnable<T1, T2, T3, T4, T5, T6, TOutput> Filter<T1, T2, T3, T4, T5, T6, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, TOutput> runnable,
            Func<T1, T2, T3, T4, T5, T6, bool> predicate,
            TOutput defaultValue = default)
        {
            return new Runnable<T1, T2, T3, T4, T5, T6, TOutput>(
                (a1, a2, a3, a4, a5, a6) => predicate(a1, a2, a3, a4, a5, a6) ? runnable.Invoke(a1, a2, a3, a4, a5, a6) : defaultValue,
                async (a1, a2, a3, a4, a5, a6) => predicate(a1, a2, a3, a4, a5, a6) ? await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6) : defaultValue
            );
        }

        // ==================== Filter for 7 parameters ====================

        /// <summary>
        /// Only execute if predicate is true, otherwise return default value (7 params)
        /// </summary>
        public static Runnable<T1, T2, T3, T4, T5, T6, T7, TOutput> Filter<T1, T2, T3, T4, T5, T6, T7, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, TOutput> runnable,
            Func<T1, T2, T3, T4, T5, T6, T7, bool> predicate,
            TOutput defaultValue = default)
        {
            return new Runnable<T1, T2, T3, T4, T5, T6, T7, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7) => predicate(a1, a2, a3, a4, a5, a6, a7) ? runnable.Invoke(a1, a2, a3, a4, a5, a6, a7) : defaultValue,
                async (a1, a2, a3, a4, a5, a6, a7) => predicate(a1, a2, a3, a4, a5, a6, a7) ? await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7) : defaultValue
            );
        }

        // ==================== Filter for 8 parameters ====================

        /// <summary>
        /// Only execute if predicate is true, otherwise return default value (8 params)
        /// </summary>
        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, TOutput> Filter<T1, T2, T3, T4, T5, T6, T7, T8, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, TOutput> runnable,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, bool> predicate,
            TOutput defaultValue = default)
        {
            return new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8) => predicate(a1, a2, a3, a4, a5, a6, a7, a8) ? runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8) : defaultValue,
                async (a1, a2, a3, a4, a5, a6, a7, a8) => predicate(a1, a2, a3, a4, a5, a6, a7, a8) ? await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8) : defaultValue
            );
        }

        // ==================== Filter for 9 parameters ====================

        /// <summary>
        /// Only execute if predicate is true, otherwise return default value (9 params)
        /// </summary>
        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, TOutput> Filter<T1, T2, T3, T4, T5, T6, T7, T8, T9, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, TOutput> runnable,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, bool> predicate,
            TOutput defaultValue = default)
        {
            return new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9) => predicate(a1, a2, a3, a4, a5, a6, a7, a8, a9) ? runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9) : defaultValue,
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9) => predicate(a1, a2, a3, a4, a5, a6, a7, a8, a9) ? await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9) : defaultValue
            );
        }

        // ==================== Filter for 10 parameters ====================

        /// <summary>
        /// Only execute if predicate is true, otherwise return default value (10 params)
        /// </summary>
        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOutput> Filter<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOutput> runnable,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, bool> predicate,
            TOutput defaultValue = default)
        {
            return new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10) => predicate(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10) ? runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10) : defaultValue,
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10) => predicate(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10) ? await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10) : defaultValue
            );
        }

        // ==================== Filter for 11 parameters ====================

        /// <summary>
        /// Only execute if predicate is true, otherwise return default value (11 params)
        /// </summary>
        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOutput> Filter<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOutput> runnable,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, bool> predicate,
            TOutput defaultValue = default)
        {
            return new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11) => predicate(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11) ? runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11) : defaultValue,
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11) => predicate(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11) ? await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11) : defaultValue
            );
        }

        // ==================== Filter for 12 parameters ====================

        /// <summary>
        /// Only execute if predicate is true, otherwise return default value (12 params)
        /// </summary>
        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOutput> Filter<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOutput> runnable,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, bool> predicate,
            TOutput defaultValue = default)
        {
            return new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12) => predicate(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12) ? runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12) : defaultValue,
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12) => predicate(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12) ? await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12) : defaultValue
            );
        }

        // ==================== Filter for 13 parameters ====================

        /// <summary>
        /// Only execute if predicate is true, otherwise return default value (13 params)
        /// </summary>
        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOutput> Filter<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOutput> runnable,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, bool> predicate,
            TOutput defaultValue = default)
        {
            return new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13) => predicate(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13) ? runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13) : defaultValue,
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13) => predicate(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13) ? await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13) : defaultValue
            );
        }

        // ==================== Filter for 14 parameters ====================

        /// <summary>
        /// Only execute if predicate is true, otherwise return default value (14 params)
        /// </summary>
        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOutput> Filter<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOutput> runnable,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, bool> predicate,
            TOutput defaultValue = default)
        {
            return new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14) => predicate(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14) ? runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14) : defaultValue,
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14) => predicate(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14) ? await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14) : defaultValue
            );
        }

        // ==================== Filter for 15 parameters ====================

        /// <summary>
        /// Only execute if predicate is true, otherwise return default value (15 params)
        /// </summary>
        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOutput> Filter<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOutput> runnable,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, bool> predicate,
            TOutput defaultValue = default)
        {
            return new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15) => predicate(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15) ? runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15) : defaultValue,
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15) => predicate(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15) ? await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15) : defaultValue
            );
        }

        // ==================== Filter for 16 parameters ====================

        /// <summary>
        /// Only execute if predicate is true, otherwise return default value (16 params)
        /// </summary>
        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TOutput> Filter<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TOutput> runnable,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, bool> predicate,
            TOutput defaultValue = default)
        {
            return new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16) => predicate(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16) ? runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16) : defaultValue,
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16) => predicate(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16) ? await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16) : defaultValue
            );
        }

        // ==================== FilterAsync (Async Predicate) ====================

        /// <summary>
        /// Only execute if async predicate is true, otherwise return default value (0 params)
        /// </summary>
        public static Runnable<TOutput> FilterAsync<TOutput>(
            this IRunnable<TOutput> runnable,
            Func<Task<bool>> asyncPredicate,
            TOutput defaultValue = default)
        {
            return new Runnable<TOutput>(
                () => asyncPredicate().GetAwaiter().GetResult() ? runnable.Invoke() : defaultValue,
                async () => await asyncPredicate().ConfigureAwait(false) ? await runnable.InvokeAsync() : defaultValue
            );
        }

        /// <summary>
        /// Only execute if async predicate is true, otherwise return default value (1 param)
        /// </summary>
        public static Runnable<TInput, TOutput> FilterAsync<TInput, TOutput>(
            this IRunnable<TInput, TOutput> runnable,
            Func<TInput, Task<bool>> asyncPredicate,
            TOutput defaultValue = default)
        {
            return new Runnable<TInput, TOutput>(
                input => asyncPredicate(input).GetAwaiter().GetResult() ? runnable.Invoke(input) : defaultValue,
                async input => await asyncPredicate(input).ConfigureAwait(false) ? await runnable.InvokeAsync(input) : defaultValue
            );
        }

        /// <summary>
        /// Only execute if async predicate is true, otherwise return default value (2 params)
        /// </summary>
        public static Runnable<T1, T2, TOutput> FilterAsync<T1, T2, TOutput>(
            this IRunnable<T1, T2, TOutput> runnable,
            Func<T1, T2, Task<bool>> asyncPredicate,
            TOutput defaultValue = default)
        {
            return new Runnable<T1, T2, TOutput>(
                (a1, a2) => asyncPredicate(a1, a2).GetAwaiter().GetResult() ? runnable.Invoke(a1, a2) : defaultValue,
                async (a1, a2) => await asyncPredicate(a1, a2).ConfigureAwait(false) ? await runnable.InvokeAsync(a1, a2) : defaultValue
            );
        }

        /// <summary>
        /// Only execute if async predicate is true, otherwise return default value (3 params)
        /// </summary>
        public static Runnable<T1, T2, T3, TOutput> FilterAsync<T1, T2, T3, TOutput>(
            this IRunnable<T1, T2, T3, TOutput> runnable,
            Func<T1, T2, T3, Task<bool>> asyncPredicate,
            TOutput defaultValue = default)
        {
            return new Runnable<T1, T2, T3, TOutput>(
                (a1, a2, a3) => asyncPredicate(a1, a2, a3).GetAwaiter().GetResult() ? runnable.Invoke(a1, a2, a3) : defaultValue,
                async (a1, a2, a3) => await asyncPredicate(a1, a2, a3).ConfigureAwait(false) ? await runnable.InvokeAsync(a1, a2, a3) : defaultValue
            );
        }

        /// <summary>
        /// Only execute if async predicate is true, otherwise return default value (4 params)
        /// </summary>
        public static Runnable<T1, T2, T3, T4, TOutput> FilterAsync<T1, T2, T3, T4, TOutput>(
            this IRunnable<T1, T2, T3, T4, TOutput> runnable,
            Func<T1, T2, T3, T4, Task<bool>> asyncPredicate,
            TOutput defaultValue = default)
        {
            return new Runnable<T1, T2, T3, T4, TOutput>(
                (a1, a2, a3, a4) => asyncPredicate(a1, a2, a3, a4).GetAwaiter().GetResult() ? runnable.Invoke(a1, a2, a3, a4) : defaultValue,
                async (a1, a2, a3, a4) => await asyncPredicate(a1, a2, a3, a4).ConfigureAwait(false) ? await runnable.InvokeAsync(a1, a2, a3, a4) : defaultValue
            );
        }

        /// <summary>
        /// Only execute if async predicate is true, otherwise return default value (5 params)
        /// </summary>
        public static Runnable<T1, T2, T3, T4, T5, TOutput> FilterAsync<T1, T2, T3, T4, T5, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, TOutput> runnable,
            Func<T1, T2, T3, T4, T5, Task<bool>> asyncPredicate,
            TOutput defaultValue = default)
        {
            return new Runnable<T1, T2, T3, T4, T5, TOutput>(
                (a1, a2, a3, a4, a5) => asyncPredicate(a1, a2, a3, a4, a5).GetAwaiter().GetResult() ? runnable.Invoke(a1, a2, a3, a4, a5) : defaultValue,
                async (a1, a2, a3, a4, a5) => await asyncPredicate(a1, a2, a3, a4, a5).ConfigureAwait(false) ? await runnable.InvokeAsync(a1, a2, a3, a4, a5) : defaultValue
            );
        }

        /// <summary>
        /// Only execute if async predicate is true, otherwise return default value (6 params)
        /// </summary>
        public static Runnable<T1, T2, T3, T4, T5, T6, TOutput> FilterAsync<T1, T2, T3, T4, T5, T6, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, TOutput> runnable,
            Func<T1, T2, T3, T4, T5, T6, Task<bool>> asyncPredicate,
            TOutput defaultValue = default)
        {
            return new Runnable<T1, T2, T3, T4, T5, T6, TOutput>(
                (a1, a2, a3, a4, a5, a6) => asyncPredicate(a1, a2, a3, a4, a5, a6).GetAwaiter().GetResult() ? runnable.Invoke(a1, a2, a3, a4, a5, a6) : defaultValue,
                async (a1, a2, a3, a4, a5, a6) => await asyncPredicate(a1, a2, a3, a4, a5, a6).ConfigureAwait(false) ? await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6) : defaultValue
            );
        }

        /// <summary>
        /// Only execute if async predicate is true, otherwise return default value (7 params)
        /// </summary>
        public static Runnable<T1, T2, T3, T4, T5, T6, T7, TOutput> FilterAsync<T1, T2, T3, T4, T5, T6, T7, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, TOutput> runnable,
            Func<T1, T2, T3, T4, T5, T6, T7, Task<bool>> asyncPredicate,
            TOutput defaultValue = default)
        {
            return new Runnable<T1, T2, T3, T4, T5, T6, T7, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7) => asyncPredicate(a1, a2, a3, a4, a5, a6, a7).GetAwaiter().GetResult() ? runnable.Invoke(a1, a2, a3, a4, a5, a6, a7) : defaultValue,
                async (a1, a2, a3, a4, a5, a6, a7) => await asyncPredicate(a1, a2, a3, a4, a5, a6, a7).ConfigureAwait(false) ? await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7) : defaultValue
            );
        }

        /// <summary>
        /// Only execute if async predicate is true, otherwise return default value (8 params)
        /// </summary>
        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, TOutput> FilterAsync<T1, T2, T3, T4, T5, T6, T7, T8, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, TOutput> runnable,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, Task<bool>> asyncPredicate,
            TOutput defaultValue = default)
        {
            return new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8) => asyncPredicate(a1, a2, a3, a4, a5, a6, a7, a8).GetAwaiter().GetResult() ? runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8) : defaultValue,
                async (a1, a2, a3, a4, a5, a6, a7, a8) => await asyncPredicate(a1, a2, a3, a4, a5, a6, a7, a8).ConfigureAwait(false) ? await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8) : defaultValue
            );
        }

        /// <summary>
        /// Only execute if async predicate is true, otherwise return default value (9 params)
        /// </summary>
        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, TOutput> FilterAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, TOutput> runnable,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, Task<bool>> asyncPredicate,
            TOutput defaultValue = default)
        {
            return new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9) => asyncPredicate(a1, a2, a3, a4, a5, a6, a7, a8, a9).GetAwaiter().GetResult() ? runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9) : defaultValue,
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9) => await asyncPredicate(a1, a2, a3, a4, a5, a6, a7, a8, a9).ConfigureAwait(false) ? await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9) : defaultValue
            );
        }

        /// <summary>
        /// Only execute if async predicate is true, otherwise return default value (10 params)
        /// </summary>
        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOutput> FilterAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOutput> runnable,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, Task<bool>> asyncPredicate,
            TOutput defaultValue = default)
        {
            return new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10) => asyncPredicate(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10).GetAwaiter().GetResult() ? runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10) : defaultValue,
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10) => await asyncPredicate(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10).ConfigureAwait(false) ? await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10) : defaultValue
            );
        }

        /// <summary>
        /// Only execute if async predicate is true, otherwise return default value (11 params)
        /// </summary>
        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOutput> FilterAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOutput> runnable,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, Task<bool>> asyncPredicate,
            TOutput defaultValue = default)
        {
            return new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11) => asyncPredicate(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11).GetAwaiter().GetResult() ? runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11) : defaultValue,
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11) => await asyncPredicate(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11).ConfigureAwait(false) ? await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11) : defaultValue
            );
        }

        /// <summary>
        /// Only execute if async predicate is true, otherwise return default value (12 params)
        /// </summary>
        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOutput> FilterAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOutput> runnable,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, Task<bool>> asyncPredicate,
            TOutput defaultValue = default)
        {
            return new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12) => asyncPredicate(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12).GetAwaiter().GetResult() ? runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12) : defaultValue,
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12) => await asyncPredicate(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12).ConfigureAwait(false) ? await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12) : defaultValue
            );
        }

        /// <summary>
        /// Only execute if async predicate is true, otherwise return default value (13 params)
        /// </summary>
        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOutput> FilterAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOutput> runnable,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, Task<bool>> asyncPredicate,
            TOutput defaultValue = default)
        {
            return new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13) => asyncPredicate(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13).GetAwaiter().GetResult() ? runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13) : defaultValue,
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13) => await asyncPredicate(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13).ConfigureAwait(false) ? await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13) : defaultValue
            );
        }

        /// <summary>
        /// Only execute if async predicate is true, otherwise return default value (14 params)
        /// </summary>
        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOutput> FilterAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOutput> runnable,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, Task<bool>> asyncPredicate,
            TOutput defaultValue = default)
        {
            return new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14) => asyncPredicate(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14).GetAwaiter().GetResult() ? runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14) : defaultValue,
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14) => await asyncPredicate(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14).ConfigureAwait(false) ? await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14) : defaultValue
            );
        }

        /// <summary>
        /// Only execute if async predicate is true, otherwise return default value (15 params)
        /// </summary>
        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOutput> FilterAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOutput> runnable,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, Task<bool>> asyncPredicate,
            TOutput defaultValue = default)
        {
            return new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15) => asyncPredicate(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15).GetAwaiter().GetResult() ? runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15) : defaultValue,
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15) => await asyncPredicate(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15).ConfigureAwait(false) ? await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15) : defaultValue
            );
        }

        /// <summary>
        /// Only execute if async predicate is true, otherwise return default value (16 params)
        /// </summary>
        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TOutput> FilterAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TOutput> runnable,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, Task<bool>> asyncPredicate,
            TOutput defaultValue = default)
        {
            return new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16) => asyncPredicate(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16).GetAwaiter().GetResult() ? runnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16) : defaultValue,
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16) => await asyncPredicate(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16).ConfigureAwait(false) ? await runnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16) : defaultValue
            );
        }
    }
}


