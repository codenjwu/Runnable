# BranchAsync Implementation Summary

## ? Implementation Complete!

### What Was Implemented

Extended **RunnableBranch** to add **CreateAsync** support for **0-16 parameters**, enabling asynchronous condition evaluation for routing logic.

## Changes Summary

| Feature | Before | After | Added |
|---------|--------|-------|-------|
| **Branch (Sync Conditions)** | 0-16 params (17 overloads) | 0-16 params (17 overloads) | ? Already complete |
| **BranchAsync (Async Conditions)** | 0 | 0-16 params (17 overloads) | **+17** ? |
| **Total Overloads** | 17 | 34 | **+17** ? |
| **Tests** | 157 | 178 | **+21** ? |

## Test Results

```
? Build successful
? 178 tests passed (up from 157)
? 0 tests failed
?? 1.8s execution time
```

## Files Modified/Created

### 1. **src/Runnable/RunnableBranch.cs** (Modified)
   - ? Kept existing `Create` (0-16 parameters with sync conditions)
   - ? Added `CreateAsync` for 0-16 parameters (all new with async conditions)
   - ?? **Total: 34 overloads** (17 Create + 17 CreateAsync)

### 2. **tests/Runnable.Tests/RunnableBranchAsyncTests.cs** (New)
   - ? 21 comprehensive tests for CreateAsync
   - Covers: basic, sync invocation, real-world, chaining, composition, errors, edge cases

## Implementation Pattern

Each `CreateAsync` method follows this pattern:

```csharp
public static Runnable<T1, T2, ..., TOutput> CreateAsync<T1, T2, ..., TOutput>(
    IRunnable<T1, T2, ..., TOutput> defaultBranch,
    params (Func<T1, T2, ..., Task<bool>> asyncCondition, IRunnable<T1, T2, ..., TOutput> runnable)[] branches)
{
    return new Runnable<T1, T2, ..., TOutput>(
        // Sync: Block on async conditions
        (a1, a2, ...) => {
            foreach (var (asyncCondition, runnable) in branches)
            {
                if (asyncCondition(a1, a2, ...).GetAwaiter().GetResult())
                    return runnable.Invoke(a1, a2, ...);
            }
            return defaultBranch.Invoke(a1, a2, ...);
        },
        
        // Async: True async condition evaluation with ConfigureAwait
        async (a1, a2, ...) => {
            foreach (var (asyncCondition, runnable) in branches)
            {
                if (await asyncCondition(a1, a2, ...).ConfigureAwait(false))
                    return await runnable.InvokeAsync(a1, a2, ...);
            }
            return await defaultBranch.InvokeAsync(a1, a2, ...);
        }
    );
}
```

## Key Features

### 1. Async Condition Evaluation

Execute async conditions for routing decisions:

```csharp
var router = RunnableBranch.CreateAsync(
    defaultHandler,
    (async userId => await database.IsPremiumAsync(userId), premiumHandler),
    (async userId => await database.IsActiveAsync(userId), activeHandler)
);

var result = await router.InvokeAsync(userId);
```

**Use Cases:**
- ? Database existence/status checks
- ? API authorization calls
- ? Rate limit validation
- ? Feature flag lookups
- ? External service checks
- ? Cache lookups

### 2. First-Match Semantics

Evaluates conditions sequentially and stops at the first match:

```csharp
var router = RunnableBranch.CreateAsync(
    defaultBranch,
    (condition1, handler1),  // ? Evaluated first
    (condition2, handler2),  // ? NOT evaluated if condition1 is true
    (condition3, handler3)   // ? NOT evaluated if condition1 or condition2 is true
);
```

### 3. Sync and Async Invocation

```csharp
// Sync invocation (blocks on async conditions)
var result = router.Invoke(input);

// Async invocation (true async)
var result = await router.InvokeAsync(input);
```

**Recommendation:** Always use `InvokeAsync` with `CreateAsync` for best performance.

## Usage Examples

### Example 1: Database-Based User Routing

