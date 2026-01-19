using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Runnable
{

    // ==================== Runnable with no parameters ====================
    /// <summary>
    /// Runnable implementation with no input parameters
    /// </summary>
    public partial class Runnable<TOutput> : BaseRunnable<TOutput>
    {
        private readonly Func<TOutput> _syncFunc;
        private readonly Func<Task<TOutput>> _asyncFunc;

        public Runnable(Func<TOutput> syncFunc, Func<Task<TOutput>> asyncFunc = null)
        {
            _syncFunc = syncFunc;
            _asyncFunc = asyncFunc;
        }

        public override TOutput Invoke()
        {
            return _syncFunc();
        }

        public override async Task<TOutput> InvokeAsync()
        {
            if (_asyncFunc != null)
            {
                return await _asyncFunc();
            }
            return await Task.FromResult(Invoke());
        }
    }
    // ==================== Runnable with 1 parameters ====================
    /// <summary>
    /// General Runnable implementation
    /// </summary>
    public partial class Runnable<T1, TOutput> : BaseRunnable<T1, TOutput>
    {
        private readonly Func<T1, TOutput> _syncFunc;
        private readonly Func<T1, Task<TOutput>> _asyncFunc;
        public Runnable(Func<T1, TOutput> syncFunc, Func<T1, Task<TOutput>> asyncFunc = null)
        {
            _syncFunc = syncFunc;
            _asyncFunc = asyncFunc;
        }

        public override TOutput Invoke(T1 arg1)
        {
            return _syncFunc(arg1);
        }

        public override async Task<TOutput> InvokeAsync(T1 arg1)
        {
            if (_asyncFunc != null)
            {
                return await _asyncFunc(arg1);
            }
            return await Task.FromResult(Invoke(arg1));
        } 
    }

    // ==================== Runnable with 2 parameters ====================
    /// <summary>
    /// Runnable implementation with 2 input parameters
    /// </summary>
    public partial class Runnable<T1, T2, TOutput> : BaseRunnable<T1, T2, TOutput>
    {
        private readonly Func<T1, T2, TOutput> _syncFunc;
        private readonly Func<T1, T2, Task<TOutput>> _asyncFunc;

        public Runnable(Func<T1, T2, TOutput> syncFunc, Func<T1, T2, Task<TOutput>> asyncFunc = null)
        {
            _syncFunc = syncFunc;
            _asyncFunc = asyncFunc;
        }

        public override TOutput Invoke(T1 arg1, T2 arg2)
        {
            return _syncFunc(arg1, arg2);
        }

        public override async Task<TOutput> InvokeAsync(T1 arg1, T2 arg2)
        {
            if (_asyncFunc != null)
            {
                return await _asyncFunc(arg1, arg2);
            }
            return await Task.FromResult(Invoke(arg1, arg2));
        }
    }
    // ==================== Runnable with 3 parameters ====================

    /// <summary>
    /// Runnable implementation with 3 input parameters
    /// </summary>
    public partial class Runnable<T1, T2, T3, TOutput> : BaseRunnable<T1, T2, T3, TOutput>
    {
        private readonly Func<T1, T2, T3, TOutput> _syncFunc;
        private readonly Func<T1, T2, T3, Task<TOutput>> _asyncFunc;

        public Runnable(Func<T1, T2, T3, TOutput> syncFunc, Func<T1, T2, T3, Task<TOutput>> asyncFunc = null)
        {
            _syncFunc = syncFunc;
            _asyncFunc = asyncFunc;
        }

        public override TOutput Invoke(T1 arg1, T2 arg2, T3 arg3)
        {
            return _syncFunc(arg1, arg2, arg3);
        }

        public override async Task<TOutput> InvokeAsync(T1 arg1, T2 arg2, T3 arg3)
        {
            if (_asyncFunc != null)
            {
                return await _asyncFunc(arg1, arg2, arg3);
            }
            return await Task.FromResult(Invoke(arg1, arg2, arg3));
        }
    }

    // ==================== Runnable with 4 parameters ====================

    /// <summary>
    /// Runnable implementation with 4 input parameters
    /// </summary>
    public partial class Runnable<T1, T2, T3, T4, TOutput> : BaseRunnable<T1, T2, T3, T4, TOutput>
    {
        private readonly Func<T1, T2, T3, T4, TOutput> _syncFunc;
        private readonly Func<T1, T2, T3, T4, Task<TOutput>> _asyncFunc;

        public Runnable(Func<T1, T2, T3, T4, TOutput> syncFunc, Func<T1, T2, T3, T4, Task<TOutput>> asyncFunc = null)
        {
            _syncFunc = syncFunc;
            _asyncFunc = asyncFunc;
        }

        public override TOutput Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            return _syncFunc(arg1, arg2, arg3, arg4);
        }

        public override async Task<TOutput> InvokeAsync(T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            if (_asyncFunc != null)
            {
                return await _asyncFunc(arg1, arg2, arg3, arg4);
            }
            return await Task.FromResult(Invoke(arg1, arg2, arg3, arg4));
        }
    }

    // ==================== Runnable with 5 parameters ====================

    /// <summary>
    /// Runnable implementation with 5 input parameters
    /// </summary>
    public partial class Runnable<T1, T2, T3, T4, T5, TOutput> : BaseRunnable<T1, T2, T3, T4, T5, TOutput>
    {
        private readonly Func<T1, T2, T3, T4, T5, TOutput> _syncFunc;
        private readonly Func<T1, T2, T3, T4, T5, Task<TOutput>> _asyncFunc;

        public Runnable(Func<T1, T2, T3, T4, T5, TOutput> syncFunc, Func<T1, T2, T3, T4, T5, Task<TOutput>> asyncFunc = null)
        {
            _syncFunc = syncFunc;
            _asyncFunc = asyncFunc;
        }

        public override TOutput Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            return _syncFunc(arg1, arg2, arg3, arg4, arg5);
        }

        public override async Task<TOutput> InvokeAsync(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            if (_asyncFunc != null)
            {
                return await _asyncFunc(arg1, arg2, arg3, arg4, arg5);
            }
            return await Task.FromResult(Invoke(arg1, arg2, arg3, arg4, arg5));
        }
    }

    // ==================== Runnable with 6 parameters ====================

    /// <summary>
    /// Runnable implementation with 6 input parameters
    /// </summary>
    public partial class Runnable<T1, T2, T3, T4, T5, T6, TOutput> : BaseRunnable<T1, T2, T3, T4, T5, T6, TOutput>
    {
        private readonly Func<T1, T2, T3, T4, T5, T6, TOutput> _syncFunc;
        private readonly Func<T1, T2, T3, T4, T5, T6, Task<TOutput>> _asyncFunc;

        public Runnable(Func<T1, T2, T3, T4, T5, T6, TOutput> syncFunc, Func<T1, T2, T3, T4, T5, T6, Task<TOutput>> asyncFunc = null)
        {
            _syncFunc = syncFunc;
            _asyncFunc = asyncFunc;
        }

        public override TOutput Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
        {
            return _syncFunc(arg1, arg2, arg3, arg4, arg5, arg6);
        }

        public override async Task<TOutput> InvokeAsync(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
        {
            if (_asyncFunc != null)
            {
                return await _asyncFunc(arg1, arg2, arg3, arg4, arg5, arg6);
            }
            return await Task.FromResult(Invoke(arg1, arg2, arg3, arg4, arg5, arg6));
        }
    }

    // ==================== Runnable with 7 parameters ====================

    /// <summary>
    /// Runnable implementation with 7 input parameters
    /// </summary>
    public partial class Runnable<T1, T2, T3, T4, T5, T6, T7, TOutput> : BaseRunnable<T1, T2, T3, T4, T5, T6, T7, TOutput>
    {
        private readonly Func<T1, T2, T3, T4, T5, T6, T7, TOutput> _syncFunc;
        private readonly Func<T1, T2, T3, T4, T5, T6, T7, Task<TOutput>> _asyncFunc;

        public Runnable(Func<T1, T2, T3, T4, T5, T6, T7, TOutput> syncFunc, Func<T1, T2, T3, T4, T5, T6, T7, Task<TOutput>> asyncFunc = null)
        {
            _syncFunc = syncFunc;
            _asyncFunc = asyncFunc;
        }

        public override TOutput Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
        {
            return _syncFunc(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        }

        public override async Task<TOutput> InvokeAsync(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
        {
            if (_asyncFunc != null)
            {
                return await _asyncFunc(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
            }
            return await Task.FromResult(Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7));
        }
    }

    // ==================== Runnable with 8 parameters ====================

    /// <summary>
    /// Runnable implementation with 8 input parameters
    /// </summary>
    public partial class Runnable<T1, T2, T3, T4, T5, T6, T7, T8, TOutput> : BaseRunnable<T1, T2, T3, T4, T5, T6, T7, T8, TOutput>
    {
        private readonly Func<T1, T2, T3, T4, T5, T6, T7, T8, TOutput> _syncFunc;
        private readonly Func<T1, T2, T3, T4, T5, T6, T7, T8, Task<TOutput>> _asyncFunc;

        public Runnable(Func<T1, T2, T3, T4, T5, T6, T7, T8, TOutput> syncFunc, Func<T1, T2, T3, T4, T5, T6, T7, T8, Task<TOutput>> asyncFunc = null)
        {
            _syncFunc = syncFunc;
            _asyncFunc = asyncFunc;
        }

        public override TOutput Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
        {
            return _syncFunc(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
        }

        public override async Task<TOutput> InvokeAsync(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
        {
            if (_asyncFunc != null)
            {
                return await _asyncFunc(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
            }
            return await Task.FromResult(Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8));
        }
    }

    // ==================== Runnable with 9 parameters ====================

    /// <summary>
    /// Runnable implementation with 9 input parameters
    /// </summary>
    public partial class Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, TOutput> : BaseRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, TOutput>
    {
        private readonly Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TOutput> _syncFunc;
        private readonly Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, Task<TOutput>> _asyncFunc;

        public Runnable(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TOutput> syncFunc, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, Task<TOutput>> asyncFunc = null)
        {
            _syncFunc = syncFunc;
            _asyncFunc = asyncFunc;
        }

        public override TOutput Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
        {
            return _syncFunc(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
        }

        public override async Task<TOutput> InvokeAsync(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
        {
            if (_asyncFunc != null)
            {
                return await _asyncFunc(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
            }
            return await Task.FromResult(Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9));
        }
    }

    // ==================== Runnable with 10 parameters ====================

    /// <summary>
    /// Runnable implementation with 10 input parameters
    /// </summary>
    public partial class Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOutput> : BaseRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOutput>
    {
        private readonly Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOutput> _syncFunc;
        private readonly Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, Task<TOutput>> _asyncFunc;

        public Runnable(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOutput> syncFunc, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, Task<TOutput>> asyncFunc = null)
        {
            _syncFunc = syncFunc;
            _asyncFunc = asyncFunc;
        }

        public override TOutput Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)
        {
            return _syncFunc(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
        }

        public override async Task<TOutput> InvokeAsync(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)
        {
            if (_asyncFunc != null)
            {
                return await _asyncFunc(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
            }
            return await Task.FromResult(Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10));
        }
    }

    // ==================== Runnable with 11 parameters ====================

    /// <summary>
    /// Runnable implementation with 11 input parameters
    /// </summary>
    public partial class Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOutput> : BaseRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOutput>
    {
        private readonly Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOutput> _syncFunc;
        private readonly Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, Task<TOutput>> _asyncFunc;

        public Runnable(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOutput> syncFunc, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, Task<TOutput>> asyncFunc = null)
        {
            _syncFunc = syncFunc;
            _asyncFunc = asyncFunc;
        }

        public override TOutput Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11)
        {
            return _syncFunc(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11);
        }

        public override async Task<TOutput> InvokeAsync(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11)
        {
            if (_asyncFunc != null)
            {
                return await _asyncFunc(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11);
            }
            return await Task.FromResult(Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11));
        }
    }

    // ==================== Runnable with 12 parameters ====================

    /// <summary>
    /// Runnable implementation with 12 input parameters
    /// </summary>
    public partial class Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOutput> : BaseRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOutput>
    {
        private readonly Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOutput> _syncFunc;
        private readonly Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, Task<TOutput>> _asyncFunc;

        public Runnable(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOutput> syncFunc, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, Task<TOutput>> asyncFunc = null)
        {
            _syncFunc = syncFunc;
            _asyncFunc = asyncFunc;
        }

        public override TOutput Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12)
        {
            return _syncFunc(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12);
        }

        public override async Task<TOutput> InvokeAsync(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12)
        {
            if (_asyncFunc != null)
            {
                return await _asyncFunc(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12);
            }
            return await Task.FromResult(Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12));
        }
    }

    // ==================== Runnable with 13 parameters ====================

    /// <summary>
    /// Runnable implementation with 13 input parameters
    /// </summary>
    public partial class Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOutput> : BaseRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOutput>
    {
        private readonly Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOutput> _syncFunc;
        private readonly Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, Task<TOutput>> _asyncFunc;

        public Runnable(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOutput> syncFunc, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, Task<TOutput>> asyncFunc = null)
        {
            _syncFunc = syncFunc;
            _asyncFunc = asyncFunc;
        }

        public override TOutput Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13)
        {
            return _syncFunc(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13);
        }

        public override async Task<TOutput> InvokeAsync(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13)
        {
            if (_asyncFunc != null)
            {
                return await _asyncFunc(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13);
            }
            return await Task.FromResult(Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13));
        }
    }

    // ==================== Runnable with 14 parameters ====================

    /// <summary>
    /// Runnable implementation with 14 input parameters
    /// </summary>
    public partial class Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOutput> : BaseRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOutput>
    {
        private readonly Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOutput> _syncFunc;
        private readonly Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, Task<TOutput>> _asyncFunc;

        public Runnable(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOutput> syncFunc, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, Task<TOutput>> asyncFunc = null)
        {
            _syncFunc = syncFunc;
            _asyncFunc = asyncFunc;
        }

        public override TOutput Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14)
        {
            return _syncFunc(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14);
        }

        public override async Task<TOutput> InvokeAsync(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14)
        {
            if (_asyncFunc != null)
            {
                return await _asyncFunc(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14);
            }
            return await Task.FromResult(Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14));
        }
    }

    // ==================== Runnable with 15 parameters ====================

    /// <summary>
    /// Runnable implementation with 15 input parameters
    /// </summary>
    public partial class Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOutput> : BaseRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOutput>
    {
        private readonly Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOutput> _syncFunc;
        private readonly Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, Task<TOutput>> _asyncFunc;

        public Runnable(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOutput> syncFunc, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, Task<TOutput>> asyncFunc = null)
        {
            _syncFunc = syncFunc;
            _asyncFunc = asyncFunc;
        }

        public override TOutput Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15)
        {
            return _syncFunc(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15);
        }

        public override async Task<TOutput> InvokeAsync(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15)
        {
            if (_asyncFunc != null)
            {
                return await _asyncFunc(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15);
            }
            return await Task.FromResult(Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15));
        }
    }

    // ==================== Runnable with 16 parameters ====================

    /// <summary>
    /// Runnable implementation with 16 input parameters
    /// </summary>
    public partial class Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TOutput> : BaseRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TOutput>
    {
        private readonly Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TOutput> _syncFunc;
        private readonly Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, Task<TOutput>> _asyncFunc;

        public Runnable(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TOutput> syncFunc, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, Task<TOutput>> asyncFunc = null)
        {
            _syncFunc = syncFunc;
            _asyncFunc = asyncFunc;
        }

        public override TOutput Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16)
        {
            return _syncFunc(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16);
        }

        public override async Task<TOutput> InvokeAsync(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16)
        {
            if (_asyncFunc != null)
            {
                return await _asyncFunc(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16);
            }
            return await Task.FromResult(Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16));
        }
    }
}