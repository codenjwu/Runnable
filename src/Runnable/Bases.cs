using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Runnable
{
    // ==================== Runnable with 0 parameters ====================

    /// <summary>
    /// Base Runnable abstract class with no input parameters
    /// </summary>
    public abstract class BaseRunnable<TOutput> : IRunnable<TOutput>
    {
        public abstract TOutput Invoke();

        public virtual async Task<TOutput> InvokeAsync()
        {
            return await Task.FromResult(Invoke());
        }

        public virtual IEnumerable<TOutput> Batch(int count)
        {
            for (int i = 0; i < count; i++)
            {
                yield return Invoke();
            }
        }

        public virtual async Task<IEnumerable<TOutput>> BatchAsync(int count)
        {
            var results = new List<TOutput>();
            for (int i = 0; i < count; i++)
            {
                results.Add(await InvokeAsync());
            }
            return results;
        }

        public virtual async IAsyncEnumerable<TOutput> Stream(int count)
        {
            for (int i = 0; i < count; i++)
            {
                yield return await InvokeAsync();
            }
        }
    }
    /// <summary>
    /// Base Runnable abstract class
    /// </summary>
    public abstract class BaseRunnable<TInput, TOutput> : IRunnable<TInput, TOutput>
    {
        public abstract TOutput Invoke(TInput input);

        public virtual async Task<TOutput> InvokeAsync(TInput input)
        {
            return await Task.FromResult(Invoke(input));
        }

        public virtual IEnumerable<TOutput> Batch(IEnumerable<TInput> inputs)
        {
            foreach (var input in inputs)
            {
                yield return Invoke(input);
            }
        }

        public virtual async Task<IEnumerable<TOutput>> BatchAsync(IEnumerable<TInput> inputs)
        {
            var results = new List<TOutput>();
            foreach (var input in inputs)
            {
                results.Add(await InvokeAsync(input));
            }
            return results;
        }

        public virtual async IAsyncEnumerable<TOutput> Stream(TInput input)
        {
            yield return await InvokeAsync(input);
        } 
    }
    /// <summary>
    /// Base Runnable abstract class with 2 input parameters
    /// </summary>
    public abstract class BaseRunnable<T1, T2, TOutput> : IRunnable<T1, T2, TOutput>
    {
        public abstract TOutput Invoke(T1 arg1, T2 arg2);

        public virtual async Task<TOutput> InvokeAsync(T1 arg1, T2 arg2)
        {
            return await Task.FromResult(Invoke(arg1, arg2));
        }

        public virtual IEnumerable<TOutput> Batch(IEnumerable<(T1, T2)> inputs)
        {
            foreach (var (arg1, arg2) in inputs)
            {
                yield return Invoke(arg1, arg2);
            }
        }

        public virtual async Task<IEnumerable<TOutput>> BatchAsync(IEnumerable<(T1, T2)> inputs)
        {
            var results = new List<TOutput>();
            foreach (var (arg1, arg2) in inputs)
            {
                results.Add(await InvokeAsync(arg1, arg2));
            }
            return results;
        }

        public virtual async IAsyncEnumerable<TOutput> Stream((T1, T2) input)
        {
            yield return await InvokeAsync(input.Item1, input.Item2);
        }
    }

    // ==================== Runnable with 3 parameters ====================
    
    /// <summary>
    /// Base Runnable abstract class with 3 input parameters
    /// </summary>
    public abstract class BaseRunnable<T1, T2, T3, TOutput> : IRunnable<T1, T2, T3, TOutput>
    {
        public abstract TOutput Invoke(T1 arg1, T2 arg2, T3 arg3);

        public virtual async Task<TOutput> InvokeAsync(T1 arg1, T2 arg2, T3 arg3)
        {
            return await Task.FromResult(Invoke(arg1, arg2, arg3));
        }

        public virtual IEnumerable<TOutput> Batch(IEnumerable<(T1, T2, T3)> inputs)
        {
            foreach (var (arg1, arg2, arg3) in inputs)
            {
                yield return Invoke(arg1, arg2, arg3);
            }
        }

        public virtual async Task<IEnumerable<TOutput>> BatchAsync(IEnumerable<(T1, T2, T3)> inputs)
        {
            var results = new List<TOutput>();
            foreach (var (arg1, arg2, arg3) in inputs)
            {
                results.Add(await InvokeAsync(arg1, arg2, arg3));
            }
            return results;
        }

        public virtual async IAsyncEnumerable<TOutput> Stream((T1, T2, T3) input)
        {
            yield return await InvokeAsync(input.Item1, input.Item2, input.Item3);
        }
    }

    // ==================== Runnable with 4 parameters ====================
    
    /// <summary>
    /// Base Runnable abstract class with 4 input parameters
    /// </summary>
    public abstract class BaseRunnable<T1, T2, T3, T4, TOutput> : IRunnable<T1, T2, T3, T4, TOutput>
    {
        public abstract TOutput Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4);

        public virtual async Task<TOutput> InvokeAsync(T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            return await Task.FromResult(Invoke(arg1, arg2, arg3, arg4));
        }

        public virtual IEnumerable<TOutput> Batch(IEnumerable<(T1, T2, T3, T4)> inputs)
        {
            foreach (var (arg1, arg2, arg3, arg4) in inputs)
            {
                yield return Invoke(arg1, arg2, arg3, arg4);
            }
        }

        public virtual async Task<IEnumerable<TOutput>> BatchAsync(IEnumerable<(T1, T2, T3, T4)> inputs)
        {
            var results = new List<TOutput>();
            foreach (var (arg1, arg2, arg3, arg4) in inputs)
            {
                results.Add(await InvokeAsync(arg1, arg2, arg3, arg4));
            }
            return results;
        }

        public virtual async IAsyncEnumerable<TOutput> Stream((T1, T2, T3, T4) input)
        {
            yield return await InvokeAsync(input.Item1, input.Item2, input.Item3, input.Item4);
        }
    }

    // ==================== Runnable with 5 parameters ====================
    
    /// <summary>
    /// Base Runnable abstract class with 5 input parameters
    /// </summary>
    public abstract class BaseRunnable<T1, T2, T3, T4, T5, TOutput> : IRunnable<T1, T2, T3, T4, T5, TOutput>
    {
        public abstract TOutput Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5);

        public virtual async Task<TOutput> InvokeAsync(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            return await Task.FromResult(Invoke(arg1, arg2, arg3, arg4, arg5));
        }

        public virtual IEnumerable<TOutput> Batch(IEnumerable<(T1, T2, T3, T4, T5)> inputs)
        {
            foreach (var (arg1, arg2, arg3, arg4, arg5) in inputs)
            {
                yield return Invoke(arg1, arg2, arg3, arg4, arg5);
            }
        }

        public virtual async Task<IEnumerable<TOutput>> BatchAsync(IEnumerable<(T1, T2, T3, T4, T5)> inputs)
        {
            var results = new List<TOutput>();
            foreach (var (arg1, arg2, arg3, arg4, arg5) in inputs)
            {
                results.Add(await InvokeAsync(arg1, arg2, arg3, arg4, arg5));
            }
            return results;
        }

        public virtual async IAsyncEnumerable<TOutput> Stream((T1, T2, T3, T4, T5) input)
        {
            yield return await InvokeAsync(input.Item1, input.Item2, input.Item3, input.Item4, input.Item5);
        }
    }

    // ==================== Runnable with 6 parameters ====================
    
    /// <summary>
    /// Base Runnable abstract class with 6 input parameters
    /// </summary>
    public abstract class BaseRunnable<T1, T2, T3, T4, T5, T6, TOutput> : IRunnable<T1, T2, T3, T4, T5, T6, TOutput>
    {
        public abstract TOutput Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6);

        public virtual async Task<TOutput> InvokeAsync(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
        {
            return await Task.FromResult(Invoke(arg1, arg2, arg3, arg4, arg5, arg6));
        }

        public virtual IEnumerable<TOutput> Batch(IEnumerable<(T1, T2, T3, T4, T5, T6)> inputs)
        {
            foreach (var (arg1, arg2, arg3, arg4, arg5, arg6) in inputs)
            {
                yield return Invoke(arg1, arg2, arg3, arg4, arg5, arg6);
            }
        }

        public virtual async Task<IEnumerable<TOutput>> BatchAsync(IEnumerable<(T1, T2, T3, T4, T5, T6)> inputs)
        {
            var results = new List<TOutput>();
            foreach (var (arg1, arg2, arg3, arg4, arg5, arg6) in inputs)
            {
                results.Add(await InvokeAsync(arg1, arg2, arg3, arg4, arg5, arg6));
            }
            return results;
        }

        public virtual async IAsyncEnumerable<TOutput> Stream((T1, T2, T3, T4, T5, T6) input)
        {
            yield return await InvokeAsync(input.Item1, input.Item2, input.Item3, input.Item4, input.Item5, input.Item6);
        }
    }

    // ==================== Runnable with 7 parameters ====================
    
    /// <summary>
    /// Base Runnable abstract class with 7 input parameters
    /// </summary>
    public abstract class BaseRunnable<T1, T2, T3, T4, T5, T6, T7, TOutput> : IRunnable<T1, T2, T3, T4, T5, T6, T7, TOutput>
    {
        public abstract TOutput Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7);

        public virtual async Task<TOutput> InvokeAsync(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
        {
            return await Task.FromResult(Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7));
        }

        public virtual IEnumerable<TOutput> Batch(IEnumerable<(T1, T2, T3, T4, T5, T6, T7)> inputs)
        {
            foreach (var (arg1, arg2, arg3, arg4, arg5, arg6, arg7) in inputs)
            {
                yield return Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
            }
        }

        public virtual async Task<IEnumerable<TOutput>> BatchAsync(IEnumerable<(T1, T2, T3, T4, T5, T6, T7)> inputs)
        {
            var results = new List<TOutput>();
            foreach (var (arg1, arg2, arg3, arg4, arg5, arg6, arg7) in inputs)
            {
                results.Add(await InvokeAsync(arg1, arg2, arg3, arg4, arg5, arg6, arg7));
            }
            return results;
        }

        public virtual async IAsyncEnumerable<TOutput> Stream((T1, T2, T3, T4, T5, T6, T7) input)
        {
            yield return await InvokeAsync(input.Item1, input.Item2, input.Item3, input.Item4, input.Item5, input.Item6, input.Item7);
        }
    }

    // ==================== Runnable with 8 parameters ====================
    
    /// <summary>
    /// Base Runnable abstract class with 8 input parameters
    /// </summary>
    public abstract class BaseRunnable<T1, T2, T3, T4, T5, T6, T7, T8, TOutput> : IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, TOutput>
    {
        public abstract TOutput Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8);

        public virtual async Task<TOutput> InvokeAsync(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
        {
            return await Task.FromResult(Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8));
        }

        public virtual IEnumerable<TOutput> Batch(IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8)> inputs)
        {
            foreach (var (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8) in inputs)
            {
                yield return Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
            }
        }

        public virtual async Task<IEnumerable<TOutput>> BatchAsync(IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8)> inputs)
        {
            var results = new List<TOutput>();
            foreach (var (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8) in inputs)
            {
                results.Add(await InvokeAsync(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8));
            }
            return results;
        }

        public virtual async IAsyncEnumerable<TOutput> Stream((T1, T2, T3, T4, T5, T6, T7, T8) input)
        {
            yield return await InvokeAsync(input.Item1, input.Item2, input.Item3, input.Item4, input.Item5, input.Item6, input.Item7, input.Item8);
        }
    }

    // ==================== Runnable with 9 parameters ====================
    
    /// <summary>
    /// Base Runnable abstract class with 9 input parameters
    /// </summary>
    public abstract class BaseRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, TOutput> : IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, TOutput>
    {
        public abstract TOutput Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9);

        public virtual async Task<TOutput> InvokeAsync(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
        {
            return await Task.FromResult(Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9));
        }

        public virtual IEnumerable<TOutput> Batch(IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> inputs)
        {
            foreach (var (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9) in inputs)
            {
                yield return Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
            }
        }

        public virtual async Task<IEnumerable<TOutput>> BatchAsync(IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> inputs)
        {
            var results = new List<TOutput>();
            foreach (var (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9) in inputs)
            {
                results.Add(await InvokeAsync(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9));
            }
            return results;
        }

        public virtual async IAsyncEnumerable<TOutput> Stream((T1, T2, T3, T4, T5, T6, T7, T8, T9) input)
        {
            yield return await InvokeAsync(input.Item1, input.Item2, input.Item3, input.Item4, input.Item5, input.Item6, input.Item7, input.Item8, input.Item9);
        }
    }

    // ==================== Runnable with 10 parameters ====================
    
    /// <summary>
    /// Base Runnable abstract class with 10 input parameters
    /// </summary>
    public abstract class BaseRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOutput> : IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOutput>
    {
        public abstract TOutput Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10);

        public virtual async Task<TOutput> InvokeAsync(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)
        {
            return await Task.FromResult(Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10));
        }

        public virtual IEnumerable<TOutput> Batch(IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> inputs)
        {
            foreach (var (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10) in inputs)
            {
                yield return Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
            }
        }

        public virtual async Task<IEnumerable<TOutput>> BatchAsync(IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> inputs)
        {
            var results = new List<TOutput>();
            foreach (var (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10) in inputs)
            {
                results.Add(await InvokeAsync(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10));
            }
            return results;
        }

        public virtual async IAsyncEnumerable<TOutput> Stream((T1, T2, T3, T4, T5, T6, T7, T8, T9, T10) input)
        {
            yield return await InvokeAsync(input.Item1, input.Item2, input.Item3, input.Item4, input.Item5, input.Item6, input.Item7, input.Item8, input.Item9, input.Item10);
        }
    }

    // ==================== Runnable with 11 parameters ====================
    
    /// <summary>
    /// Base Runnable abstract class with 11 input parameters
    /// </summary>
    public abstract class BaseRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOutput> : IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOutput>
    {
        public abstract TOutput Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11);

        public virtual async Task<TOutput> InvokeAsync(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11)
        {
            return await Task.FromResult(Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11));
        }

        public virtual IEnumerable<TOutput> Batch(IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> inputs)
        {
            foreach (var (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11) in inputs)
            {
                yield return Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11);
            }
        }

        public virtual async Task<IEnumerable<TOutput>> BatchAsync(IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> inputs)
        {
            var results = new List<TOutput>();
            foreach (var (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11) in inputs)
            {
                results.Add(await InvokeAsync(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11));
            }
            return results;
        }

        public virtual async IAsyncEnumerable<TOutput> Stream((T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11) input)
        {
            yield return await InvokeAsync(input.Item1, input.Item2, input.Item3, input.Item4, input.Item5, input.Item6, input.Item7, input.Item8, input.Item9, input.Item10, input.Item11);
        }
    }

    // ==================== Runnable with 12 parameters ====================
    
    /// <summary>
    /// Base Runnable abstract class with 12 input parameters
    /// </summary>
    public abstract class BaseRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOutput> : IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOutput>
    {
        public abstract TOutput Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12);

        public virtual async Task<TOutput> InvokeAsync(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12)
        {
            return await Task.FromResult(Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12));
        }

        public virtual IEnumerable<TOutput> Batch(IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> inputs)
        {
            foreach (var (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12) in inputs)
            {
                yield return Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12);
            }
        }

        public virtual async Task<IEnumerable<TOutput>> BatchAsync(IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> inputs)
        {
            var results = new List<TOutput>();
            foreach (var (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12) in inputs)
            {
                results.Add(await InvokeAsync(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12));
            }
            return results;
        }

        public virtual async IAsyncEnumerable<TOutput> Stream((T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12) input)
        {
            yield return await InvokeAsync(input.Item1, input.Item2, input.Item3, input.Item4, input.Item5, input.Item6, input.Item7, input.Item8, input.Item9, input.Item10, input.Item11, input.Item12);
        }
    }

    // ==================== Runnable with 13 parameters ====================
    
    /// <summary>
    /// Base Runnable abstract class with 13 input parameters
    /// </summary>
    public abstract class BaseRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOutput> : IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOutput>
    {
        public abstract TOutput Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13);

        public virtual async Task<TOutput> InvokeAsync(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13)
        {
            return await Task.FromResult(Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13));
        }

        public virtual IEnumerable<TOutput> Batch(IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13)> inputs)
        {
            foreach (var (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13) in inputs)
            {
                yield return Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13);
            }
        }

        public virtual async Task<IEnumerable<TOutput>> BatchAsync(IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13)> inputs)
        {
            var results = new List<TOutput>();
            foreach (var (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13) in inputs)
            {
                results.Add(await InvokeAsync(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13));
            }
            return results;
        }

        public virtual async IAsyncEnumerable<TOutput> Stream((T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13) input)
        {
            yield return await InvokeAsync(input.Item1, input.Item2, input.Item3, input.Item4, input.Item5, input.Item6, input.Item7, input.Item8, input.Item9, input.Item10, input.Item11, input.Item12, input.Item13);
        }
    }

    // ==================== Runnable with 14 parameters ====================
    
    /// <summary>
    /// Base Runnable abstract class with 14 input parameters
    /// </summary>
    public abstract class BaseRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOutput> : IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOutput>
    {
        public abstract TOutput Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14);

        public virtual async Task<TOutput> InvokeAsync(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14)
        {
            return await Task.FromResult(Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14));
        }

        public virtual IEnumerable<TOutput> Batch(IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14)> inputs)
        {
            foreach (var (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14) in inputs)
            {
                yield return Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14);
            }
        }

        public virtual async Task<IEnumerable<TOutput>> BatchAsync(IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14)> inputs)
        {
            var results = new List<TOutput>();
            foreach (var (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14) in inputs)
            {
                results.Add(await InvokeAsync(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14));
            }
            return results;
        }

        public virtual async IAsyncEnumerable<TOutput> Stream((T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14) input)
        {
            yield return await InvokeAsync(input.Item1, input.Item2, input.Item3, input.Item4, input.Item5, input.Item6, input.Item7, input.Item8, input.Item9, input.Item10, input.Item11, input.Item12, input.Item13, input.Item14);
        }
    }

    // ==================== Runnable with 15 parameters ====================
    
    /// <summary>
    /// Base Runnable abstract class with 15 input parameters
    /// </summary>
    public abstract class BaseRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOutput> : IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOutput>
    {
        public abstract TOutput Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15);

        public virtual async Task<TOutput> InvokeAsync(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15)
        {
            return await Task.FromResult(Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15));
        }

        public virtual IEnumerable<TOutput> Batch(IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15)> inputs)
        {
            foreach (var (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15) in inputs)
            {
                yield return Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15);
            }
        }

        public virtual async Task<IEnumerable<TOutput>> BatchAsync(IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15)> inputs)
        {
            var results = new List<TOutput>();
            foreach (var (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15) in inputs)
            {
                results.Add(await InvokeAsync(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15));
            }
            return results;
        }

        public virtual async IAsyncEnumerable<TOutput> Stream((T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15) input)
        {
            yield return await InvokeAsync(input.Item1, input.Item2, input.Item3, input.Item4, input.Item5, input.Item6, input.Item7, input.Item8, input.Item9, input.Item10, input.Item11, input.Item12, input.Item13, input.Item14, input.Item15);
        }
    }

    // ==================== Runnable with 16 parameters ====================
    
    /// <summary>
    /// Base Runnable abstract class with 16 input parameters
    /// </summary>
    public abstract class BaseRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TOutput> : IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TOutput>
    {
        public abstract TOutput Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16);

        public virtual async Task<TOutput> InvokeAsync(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16)
        {
            return await Task.FromResult(Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16));
        }

        public virtual IEnumerable<TOutput> Batch(IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16)> inputs)
        {
            foreach (var (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16) in inputs)
            {
                yield return Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16);
            }
        }

        public virtual async Task<IEnumerable<TOutput>> BatchAsync(IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16)> inputs)
        {
            var results = new List<TOutput>();
            foreach (var (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16) in inputs)
            {
                results.Add(await InvokeAsync(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16));
            }
            return results;
        }

        public virtual async IAsyncEnumerable<TOutput> Stream((T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16) input)
        {
            yield return await InvokeAsync(input.Item1, input.Item2, input.Item3, input.Item4, input.Item5, input.Item6, input.Item7, input.Item8, input.Item9, input.Item10, input.Item11, input.Item12, input.Item13, input.Item14, input.Item15, input.Item16);
        }
    }

}


