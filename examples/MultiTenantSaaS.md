# Multi-Tenant SaaS Application Example ??

This example demonstrates building a complete multi-tenant SaaS API using Runnable's context-aware features.

## ?? Overview

This example covers:
- ? Tenant isolation and security
- ? Per-tenant caching strategies
- ? User-specific processing
- ? Distributed tracing with correlation IDs
- ? Tenant-tier routing (Free, Pro, Enterprise)
- ? Audit logging and compliance

## ??? Architecture

```
Request ¡ú Context Setup ¡ú Security Filter ¡ú Tier Routing ¡ú Processing ¡ú Enrichment ¡ú Cache ¡ú Response
   ¡ý           ¡ý              ¡ý                ¡ý              ¡ý            ¡ý         ¡ý        ¡ý
Headers   TenantId      ValidateTenant   BranchByTenant   Transform   AddMetadata  TTL+LRU  JSON
          UserId        FilterContext    Premium/Basic    Business     Context
          CorrelationId                  Enterprise       Logic        Values
```

## ?? Complete Example

### 1. Data Models

```csharp
public class ApiRequest
{
    public string TenantId { get; set; }
    public string UserId { get; set; }
    public string Operation { get; set; }
    public Dictionary<string, object> Parameters { get; set; }
}

public class ApiResponse
{
    public object Data { get; set; }
    public string TenantId { get; set; }
    public string UserId { get; set; }
    public string CorrelationId { get; set; }
    public DateTime ProcessedAt { get; set; }
    public string ProcessedBy { get; set; }
    public int CacheHits { get; set; }
}

public class TenantConfig
{
    public string TenantId { get; set; }
    public string Tier { get; set; }  // "free", "pro", "enterprise"
    public int RateLimitPerMinute { get; set; }
    public bool EnableAdvancedFeatures { get; set; }
    public Dictionary<string, object> Settings { get; set; }
}
```

### 2. Service Layer

```csharp
public class DataService
{
    private readonly ILogger<DataService> _logger;
    private readonly IDatabase _database;
    private readonly ICache _cache;

    public DataService(ILogger<DataService> logger, IDatabase database, ICache cache)
    {
        _logger = logger;
        _database = database;
        _cache = cache;
    }

    public async Task<object> GetDataAsync(ApiRequest request)
    {
        // Simulate data retrieval
        await Task.Delay(100);
        
        return new
        {
            RequestId = Guid.NewGuid(),
            Operation = request.Operation,
            TenantId = request.TenantId,
            Timestamp = DateTime.UtcNow,
            Data = GenerateMockData(request.Operation)
        };
    }

    private object GenerateMockData(string operation)
    {
        return operation switch
        {
            "users" => new { Users = new[] { "User1", "User2", "User3" } },
            "products" => new { Products = new[] { "Product A", "Product B" } },
            "analytics" => new { Metrics = new { PageViews = 1000, Users = 50 } },
            _ => new { Message = "Operation not found" }
        };
    }
}
```

### 3. Multi-Tenant Pipeline

