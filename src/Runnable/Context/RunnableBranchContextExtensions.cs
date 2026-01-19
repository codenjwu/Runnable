using System;
using System.Threading.Tasks;

namespace Runnable
{
    /// <summary>
    /// Extensions for branching/routing based on RunnableContext for tenant-specific logic and A/B testing
    /// </summary>
    public static class RunnableBranchContextExtensions
    {
        // ==================== BranchContext (Route based on context) ====================

        /// <summary>
        /// Branch based on context predicate (1 input version)
        /// </summary>
        public static Runnable<TInput, TOutput> BranchContext<TInput, TOutput>(
            this IRunnable<TInput, TOutput> defaultRunnable,
            Func<TInput, RunnableContext, bool> predicate,
            IRunnable<TInput, TOutput> branchRunnable)
        {
            return new Runnable<TInput, TOutput>(
                input => predicate(input, RunnableContext.Current) 
                    ? branchRunnable.Invoke(input) 
                    : defaultRunnable.Invoke(input),
                async input => predicate(input, RunnableContext.Current)
                    ? await branchRunnable.InvokeAsync(input)
                    : await defaultRunnable.InvokeAsync(input)
            );
        }

        /// <summary>
        /// Branch based on context predicate with async evaluation (1 input version)
        /// </summary>
        public static Runnable<TInput, TOutput> BranchAsyncContext<TInput, TOutput>(
            this IRunnable<TInput, TOutput> defaultRunnable,
            Func<TInput, RunnableContext, Task<bool>> predicate,
            IRunnable<TInput, TOutput> branchRunnable)
        {
            return new Runnable<TInput, TOutput>(
                input => predicate(input, RunnableContext.Current).GetAwaiter().GetResult()
                    ? branchRunnable.Invoke(input)
                    : defaultRunnable.Invoke(input),
                async input => await predicate(input, RunnableContext.Current)
                    ? await branchRunnable.InvokeAsync(input)
                    : await defaultRunnable.InvokeAsync(input)
            );
        }

        /// <summary>
        /// Multi-way branch based on context (1 input version)
        /// </summary>
        public static Runnable<TInput, TOutput> BranchContext<TInput, TOutput>(
            this IRunnable<TInput, TOutput> defaultRunnable,
            params (Func<TInput, RunnableContext, bool> predicate, IRunnable<TInput, TOutput> runnable)[] branches)
        {
            return new Runnable<TInput, TOutput>(
                input =>
                {
                    var ctx = RunnableContext.Current;
                    foreach (var (predicate, runnable) in branches)
                    {
                        if (predicate(input, ctx))
                            return runnable.Invoke(input);
                    }
                    return defaultRunnable.Invoke(input);
                },
                async input =>
                {
                    var ctx = RunnableContext.Current;
                    foreach (var (predicate, runnable) in branches)
                    {
                        if (predicate(input, ctx))
                            return await runnable.InvokeAsync(input);
                    }
                    return await defaultRunnable.InvokeAsync(input);
                }
            );
        }

        // ==================== Common branching patterns ====================

        /// <summary>
        /// Branch based on tenant ID
        /// </summary>
        public static Runnable<TInput, TOutput> BranchByTenant<TInput, TOutput>(
            this IRunnable<TInput, TOutput> defaultRunnable,
            string tenantId,
            IRunnable<TInput, TOutput> tenantRunnable)
        {
            return defaultRunnable.BranchContext(
                (input, ctx) => ctx.TenantId == tenantId,
                tenantRunnable);
        }

        /// <summary>
        /// Branch based on multiple tenants
        /// </summary>
        public static Runnable<TInput, TOutput> BranchByTenants<TInput, TOutput>(
            this IRunnable<TInput, TOutput> defaultRunnable,
            params (string tenantId, IRunnable<TInput, TOutput> runnable)[] tenantBranches)
        {
            var branches = new (Func<TInput, RunnableContext, bool>, IRunnable<TInput, TOutput>)[tenantBranches.Length];
            for (int i = 0; i < tenantBranches.Length; i++)
            {
                var (tenantId, runnable) = tenantBranches[i];
                branches[i] = ((input, ctx) => ctx.TenantId == tenantId, runnable);
            }
            return defaultRunnable.BranchContext(branches);
        }

