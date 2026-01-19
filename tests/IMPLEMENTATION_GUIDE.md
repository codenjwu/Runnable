# ?? Runnable Library v1.1 - Implementation Guide

## Quick Start: Using New Features

### 1. ? Exception Type Filtering

**Before** (catches everything):
```csharp
var result = riskyOperation
    .WithFallback(fallbackOperation);
```

**After** (type-safe):
```csharp
// Only catch IOException, let programming errors propagate
var result = fileReader
    .WithFallback<string, IOException>(cachedReader);

// Catch multiple specific types with predicate
var result = apiCall
    .WithFallbackWhen(
        ex => ex is HttpRequestException || ex is TimeoutException,
        cachedResponse);
```

---

### 2. ?? Smart Retry with Backoff

**Before** (hammers failing service):
```csharp
var result = apiCall.WithRetry(5);  // Retries immediately ??
```

**After** (intelligent backoff):
```csharp
// Exponential: 100ms, 200ms, 400ms, 800ms, 1600ms
var result = apiCall
    .WithExponentialBackoff(
        maxAttempts: 5,
        baseDelay: TimeSpan.FromMilliseconds(100),
        maxDelay: TimeSpan.FromSeconds(10));

// With jitter (prevents thundering herd)
var result = apiCall
    .WithExponentialBackoffAndJitter(
        maxAttempts: 5,
        baseDelay: TimeSpan.FromMilliseconds(100));

// Custom strategy
var result = apiCall
    .WithRetryAndDelay(
        maxAttempts: 3,
        delayStrategy: attempt => TimeSpan.FromSeconds(attempt * 2));
```

---

### 3. ?? Smart Caching

**Before** (grows forever):
```csharp
var cached = expensive.WithCache();  // Memory leak waiting to happen
```

**After** (production-ready):
```csharp
// Simple TTL (expires after 5 minutes)
var cached = expensive
    .WithCacheTTL(TimeSpan.FromMinutes(5));

// LRU cache (max 1000 entries)
var cached = expensive
    .WithCacheLRU(maxSize: 1000);

// Combined TTL + LRU
var cached = expensive
    .WithCacheTTLAndSize(
        ttl: TimeSpan.FromMinutes(5),
        maxSize: 1000);

// Full control with sliding expiration
var cached = expensive
    .WithCache(new CacheOptions
    {
        ExpirationTime = TimeSpan.FromMinutes(10),
        MaxSize = 500,
        EvictionPolicy = CacheEvictionPolicy.LRU,
        SlidingExpiration = true  // Reset timer on each access
    });
```

---

### 4. ?? Distributed Tracing & Context

**Before** (manual correlation IDs):
```csharp
var correlationId = Guid.NewGuid().ToString();
var result = step1
    .TapAsync(async x => await Log(correlationId, x))
    .Pipe(step2)
    .TapAsync(async x => await Log(correlationId, x));  // Repetitive!
```

**After** (automatic context propagation):
```csharp
// Set once, use everywhere
var result = step1
    .WithCorrelationId()  // Auto-generates GUID
    .Pipe(step2)
    .Pipe(step3)
    .TapContextAsync(async (input, ctx) => {
        await Log(ctx.CorrelationId, input);  // Available everywhere!
    });

// Access context anywhere in your code
void YourMethod()
{
    var correlationId = RunnableContext.Current.CorrelationId;
    logger.Log($"Processing request {correlationId}");
}
```

---

### 5. ?? Multi-Tenancy Support

```csharp
// Extract tenant from request
var pipeline = validateRequest
    .WithTenant(req => req.Headers["X-Tenant-Id"])
    .Pipe(queryDatabase)  // Uses tenant context automatically
    .WithUser(req => req.UserId)  // For audit logging
    .TapContextAsync(async (req, ctx) => {
        await auditLogger.LogAsync(
            $"Tenant: {ctx.TenantId}, " +
            $"User: {ctx.UserId}, " +
            $"CorrelationId: {ctx.CorrelationId}");
    });

// Access tenant info anywhere
void DatabaseQuery()
{
    var tenantId = RunnableContext.Current.TenantId;
    var query = $"SELECT * FROM Data WHERE TenantId = '{tenantId}'";
}
```

---

### 6. ?? Complete Real-World Example

Here's a production-ready API request pipeline:

```csharp
public class ApiClient
{
    public async Task<TResponse> MakeRequestAsync<TRequest, TResponse>(
        string endpoint,
        TRequest request)
    {
        // Build resilient pipeline
        var pipeline = CreateRequest<TRequest, TResponse>(endpoint)
            // Distributed tracing
            .WithCorrelationId()
            .WithUser(req => GetCurrentUserId())
            
            // Smart retries with exponential backoff
            .WithExponentialBackoffAndJitter(
                maxAttempts: 5,
                baseDelay: TimeSpan.FromMilliseconds(100),
                maxDelay: TimeSpan.FromSeconds(30))
            
            // Timeout protection
            .WithTimeout(TimeSpan.FromSeconds(10))
            
            // Type-safe fallback (only for network errors)
            .WithFallback<TResponse, HttpRequestException>(
                GetCachedResponse<TResponse>)
            
            // Smart caching with TTL
            .WithCacheTTLAndSize(
                ttl: TimeSpan.FromMinutes(5),
                maxSize: 1000)
            
            // Logging with context
            .TapContextAsync(async (req, ctx) => {
                await logger.LogInfoAsync(
                    $"API Request: {endpoint}",
                    new {
                        CorrelationId = ctx.CorrelationId,
                        UserId = ctx.UserId,
                        Endpoint = endpoint
                    });
            });

        return await pipeline.InvokeAsync(request);
    }
}
```