```csharp
async Task<bool> IsPremiumUserAsync(int userId)
{
    await Task.Delay(15);  // Simulate DB query
    return await database.IsPremiumAsync(userId);
}

async Task<bool> IsActiveUserAsync(int userId)
{
    await Task.Delay(12);  // Simulate DB query
    return await database.IsActiveAsync(userId);
}

var premiumHandler = RunnableLambda.Create<int, string>(id => $"Premium user: {id}");
var activeHandler = RunnableLambda.Create<int, string>(id => $"Active user: {id}");
var inactiveHandler = RunnableLambda.Create<int, string>(id => $"Inactive user: {id}");

var router = RunnableBranch.CreateAsync(
    inactiveHandler,
    (async userId => await IsPremiumUserAsync(userId), premiumHandler),
    (async userId => await IsActiveUserAsync(userId), activeHandler)
);

var result = await router.InvokeAsync(userId);
```

### Example 2: API Authorization Routing

```csharp
async Task<bool> IsAdminAsync(string userId, string resource)
{
    return await authApi.CheckRoleAsync(userId, "admin");
}

async Task<bool> HasAccessAsync(string userId, string resource)
{
    return await authApi.CheckPermissionAsync(userId, resource);
}

var adminHandler = RunnableLambda.Create<string, string, string>(
    (userId, resource) => $"Admin access to {resource}");
var userHandler = RunnableLambda.Create<string, string, string>(
    (userId, resource) => $"User access to {resource}");
var deniedHandler = RunnableLambda.Create<string, string, string>(
    (userId, resource) => "Access denied");

var router = RunnableBranch.CreateAsync(
    deniedHandler,
    (async (userId, resource) => await IsAdminAsync(userId, resource), adminHandler),
    (async (userId, resource) => await HasAccessAsync(userId, resource), userHandler)
);

var result = await router.InvokeAsync("alice", "data");
```

### Example 3: Feature Flag Routing

```csharp
async Task<bool> IsFeatureEnabledAsync(string feature, string userId)
{
    return await featureFlags.IsEnabledForUserAsync(feature, userId);
}

var newUiHandler = RunnableLambda.Create<string, string>(user => $"New UI for {user}");
var betaHandler = RunnableLambda.Create<string, string>(user => $"Beta for {user}");
var defaultHandler = RunnableLambda.Create<string, string>(user => $"Default UI for {user}");

var router = RunnableBranch.CreateAsync(
    defaultHandler,
    (async user => await IsFeatureEnabledAsync("new-ui", user), newUiHandler),
    (async user => await IsFeatureEnabledAsync("beta-features", user), betaHandler)
);

var result = await router.InvokeAsync("alice");
```

### Example 4: Rate Limiting Router

```csharp
async Task<bool> IsWithinRateLimitAsync(string clientId)
{
    return await rateLimiter.AllowAsync(clientId);
}

async Task<bool> IsPremiumClientAsync(string clientId)
{
    return await database.IsPremiumAsync(clientId);
}

var premiumHandler = RunnableLambda.Create<string, string>(
    id => $"Premium processing for {id}");
var standardHandler = RunnableLambda.Create<string, string>(
    id => $"Standard processing for {id}");
var rateLimitedHandler = RunnableLambda.Create<string, string>(
    id => "Rate limit exceeded");

var router = RunnableBranch.CreateAsync(
    rateLimitedHandler,
    (async id => await IsPremiumClientAsync(id), premiumHandler),  // Premium: no rate limit
    (async id => await IsWithinRateLimitAsync(id), standardHandler)
);

var result = await router.InvokeAsync("client-123");
```

## Comparison Tables

### Branch vs BranchAsync

| Feature | Branch (Create) | BranchAsync (CreateAsync) |
|---------|----------------|---------------------------|
| **Condition Type** | `Func<T..., bool>` | `Func<T..., Task<bool>>` |
| **Execution** | Synchronous | Asynchronous |
| **Use Case** | Simple property checks | Database/API checks |
| **Performance** | Faster (no async overhead) | Enables async I/O |

**When to use Branch (Create):**
- Simple property checks
- In-memory validation
- Quick business rules
- No I/O operations

**When to use BranchAsync (CreateAsync):**
- Database lookups
- API authorization
- Feature flag checks
- External service calls
- Any async condition evaluation

