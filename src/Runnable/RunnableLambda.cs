using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Runnable
{
    // ==================== Lambda Runnable ====================
    
    /// <summary>
    /// Create a runnable from a simple lambda function
    /// </summary>
    public static class RunnableLambda
    {
        public static Runnable<TOutput> Create<TOutput>(Func<TOutput> func)
        {
            return new Runnable<TOutput>(func);
        }

        public static Runnable<TOutput> Create<TOutput>(
            Func<TOutput> syncFunc,
            Func<Task<TOutput>> asyncFunc)
        {
            return new Runnable<TOutput>(syncFunc, asyncFunc);
        }

        public static Runnable<TInput, TOutput> Create<TInput, TOutput>(Func<TInput, TOutput> func)
        {
            return new Runnable<TInput, TOutput>(func);
        }

        public static Runnable<TInput, TOutput> Create<TInput, TOutput>(
            Func<TInput, TOutput> syncFunc,
            Func<TInput, Task<TOutput>> asyncFunc)
        {
            return new Runnable<TInput, TOutput>(syncFunc, asyncFunc);
        }

        public static Runnable<T1, T2, TOutput> Create<T1, T2, TOutput>(
            Func<T1, T2, TOutput> func)
        {
            return new Runnable<T1, T2, TOutput>(func);
        }

        public static Runnable<T1, T2, TOutput> Create<T1, T2, TOutput>(
            Func<T1, T2, TOutput> syncFunc,
            Func<T1, T2, Task<TOutput>> asyncFunc)
        {
            return new Runnable<T1, T2, TOutput>(syncFunc, asyncFunc);
        }

        public static Runnable<T1, T2, T3, TOutput> Create<T1, T2, T3, TOutput>(
            Func<T1, T2, T3, TOutput> func)
        {
            return new Runnable<T1, T2, T3, TOutput>(func);
        }

        public static Runnable<T1, T2, T3, TOutput> Create<T1, T2, T3, TOutput>(
            Func<T1, T2, T3, TOutput> syncFunc,
            Func<T1, T2, T3, Task<TOutput>> asyncFunc)
        {
            return new Runnable<T1, T2, T3, TOutput>(syncFunc, asyncFunc);
        }

        public static Runnable<T1, T2, T3, T4, TOutput> Create<T1, T2, T3, T4, TOutput>(
            Func<T1, T2, T3, T4, TOutput> func)
        {
            return new Runnable<T1, T2, T3, T4, TOutput>(func);
        }

        public static Runnable<T1, T2, T3, T4, TOutput> Create<T1, T2, T3, T4, TOutput>(
            Func<T1, T2, T3, T4, TOutput> syncFunc,
            Func<T1, T2, T3, T4, Task<TOutput>> asyncFunc)
        {
            return new Runnable<T1, T2, T3, T4, TOutput>(syncFunc, asyncFunc);
        }

        public static Runnable<T1, T2, T3, T4, T5, TOutput> Create<T1, T2, T3, T4, T5, TOutput>(
            Func<T1, T2, T3, T4, T5, TOutput> func)
        {
            return new Runnable<T1, T2, T3, T4, T5, TOutput>(func);
        }

        public static Runnable<T1, T2, T3, T4, T5, TOutput> Create<T1, T2, T3, T4, T5, TOutput>(
            Func<T1, T2, T3, T4, T5, TOutput> syncFunc,
            Func<T1, T2, T3, T4, T5, Task<TOutput>> asyncFunc)
        {
            return new Runnable<T1, T2, T3, T4, T5, TOutput>(syncFunc, asyncFunc);
        }

        public static Runnable<T1, T2, T3, T4, T5, T6, TOutput> Create<T1, T2, T3, T4, T5, T6, TOutput>(
            Func<T1, T2, T3, T4, T5, T6, TOutput> func)
        {
            return new Runnable<T1, T2, T3, T4, T5, T6, TOutput>(func);
        }

        public static Runnable<T1, T2, T3, T4, T5, T6, TOutput> Create<T1, T2, T3, T4, T5, T6, TOutput>(
            Func<T1, T2, T3, T4, T5, T6, TOutput> syncFunc,
            Func<T1, T2, T3, T4, T5, T6, Task<TOutput>> asyncFunc)
        {
            return new Runnable<T1, T2, T3, T4, T5, T6, TOutput>(syncFunc, asyncFunc);
        }

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, TOutput> Create<T1, T2, T3, T4, T5, T6, T7, TOutput>(
            Func<T1, T2, T3, T4, T5, T6, T7, TOutput> func)
        {
            return new Runnable<T1, T2, T3, T4, T5, T6, T7, TOutput>(func);
        }

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, TOutput> Create<T1, T2, T3, T4, T5, T6, T7, TOutput>(
            Func<T1, T2, T3, T4, T5, T6, T7, TOutput> syncFunc,
            Func<T1, T2, T3, T4, T5, T6, T7, Task<TOutput>> asyncFunc)
        {
            return new Runnable<T1, T2, T3, T4, T5, T6, T7, TOutput>(syncFunc, asyncFunc);
        }

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, TOutput> Create<T1, T2, T3, T4, T5, T6, T7, T8, TOutput>(
            Func<T1, T2, T3, T4, T5, T6, T7, T8, TOutput> func)
        {
            return new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, TOutput>(func);
        }

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, TOutput> Create<T1, T2, T3, T4, T5, T6, T7, T8, TOutput>(
            Func<T1, T2, T3, T4, T5, T6, T7, T8, TOutput> syncFunc,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, Task<TOutput>> asyncFunc)
        {
            return new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, TOutput>(syncFunc, asyncFunc);
        }

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, TOutput> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, TOutput>(
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TOutput> func)
        {
            return new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, TOutput>(func);
        }

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, TOutput> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, TOutput>(
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TOutput> syncFunc,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, Task<TOutput>> asyncFunc)
        {
            return new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, TOutput>(syncFunc, asyncFunc);
        }

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOutput> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOutput>(
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOutput> func)
        {
            return new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOutput>(func);
        }

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOutput> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOutput>(
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOutput> syncFunc,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, Task<TOutput>> asyncFunc)
        {
            return new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOutput>(syncFunc, asyncFunc);
        }

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOutput> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOutput>(
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOutput> func)
        {
            return new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOutput>(func);
        }

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOutput> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOutput>(
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOutput> syncFunc,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, Task<TOutput>> asyncFunc)
        {
            return new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOutput>(syncFunc, asyncFunc);
        }

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOutput> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOutput>(
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOutput> func)
        {
            return new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOutput>(func);
        }

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOutput> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOutput>(
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOutput> syncFunc,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, Task<TOutput>> asyncFunc)
        {
            return new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOutput>(syncFunc, asyncFunc);
        }

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOutput> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOutput>(
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOutput> func)
        {
            return new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOutput>(func);
        }

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOutput> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOutput>(
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOutput> syncFunc,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, Task<TOutput>> asyncFunc)
        {
            return new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOutput>(syncFunc, asyncFunc);
        }

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOutput> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOutput>(
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOutput> func)
        {
            return new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOutput>(func);
        }

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOutput> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOutput>(
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOutput> syncFunc,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, Task<TOutput>> asyncFunc)
        {
            return new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOutput>(syncFunc, asyncFunc);
        }

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOutput> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOutput>(
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOutput> func)
        {
            return new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOutput>(func);
        }

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOutput> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOutput>(
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOutput> syncFunc,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, Task<TOutput>> asyncFunc)
        {
            return new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOutput>(syncFunc, asyncFunc);
        }

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TOutput> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TOutput>(
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TOutput> func)
        {
            return new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TOutput>(func);
        }

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TOutput> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TOutput>(
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TOutput> syncFunc,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, Task<TOutput>> asyncFunc)
        {
            return new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TOutput>(syncFunc, asyncFunc);
        }
    }
      
}
