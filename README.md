# Runnable 🚀

[![.NET](https://img.shields.io/badge/.NET-5%2C6%2C8%2C9%2C10-512BD4)](https://dotnet.microsoft.com/)
[![C#](https://img.shields.io/badge/C%23-14.0-239120)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)

**Runnable** is a powerful, composable pipeline library for .NET inspired by LangChain's LCEL (LangChain Expression Language). Build sophisticated data processing pipelines with a clean, functional API that supports multi-tenancy, distributed tracing, caching, and resilience patterns.

## ✨ Features

- 🔗 **Composable Pipelines** - Chain operations with fluent API
- 🏢 **Multi-Tenant Support** - Built-in tenant, user, and correlation ID tracking
- ⚡ **Context-Aware Operations** - Map, filter, branch, and cache based on execution context
- 🔄 **Retry & Resilience** - Exponential backoff, circuit breaker, timeout support
- 💾 **Advanced Caching** - Per-tenant/user caching with TTL and LRU eviction
- 📊 **Distributed Tracing** - Correlation IDs and trace propagation
- 🎯 **Type-Safe** - Full generic support for 0-16 parameters
- ⚙️ **Async-First** - Native async/await support throughout
- 🧪 **A/B Testing** - Built-in experimentation and feature flags
- 📦 **.NET Standard 2.0+** - Compatible with .NET 5, 6, 8, 9, 10

## 📦 Installation

```bash
dotnet add package Runnable
```

## 🚀 Quick Start

### Basic Pipeline

```csharp
using Runnable;

// Create a simple pipeline
var pipeline = RunnableLambda.Create<string, string>(input => input.ToUpper())
    .Map(upper => $"Hello, {upper}!")
    .Tap(result => Console.WriteLine(result));

var result = pipeline.Invoke("world");
// Output: "HELLO, WORLD!"
```

### Async Pipeline with Retry

```csharp
var apiPipeline = RunnableLambda.Create<string, Task<ApiResponse>>(async url =>
    await httpClient.GetFromJsonAsync<ApiResponse>(url))
    .WithExponentialBackoff(maxRetries: 3)
    .WithTimeout(TimeSpan.FromSeconds(30))
    .Map(response => response.Data)
    .WithCache();

var data = await apiPipeline.InvokeAsync("https://api.example.com/data");
```

### Branching Logic

```csharp
var processor = RunnableLambda.Create<Order, Order>(order => order)
    .Branch(
        order => order.Total > 1000,
        premiumProcessor,  // For high-value orders
        standardProcessor  // For regular orders
    );
```

## 🏢 Multi-Tenant & Context-Aware Pipelines

Runnable provides powerful context-aware operations perfect for multi-tenant SaaS applications:

### Setting Context

```csharp
var pipeline = apiService.GetData()
    .WithTenant(request.Headers["X-Tenant-ID"])
    .WithUser(request.User.Id)
    .WithCorrelationId(request.Headers["X-Correlation-ID"]);
```

### Context-Aware Filtering

```csharp
// Automatic tenant isolation
var tenantData = dataService.GetAll()
    .FilterContext((data, ctx) => data.TenantId == ctx.TenantId);
```

### Context-Aware Mapping

```csharp
// Enrich data with context information
var enriched = processor.Process()
    .MapContext((result, ctx) => new Response {
        Data = result,
        TenantId = ctx.TenantId,
        UserId = ctx.UserId,
        CorrelationId = ctx.CorrelationId,
        ProcessedAt = DateTime.UtcNow
    });
```

### Context-Based Routing

```csharp
// Route premium tenants to optimized handler
var handler = defaultHandler
    .BranchByTenant("premium", premiumHandler)
    .BranchByTenant("enterprise", enterpriseHandler);

// A/B testing
var experiment = defaultHandler
    .ABTestContext("new_feature", variantHandler);

// Debug mode routing
var pipeline = productionHandler
    .BranchByDebugMode(debugHandler);
```

### Per-Tenant Caching 🔥

```csharp
// Each tenant gets isolated cache
var service = expensiveOperation
    .WithCachePerTenant();

// Per-tenant cache with TTL
var catalog = getCatalog
    .WithCachePerTenantTTL(TimeSpan.FromMinutes(15));

// Per-tenant cache with LRU (memory-bounded)
var products = getProducts
    .WithCachePerTenantLRU(maxSize: 1000);

// Ultimate: TTL + LRU per tenant
var analytics = getAnalytics
    .WithCachePerTenantTTLAndLRU(
        ttl: TimeSpan.FromMinutes(10),
        maxSize: 500);
```

## 🎯 Real-World Example

### Complete Multi-Tenant SaaS Pipeline

```csharp
var metrics = new MetricsCollector();
var logger = new Logger();

var pipeline = apiService
    .GetRequest()
    
    // 1. Set execution context
    .WithTenant(request.Headers["X-Tenant-ID"])
    .WithUser(request.User.Id)
    .WithCorrelationId(request.Headers["X-Correlation-ID"])
    
    // 2. Security: Filter by tenant
    .FilterContext((req, ctx) => 
        req.TenantId == ctx.TenantId)
    
    // 3. Route premium tenants
    .BranchByTenant("premium", premiumProcessor)
    
    // 4. Process data
    .Map(data => transformer.Transform(data))
    
    // 5. Enrich with context
    .MapContext((result, ctx) => new Response {
        Data = result,
        TenantId = ctx.TenantId,
        UserId = ctx.UserId,
        CorrelationId = ctx.CorrelationId
    })
    
    // 6. Cache per tenant
    .WithCachePerTenantTTLAndLRU(
        ttl: TimeSpan.FromMinutes(5),
        maxSize: 1000)
    
    // 7. Observability
    .TapContext((response, ctx) =>
        logger.LogInfo($"Processed for tenant {ctx.TenantId}"))
    .WithMetrics(metrics)
    
    // 8. Resilience
    .WithExponentialBackoff(maxRetries: 3)
    .WithTimeout(TimeSpan.FromSeconds(30));

var result = await pipeline.InvokeAsync(request);
```

## 📚 Core Concepts

### Runnable Types

| Type | Description | Example |
|------|-------------|---------|
| `RunnableLambda` | Wraps any function/lambda | `RunnableLambda.Create<int, int>(x => x * 2)` |
| `RunnableMap` | Transforms input to output | `input.Map(x => x.ToString())` |
| `RunnableBranch` | Conditional routing | `input.Branch(predicate, trueHandler, falseHandler)` |
| `RunnablePassthrough` | Passes input through unchanged | `RunnablePassthrough.Create<T>()` |

### Extension Methods

#### Transformation
- `.Map(func)` - Transform output
- `.MapAsync(func)` - Async transformation
- `.MapContext(func)` - Transform with context access

#### Filtering
- `.Filter(predicate)` - Filter based on condition
- `.FilterContext(predicate)` - Filter with context
- `.FilterOrDefault(predicate, default)` - Filter with fallback

#### Branching
- `.Branch(predicate, truePath, falsePath)` - Conditional routing
- `.BranchContext(predicate, branch)` - Context-aware routing
- `.BranchByTenant(tenantId, handler)` - Tenant-specific routing
- `.ABTestContext(experimentKey, variant)` - A/B testing

#### Observability
- `.Tap(action)` - Side effect without changing output
- `.TapAsync(action)` - Async side effect
- `.TapContext(action)` - Observe with context access
- `.WithMetrics(collector)` - Collect metrics
- `.WithTelemetry(tracer)` - Distributed tracing

#### Caching
- `.WithCache()` - Simple memoization
- `.WithCacheTTL(timespan)` - Cache with expiration
- `.WithCachePerTenant()` - Per-tenant isolation
- `.WithCachePerUser()` - Per-user isolation
- `.WithCachePerTenantTTLAndLRU(ttl, maxSize)` - Advanced caching

#### Resilience
- `.WithRetry(maxAttempts)` - Simple retry
- `.WithExponentialBackoff(maxRetries)` - Exponential backoff
- `.WithTimeout(timespan)` - Execution timeout
- `.WithCircuitBreaker(threshold)` - Circuit breaker pattern
- `.WithFallback(handler)` - Fallback on error

#### Context
- `.WithContext(key, value)` - Set context value
- `.WithCorrelationId(id)` - Set correlation ID
- `.WithTenant(tenantId)` - Set tenant context
- `.WithUser(userId)` - Set user context

## 🎨 Advanced Patterns

### Parallel Processing

```csharp
var pipeline = RunnableLambda.Create<List<Item>, List<Result>>(items =>
    items.AsParallel()
         .Select(item => processor.Invoke(item))
         .ToList());
```

### Batch Processing

```csharp
var batchProcessor = RunnableLambda.Create<Item, Result>(item => 
    Process(item));

var results = batchProcessor.Batch(items);
```

### Streaming

```csharp
var stream = processor.Stream(input);
await foreach (var result in stream)
{
    Console.WriteLine(result);
}
```

### Complex Branching

```csharp
var handler = defaultHandler.BranchContext(
    // Premium tier
    ((req, ctx) => ctx.TenantId == "premium", premiumHandler),
    // Enterprise tier
    ((req, ctx) => ctx.TenantId == "enterprise", enterpriseHandler),
    // Debug mode
    ((req, ctx) => ctx.GetValue<bool>("IsDebug"), debugHandler)
    // Default handler used if no match
);
```

### Custom Cache Key Generation

```csharp
var cached = service
    .WithCacheContext(
        ctx => $"region:{ctx.GetValue<string>("Region")}:tenant:{ctx.TenantId}",
        input => input.Id.ToString());
```

## 🔧 Configuration

### Context Configuration

```csharp
// Set context programmatically
RunnableContext.Current.TenantId = "tenant-123";
RunnableContext.Current.UserId = "user-456";
RunnableContext.Current.SetValue("CustomKey", "CustomValue");

// Access context anywhere
var tenantId = RunnableContext.Current.TenantId;
var customValue = RunnableContext.Current.GetValue<string>("CustomKey");
```

### Metrics & Monitoring

```csharp
var metrics = new MetricsCollector();

var pipeline = service
    .WithMetrics(metrics)
    .TapContext((result, ctx) => 
        metrics.Record("processed", 1, new {
            TenantId = ctx.TenantId,
            CorrelationId = ctx.CorrelationId
        }));
```

## 🧪 Testing

```csharp
[Test]
public async Task TestPipelineWithContext()
{
    // Arrange
    RunnableContext.Current.TenantId = "test-tenant";
    
    var pipeline = processor
        .FilterContext((data, ctx) => data.TenantId == ctx.TenantId)
        .MapContext((result, ctx) => new Response {
            Data = result,
            TenantId = ctx.TenantId
        });
    
    // Act
    var result = await pipeline.InvokeAsync(testData);
    
    // Assert
    Assert.Equal("test-tenant", result.TenantId);
}
```

## 📖 API Reference

### Context Properties

| Property | Type | Description |
|----------|------|-------------|
| `CorrelationId` | `string` | Correlation ID for distributed tracing |
| `TraceId` | `string` | Trace ID (defaults to CorrelationId) |
| `ParentSpanId` | `string` | Parent span ID for nested traces |
| `TenantId` | `string` | Tenant identifier for multi-tenancy |
| `UserId` | `string` | User identifier for audit logging |

### Context Methods

| Method | Description |
|--------|-------------|
| `GetValue<T>(key)` | Get custom context value |
| `SetValue(key, value)` | Set custom context value |
| `Clear()` | Clear all context data |
| `GetAllData()` | Get all context as dictionary |

## 🌟 Why Runnable?

### ✅ Type-Safe
Full IntelliSense support with generic types throughout

### ✅ Composable
Build complex pipelines from simple, reusable components

### ✅ Multi-Tenant Ready
Built-in tenant isolation and context propagation

### ✅ Production-Ready
Retry logic, caching, timeouts, and circuit breakers included

### ✅ Testable
Easy to mock and test individual pipeline stages

### ✅ Performant
Async-first design with efficient caching and LRU eviction

## 🤝 Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🙏 Acknowledgments

Inspired by:
- [LangChain](https://www.langchain.com/) - The original LCEL concept
- Functional programming principles
- Railway-oriented programming

## 📚 More Examples

Check out the [examples](examples/) directory for more detailed examples:
- Multi-tenant API processing
- A/B testing implementations
- Complex branching scenarios
- Custom caching strategies
- Distributed tracing setup

## 💬 Support

- 📖 [Documentation](https://github.com/codenjwu/Runnable/wiki)
- 🐛 [Issue Tracker](https://github.com/codenjwu/Runnable/issues)
- 💡 [Discussions](https://github.com/codenjwu/Runnable/discussions)

---

**Built with ❤️ for the .NET community**