```csharp
using Runnable;

public class MultiTenantApiService
{
    private readonly DataService _dataService;
    private readonly ILogger<MultiTenantApiService> _logger;
    private readonly ITenantRepository _tenantRepo;
    private readonly MetricsCollector _metrics;

    public MultiTenantApiService(
        DataService dataService,
        ILogger<MultiTenantApiService> logger,
        ITenantRepository tenantRepo,
        MetricsCollector metrics)
    {
        _dataService = dataService;
        _logger = logger;
        _tenantRepo = tenantRepo;
        _metrics = metrics;
    }

    public IRunnable<ApiRequest, ApiResponse> CreatePipeline()
    {
        // Base processor - retrieves data
        var baseProcessor = RunnableLambda.Create<ApiRequest, Task<object>>(
            async request => await _dataService.GetDataAsync(request));

        // Premium processor - with enhanced features
        var premiumProcessor = baseProcessor
            .MapAsync(async data => await EnhanceDataAsync(data))
            .WithCachePerTenantTTLAndLRU(
                ttl: TimeSpan.FromMinutes(10),
                maxSize: 1000);

        // Enterprise processor - with maximum performance
        var enterpriseProcessor = baseProcessor
            .MapAsync(async data => await EnhanceDataAsync(data))
            .MapAsync(async data => await AddAnalyticsAsync(data))
            .WithCachePerTenantTTLAndLRU(
                ttl: TimeSpan.FromMinutes(30),
                maxSize: 5000);

        // Free tier processor - basic features only
        var freeProcessor = baseProcessor
            .Map(data => FilterSensitiveData(data))
            .WithCachePerTenantTTL(TimeSpan.FromMinutes(1));

        // Complete multi-tenant pipeline
        return RunnableLambda.Create<ApiRequest, ApiRequest>(x => x)
            // Step 1: Setup context from request
            .Map(SetupContext)
            
            // Step 2: Security - Validate tenant
            .FilterAsyncContext(async (req, ctx) => 
                await ValidateTenantAsync(req.TenantId))
            
            // Step 3: Security - Tenant isolation
            .FilterContext((req, ctx) => 
                req.TenantId == ctx.TenantId)
            
            // Step 4: Rate limiting per tenant
            .TapAsyncContext(async (req, ctx) =>
                await CheckRateLimitAsync(ctx.TenantId))
            
            // Step 5: Route by tenant tier
            .BranchAsyncContext(
                async (req, ctx) => await IsTenantTierAsync(ctx.TenantId, "enterprise"),
                enterpriseProcessor)
            .BranchAsyncContext(
                async (req, ctx) => await IsTenantTierAsync(ctx.TenantId, "pro"),
                premiumProcessor)
            
            // Default to free tier
            .Pipe(freeProcessor)
            
            // Step 6: Enrich with context metadata
            .MapContext((data, ctx) => new ApiResponse
            {
                Data = data,
                TenantId = ctx.TenantId,
                UserId = ctx.UserId,
                CorrelationId = ctx.CorrelationId,
                ProcessedAt = DateTime.UtcNow,
                ProcessedBy = "MultiTenantApiService",
                CacheHits = ctx.GetValue<int>("CacheHits")
            })
            
            // Step 7: Audit logging
            .TapContext((response, ctx) =>
                _logger.LogInformation(
                    "Request processed - Tenant: {TenantId}, User: {UserId}, Correlation: {CorrelationId}",
                    ctx.TenantId, ctx.UserId, ctx.CorrelationId))
            
            // Step 8: Metrics collection
            .Tap(response => _metrics.RecordRequest(response.TenantId))
            
            // Step 9: Resilience
            .WithExponentialBackoff(maxRetries: 3)
            .WithTimeout(TimeSpan.FromSeconds(30));
    }

    // Helper Methods

    private ApiRequest SetupContext(ApiRequest request)
    {
        RunnableContext.Current.TenantId = request.TenantId;
        RunnableContext.Current.UserId = request.UserId;
        RunnableContext.Current.CorrelationId = Guid.NewGuid().ToString();
        RunnableContext.Current.SetValue("RequestTime", DateTime.UtcNow);
        
        return request;
    }

    private async Task<bool> ValidateTenantAsync(string tenantId)
    {
        var tenant = await _tenantRepo.GetTenantAsync(tenantId);
        return tenant != null && tenant.IsActive;
    }

    private async Task<bool> IsTenantTierAsync(string tenantId, string tier)
    {
        var config = await _tenantRepo.GetTenantConfigAsync(tenantId);
        return config?.Tier?.ToLower() == tier.ToLower();
    }

    private async Task CheckRateLimitAsync(string tenantId)
    {
        // Implement rate limiting logic
        var config = await _tenantRepo.GetTenantConfigAsync(tenantId);
        // Check if within rate limit...
    }

    private async Task<object> EnhanceDataAsync(object data)
    {
        // Add premium features to data
        await Task.Delay(50);
        return new { Original = data, Enhanced = true };
    }

    private async Task<object> AddAnalyticsAsync(object data)
    {
        // Add analytics data
        await Task.Delay(50);
        return new { Original = data, Analytics = new { Views = 100 } };
    }

    private object FilterSensitiveData(object data)
    {
        // Remove sensitive fields for free tier
        return new { Limited = true, Data = "Basic data only" };
    }
}
```

### 4. API Controller

```csharp
[ApiController]
[Route("api/[controller]")]
public class TenantDataController : ControllerBase
{
    private readonly MultiTenantApiService _apiService;
    private readonly ILogger<TenantDataController> _logger;

    public TenantDataController(
        MultiTenantApiService apiService,
        ILogger<TenantDataController> logger)
    {
        _apiService = apiService;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> ProcessRequest([FromBody] ApiRequest request)
    {
        try
        {
            // Extract context from headers
            request.TenantId = Request.Headers["X-Tenant-ID"];
            request.UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Create and execute pipeline
            var pipeline = _apiService.CreatePipeline();
            var response = await pipeline.InvokeAsync(request);

            // Add correlation ID to response headers
            Response.Headers.Add("X-Correlation-ID", response.CorrelationId);

            return Ok(response);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized(new { Error = "Invalid tenant credentials" });
        }
        catch (TimeoutException)
        {
            return StatusCode(504, new { Error = "Request timeout" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing request");
            return StatusCode(500, new { Error = "Internal server error" });
        }
    }

    [HttpGet("health")]
    public IActionResult Health()
    {
        return Ok(new { Status = "Healthy", Timestamp = DateTime.UtcNow });
    }
}
```

## ?? Usage Examples

### 1. Free Tier Request