## Real-World Scenarios

### Scenario 1: Authentication & Authorization Pipeline

```csharp
var authenticatedUsers = new HashSet<string> { "alice", "bob" };
var adminUsers = new HashSet<string> { "admin" };

async Task<bool> IsAdminAsync(string username)
{
    await Task.Delay(15);  // Simulate DB query
    return adminUsers.Contains(username);
}

async Task<bool> IsAuthenticatedAsync(string username)
{
    await Task.Delay(12);  // Simulate DB query
    return authenticatedUsers.Contains(username);
}

var adminHandler = RunnableLambda.Create<string, string>(
    u => $"Admin dashboard for {u}");
var userHandler = RunnableLambda.Create<string, string>(
    u => $"User dashboard for {u}");
var loginHandler = RunnableLambda.Create<string, string>(
    u => "Please login");

var authRouter = RunnableBranch.CreateAsync(
    loginHandler,
    (async u => await IsAdminAsync(u), adminHandler),
    (async u => await IsAuthenticatedAsync(u), userHandler)
);

var pipeline = authRouter
    .TapAsync(async result => await logger.LogAsync($"Route: {result}"))
    .Map(result => result.ToUpper());

var result = await pipeline.InvokeAsync("alice");
```

### Scenario 2: Content Delivery Network (CDN) Routing

```csharp
async Task<bool> IsPremiumContentAsync(int contentId)
{
    return await database.IsPremiumContentAsync(contentId);
}

async Task<bool> IsAvailableInRegionAsync(int contentId, string region)
{
    return await cdn.IsAvailableAsync(contentId, region);
}

var premiumHandler = RunnableLambda.Create<int, string, string>(
    (id, region) => $"Premium CDN: {id} in {region}");
var standardHandler = RunnableLambda.Create<int, string, string>(
    (id, region) => $"Standard CDN: {id} in {region}");
var notFoundHandler = RunnableLambda.Create<int, string, string>(
    (id, region) => "Content not available");

var router = RunnableBranch.CreateAsync(
    notFoundHandler,
    (async (id, region) => await IsPremiumContentAsync(id), premiumHandler),
    (async (id, region) => await IsAvailableInRegionAsync(id, region), standardHandler)
);

var pipeline = router
    .TapAsync(async result => await metrics.TrackAsync("content_delivery"))
    .MapAsync(async result => await cache.GetOrSetAsync(result));

var result = await pipeline.InvokeAsync(contentId, "us-west");
```

### Scenario 3: Multi-Tenant Routing

```csharp
async Task<bool> IsTenantActiveAsync(string tenantId)
{
    return await database.IsTenantActiveAsync(tenantId);
}

async Task<bool> HasFeatureAccessAsync(string tenantId, string feature)
{
    return await licenseManager.HasFeatureAsync(tenantId, feature);
}

var fullAccessHandler = RunnableLambda.Create<string, string, string>(
    (tenant, feature) => $"Full access: {tenant}/{feature}");
var limitedHandler = RunnableLambda.Create<string, string, string>(
    (tenant, feature) => $"Limited access: {tenant}");
var suspendedHandler = RunnableLambda.Create<string, string, string>(
    (tenant, feature) => "Tenant suspended");

var router = RunnableBranch.CreateAsync(
    suspendedHandler,
    (async (tenant, feature) => 
        await IsTenantActiveAsync(tenant) && await HasFeatureAccessAsync(tenant, feature),
        fullAccessHandler),
    (async (tenant, feature) => 
        await IsTenantActiveAsync(tenant),
        limitedHandler)
);

var result = await router.InvokeAsync("tenant-123", "analytics");
```

## Composition Examples

### With MapAsync

```csharp
var router = RunnableBranch.CreateAsync(
    defaultBranch,
    (async x => await CheckConditionAsync(x), branch1)
);

var pipeline = router.MapAsync(async result => {
    return await TransformAsync(result);
});
```

### With FilterAsync

```csharp
var router = RunnableBranch.CreateAsync(
    defaultBranch,
    (async x => await CheckConditionAsync(x), branch1)
);

var pipeline = router.FilterAsync(async result => {
    return await ValidateAsync(result);
}, fallbackValue);
```

