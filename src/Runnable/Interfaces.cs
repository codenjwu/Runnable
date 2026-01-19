using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Runnable
{
    /// <summary>
    /// Base Runnable interface with no input parameters
    /// </summary>
    public interface IRunnable<TOutput>
    {
        /// <summary>
        /// Synchronous execution
        /// </summary>
        TOutput Invoke();

        /// <summary>
        /// Asynchronous execution
        /// </summary>
        Task<TOutput> InvokeAsync();

        /// <summary>
        /// Batch execution
        /// </summary>
        IEnumerable<TOutput> Batch(int count);

        /// <summary>
        /// Asynchronous batch execution
        /// </summary>
        Task<IEnumerable<TOutput>> BatchAsync(int count);

        /// <summary>
        /// Stream execution
        /// </summary>
        IAsyncEnumerable<TOutput> Stream(int count);
    }
    /// <summary>
    /// Base Runnable interface with 1 input parameter
    /// </summary>
    public interface IRunnable<TInput, TOutput>
    {
        /// <summary>
        /// Synchronous execution
        /// </summary>
        TOutput Invoke(TInput input);

        /// <summary>
        /// Asynchronous execution
        /// </summary>
        Task<TOutput> InvokeAsync(TInput input);

        /// <summary>
        /// Batch execution
        /// </summary>
        IEnumerable<TOutput> Batch(IEnumerable<TInput> inputs);

        /// <summary>
        /// Asynchronous batch execution
        /// </summary>
        Task<IEnumerable<TOutput>> BatchAsync(IEnumerable<TInput> inputs);

        /// <summary>
        /// Stream execution
        /// </summary>
        IAsyncEnumerable<TOutput> Stream(TInput input);
    }
    // ==================== Runnable with 2 parameters ====================
    /// <summary>
    /// Base Runnable interface with 2 input parameters
    /// </summary>
    public interface IRunnable<T1, T2, TOutput>
    {
        TOutput Invoke(T1 arg1, T2 arg2);
        Task<TOutput> InvokeAsync(T1 arg1, T2 arg2);
        IEnumerable<TOutput> Batch(IEnumerable<(T1, T2)> inputs);
        Task<IEnumerable<TOutput>> BatchAsync(IEnumerable<(T1, T2)> inputs);
        IAsyncEnumerable<TOutput> Stream((T1, T2) input);
    }

    // ==================== Runnable with 3 parameters ====================
    /// <summary>
    /// Base Runnable interface with 3 input parameters
    /// </summary>
    public interface IRunnable<T1, T2, T3, TOutput>
    {
        TOutput Invoke(T1 arg1, T2 arg2, T3 arg3);
        Task<TOutput> InvokeAsync(T1 arg1, T2 arg2, T3 arg3);
        IEnumerable<TOutput> Batch(IEnumerable<(T1, T2, T3)> inputs);
        Task<IEnumerable<TOutput>> BatchAsync(IEnumerable<(T1, T2, T3)> inputs);
        IAsyncEnumerable<TOutput> Stream((T1, T2, T3) input);
    }

    // ==================== Runnable with 4 parameters ====================
    /// <summary>
    /// Base Runnable interface with 4 input parameters
    /// </summary>
    public interface IRunnable<T1, T2, T3, T4, TOutput>
    {
        TOutput Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4);
        Task<TOutput> InvokeAsync(T1 arg1, T2 arg2, T3 arg3, T4 arg4);
        IEnumerable<TOutput> Batch(IEnumerable<(T1, T2, T3, T4)> inputs);
        Task<IEnumerable<TOutput>> BatchAsync(IEnumerable<(T1, T2, T3, T4)> inputs);
        IAsyncEnumerable<TOutput> Stream((T1, T2, T3, T4) input);
    }

    // ==================== Runnable with 5 parameters ====================
    /// <summary>
    /// Base Runnable interface with 5 input parameters
    /// </summary>
    public interface IRunnable<T1, T2, T3, T4, T5, TOutput>
    {
        TOutput Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5);
        Task<TOutput> InvokeAsync(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5);
        IEnumerable<TOutput> Batch(IEnumerable<(T1, T2, T3, T4, T5)> inputs);
        Task<IEnumerable<TOutput>> BatchAsync(IEnumerable<(T1, T2, T3, T4, T5)> inputs);
        IAsyncEnumerable<TOutput> Stream((T1, T2, T3, T4, T5) input);
    }

    // ==================== Runnable with 6 parameters ====================
    /// <summary>
    /// Base Runnable interface with 6 input parameters
    /// </summary>
    public interface IRunnable<T1, T2, T3, T4, T5, T6, TOutput>
    {
        TOutput Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6);
        Task<TOutput> InvokeAsync(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6);
        IEnumerable<TOutput> Batch(IEnumerable<(T1, T2, T3, T4, T5, T6)> inputs);
        Task<IEnumerable<TOutput>> BatchAsync(IEnumerable<(T1, T2, T3, T4, T5, T6)> inputs);
        IAsyncEnumerable<TOutput> Stream((T1, T2, T3, T4, T5, T6) input);
    }

    // ==================== Runnable with 7 parameters ====================
    /// <summary>
    /// Base Runnable interface with 7 input parameters
    /// </summary>
    public interface IRunnable<T1, T2, T3, T4, T5, T6, T7, TOutput>
    {
        TOutput Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7);
        Task<TOutput> InvokeAsync(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7);
        IEnumerable<TOutput> Batch(IEnumerable<(T1, T2, T3, T4, T5, T6, T7)> inputs);
        Task<IEnumerable<TOutput>> BatchAsync(IEnumerable<(T1, T2, T3, T4, T5, T6, T7)> inputs);
        IAsyncEnumerable<TOutput> Stream((T1, T2, T3, T4, T5, T6, T7) input);
    }

    // ==================== Runnable with 8 parameters ====================
    /// <summary>
    /// Base Runnable interface with 8 input parameters
    /// </summary>
    public interface IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, TOutput>
    {
        TOutput Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8);
        Task<TOutput> InvokeAsync(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8);
        IEnumerable<TOutput> Batch(IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8)> inputs);
        Task<IEnumerable<TOutput>> BatchAsync(IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8)> inputs);
        IAsyncEnumerable<TOutput> Stream((T1, T2, T3, T4, T5, T6, T7, T8) input);
    }

    // ==================== Runnable with 9 parameters ====================
    /// <summary>
    /// Base Runnable interface with 9 input parameters
    /// </summary>
    public interface IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, TOutput>
    {
        TOutput Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9);
        Task<TOutput> InvokeAsync(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9);
        IEnumerable<TOutput> Batch(IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> inputs);
        Task<IEnumerable<TOutput>> BatchAsync(IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> inputs);
        IAsyncEnumerable<TOutput> Stream((T1, T2, T3, T4, T5, T6, T7, T8, T9) input);
    }

    // ==================== Runnable with 10 parameters ====================
    /// <summary>
    /// Base Runnable interface with 10 input parameters
    /// </summary>
    public interface IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOutput>
    {
        TOutput Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10);
        Task<TOutput> InvokeAsync(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10);
        IEnumerable<TOutput> Batch(IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> inputs);
        Task<IEnumerable<TOutput>> BatchAsync(IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> inputs);
        IAsyncEnumerable<TOutput> Stream((T1, T2, T3, T4, T5, T6, T7, T8, T9, T10) input);
    }

    // ==================== Runnable with 11 parameters ====================
    /// <summary>
    /// Base Runnable interface with 11 input parameters
    /// </summary>
    public interface IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOutput>
    {
        TOutput Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11);
        Task<TOutput> InvokeAsync(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11);
        IEnumerable<TOutput> Batch(IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> inputs);
        Task<IEnumerable<TOutput>> BatchAsync(IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> inputs);
        IAsyncEnumerable<TOutput> Stream((T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11) input);
    }

    // ==================== Runnable with 12 parameters ====================
    /// <summary>
    /// Base Runnable interface with 12 input parameters
    /// </summary>
    public interface IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOutput>
    {
        TOutput Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12);
        Task<TOutput> InvokeAsync(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12);
        IEnumerable<TOutput> Batch(IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> inputs);
        Task<IEnumerable<TOutput>> BatchAsync(IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> inputs);
        IAsyncEnumerable<TOutput> Stream((T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12) input);
    }

    // ==================== Runnable with 13 parameters ====================
    /// <summary>
    /// Base Runnable interface with 13 input parameters
    /// </summary>
    public interface IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOutput>
    {
        TOutput Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13);
        Task<TOutput> InvokeAsync(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13);
        IEnumerable<TOutput> Batch(IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13)> inputs);
        Task<IEnumerable<TOutput>> BatchAsync(IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13)> inputs);
        IAsyncEnumerable<TOutput> Stream((T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13) input);
    }

    // ==================== Runnable with 14 parameters ====================
    /// <summary>
    /// Base Runnable interface with 14 input parameters
    /// </summary>
    public interface IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOutput>
    {
        TOutput Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14);
        Task<TOutput> InvokeAsync(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14);
        IEnumerable<TOutput> Batch(IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14)> inputs);
        Task<IEnumerable<TOutput>> BatchAsync(IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14)> inputs);
        IAsyncEnumerable<TOutput> Stream((T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14) input);
    }

    // ==================== Runnable with 15 parameters ====================
    /// <summary>
    /// Base Runnable interface with 15 input parameters
    /// </summary>
    public interface IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOutput>
    {
        TOutput Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15);
        Task<TOutput> InvokeAsync(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15);
        IEnumerable<TOutput> Batch(IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15)> inputs);
        Task<IEnumerable<TOutput>> BatchAsync(IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15)> inputs);
        IAsyncEnumerable<TOutput> Stream((T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15) input);
    }

    // ==================== Runnable with 16 parameters ====================
    /// <summary>
    /// Base Runnable interface with 16 input parameters
    /// </summary>
    public interface IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TOutput>
    {
        TOutput Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16);
        Task<TOutput> InvokeAsync(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16);
        IEnumerable<TOutput> Batch(IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16)> inputs);
        Task<IEnumerable<TOutput>> BatchAsync(IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16)> inputs);
        IAsyncEnumerable<TOutput> Stream((T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16) input);
    }
}
