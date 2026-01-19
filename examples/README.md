# Runnable Examples ??

This directory contains comprehensive examples demonstrating various Runnable features and patterns.

## ?? Examples Overview

| Example | Description | Key Features |
|---------|-------------|--------------|
| [Basic Pipeline](BasicPipeline.md) | Simple pipeline composition | Map, Tap, Filter |
| [Multi-Tenant SaaS](MultiTenantSaaS.md) | Complete multi-tenant application | Context, Per-Tenant Caching, Security |
| [A/B Testing](ABTesting.md) | Feature flag and experimentation | BranchContext, ABTestContext |
| [Resilience Patterns](ResiliencePatterns.md) | Retry, timeout, circuit breaker | WithRetry, WithTimeout, WithCircuitBreaker |
| [Advanced Caching](AdvancedCaching.md) | Custom cache strategies | TTL, LRU, Per-Tenant |
| [Distributed Tracing](DistributedTracing.md) | Observability and monitoring | CorrelationId, Metrics, Telemetry |
| [Async Patterns](AsyncPatterns.md) | Async/await best practices | InvokeAsync, Batch, Stream |
| [Complex Branching](ComplexBranching.md) | Advanced routing scenarios | Multi-way branching, Nested branches |

## ?? Running Examples

### Prerequisites

```bash
# Install .NET 8.0 SDK
dotnet --version  # Should be 8.0 or higher

# Clone the repository
git clone https://github.com/codenjwu/Runnable.git
cd Runnable
```

### Build and Run

```bash
# Restore packages
dotnet restore

# Build examples
dotnet build examples/

# Run specific example
dotnet run --project examples/MultiTenantSaaS/MultiTenantSaaS.csproj
```

## ?? Quick Examples

### Simple Pipeline

```csharp
using Runnable;

// Create a simple transformation pipeline
var pipeline = RunnableLambda.Create<string, string>(input => input.ToUpper())
    .Map(upper => $"Hello, {upper}!")
    .Tap(result => Console.WriteLine(result));

var result = pipeline.Invoke("world");
// Output: "HELLO, WORLD!"
```

### Multi-Tenant with Caching

```csharp
// Set tenant context
var pipeline = apiService
    .GetData()
    .WithTenant(request.Headers["X-Tenant-ID"])
    .FilterContext((data, ctx) => data.TenantId == ctx.TenantId)
    .WithCachePerTenantTTL(TimeSpan.FromMinutes(5));

var result = await pipeline.InvokeAsync(request);
```

### A/B Testing

```csharp
// Route users to different implementations
var experiment = defaultHandler
    .ABTestContext("new_checkout_flow", newCheckoutHandler);

RunnableContext.Current.SetValue("new_checkout_flow", "B");
var result = await experiment.InvokeAsync(order);
```

## ?? Learning Path

**Beginner:**
1. Start with [Basic Pipeline](BasicPipeline.md)
2. Try [Async Patterns](AsyncPatterns.md)
3. Explore [Resilience Patterns](ResiliencePatterns.md)

**Intermediate:**
4. Learn [Multi-Tenant SaaS](MultiTenantSaaS.md)
5. Master [Advanced Caching](AdvancedCaching.md)
6. Understand [Complex Branching](ComplexBranching.md)

**Advanced:**
7. Implement [Distributed Tracing](DistributedTracing.md)
8. Build [A/B Testing](ABTesting.md) systems
9. Create custom extensions

## ?? Best Practices

### Context Management

```csharp
// ? Good - Set context at the beginning
var pipeline = service
    .WithTenant(tenantId)
    .WithUser(userId)
    .WithCorrelationId(correlationId)
    .Process();

// ? Bad - Setting context in the middle
var pipeline = service
    .Process()
    .WithTenant(tenantId)  // Too late!
    .Map(x => x);
```

### Caching Strategy

```csharp
// ? Good - Cache expensive operations early
var pipeline = expensiveService
    .GetData()
    .WithCachePerTenantTTLAndLRU(
        ttl: TimeSpan.FromMinutes(10),
        maxSize: 1000)
    .Map(data => transform(data));  // Transform happens after cache check

// ? Bad - Caching after transformation
var pipeline = expensiveService
    .GetData()
    .Map(data => transform(data))  // Always executes
    .WithCachePerTenant();  // Caches transformed result
```

### Error Handling

```csharp
// ? Good - Resilience early in pipeline
var pipeline = unreliableService
    .GetData()
    .WithExponentialBackoff(maxRetries: 3)
    .WithTimeout(TimeSpan.FromSeconds(30))
    .Map(data => process(data));

// ? Good - Fallback for critical operations
var pipeline = service
    .GetData()
    .WithFallback(fallbackService.GetData());
```

## ?? Troubleshooting

### Common Issues

**Pipeline not executing:**
```csharp
// ? Bad - Pipeline definition doesn't execute
var pipeline = RunnableLambda.Create<string, string>(x => x.ToUpper());
// Nothing happens!

// ? Good - Invoke the pipeline
var result = pipeline.Invoke("hello");
```

**Context not propagating:**
```csharp
// ? Bad - Creating new context instead of using current
RunnableContext.Current = new RunnableContext();

// ? Good - Modify existing context
RunnableContext.Current.TenantId = "tenant-123";
```

**Cache not working:**
```csharp
// ? Bad - Cache after async operation
var pipeline = service
    .InvokeAsync()  // Async operation
    .WithCache();   // Too late!

// ? Good - Cache the runnable itself
var pipeline = service
    .WithCache();
await pipeline.InvokeAsync();
```

## ?? Additional Resources

- [Main Documentation](../README.md)
- [API Reference](../docs/API.md)
- [Contributing Guide](../CONTRIBUTING.md)
- [GitHub Discussions](https://github.com/codenjwu/Runnable/discussions)

## ?? Contributing Examples

Have a great example? We'd love to include it!

1. Create a new markdown file in this directory
2. Follow the existing format
3. Include working code samples
4. Explain key concepts
5. Submit a pull request

See [CONTRIBUTING.md](../CONTRIBUTING.md) for details.

---

**Happy coding! ??**
