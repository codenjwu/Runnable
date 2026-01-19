using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Runnable
{
    // Extension methods for Runnable with 0-5+ parameters
    public static class RunnableAsRunnableExtensions
    {
        // ==================== 0 Parameters ====================
        
        public static Runnable<TOutput> AsRunnable<TOutput>(
            this Func<TOutput> func)
        {
            return RunnableLambda.Create(func);
        }

        public static Runnable<TOutput> AsRunnable<TOutput>(
            this Func<TOutput> syncFunc,
            Func<Task<TOutput>> asyncFunc)
        {
            return RunnableLambda.Create(syncFunc, asyncFunc);
        }

        // ==================== 1 Parameter ====================
        
        public static Runnable<T1, TOutput> AsRunnable<T1, TOutput>(
            this Func<T1, TOutput> func)
        {
            return RunnableLambda.Create(func);
        }

        public static Runnable<T1, TOutput> AsRunnable<T1, TOutput>(
            this Func<T1, TOutput> syncFunc,
            Func<T1, Task<TOutput>> asyncFunc)
        {
            return RunnableLambda.Create(syncFunc, asyncFunc);
        }

        // ==================== 2 Parameters ====================
        
        public static Runnable<T1, T2, TOutput> AsRunnable<T1, T2, TOutput>(
            this Func<T1, T2, TOutput> func)
        {
            return RunnableLambda.Create(func);
        }

        public static Runnable<T1, T2, TOutput> AsRunnable<T1, T2, TOutput>(
            this Func<T1, T2, TOutput> syncFunc,
            Func<T1, T2, Task<TOutput>> asyncFunc)
        {
            return RunnableLambda.Create(syncFunc, asyncFunc);
        }

        // ==================== 3 Parameters ====================
        
        public static Runnable<T1, T2, T3, TOutput> AsRunnable<T1, T2, T3, TOutput>(
            this Func<T1, T2, T3, TOutput> func)
        {
            return RunnableLambda.Create(func);
        }

        public static Runnable<T1, T2, T3, TOutput> AsRunnable<T1, T2, T3, TOutput>(
            this Func<T1, T2, T3, TOutput> syncFunc,
            Func<T1, T2, T3, Task<TOutput>> asyncFunc)
        {
            return RunnableLambda.Create(syncFunc, asyncFunc);
        }

        // ==================== 4 Parameters ====================
        
        public static Runnable<T1, T2, T3, T4, TOutput> AsRunnable<T1, T2, T3, T4, TOutput>(
            this Func<T1, T2, T3, T4, TOutput> func)
        {
            return RunnableLambda.Create(func);
        }

        public static Runnable<T1, T2, T3, T4, TOutput> AsRunnable<T1, T2, T3, T4, TOutput>(
            this Func<T1, T2, T3, T4, TOutput> syncFunc,
            Func<T1, T2, T3, T4, Task<TOutput>> asyncFunc)
        {
            return RunnableLambda.Create(syncFunc, asyncFunc);
        }

        // ==================== 5 Parameters ====================
        
        public static Runnable<T1, T2, T3, T4, T5, TOutput> AsRunnable<T1, T2, T3, T4, T5, TOutput>(
            this Func<T1, T2, T3, T4, T5, TOutput> func)
        {
            return RunnableLambda.Create(func);
        }

        public static Runnable<T1, T2, T3, T4, T5, TOutput> AsRunnable<T1, T2, T3, T4, T5, TOutput>(
            this Func<T1, T2, T3, T4, T5, TOutput> syncFunc,
            Func<T1, T2, T3, T4, T5, Task<TOutput>> asyncFunc)
        {
            return RunnableLambda.Create(syncFunc, asyncFunc);
        }

        // ==================== 6 Parameters ====================
        
        public static Runnable<T1, T2, T3, T4, T5, T6, TOutput> AsRunnable<T1, T2, T3, T4, T5, T6, TOutput>(
            this Func<T1, T2, T3, T4, T5, T6, TOutput> func)
        {
            return RunnableLambda.Create(func);
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