### With TapAsync

```csharp
var router = RunnableBranch.CreateAsync(
    defaultBranch,
    (async x => await CheckConditionAsync(x), branch1)
);

var pipeline = router.TapAsync(async result => {
    await logger.LogAsync($"Routed to: {result}");
});
```

## Performance Characteristics

### Sync Invocation

```csharp
var result = router.Invoke(input);
// Uses .GetAwaiter().GetResult() - blocks thread
```

**Warning:** Sync invocation of CreateAsync can block. Use async when possible.

### Async Invocation

```csharp
var result = await router.InvokeAsync(input);
// True async execution with ConfigureAwait(false) - doesn't block
```

**Optimization:** Uses `ConfigureAwait(false)` for better performance in library code.

### Sequential Evaluation

Conditions are evaluated sequentially until the first match:

```csharp
var router = RunnableBranch.CreateAsync(
    defaultBranch,
    (condition1, handler1),  // ?? 15ms
    (condition2, handler2),  // ?? 12ms (NOT evaluated if condition1 is true)
    (condition3, handler3)   // ?? 10ms (NOT evaluated if condition1 or condition2 is true)
);

// If condition1 is true: Total time = 15ms
// If condition1 is false, condition2 is true: Total time = 27ms (15ms + 12ms)
// If both false, condition3 is true: Total time = 37ms (15ms + 12ms + 10ms)
```

## Error Handling

Both condition and handler exceptions propagate correctly:

```csharp
// Condition throws
var router = RunnableBranch.CreateAsync(
    defaultBranch,
    (async x => {
        throw new InvalidOperationException("Condition error");
    }, handler1)
);
await Assert.ThrowsAsync<InvalidOperationException>(() => router.InvokeAsync(input));

// Handler throws
var throwingHandler = RunnableLambda.Create<int, string>(x => throw new Exception());
var router = RunnableBranch.CreateAsync(
    defaultBranch,
    (async x => await Task.FromResult(true), throwingHandler)
);
await Assert.ThrowsAsync<Exception>(() => router.InvokeAsync(input));
```

## Coverage Status

**Complete parameter coverage for Branch family:**

| Extension | Sync (0-16) | Async (0-16) | Status |
|-----------|-------------|--------------|--------|
| **Branch (Create)** | ? | - | Complete (sync conditions) |
| **BranchAsync (CreateAsync)** | - | ? | Complete (async conditions) |

## Summary

? **CreateAsync: 17 new overloads** (0-16 parameters)  
? **21 comprehensive tests** added  
? **178 total tests** now passing  
? **100% backwards compatible**  
? **Production ready**  

## Key Benefits

1. **Async Condition Evaluation** - Database, API, service checks
2. **True Async/Await Support** - No thread blocking with InvokeAsync
3. **Sequential Evaluation** - Stops at first match for efficiency
4. **Type-Safe** - Full compile-time checking
5. **Composable** - Works with all Runnable extensions
6. **Well-Tested** - 21 tests covering all scenarios

## Comparison: Complete Library Status

Your Runnable library now has **comprehensive async support**:

| Feature | Sync (0-16) | Async (0-16) | Status |
|---------|-------------|--------------|--------|
| **Map** | ? | ? (MapAsync) | Complete |
| **Tap** | ? | ? (TapAsync) | Complete |
| **Filter** | ? | ? (FilterAsync) | Complete |
| **Branch** | ? (Create) | ? (CreateAsync) | Complete |
| **RunnableMap** | ? (Already async-optimized) | N/A | Complete |

?? **The Runnable library now has complete async support for routing, filtering, side effects, and transformations across all parameter arities!**

## Next Steps (Optional)

Consider these advanced features:
- **WithRetryAsync** - Async retry with exponential backoff
- **WithCircuitBreakerAsync** - Async circuit breaker pattern
- **WithCacheAsync** - Async cache with distributed cache support
- **BranchAsync with parallel condition evaluation** - Evaluate all conditions in parallel and pick first true

The foundation is now in place for comprehensive async pipeline construction with conditional routing! ??