        /// <summary>
        /// Branch based on user ID
        /// </summary>
        public static Runnable<TInput, TOutput> BranchByUser<TInput, TOutput>(
            this IRunnable<TInput, TOutput> defaultRunnable,
            string userId,
            IRunnable<TInput, TOutput> userRunnable)
        {
            return defaultRunnable.BranchContext(
                (input, ctx) => ctx.UserId == userId,
                userRunnable);
        }

        /// <summary>
        /// Branch based on context key existence
        /// </summary>
        public static Runnable<TInput, TOutput> BranchByContextKey<TInput, TOutput>(
            this IRunnable<TInput, TOutput> defaultRunnable,
            string key,
            IRunnable<TInput, TOutput> branchRunnable)
        {
            return defaultRunnable.BranchContext(
                (input, ctx) => ctx.GetValue<object>(key) != null,
                branchRunnable);
        }

        /// <summary>
        /// Branch based on context key value
        /// </summary>
        public static Runnable<TInput, TOutput> BranchByContextValue<TInput, TOutput, TValue>(
            this IRunnable<TInput, TOutput> defaultRunnable,
            string key,
            TValue expectedValue,
            IRunnable<TInput, TOutput> branchRunnable)
        {
            return defaultRunnable.BranchContext(
                (input, ctx) =>
                {
                    var value = ctx.GetValue<TValue>(key);
                    return value != null && value.Equals(expectedValue);
                },
                branchRunnable);
        }

        /// <summary>
        /// A/B testing based on context
        /// </summary>
        public static Runnable<TInput, TOutput> ABTestContext<TInput, TOutput>(
            this IRunnable<TInput, TOutput> defaultRunnable,
            string experimentKey,
            IRunnable<TInput, TOutput> variantRunnable)
        {
            return defaultRunnable.BranchContext(
                (input, ctx) =>
                {
                    var variant = ctx.GetValue<string>(experimentKey);
                    return variant == "B" || variant == "variant";
                },
                variantRunnable);
        }

        /// <summary>
        /// Branch based on debug/production mode
        /// </summary>
        public static Runnable<TInput, TOutput> BranchByDebugMode<TInput, TOutput>(
            this IRunnable<TInput, TOutput> productionRunnable,
            IRunnable<TInput, TOutput> debugRunnable)
        {
            return productionRunnable.BranchContext(
                (input, ctx) => ctx.GetValue<bool>("IsDebug") || ctx.GetValue<bool>("DebugMode"),
                debugRunnable);
        }

        // ==================== BranchContext for 2-15 parameters ====================

        public static Runnable<T1, T2, TOutput> BranchContext<T1, T2, TOutput>(
            this IRunnable<T1, T2, TOutput> defaultRunnable,
            Func<T1, T2, RunnableContext, bool> predicate,
            IRunnable<T1, T2, TOutput> branchRunnable) =>
            new Runnable<T1, T2, TOutput>(
                (a1, a2) => predicate(a1, a2, RunnableContext.Current)
                    ? branchRunnable.Invoke(a1, a2)
                    : defaultRunnable.Invoke(a1, a2),
                async (a1, a2) => predicate(a1, a2, RunnableContext.Current)
                    ? await branchRunnable.InvokeAsync(a1, a2)
                    : await defaultRunnable.InvokeAsync(a1, a2));

