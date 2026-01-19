using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Runnable
{ 
    // ==================== Passthrough Runnable ====================

    /// <summary>
    /// Pass input through unchanged
    /// </summary>
    public static class RunnablePassthrough
    {
        public static Runnable<T, T> Create<T>()
        {
            return new Runnable<T, T>(x => x);
        }

        /// <summary>
        /// Passthrough with side effect (tap)
        /// </summary>
        public static Runnable<T, T> Create<T>(Action<T> sideEffect)
        {
            return new Runnable<T, T>(x =>
            {
                sideEffect(x);
                return x;
            });
        }

        /// <summary>
        /// Async passthrough with side effect
        /// </summary>
        public static Runnable<T, T> Create<T>(Func<T, Task> asyncSideEffect)
        {
            return new Runnable<T, T>(
                x => x,
                async x =>
                {
                    await asyncSideEffect(x);
                    return x;
                }
            );
        }
    }
}
