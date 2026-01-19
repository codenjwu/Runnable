using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Runnable
{
    /// <summary>
    /// Extension methods to convert Func and Action delegates to Runnable pipeline components.
    /// Supports 0-16 parameters with full async/await support.
    /// </summary>
    public static class RunnableAsRunnableExtensions
    {
        // ==================== 0 Parameters ====================
        // Converts Func&lt;TOutput&gt; to Runnable&lt;TOutput&gt;
        // Use case: Configuration providers, factory methods, constants
        
        /// <summary>
        /// Converts a parameterless function to a Runnable pipeline component.
        /// </summary>
        /// <typeparam name="TOutput">The output type of the function</typeparam>
        /// <param name="func">The function to convert (must not be null)</param>
        /// <returns>A Runnable that wraps the function</returns>
        /// <exception cref="ArgumentNullException">Thrown when func is null</exception>
        /// <example>
        /// <code>
        /// Func&lt;int&gt; getNumber = () => 42;
        /// var result = getNumber.AsRunnable()
        ///     .Map(x => x * 2)
        ///     .Invoke();
        /// // Result: 84
        /// </code>
        /// </example>
        public static Runnable<TOutput> AsRunnable<TOutput>(
            this Func<TOutput> func)
        {
            if (func == null) throw new ArgumentNullException(nameof(func));
            return RunnableLambda.Create(func);
        }

        /// <summary>
        /// Converts sync and async functions to a Runnable with both execution modes.
        /// </summary>
        /// <typeparam name="TOutput">The output type</typeparam>
        /// <param name="syncFunc">Synchronous version (must not be null)</param>
        /// <param name="asyncFunc">Asynchronous version (must not be null)</param>
        /// <returns>A Runnable supporting both sync and async execution</returns>
        /// <exception cref="ArgumentNullException">Thrown when either func is null</exception>
        /// <example>
        /// <code>
        /// Func&lt;int&gt; sync = () => 42;
        /// Func&lt;Task&lt;int&gt;&gt; async = async () => await GetDataAsync();
        /// var runnable = sync.AsRunnable(async);
        /// </code>
        /// </example>
        public static Runnable<TOutput> AsRunnable<TOutput>(
            this Func<TOutput> syncFunc,
            Func<Task<TOutput>> asyncFunc)
        {
            if (syncFunc == null) throw new ArgumentNullException(nameof(syncFunc));
            if (asyncFunc == null) throw new ArgumentNullException(nameof(asyncFunc));
            return RunnableLambda.Create(syncFunc, asyncFunc);
        }

        /// <summary>
        /// Converts an async-only function to a Runnable (sync execution will throw).
        /// </summary>
        /// <typeparam name="TOutput">The output type</typeparam>
        /// <param name="asyncFunc">Async function (must not be null)</param>
        /// <returns>A Runnable supporting only async execution</returns>
        /// <exception cref="ArgumentNullException">Thrown when asyncFunc is null</exception>
        /// <exception cref="NotSupportedException">Thrown when Invoke() is called instead of InvokeAsync()</exception>
        /// <example>
        /// <code>
        /// Func&lt;Task&lt;int&gt;&gt; asyncOnly = async () => await GetDataAsync();
        /// var runnable = asyncOnly.AsRunnableAsync();
        /// var result = await runnable.InvokeAsync(); // ? Works
        /// // runnable.Invoke(); // ? Throws NotSupportedException
        /// </code>
        /// </example>
        public static Runnable<TOutput> AsRunnableAsync<TOutput>(
            this Func<Task<TOutput>> asyncFunc)
        {
            if (asyncFunc == null) throw new ArgumentNullException(nameof(asyncFunc));
            return RunnableLambda.Create(
                () => throw new NotSupportedException("This runnable only supports async execution. Use InvokeAsync() instead of Invoke()."),
                asyncFunc);
        }

        /// <summary>
        /// Converts an Action (void-returning method) to a Runnable&lt;Unit&gt; for side-effect pipelines.
        /// </summary>
        /// <param name="action">The action to convert (must not be null)</param>
        /// <returns>A Runnable&lt;Unit&gt; that executes the action</returns>
        /// <exception cref="ArgumentNullException">Thrown when action is null</exception>
        /// <example>
        /// <code>
        /// Action logMessage = () => Console.WriteLine("Hello");
        /// var runnable = logMessage.AsRunnable()
        ///     .Tap(_ => Console.WriteLine("After"))
        ///     .Invoke();
        /// </code>
        /// </example>
        public static Runnable<Unit> AsRunnable(this Action action)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));
            return RunnableLambda.Create(() => { action(); return Unit.Default; });
        }

        /// <summary>
        /// Converts an async Action to a Runnable&lt;Unit&gt; for async side-effect pipelines.
        /// </summary>
        /// <param name="asyncAction">The async action to convert (must not be null)</param>
        /// <returns>A Runnable&lt;Unit&gt; that executes the async action</returns>
        /// <exception cref="ArgumentNullException">Thrown when asyncAction is null</exception>
        /// <example>
        /// <code>
        /// Func&lt;Task&gt; logAsync = async () => await LogToDbAsync("Hello");
        /// var runnable = logAsync.AsRunnableAsync();
        /// await runnable.InvokeAsync();
        /// </code>
        /// </example>
        public static Runnable<Unit> AsRunnableAsync(this Func<Task> asyncAction)
        {
            if (asyncAction == null) throw new ArgumentNullException(nameof(asyncAction));
            return RunnableLambda.Create(
                () => throw new NotSupportedException("This runnable only supports async execution. Use InvokeAsync() instead of Invoke()."),
                async () => { await asyncAction(); return Unit.Default; });
        }


        // ==================== 1 Parameter ====================
        // Converts Func&lt;T1, TOutput&gt; to Runnable&lt;T1, TOutput&gt;
        // Use case: Data transformation, mappers, converters, validators
        
        /// <summary>
        /// Converts a single-parameter function to a Runnable pipeline component.
        /// </summary>
        /// <typeparam name="T1">The input parameter type</typeparam>
        /// <typeparam name="TOutput">The output type</typeparam>
        /// <param name="func">The function to convert (must not be null)</param>
        /// <returns>A Runnable that wraps the function</returns>
        /// <exception cref="ArgumentNullException">Thrown when func is null</exception>
        /// <example>
        /// <code>
        /// Func&lt;int, string&gt; toString = x => x.ToString();
        /// var runnable = toString.AsRunnable()
        ///     .Map(s => s.Length)
        ///     .Invoke(42);
        /// </code>
        /// </example>
        public static Runnable<T1, TOutput> AsRunnable<T1, TOutput>(
            this Func<T1, TOutput> func)
        {
            if (func == null) throw new ArgumentNullException(nameof(func));
            return RunnableLambda.Create(func);
        }

        /// <summary>
        /// Converts sync and async single-parameter functions to a Runnable.
        /// </summary>
        /// <typeparam name="T1">The input parameter type</typeparam>
        /// <typeparam name="TOutput">The output type</typeparam>
        /// <param name="syncFunc">Synchronous version (must not be null)</param>
        /// <param name="asyncFunc">Asynchronous version (must not be null)</param>
        /// <returns>A Runnable supporting both sync and async execution</returns>
        /// <exception cref="ArgumentNullException">Thrown when either func is null</exception>
        public static Runnable<T1, TOutput> AsRunnable<T1, TOutput>(
            this Func<T1, TOutput> syncFunc,
            Func<T1, Task<TOutput>> asyncFunc)
        {
            if (syncFunc == null) throw new ArgumentNullException(nameof(syncFunc));
            if (asyncFunc == null) throw new ArgumentNullException(nameof(asyncFunc));
            return RunnableLambda.Create(syncFunc, asyncFunc);
        }

        /// <summary>
        /// Converts an async-only single-parameter function to a Runnable.
        /// </summary>
        /// <typeparam name="T1">The input parameter type</typeparam>
        /// <typeparam name="TOutput">The output type</typeparam>
        /// <param name="asyncFunc">Async function (must not be null)</param>
        /// <returns>A Runnable supporting only async execution</returns>
        /// <exception cref="ArgumentNullException">Thrown when asyncFunc is null</exception>
        /// <exception cref="NotSupportedException">Thrown when Invoke() is called</exception>
        public static Runnable<T1, TOutput> AsRunnableAsync<T1, TOutput>(
            this Func<T1, Task<TOutput>> asyncFunc)
        {
            if (asyncFunc == null) throw new ArgumentNullException(nameof(asyncFunc));
            return RunnableLambda.Create<T1, TOutput>(
                _ => throw new NotSupportedException("This runnable only supports async execution. Use InvokeAsync() instead of Invoke()."),
                asyncFunc);
        }

        /// <summary>
        /// Converts an Action&lt;T1&gt; to a Runnable&lt;T1, Unit&gt; for side-effect pipelines.
        /// </summary>
        /// <typeparam name="T1">The input parameter type</typeparam>
        /// <param name="action">The action to convert (must not be null)</param>
        /// <returns>A Runnable&lt;T1, Unit&gt; that executes the action</returns>
        /// <exception cref="ArgumentNullException">Thrown when action is null</exception>
        public static Runnable<T1, Unit> AsRunnable<T1>(this Action<T1> action)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));
            return RunnableLambda.Create<T1, Unit>(input => { action(input); return Unit.Default; });
        }


        // ==================== 2 Parameters ====================
        // Converts Func&lt;T1, T2, TOutput&gt; to Runnable&lt;T1, T2, TOutput&gt;
        // Use case: Binary operations, comparisons, combiners
        
        /// <summary>
        /// Converts a two-parameter function to a Runnable pipeline component.
        /// </summary>
        public static Runnable<T1, T2, TOutput> AsRunnable<T1, T2, TOutput>(
            this Func<T1, T2, TOutput> func)
        {
            if (func == null) throw new ArgumentNullException(nameof(func));
            return RunnableLambda.Create(func);
        }

        /// <summary>
        /// Converts sync and async two-parameter functions to a Runnable.
        /// </summary>
        public static Runnable<T1, T2, TOutput> AsRunnable<T1, T2, TOutput>(
            this Func<T1, T2, TOutput> syncFunc,
            Func<T1, T2, Task<TOutput>> asyncFunc)
        {
            if (syncFunc == null) throw new ArgumentNullException(nameof(syncFunc));
            if (asyncFunc == null) throw new ArgumentNullException(nameof(asyncFunc));
            return RunnableLambda.Create(syncFunc, asyncFunc);
        }

        // ==================== 3 Parameters ====================
        
        /// <summary>
        /// Converts a three-parameter function to a Runnable pipeline component.
        /// </summary>
        public static Runnable<T1, T2, T3, TOutput> AsRunnable<T1, T2, T3, TOutput>(
            this Func<T1, T2, T3, TOutput> func)
        {
            if (func == null) throw new ArgumentNullException(nameof(func));
            return RunnableLambda.Create(func);
        }

        public static Runnable<T1, T2, T3, TOutput> AsRunnable<T1, T2, T3, TOutput>(
            this Func<T1, T2, T3, TOutput> syncFunc,
            Func<T1, T2, T3, Task<TOutput>> asyncFunc)
        {
            if (syncFunc == null) throw new ArgumentNullException(nameof(syncFunc));
            if (asyncFunc == null) throw new ArgumentNullException(nameof(asyncFunc));
            return RunnableLambda.Create(syncFunc, asyncFunc);
        }

        // ==================== 4 Parameters ====================
        
        /// <summary>
        /// Converts a four-parameter function to a Runnable pipeline component.
        /// </summary>
        public static Runnable<T1, T2, T3, T4, TOutput> AsRunnable<T1, T2, T3, T4, TOutput>(
            this Func<T1, T2, T3, T4, TOutput> func)
        {
            if (func == null) throw new ArgumentNullException(nameof(func));
            return RunnableLambda.Create(func);
        }

        public static Runnable<T1, T2, T3, T4, TOutput> AsRunnable<T1, T2, T3, T4, TOutput>(
            this Func<T1, T2, T3, T4, TOutput> syncFunc,
            Func<T1, T2, T3, T4, Task<TOutput>> asyncFunc)
        {
            if (syncFunc == null) throw new ArgumentNullException(nameof(syncFunc));
            if (asyncFunc == null) throw new ArgumentNullException(nameof(asyncFunc));
            return RunnableLambda.Create(syncFunc, asyncFunc);
        }

        // ==================== 5 Parameters ====================
        
        /// <summary>
        /// Converts a five-parameter function to a Runnable pipeline component.
        /// </summary>
        public static Runnable<T1, T2, T3, T4, T5, TOutput> AsRunnable<T1, T2, T3, T4, T5, TOutput>(
            this Func<T1, T2, T3, T4, T5, TOutput> func)
        {
            if (func == null) throw new ArgumentNullException(nameof(func));
            return RunnableLambda.Create(func);
        }

        public static Runnable<T1, T2, T3, T4, T5, TOutput> AsRunnable<T1, T2, T3, T4, T5, TOutput>(
            this Func<T1, T2, T3, T4, T5, TOutput> syncFunc,
            Func<T1, T2, T3, T4, T5, Task<TOutput>> asyncFunc)
        {
            if (syncFunc == null) throw new ArgumentNullException(nameof(syncFunc));
            if (asyncFunc == null) throw new ArgumentNullException(nameof(asyncFunc));
            return RunnableLambda.Create(syncFunc, asyncFunc);
        }

        // ==================== 6 Parameters ====================
        
        public static Runnable<T1, T2, T3, T4, T5, T6, TOutput> AsRunnable<T1, T2, T3, T4, T5, T6, TOutput>(this Func<T1, T2, T3, T4, T5, T6, TOutput> func){if (func == null) throw new ArgumentNullException(nameof(func));return RunnableLambda.Create(func);
        }

        public static Runnable<T1, T2, T3, T4, T5, T6, TOutput> AsRunnable<T1, T2, T3, T4, T5, T6, TOutput>(
            this Func<T1, T2, T3, T4, T5, T6, TOutput> syncFunc,
            Func<T1, T2, T3, T4, T5, T6, Task<TOutput>> asyncFunc)
        {
            return RunnableLambda.Create(syncFunc, asyncFunc);
        }

        // ==================== 7 Parameters ====================
        
        public static Runnable<T1, T2, T3, T4, T5, T6, T7, TOutput> AsRunnable<T1, T2, T3, T4, T5, T6, T7, TOutput>(
            this Func<T1, T2, T3, T4, T5, T6, T7, TOutput> func)
        {
            return RunnableLambda.Create(func);
        }

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, TOutput> AsRunnable<T1, T2, T3, T4, T5, T6, T7, TOutput>(
            this Func<T1, T2, T3, T4, T5, T6, T7, TOutput> syncFunc,
            Func<T1, T2, T3, T4, T5, T6, T7, Task<TOutput>> asyncFunc)
        {
            return RunnableLambda.Create(syncFunc, asyncFunc);
        }

        // ==================== 8 Parameters ====================
        
        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, TOutput> AsRunnable<T1, T2, T3, T4, T5, T6, T7, T8, TOutput>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, TOutput> func)
        {
            return RunnableLambda.Create(func);
        }

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, TOutput> AsRunnable<T1, T2, T3, T4, T5, T6, T7, T8, TOutput>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, TOutput> syncFunc,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, Task<TOutput>> asyncFunc)
        {
            return RunnableLambda.Create(syncFunc, asyncFunc);
        }

        // ==================== 9 Parameters ====================
        
        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, TOutput> AsRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, TOutput>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TOutput> func)
        {
            return RunnableLambda.Create(func);
        }

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, TOutput> AsRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, TOutput>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TOutput> syncFunc,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, Task<TOutput>> asyncFunc)
        {
            return RunnableLambda.Create(syncFunc, asyncFunc);
        }

        // ==================== 10 Parameters ====================
        
        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOutput> AsRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOutput>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOutput> func)
        {
            return RunnableLambda.Create(func);
        }

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOutput> AsRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOutput>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOutput> syncFunc,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, Task<TOutput>> asyncFunc)
        {
            return RunnableLambda.Create(syncFunc, asyncFunc);
        }

        // ==================== 11 Parameters ====================
        
        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOutput> AsRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOutput>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOutput> func)
        {
            return RunnableLambda.Create(func);
        }

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOutput> AsRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOutput>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOutput> syncFunc,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, Task<TOutput>> asyncFunc)
        {
            return RunnableLambda.Create(syncFunc, asyncFunc);
        }

        // ==================== 12 Parameters ====================
        
        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOutput> AsRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOutput>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOutput> func)
        {
            return RunnableLambda.Create(func);
        }

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOutput> AsRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOutput>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOutput> syncFunc,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, Task<TOutput>> asyncFunc)
        {
            return RunnableLambda.Create(syncFunc, asyncFunc);
        }

        // ==================== 13 Parameters ====================
        
        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOutput> AsRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOutput>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOutput> func)
        {
            return RunnableLambda.Create(func);
        }

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOutput> AsRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOutput>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOutput> syncFunc,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, Task<TOutput>> asyncFunc)
        {
            return RunnableLambda.Create(syncFunc, asyncFunc);
        }

        // ==================== 14 Parameters ====================
        
        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOutput> AsRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOutput>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOutput> func)
        {
            return RunnableLambda.Create(func);
        }

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOutput> AsRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOutput>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOutput> syncFunc,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, Task<TOutput>> asyncFunc)
        {
            return RunnableLambda.Create(syncFunc, asyncFunc);
        }

        // ==================== 15 Parameters ====================
        
        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOutput> AsRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOutput>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOutput> func)
        {
            return RunnableLambda.Create(func);
        }

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOutput> AsRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOutput>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOutput> syncFunc,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, Task<TOutput>> asyncFunc)
        {
            return RunnableLambda.Create(syncFunc, asyncFunc);
        }

        // ==================== 16 Parameters ====================
        
        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TOutput> AsRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TOutput>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TOutput> func)
        {
            return RunnableLambda.Create(func);
        }

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TOutput> AsRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TOutput>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TOutput> syncFunc,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, Task<TOutput>> asyncFunc)
        {
            return RunnableLambda.Create(syncFunc, asyncFunc);
        }
    }
}