        public static Runnable<T1, T2, T3, TOutput> BranchContext<T1, T2, T3, TOutput>(
            this IRunnable<T1, T2, T3, TOutput> defaultRunnable,
            Func<T1, T2, T3, RunnableContext, bool> predicate,
            IRunnable<T1, T2, T3, TOutput> branchRunnable) =>
            new Runnable<T1, T2, T3, TOutput>(
                (a1, a2, a3) => predicate(a1, a2, a3, RunnableContext.Current)
                    ? branchRunnable.Invoke(a1, a2, a3)
                    : defaultRunnable.Invoke(a1, a2, a3),
                async (a1, a2, a3) => predicate(a1, a2, a3, RunnableContext.Current)
                    ? await branchRunnable.InvokeAsync(a1, a2, a3)
                    : await defaultRunnable.InvokeAsync(a1, a2, a3));

        public static Runnable<T1, T2, T3, T4, TOutput> BranchContext<T1, T2, T3, T4, TOutput>(
            this IRunnable<T1, T2, T3, T4, TOutput> defaultRunnable,
            Func<T1, T2, T3, T4, RunnableContext, bool> predicate,
            IRunnable<T1, T2, T3, T4, TOutput> branchRunnable) =>
            new Runnable<T1, T2, T3, T4, TOutput>(
                (a1, a2, a3, a4) => predicate(a1, a2, a3, a4, RunnableContext.Current)
                    ? branchRunnable.Invoke(a1, a2, a3, a4)
                    : defaultRunnable.Invoke(a1, a2, a3, a4),
                async (a1, a2, a3, a4) => predicate(a1, a2, a3, a4, RunnableContext.Current)
                    ? await branchRunnable.InvokeAsync(a1, a2, a3, a4)
                    : await defaultRunnable.InvokeAsync(a1, a2, a3, a4));

        public static Runnable<T1, T2, T3, T4, T5, TOutput> BranchContext<T1, T2, T3, T4, T5, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, TOutput> defaultRunnable,
            Func<T1, T2, T3, T4, T5, RunnableContext, bool> predicate,
            IRunnable<T1, T2, T3, T4, T5, TOutput> branchRunnable) =>
            new Runnable<T1, T2, T3, T4, T5, TOutput>(
                (a1, a2, a3, a4, a5) => predicate(a1, a2, a3, a4, a5, RunnableContext.Current)
                    ? branchRunnable.Invoke(a1, a2, a3, a4, a5)
                    : defaultRunnable.Invoke(a1, a2, a3, a4, a5),
                async (a1, a2, a3, a4, a5) => predicate(a1, a2, a3, a4, a5, RunnableContext.Current)
                    ? await branchRunnable.InvokeAsync(a1, a2, a3, a4, a5)
                    : await defaultRunnable.InvokeAsync(a1, a2, a3, a4, a5));