```csharp
// Request from free tier tenant
var request = new ApiRequest
{
    TenantId = "free-tenant-123",
    UserId = "user-456",
    Operation = "users",
    Parameters = new Dictionary<string, object>()
};

var response = await pipeline.InvokeAsync(request);
// Uses basic cache (1 minute TTL)
// Returns filtered data only
```

### 2. Pro Tier Request

```csharp
// Request from pro tier tenant
var request = new ApiRequest
{
    TenantId = "pro-tenant-789",
    UserId = "user-012",
    Operation = "analytics",
    Parameters = new Dictionary<string, object>()
};

var response = await pipeline.InvokeAsync(request);
// Uses enhanced cache (10 minutes TTL, 1000 items)
// Returns enhanced data with premium features
```

### 3. Enterprise Tier Request

```csharp
// Request from enterprise tier tenant
var request = new ApiRequest
{
    TenantId = "enterprise-tenant-456",
    UserId = "admin-789",
    Operation = "analytics",
    Parameters = new Dictionary<string, object>()
};

var response = await pipeline.InvokeAsync(request);
// Uses maximum cache (30 minutes TTL, 5000 items)
// Returns fully enhanced data with analytics
```

## ?? Metrics and Monitoring

```csharp
public class MetricsCollector
{
    private readonly ConcurrentDictionary<string, int> _requestCounts = new();
    private readonly ConcurrentDictionary<string, long> _responseTimes = new();

    public void RecordRequest(string tenantId)
    {
        _requestCounts.AddOrUpdate(tenantId, 1, (_, count) => count + 1);
    }

    public void RecordResponseTime(string tenantId, long milliseconds)
    {
        _responseTimes.AddOrUpdate(tenantId, milliseconds, (_, time) => (time + milliseconds) / 2);
    }

    public Dictionary<string, object> GetMetrics(string tenantId)
    {
        return new Dictionary<string, object>
        {
            ["RequestCount"] = _requestCounts.GetValueOrDefault(tenantId),
            ["AvgResponseTime"] = _responseTimes.GetValueOrDefault(tenantId)
        };
    }
}
```

## ?? Security Best Practices

### 1. Tenant Validation

```csharp
// ? Good - Validate tenant before processing
.FilterAsyncContext(async (req, ctx) => 
    await ValidateTenantAsync(req.TenantId))

// ? Bad - Trust tenant ID without validation
.Map(req => ProcessRequest(req))
```

### 2. Tenant Isolation

```csharp
// ? Good - Enforce tenant isolation
.FilterContext((req, ctx) => 
    req.TenantId == ctx.TenantId)

// ? Bad - No isolation check
.Map(req => GetData(req))
```

### 3. Audit Logging

```csharp
// ? Good - Log with full context
.TapContext((response, ctx) =>
    _logger.LogInformation(
        "Request: {Operation}, Tenant: {TenantId}, User: {UserId}, Correlation: {CorrelationId}",
        operation, ctx.TenantId, ctx.UserId, ctx.CorrelationId))

// ? Bad - Insufficient logging
.Tap(response => _logger.LogInformation("Request processed"))
```

## ?? Performance Optimization

### Cache Strategy by Tier

| Tier | TTL | Max Size | Strategy |
|------|-----|----------|----------|
| Free | 1 min | None | Simple TTL |
| Pro | 10 min | 1,000 | TTL + LRU |
| Enterprise | 30 min | 5,000 | TTL + LRU |

### Why This Works

- ? **Free tier**: Short cache reduces memory usage
- ? **Pro tier**: Balance between performance and memory
- ? **Enterprise tier**: Maximum performance with larger cache

## ?? Scaling Considerations

### Horizontal Scaling

```csharp
// Use distributed cache for multi-instance deployment
var distributedCache = new DistributedCache(redisConnection);

var pipeline = service
    .WithCacheContext(
        ctx => $"tenant:{ctx.TenantId}",
        customCache: distributedCache);
```

### Database Sharding

```csharp
// Route to correct shard based on tenant
var pipeline = service
    .BranchContext(
        (req, ctx) => HashTenantId(ctx.TenantId) % 4 == 0,
        shard0Service)
    .BranchContext(
        (req, ctx) => HashTenantId(ctx.TenantId) % 4 == 1,
        shard1Service)
    // ... more shards
```

## ?? Key Takeaways

1. ? **Always set context early** in the pipeline
2. ? **Validate and isolate tenants** for security
3. ? **Use tier-based routing** for different service levels
4. ? **Cache per tenant** to avoid cross-tenant contamination
5. ? **Log with correlation IDs** for debugging
6. ? **Implement rate limiting** per tenant
7. ? **Add resilience patterns** for production reliability

## ?? Related Examples

- [Advanced Caching](AdvancedCaching.md) - Deep dive into caching strategies
- [Distributed Tracing](DistributedTracing.md) - Correlation IDs and telemetry
- [A/B Testing](ABTesting.md) - Feature flags per tenant

---

**Next Steps:** Try implementing [A/B Testing](ABTesting.md) to roll out features gradually per tenant!
