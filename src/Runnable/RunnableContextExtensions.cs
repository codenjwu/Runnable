using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Runnable
{
    /// <summary>
    /// Execution context for Runnable pipelines - supports correlation IDs, tracing, and custom metadata
    /// </summary>
    public class RunnableContext
    {
        private static readonly AsyncLocal<RunnableContext> _current = new AsyncLocal<RunnableContext>();

        private readonly Dictionary<string, object> _data = new Dictionary<string, object>();

        /// <summary>
        /// Gets or sets the current context for the async flow
        /// </summary>
        public static RunnableContext Current
        {
            get => _current.Value ?? (_current.Value = new RunnableContext());
            set => _current.Value = value;
        }

        /// <summary>
        /// Correlation ID for distributed tracing
        /// </summary>
        public string CorrelationId
        {
            get => GetValue<string>("CorrelationId") ?? Guid.NewGuid().ToString();
            set => SetValue("CorrelationId", value);
        }

        /// <summary>
        /// Parent span ID for distributed tracing
        /// </summary>
        public string ParentSpanId
        {
            get => GetValue<string>("ParentSpanId");
            set => SetValue("ParentSpanId", value);
        }

        /// <summary>
        /// Trace ID for distributed tracing (can be different from correlation ID)
        /// </summary>
        public string TraceId
        {
            get => GetValue<string>("TraceId") ?? CorrelationId;
            set => SetValue("TraceId", value);
        }

        /// <summary>
        /// User ID for audit logging
        /// </summary>
        public string UserId
        {
            get => GetValue<string>("UserId");
            set => SetValue("UserId", value);
        }

        /// <summary>
        /// Tenant ID for multi-tenant applications
        /// </summary>
        public string TenantId
        {
            get => GetValue<string>("TenantId");
            set => SetValue("TenantId", value);
        }

        /// <summary>
        /// Get a context value
        /// </summary>
        public T GetValue<T>(string key)
        {
            return _data.TryGetValue(key, out var value) ? (T)value : default;
        }

        /// <summary>
        /// Set a context value
        /// </summary>
        public void SetValue(string key, object value)
        {
            if (value == null)
                _data.Remove(key);
            else
                _data[key] = value;
        }

        /// <summary>
        /// Clear all context data
        /// </summary>
        public void Clear()
        {
            _data.Clear();
        }

        /// <summary>
        /// Get all context data as read-only dictionary
        /// </summary>
        public IReadOnlyDictionary<string, object> GetAllData()
        {
            return new Dictionary<string, object>(_data);
        }
    }

    /// <summary>
    /// Extensions for adding context to Runnable pipelines
    /// </summary>
    public static class RunnableContextExtensions
    {
        // ==================== WithContext (Set context values) ====================

        /// <summary>
        /// Set a context value before executing (no input version)
        /// </summary>
        public static Runnable<TOutput> WithContext<TOutput>(
            this IRunnable<TOutput> runnable,
            string key,
            object value)
        {
            return new Runnable<TOutput>(
                () =>
                {
                    var ctx = RunnableContext.Current;
                    if (ctx == null)
                    {
                        ctx = new RunnableContext();
                        RunnableContext.Current = ctx;
                    }
                    
                    ctx.SetValue(key, value);
                    return runnable.Invoke();
                },
                async () =>
                {
                    var ctx = RunnableContext.Current;
                    if (ctx == null)
                    {
                        ctx = new RunnableContext();
                        RunnableContext.Current = ctx;
                    }
                    
                    ctx.SetValue(key, value);
                    return await runnable.InvokeAsync();
                }
            );
        }

        /// <summary>
        /// Set a context value before executing (1 input version)
        /// </summary>
        public static Runnable<TInput, TOutput> WithContext<TInput, TOutput>(
            this IRunnable<TInput, TOutput> runnable,
            string key,
            object value)
        {
            return new Runnable<TInput, TOutput>(
                input =>
                {
                    // Capture or create context and ensure it flows
                    var ctx = RunnableContext.Current;
                    if (ctx == null)
                    {
                        ctx = new RunnableContext();
                        RunnableContext.Current = ctx;
                    }
                    
                    ctx.SetValue(key, value);
                    return runnable.Invoke(input);
                },
                async input =>
                {
                    // Capture or create context and ensure it flows
                    var ctx = RunnableContext.Current;
                    if (ctx == null)
                    {
                        ctx = new RunnableContext();
                        RunnableContext.Current = ctx;
                    }
                    
                    ctx.SetValue(key, value);
                    return await runnable.InvokeAsync(input);
                }
            );
        }

        /// <summary>
        /// Set a context value dynamically based on input (1 input version)
        /// </summary>
        public static Runnable<TInput, TOutput> WithContext<TInput, TOutput>(
            this IRunnable<TInput, TOutput> runnable,
            string key,
            Func<TInput, object> valueSelector)
        {
            return new Runnable<TInput, TOutput>(
                input =>
                {
                    var ctx = RunnableContext.Current;
                    if (ctx == null)
                    {
                        ctx = new RunnableContext();
                        RunnableContext.Current = ctx;
                    }
                    
                    var value = valueSelector(input);
                    ctx.SetValue(key, value);
                    return runnable.Invoke(input);
                },
                async input =>
                {
                    var ctx = RunnableContext.Current;
                    if (ctx == null)
                    {
                        ctx = new RunnableContext();
                        RunnableContext.Current = ctx;
                    }
                    
                    var value = valueSelector(input);
                    ctx.SetValue(key, value);
                    return await runnable.InvokeAsync(input);
                }
            );
        }

        // ==================== WithCorrelationId ====================

        /// <summary>
        /// Set correlation ID for distributed tracing (no input version)
        /// </summary>
        public static Runnable<TOutput> WithCorrelationId<TOutput>(
            this IRunnable<TOutput> runnable,
            string correlationId = null)
        {
            return runnable.WithContext("CorrelationId", correlationId ?? Guid.NewGuid().ToString());
        }

        /// <summary>
        /// Set correlation ID for distributed tracing (1 input version)
        /// </summary>
        public static Runnable<TInput, TOutput> WithCorrelationId<TInput, TOutput>(
            this IRunnable<TInput, TOutput> runnable,
            string correlationId = null)
        {
            return runnable.WithContext("CorrelationId", correlationId ?? Guid.NewGuid().ToString());
        }

        /// <summary>
        /// Extract correlation ID from input (1 input version)
        /// </summary>
        public static Runnable<TInput, TOutput> WithCorrelationId<TInput, TOutput>(
            this IRunnable<TInput, TOutput> runnable,
            Func<TInput, string> correlationIdExtractor)
        {
            return runnable.WithContext("CorrelationId", correlationIdExtractor);
        }

        // ==================== WithTenant (Multi-tenancy support) ====================

        /// <summary>
        /// Set tenant ID for multi-tenant applications (1 input version)
        /// </summary>
        public static Runnable<TInput, TOutput> WithTenant<TInput, TOutput>(
            this IRunnable<TInput, TOutput> runnable,
            string tenantId)
        {
            return runnable.WithContext("TenantId", tenantId);
        }

        /// <summary>
        /// Extract tenant ID from input (1 input version)
        /// </summary>
        public static Runnable<TInput, TOutput> WithTenant<TInput, TOutput>(
            this IRunnable<TInput, TOutput> runnable,
            Func<TInput, string> tenantIdExtractor)
        {
            return runnable.WithContext("TenantId", tenantIdExtractor);
        }

        // ==================== WithUser (Audit logging support) ====================

        /// <summary>
        /// Set user ID for audit logging (1 input version)
        /// </summary>
        public static Runnable<TInput, TOutput> WithUser<TInput, TOutput>(
            this IRunnable<TInput, TOutput> runnable,
            string userId)
        {
            return runnable.WithContext("UserId", userId);
        }

        /// <summary>
        /// Extract user ID from input (1 input version)
        /// </summary>
        public static Runnable<TInput, TOutput> WithUser<TInput, TOutput>(
            this IRunnable<TInput, TOutput> runnable,
            Func<TInput, string> userIdExtractor)
        {
            return runnable.WithContext("UserId", userIdExtractor);
        }

        // ==================== TapContext (Observe context) ====================

        /// <summary>
        /// Tap into context for logging/observability (no input version)
        /// </summary>
        public static Runnable<TOutput> TapContext<TOutput>(
            this IRunnable<TOutput> runnable,
            Action<RunnableContext> contextAction)
        {
            return new Runnable<TOutput>(
                () =>
                {
                    contextAction(RunnableContext.Current);
                    return runnable.Invoke();
                },
                async () =>
                {
                    contextAction(RunnableContext.Current);
                    return await runnable.InvokeAsync();
                }
            );
        }

        /// <summary>
        /// Tap into context for logging/observability (1 input version)
        /// </summary>
        public static Runnable<TInput, TOutput> TapContext<TInput, TOutput>(
            this IRunnable<TInput, TOutput> runnable,
            Action<TInput, RunnableContext> contextAction)
        {
            return new Runnable<TInput, TOutput>(
                input =>
                {
                    contextAction(input, RunnableContext.Current);
                    return runnable.Invoke(input);
                },
                async input =>
                {
                    contextAction(input, RunnableContext.Current);
                    return await runnable.InvokeAsync(input);
                }
            );
        }

        /// <summary>
        /// Tap into context asynchronously (1 input version)
        /// </summary>
        public static Runnable<TInput, TOutput> TapContextAsync<TInput, TOutput>(
            this IRunnable<TInput, TOutput> runnable,
            Func<TInput, RunnableContext, Task> contextAction)
        {
            return new Runnable<TInput, TOutput>(
                input => runnable.Invoke(input),
                async input =>
                {
                    await contextAction(input, RunnableContext.Current);
                    return await runnable.InvokeAsync(input);
                }
            );
        }
    }
}
