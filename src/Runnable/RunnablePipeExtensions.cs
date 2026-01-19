using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Runnable
{
    /// <summary>
    /// Pipeline extension methods for composing Runnables.  
    /// Pipe feeds the output of one runnable into the input of another.
    /// </summary>
    public static partial class RunnablePipeExtensions
    {
        // ==================== Pipe for 0 parameters ====================
        
        /// <summary>
        /// Pipe the output of a no-input runnable into another runnable
        /// </summary>
        public static Runnable<TNext> Pipe<TOutput, TNext>(
            this IRunnable<TOutput> first,
            IRunnable<TOutput, TNext> second)
        {
            return new Runnable<TNext>(
                () => second.Invoke(first.Invoke()),
                async () => await second.InvokeAsync(await first.InvokeAsync()));
        }

        // ==================== Pipe for 1 parameter ====================
        
        /// <summary>
        /// Pipe the output of a runnable into another runnable (classic composition)
        /// </summary>
        public static Runnable<TInput, TNext> Pipe<TInput, TOutput, TNext>(
            this IRunnable<TInput, TOutput> first,
            IRunnable<TOutput, TNext> second)
        {
            return new Runnable<TInput, TNext>(
                input => second.Invoke(first.Invoke(input)),
                async input => await second.InvokeAsync(await first.InvokeAsync(input)));
        }

        // ==================== Pipe for 2 parameters ====================
        
        /// <summary>
        /// Pipe the output of a 2-parameter runnable into another runnable
        /// </summary>
        public static Runnable<T1, T2, TNext> Pipe<T1, T2, TOutput, TNext>(
            this IRunnable<T1, T2, TOutput> first,
            IRunnable<TOutput, TNext> second)
        {
            return new Runnable<T1, T2, TNext>(
                (a1, a2) => second.Invoke(first.Invoke(a1, a2)),
                async (a1, a2) => await second.InvokeAsync(await first.InvokeAsync(a1, a2)));
        }

        // ==================== Pipe for 3 parameters ====================
        
        /// <summary>
        /// Pipe the output of a 3-parameter runnable into another runnable
        /// </summary>
        public static Runnable<T1, T2, T3, TNext> Pipe<T1, T2, T3, TOutput, TNext>(
            this IRunnable<T1, T2, T3, TOutput> first,
            IRunnable<TOutput, TNext> second)
        {
            return new Runnable<T1, T2, T3, TNext>(
                (a1, a2, a3) => second.Invoke(first.Invoke(a1, a2, a3)),
                async (a1, a2, a3) => await second.InvokeAsync(await first.InvokeAsync(a1, a2, a3)));
        }

        // ==================== Pipe for 4 parameters ====================
        
        /// <summary>
        /// Pipe the output of a 4-parameter runnable into another runnable
        /// </summary>
        public static Runnable<T1, T2, T3, T4, TNext> Pipe<T1, T2, T3, T4, TOutput, TNext>(
            this IRunnable<T1, T2, T3, T4, TOutput> first,
            IRunnable<TOutput, TNext> second)
        {
            return new Runnable<T1, T2, T3, T4, TNext>(
                (a1, a2, a3, a4) => second.Invoke(first.Invoke(a1, a2, a3, a4)),
                async (a1, a2, a3, a4) => await second.InvokeAsync(await first.InvokeAsync(a1, a2, a3, a4)));
        }

        // ==================== Pipe for 5 parameters ====================
        
        /// <summary>
        /// Pipe the output of a 5-parameter runnable into another runnable
        /// </summary>
        public static Runnable<T1, T2, T3, T4, T5, TNext> Pipe<T1, T2, T3, T4, T5, TOutput, TNext>(
            this IRunnable<T1, T2, T3, T4, T5, TOutput> first,
            IRunnable<TOutput, TNext> second)
        {
            return new Runnable<T1, T2, T3, T4, T5, TNext>(
                (a1, a2, a3, a4, a5) => second.Invoke(first.Invoke(a1, a2, a3, a4, a5)),
                async (a1, a2, a3, a4, a5) => await second.InvokeAsync(await first.InvokeAsync(a1, a2, a3, a4, a5)));
        }

        // ==================== Pipe for 6 parameters ====================
        
        /// <summary>
        /// Pipe the output of a 6-parameter runnable into another runnable
        /// </summary>
        public static Runnable<T1, T2, T3, T4, T5, T6, TNext> Pipe<T1, T2, T3, T4, T5, T6, TOutput, TNext>(
            this IRunnable<T1, T2, T3, T4, T5, T6, TOutput> first,
            IRunnable<TOutput, TNext> second)
        {
            return new Runnable<T1, T2, T3, T4, T5, T6, TNext>(
                (a1, a2, a3, a4, a5, a6) => second.Invoke(first.Invoke(a1, a2, a3, a4, a5, a6)),
                async (a1, a2, a3, a4, a5, a6) => await second.InvokeAsync(await first.InvokeAsync(a1, a2, a3, a4, a5, a6)));
        }

        // ==================== Pipe for 7 parameters ====================
        
        /// <summary>
        /// Pipe the output of a 7-parameter runnable into another runnable
        /// </summary>
        public static Runnable<T1, T2, T3, T4, T5, T6, T7, TNext> Pipe<T1, T2, T3, T4, T5, T6, T7, TOutput, TNext>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, TOutput> first,
            IRunnable<TOutput, TNext> second)
        {
            return new Runnable<T1, T2, T3, T4, T5, T6, T7, TNext>(
                (a1, a2, a3, a4, a5, a6, a7) => second.Invoke(first.Invoke(a1, a2, a3, a4, a5, a6, a7)),
                async (a1, a2, a3, a4, a5, a6, a7) => await second.InvokeAsync(await first.InvokeAsync(a1, a2, a3, a4, a5, a6, a7)));
        }

        // ==================== Pipe for 8 parameters ====================
        
        /// <summary>
        /// Pipe the output of a 8-parameter runnable into another runnable
        /// </summary>
        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, TNext> Pipe<T1, T2, T3, T4, T5, T6, T7, T8, TOutput, TNext>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, TOutput> first,
            IRunnable<TOutput, TNext> second)
        {
            return new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, TNext>(
                (a1, a2, a3, a4, a5, a6, a7, a8) => second.Invoke(first.Invoke(a1, a2, a3, a4, a5, a6, a7, a8)),
                async (a1, a2, a3, a4, a5, a6, a7, a8) => await second.InvokeAsync(await first.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8)));
        }

        // ==================== Pipe for 9 parameters ====================
        
        /// <summary>
        /// Pipe the output of a 9-parameter runnable into another runnable
        /// </summary>
        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, TNext> Pipe<T1, T2, T3, T4, T5, T6, T7, T8, T9, TOutput, TNext>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, TOutput> first,
            IRunnable<TOutput, TNext> second)
        {
            return new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, TNext>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9) => second.Invoke(first.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9)),
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9) => await second.InvokeAsync(await first.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9)));
        }

        // ==================== Pipe for 10 parameters ====================
        
        /// <summary>
        /// Pipe the output of a 10-parameter runnable into another runnable
        /// </summary>
        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TNext> Pipe<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOutput, TNext>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOutput> first,
            IRunnable<TOutput, TNext> second)
        {
            return new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TNext>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10) => second.Invoke(first.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10)),
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10) => await second.InvokeAsync(await first.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10)));
        }

        // ==================== Pipe for 11 parameters ====================
        
        /// <summary>
        /// Pipe the output of a 11-parameter runnable into another runnable
        /// </summary>
        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TNext> Pipe<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOutput, TNext>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOutput> first,
            IRunnable<TOutput, TNext> second)
        {
            return new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TNext>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11) => second.Invoke(first.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11)),
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11) => await second.InvokeAsync(await first.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11)));
        }

        // ==================== Pipe for 12 parameters ====================
        
        /// <summary>
        /// Pipe the output of a 12-parameter runnable into another runnable
        /// </summary>
        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TNext> Pipe<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOutput, TNext>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOutput> first,
            IRunnable<TOutput, TNext> second)
        {
            return new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TNext>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12) => second.Invoke(first.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12)),
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12) => await second.InvokeAsync(await first.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12)));
        }

        // ==================== Pipe for 13 parameters ====================
        
        /// <summary>
        /// Pipe the output of a 13-parameter runnable into another runnable
        /// </summary>
        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TNext> Pipe<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOutput, TNext>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOutput> first,
            IRunnable<TOutput, TNext> second)
        {
            return new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TNext>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13) => second.Invoke(first.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13)),
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13) => await second.InvokeAsync(await first.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13)));
        }

        // ==================== Pipe for 14 parameters ====================
        
        /// <summary>
        /// Pipe the output of a 14-parameter runnable into another runnable
        /// </summary>
        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TNext> Pipe<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOutput, TNext>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOutput> first,
            IRunnable<TOutput, TNext> second)
        {
            return new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TNext>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14) => second.Invoke(first.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14)),
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14) => await second.InvokeAsync(await first.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14)));
        }

        // ==================== Pipe for 15 parameters ====================
        
        /// <summary>
        /// Pipe the output of a 15-parameter runnable into another runnable
        /// </summary>
        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TNext> Pipe<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOutput, TNext>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOutput> first,
            IRunnable<TOutput, TNext> second)
        {
            return new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TNext>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15) => second.Invoke(first.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15)),
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15) => await second.InvokeAsync(await first.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15)));
        }

        // ==================== Pipe for 16 parameters ====================
        
        /// <summary>
        /// Pipe the output of a 16-parameter runnable into another runnable
        /// </summary>
        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TNext> Pipe<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TOutput, TNext>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TOutput> first,
            IRunnable<TOutput, TNext> second)
        {
            return new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TNext>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16) => second.Invoke(first.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16)),
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16) => await second.InvokeAsync(await first.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16)));
        }
    }
}