---

## ?? Pattern Library

### Pattern 1: Circuit Breaker with Smart Retry

```csharp
var circuitBreaker = service
    .WithExponentialBackoff(
        maxAttempts: 3,
        baseDelay: TimeSpan.FromMilliseconds(100))
    .WithFallback<Response, Exception>(fallbackService)
    .WithCache(new CacheOptions
    {
        ExpirationTime = TimeSpan.FromMinutes(1),
        MaxSize = 100
    });
```

### Pattern 2: Multi-Tier Cache with Correlation

```csharp
var multiTierCache = l1Cache
    .WithCorrelationId()
    .WithFallback(l2Cache
        .WithTimeout(TimeSpan.FromMilliseconds(100))
        .WithFallback(database
            .WithExponentialBackoff(
                maxAttempts: 3,
                baseDelay: TimeSpan.FromMilliseconds(200))
            .WithCacheTTL(TimeSpan.FromMinutes(5))))
    .TapContextAsync(async (key, ctx) => {
        await metrics.RecordAsync($"Cache.Access.{ctx.CorrelationId}");
    });
```

### Pattern 3: SaaS Multi-Tenant Request Processing

```csharp
var tenantPipeline = parseRequest
    .WithCorrelationId()
    .WithTenant(req => req.Headers["X-Tenant-Id"])
    .WithUser(req => req.UserId)
    .Pipe(validateTenant)
    .Pipe(rateLimitByTenant
        .WithDelay(TimeSpan.FromMilliseconds(100)))  // Rate limiting
    .Pipe(processTenantRequest
        .WithExponentialBackoff(
            maxAttempts: 3,
            baseDelay: TimeSpan.FromMilliseconds(50)))
    .WithCacheLRU(maxSize: 10000)
    .TapContextAsync(async (req, ctx) => {
        await auditLog.LogAsync(new {
            TenantId = ctx.TenantId,
            UserId = ctx.UserId,
            CorrelationId = ctx.CorrelationId,
            Request = req
        });
    });
```

### Pattern 4: ML Model with Smart Caching

```csharp
var mlPipeline = preprocessData
    .Pipe(extractFeatures)
    .Pipe(modelInference
        .WithCacheTTLAndSize(  // Cache expensive model results
            ttl: TimeSpan.FromHours(1),
            maxSize: 10000))
    .WithTimeout(TimeSpan.FromSeconds(5))  // Model must be fast
    .WithFallback<Prediction, TimeoutException>(  // Fallback on timeout
        defaultPrediction)
    .TapContextAsync(async (features, ctx) => {
        await telemetry.TrackAsync("ModelInference", new {
            CorrelationId = ctx.CorrelationId,
            Duration = DateTime.UtcNow - ctx.GetValue<DateTime>("StartTime")
        });
    });
```

---

## ??? Migration Checklist

### ? Step 1: Update Retries
- [ ] Find all `.WithRetry()` calls
- [ ] Replace with `.WithExponentialBackoff()` where appropriate
- [ ] Add jitter for high-traffic scenarios

### ? Step 2: Add Exception Filtering
- [ ] Find all `.WithFallback()` calls
- [ ] Add type parameters for specific exceptions
- [ ] Use `.WithFallbackWhen()` for complex conditions

### ? Step 3: Fix Cache Memory Leaks
- [ ] Find all `.WithCache()` calls
- [ ] Replace with `.WithCacheTTL()` or `.WithCacheLRU()`
- [ ] Set appropriate TTL and size limits

### ? Step 4: Add Distributed Tracing
- [ ] Add `.WithCorrelationId()` at pipeline entry points
- [ ] Add `.TapContextAsync()` for logging
- [ ] Update existing logs to use `RunnableContext.Current.CorrelationId`

### ? Step 5: Test Everything
- [ ] Run existing unit tests (should all pass!)
- [ ] Add tests for new features
- [ ] Performance test caching improvements
- [ ] Verify context propagates across async boundaries

---

## ?? Best Practices

### DO ?
- Use exponential backoff for network calls
- Set cache TTL for long-running services
- Add correlation IDs to all entry points
- Use type-safe exception handling
- Set timeouts on external calls

### DON'T ?
- Use immediate retry on network failures
- Create unlimited caches in production
- Catch all exceptions indiscriminately
- Ignore correlation IDs in distributed systems
- Skip timeout configuration

---

## ?? Debugging Tips

### Check Context Values
```csharp
// Anywhere in your code:
var ctx = RunnableContext.Current;
Console.WriteLine($"CorrelationId: {ctx.CorrelationId}");
Console.WriteLine($"TenantId: {ctx.TenantId}");
Console.WriteLine($"UserId: {ctx.UserId}");

// Custom values
var requestId = ctx.GetValue<string>("RequestId");
```

### Monitor Cache Performance
```csharp
// Log cache hits/misses
var cached = expensive
    .WithCacheLRU(1000)
    .TapAsync(async result => {
        // This executes after cache check
        await metrics.Increment("CacheAccess");
    });
```

### Track Retry Attempts
```csharp
var attempts = 0;
var resilient = operation
    .TapAsync(async x => {
        attempts++;
        await logger.LogAsync($"Attempt {attempts}");
    })
    .WithExponentialBackoff(5, TimeSpan.FromMilliseconds(100));
```

---

## ?? Additional Resources

- See `tests/IMPROVEMENTS_SUMMARY.md` for complete feature list
- See `tests/ADVANCED_TESTS_SUMMARY.md` for integration examples
- See `tests/RunnableAdvancedIntegrationTests.cs` for test examples

---

**?? You're now ready to build production-grade, resilient, observable pipelines with Runnable v1.1!**