        public static Runnable<T1, T2, T3, T4, T5, T6, TOutput> BranchContext<T1, T2, T3, T4, T5, T6, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, TOutput> defaultRunnable,
            Func<T1, T2, T3, T4, T5, T6, RunnableContext, bool> predicate,
            IRunnable<T1, T2, T3, T4, T5, T6, TOutput> branchRunnable) =>
            new Runnable<T1, T2, T3, T4, T5, T6, TOutput>(
                (a1, a2, a3, a4, a5, a6) => predicate(a1, a2, a3, a4, a5, a6, RunnableContext.Current)
                    ? branchRunnable.Invoke(a1, a2, a3, a4, a5, a6)
                    : defaultRunnable.Invoke(a1, a2, a3, a4, a5, a6),
                async (a1, a2, a3, a4, a5, a6) => predicate(a1, a2, a3, a4, a5, a6, RunnableContext.Current)
                    ? await branchRunnable.InvokeAsync(a1, a2, a3, a4, a5, a6)
                    : await defaultRunnable.InvokeAsync(a1, a2, a3, a4, a5, a6));

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, TOutput> BranchContext<T1, T2, T3, T4, T5, T6, T7, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, TOutput> defaultRunnable,
            Func<T1, T2, T3, T4, T5, T6, T7, RunnableContext, bool> predicate,
            IRunnable<T1, T2, T3, T4, T5, T6, T7, TOutput> branchRunnable) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7) => predicate(a1, a2, a3, a4, a5, a6, a7, RunnableContext.Current)
                    ? branchRunnable.Invoke(a1, a2, a3, a4, a5, a6, a7)
                    : defaultRunnable.Invoke(a1, a2, a3, a4, a5, a6, a7),
                async (a1, a2, a3, a4, a5, a6, a7) => predicate(a1, a2, a3, a4, a5, a6, a7, RunnableContext.Current)
                    ? await branchRunnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7)
                    : await defaultRunnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7));

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, TOutput> BranchContext<T1, T2, T3, T4, T5, T6, T7, T8, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, TOutput> defaultRunnable,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, RunnableContext, bool> predicate,
            IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, TOutput> branchRunnable) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8) => predicate(a1, a2, a3, a4, a5, a6, a7, a8, RunnableContext.Current)
                    ? branchRunnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8)
                    : defaultRunnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8),
                async (a1, a2, a3, a4, a5, a6, a7, a8) => predicate(a1, a2, a3, a4, a5, a6, a7, a8, RunnableContext.Current)
                    ? await branchRunnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8)
                    : await defaultRunnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8));

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, TOutput> BranchContext<T1, T2, T3, T4, T5, T6, T7, T8, T9, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, TOutput> defaultRunnable,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, RunnableContext, bool> predicate,
            IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, TOutput> branchRunnable) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9) => predicate(a1, a2, a3, a4, a5, a6, a7, a8, a9, RunnableContext.Current)
                    ? branchRunnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9)
                    : defaultRunnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9),
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9) => predicate(a1, a2, a3, a4, a5, a6, a7, a8, a9, RunnableContext.Current)
                    ? await branchRunnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9)
                    : await defaultRunnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9));

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOutput> BranchContext<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOutput> defaultRunnable,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, RunnableContext, bool> predicate,
            IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOutput> branchRunnable) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10) => predicate(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, RunnableContext.Current)
                    ? branchRunnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10)
                    : defaultRunnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10),
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10) => predicate(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, RunnableContext.Current)
                    ? await branchRunnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10)
                    : await defaultRunnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10));

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOutput> BranchContext<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOutput> defaultRunnable,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, RunnableContext, bool> predicate,
            IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOutput> branchRunnable) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11) => predicate(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, RunnableContext.Current)
                    ? branchRunnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11)
                    : defaultRunnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11),
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11) => predicate(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, RunnableContext.Current)
                    ? await branchRunnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11)
                    : await defaultRunnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11));

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOutput> BranchContext<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOutput> defaultRunnable,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, RunnableContext, bool> predicate,
            IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOutput> branchRunnable) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12) => predicate(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, RunnableContext.Current)
                    ? branchRunnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12)
                    : defaultRunnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12),
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12) => predicate(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, RunnableContext.Current)
                    ? await branchRunnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12)
                    : await defaultRunnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12));

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOutput> BranchContext<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOutput> defaultRunnable,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, RunnableContext, bool> predicate,
            IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOutput> branchRunnable) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13) => predicate(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, RunnableContext.Current)
                    ? branchRunnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13)
                    : defaultRunnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13),
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13) => predicate(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, RunnableContext.Current)
                    ? await branchRunnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13)
                    : await defaultRunnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13));

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOutput> BranchContext<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOutput> defaultRunnable,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, RunnableContext, bool> predicate,
            IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOutput> branchRunnable) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14) => predicate(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, RunnableContext.Current)
                    ? branchRunnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14)
                    : defaultRunnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14),
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14) => predicate(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, RunnableContext.Current)
                    ? await branchRunnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14)
                    : await defaultRunnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14));

        public static Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOutput> BranchContext<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOutput>(
            this IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOutput> defaultRunnable,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, RunnableContext, bool> predicate,
            IRunnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOutput> branchRunnable) =>
            new Runnable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOutput>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15) => predicate(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, RunnableContext.Current)
                    ? branchRunnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15)
                    : defaultRunnable.Invoke(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15),
                async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15) => predicate(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, RunnableContext.Current)
                    ? await branchRunnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15)
                    : await defaultRunnable.InvokeAsync(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15));
    }
}
