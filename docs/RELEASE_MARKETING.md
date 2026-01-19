# Runnable - Release Marketing Materials ??

## ?? GitHub Repository Description (280 chars max)

```
?? LangChain-inspired composable pipelines for .NET with first-class multi-tenancy support. 
Build type-safe data pipelines with context-aware caching, tenant isolation, A/B testing, 
and resilience patterns. Perfect for SaaS applications! ??
```

## ?? Short Tagline

**"LangChain meets .NET Multi-Tenancy - Composable pipelines that actually understand your tenants"**

---

## ?? NuGet Package Description

### Title
**Runnable - Composable Pipelines with Multi-Tenant Magic**

### Summary
LangChain-inspired pipeline library for .NET with first-class multi-tenancy. Build sophisticated data processing pipelines with context-aware operations, per-tenant caching, automatic tenant isolation, and built-in resilience patterns. Perfect for modern SaaS applications.

### Tags
`pipeline`, `multi-tenant`, `langchain`, `functional`, `composable`, `saas`, `context`, `caching`, `resilience`, `async`, `type-safe`

### Full Description

```markdown
# Runnable ??

Build beautiful, composable data pipelines with **automatic multi-tenant isolation** and **context-aware operations**.

## ? What Makes It Special

- ?? **Multi-Tenancy First** - Per-tenant caching, routing, and isolation built-in
- ?? **Fluent Composition** - Chain operations like LINQ, but better
- ? **Context Flows Everywhere** - Tenant ID, User ID, Correlation ID propagate automatically
- ?? **Type-Safe** - Full IntelliSense for 0-16 parameters
- ?? **Production-Ready** - Retry, timeout, circuit breaker, caching with TTL+LRU

## ?? Quick Example

```csharp
// Multi-tenant API pipeline in 15 lines!
var pipeline = apiService
    .GetData()
    .WithTenant(request.TenantId)              // Set tenant context
    .FilterContext((data, ctx) =>               // Automatic tenant isolation
        data.TenantId == ctx.TenantId)
    .BranchByTenant("premium",                  // Route premium tenants
        premiumHandler)
    .WithCachePerTenantTTL(                     // Per-tenant cache with TTL
        TimeSpan.FromMinutes(10))
    .MapContext((result, ctx) => new {          // Enrich with context
        Data = result,
        TenantId = ctx.TenantId,
        CorrelationId = ctx.CorrelationId
    })
    .WithExponentialBackoff(3)                  // Add resilience
    .WithTimeout(TimeSpan.FromSeconds(30));

var result = await pipeline.InvokeAsync(request);
```

## ?? Perfect For

- ? Multi-tenant SaaS applications
- ? Microservices with distributed tracing
- ? Data processing pipelines
- ? API gateways and middleware
- ? A/B testing and feature flags

**Made with ?? for the .NET community**
```

---

## ?? Twitter/X Announcement Thread

### Tweet 1 (Main Announcement)
```
?? Excited to release Runnable - LangChain-inspired pipelines for .NET with FIRST-CLASS multi-tenancy!

Build composable data pipelines where context (tenant, user, correlation ID) flows automatically.

Perfect for SaaS apps! ??

?? https://nuget.org/packages/Runnable
? https://github.com/codenjwu/Runnable

#dotnet #csharp #saas
```

### Tweet 2 (Code Example)
```
Here's what makes it special - Per-tenant caching in ONE LINE:

```csharp
var data = expensiveOperation
    .WithTenant(request.TenantId)
    .WithCachePerTenantTTL(TimeSpan.FromMinutes(10))
    .Invoke(request);
```

Each tenant gets isolated cache automatically! No more cross-tenant contamination ??

#dotnet
```

### Tweet 3 (Multi-Tenant Magic)
```
Multi-tenant routing? Easy:

```csharp
var handler = defaultHandler
    .BranchByTenant("premium", premiumHandler)
    .BranchByTenant("enterprise", enterpriseHandler);
```

Premium customers get the fast lane ???
Free users get... the scenic route ??

#SaaS #dotnet
```

### Tweet 4 (Context Propagation)
```
The magic? Context propagates EVERYWHERE:

```csharp
service
    .WithTenant("acme-corp")
    .WithUser("alice")
    .Map(x => transform(x))        // Context here
    .TapContext((x, ctx) =>         // And here
        log(ctx.TenantId))
    .MapContext((x, ctx) =>         // And here!
        enrich(x, ctx));
```

Never lose your context again! ??
```

### Tweet 5 (Call to Action)
```
Built for modern .NET (Standard 2.0, 5, 6, 8, 9, 10)

? 500+ extension methods
? 0-16 parameter support
? Full async/await
? Production-tested
? MIT licensed

Try it today! ? the repo if you like it!

?? https://nuget.org/packages/Runnable
```

---

## ?? LinkedIn Announcement

### Professional Post

```
?? Excited to announce the release of Runnable - a composable pipeline library for .NET!

After months of development, I'm thrilled to share a library that solves a real problem I've faced in every multi-tenant SaaS application I've built: managing tenant context and isolation across complex data pipelines.

?? What is Runnable?

Inspired by LangChain's LCEL, Runnable brings functional pipeline composition to .NET with first-class multi-tenancy support. Think LINQ meets microservices middleware, but with automatic context propagation.

? Key Features:

?? Multi-Tenant First
  ¡ú Per-tenant cache isolation
  ¡ú Automatic tenant routing
  ¡ú Built-in security filtering

? Context-Aware Operations
  ¡ú Tenant ID, User ID, Correlation ID flow automatically
  ¡ú No more passing context manually
  ¡ú Clean, readable code

?? Production-Ready
  ¡ú Exponential backoff retry
  ¡ú Circuit breaker pattern
  ¡ú TTL + LRU caching
  ¡ú Distributed tracing

?? Example:

```csharp
var pipeline = apiService
    .GetData()
    .WithTenant(request.Headers["X-Tenant-ID"])
    .FilterContext((data, ctx) => 
        data.TenantId == ctx.TenantId)  // Security
    .BranchByTenant("premium", 
        premiumProcessor)                // Routing
    .WithCachePerTenantTTL(
        TimeSpan.FromMinutes(10))        // Per-tenant cache
    .WithExponentialBackoff(3);          // Resilience

var result = await pipeline.InvokeAsync(request);
```

?? Perfect for:
  ? Multi-tenant SaaS applications
  ? Microservices architectures
  ? Data processing pipelines
  ? API gateways

?? Available now on NuGet
? Open source (MIT) on GitHub

I'd love to hear your thoughts! What challenges do you face with multi-tenant architectures?

#dotnet #csharp #softwaredevelopment #saas #opensource #microservices
```

---

## ?? Reddit Post (r/dotnet, r/csharp)

### Title
```
[Open Source] Runnable - LangChain-inspired pipelines for .NET with first-class multi-tenancy support ??
```

### Body
```markdown
Hey r/dotnet! ??

I'm excited to share a library I've been working on that solves a problem I've faced in every multi-tenant SaaS app: **managing tenant context across complex pipelines**.

## ?? What is Runnable?

Think **LangChain's LCEL** meets **.NET multi-tenancy**. Build composable data pipelines where context (tenant ID, user ID, correlation ID) flows automatically through every operation.

## ?? The Problem It Solves

Ever written code like this?

```csharp
// Bad: Manual context passing everywhere ??
var data = await GetData(request, tenantId, userId, correlationId);
var filtered = FilterByTenant(data, tenantId);
var cached = GetFromCache(filtered, tenantId);
var enriched = Enrich(cached, tenantId, userId);
```

**Runnable makes it beautiful:**

```csharp
// Good: Context flows automatically ?
var result = await service
    .GetData()
    .WithTenant(request.TenantId)
    .FilterContext((data, ctx) => data.TenantId == ctx.TenantId)
    .WithCachePerTenant()
    .MapContext((data, ctx) => Enrich(data, ctx))
    .InvokeAsync(request);
```

## ?? Real-World Example

Here's a complete multi-tenant API pipeline:

```csharp
var pipeline = apiService
    .GetRequest()
    
    // 1. Setup context
    .WithTenant(request.Headers["X-Tenant-ID"])
    .WithUser(request.User.Id)
    .WithCorrelationId(request.Headers["X-Correlation-ID"])
    
    // 2. Security: Automatic tenant isolation
    .FilterContext((req, ctx) => req.TenantId == ctx.TenantId)
    
    // 3. Route premium tenants to faster handler
    .BranchByTenant("premium", premiumHandler)
    
    // 4. Per-tenant caching with TTL + LRU
    .WithCachePerTenantTTLAndLRU(
        ttl: TimeSpan.FromMinutes(10),
        maxSize: 1000)
    
    // 5. Enrich with context
    .MapContext((result, ctx) => new Response {
        Data = result,
        TenantId = ctx.TenantId,
        ProcessedAt = DateTime.UtcNow
    })
    
    // 6. Add resilience
    .WithExponentialBackoff(maxRetries: 3)
    .WithTimeout(TimeSpan.FromSeconds(30));

var result = await pipeline.InvokeAsync(request);
```

## ? Cool Features

- ?? **Per-Tenant Caching**: Each tenant gets isolated cache
- ?? **Type-Safe**: Full IntelliSense for 0-16 parameters
- ? **Async-First**: Native async/await throughout
- ?? **Smart Routing**: Branch by tenant, user, or custom context
- ?? **A/B Testing**: Built-in feature flag support
- ?? **Resilience**: Retry, timeout, circuit breaker patterns
- ?? **Observability**: Distributed tracing, correlation IDs

## ?? Why I Built This

I was tired of:
1. Passing context manually through every method
2. Implementing per-tenant caching from scratch
3. Worrying about cross-tenant data leakage
4. Writing the same retry/timeout logic repeatedly

**Runnable gives you all this out-of-the-box.**

## ?? Getting Started

```bash
dotnet add package Runnable
```

```csharp
using Runnable;

// Your first pipeline!
var result = RunnableLambda
    .Create<string, string>(x => x.ToUpper())
    .Map(x => $"Hello, {x}!")
    .Invoke("world");
// Output: "HELLO, WORLD!"
```

## ?? Documentation

- ?? [GitHub](https://github.com/codenjwu/Runnable)
- ?? [NuGet](https://nuget.org/packages/Runnable)
- ?? [Full Examples](https://github.com/codenjwu/Runnable/tree/master/examples)

## ?? Feedback Welcome!

This is my first major open-source contribution, so I'd love your feedback:
- What do you think of the API?
- What features would you like to see?
- Would you use this in production?

**MIT licensed** - use it however you want!

Thanks for reading! ? the repo if you find it useful!
```

---

## ?? Dev.to / Medium Article

### Title
```
Building Multi-Tenant SaaS Pipelines in .NET: Introducing Runnable ??
```

### Subtitle
```
LangChain-inspired composable pipelines with automatic tenant isolation, context propagation, and per-tenant caching
```

### Article Outline

```markdown
# Building Multi-Tenant SaaS Pipelines in .NET: Introducing Runnable ??

## The Problem with Multi-Tenant Applications

If you've ever built a multi-tenant SaaS application, you know the pain:

```csharp
// ?? The nightmare of manual context passing
public async Task<Response> ProcessRequest(
    Request request, 
    string tenantId, 
    string userId, 
    string correlationId)
{
    var data = await GetData(request, tenantId);
    var filtered = FilterByTenant(data, tenantId);
    var cached = await GetFromCache(filtered, tenantId);
    var enriched = Enrich(cached, tenantId, userId, correlationId);
    var result = await Save(enriched, tenantId, correlationId);
    return result;
}
```

**Problems:**
- ? Context passed manually everywhere
- ? Easy to forget tenant filtering ¡ú security issue!
- ? Per-tenant caching is complex
- ? Cross-tenant data contamination risk
- ? Verbose, hard to maintain

## Introducing Runnable

**Runnable** is a composable pipeline library inspired by LangChain's LCEL, with first-class multi-tenancy support.

```csharp
// ? The beautiful way
var result = await service
    .GetData()
    .WithTenant(request.TenantId)
    .FilterContext((data, ctx) => data.TenantId == ctx.TenantId)
    .WithCachePerTenant()
    .MapContext((data, ctx) => Enrich(data, ctx))
    .InvokeAsync(request);
```

**Benefits:**
- ? Context flows automatically
- ? Tenant isolation enforced by type system
- ? Per-tenant caching in one line
- ? Clean, readable, composable
- ? Production-ready resilience patterns

## How It Works

### 1. Context Propagation

```csharp
var pipeline = service
    .WithTenant("acme-corp")
    .WithUser("alice@acme.com")
    .WithCorrelationId(Guid.NewGuid().ToString());

// Context is available in ALL downstream operations!
```

### 2. Per-Tenant Caching

```csharp
// Each tenant gets isolated cache automatically
var cached = expensiveOperation
    .WithCachePerTenantTTL(TimeSpan.FromMinutes(10));

// tenant-A cache != tenant-B cache
```

### 3. Smart Routing

```csharp
// Route premium tenants to faster infrastructure
var handler = defaultHandler
    .BranchByTenant("premium", premiumHandler)
    .BranchByTenant("enterprise", enterpriseHandler);
```

### 4. Security by Default

```csharp
// Automatic tenant isolation
dataService
    .GetAll()
    .FilterContext((data, ctx) => data.TenantId == ctx.TenantId);
```

## Real-World Example: Multi-Tenant API

[... Include the complete multi-tenant SaaS pipeline example ...]

## Advanced Features

### A/B Testing
### Distributed Tracing
### Resilience Patterns
### Custom Caching Strategies

## Performance

- Context propagation: ~1-2 ¦Ìs overhead
- LRU cache: Bounded memory
- Async-first: No blocking calls

## Getting Started

[... Installation and basic examples ...]

## Conclusion

Runnable makes multi-tenant SaaS development in .NET cleaner, safer, and more maintainable.

Try it today! ? the repo if you like it!

---

**Links:**
- ?? [NuGet](https://nuget.org/packages/Runnable)
- ?? [GitHub](https://github.com/codenjwu/Runnable)
- ?? [Discussions](https://github.com/codenjwu/Runnable/discussions)
```

---

## ?? YouTube Video Script (5 minutes)

### Title
```
Runnable: Multi-Tenant Pipelines in .NET Made Easy | Open Source
```

### Description
```
Learn how to build clean, composable multi-tenant data pipelines in .NET with Runnable - an open-source library inspired by LangChain's LCEL.

?? Timestamps:
0:00 - The Problem
0:45 - What is Runnable?
1:30 - Context Propagation
2:15 - Per-Tenant Caching
3:00 - Real-World Example
4:15 - Getting Started
4:45 - Call to Action

?? Links:
?? NuGet: https://nuget.org/packages/Runnable
?? GitHub: https://github.com/codenjwu/Runnable
?? Docs: [link]

#dotnet #csharp #opensource #saas #multitenant
```

---

## ?? Hacker News Post

### Title
```
Runnable ¨C Composable pipelines for .NET with first-class multi-tenancy
```

### Body
```
Hey HN!

I built Runnable to solve a problem I faced in every multi-tenant SaaS app: managing tenant context across complex data pipelines without passing it manually everywhere.

Key features:
- Context (tenant ID, user ID, correlation ID) flows automatically through the pipeline
- Per-tenant cache isolation in one line of code
- Tenant-based routing (premium vs free tier)
- Type-safe composition (0-16 parameters)
- Production-ready resilience patterns (retry, timeout, circuit breaker)

Example:

```csharp
var result = await service
    .GetData()
    .WithTenant(request.TenantId)
    .FilterContext((data, ctx) => data.TenantId == ctx.TenantId)
    .WithCachePerTenant()
    .WithExponentialBackoff(3)
    .InvokeAsync(request);
```

Inspired by LangChain's LCEL but optimized for .NET multi-tenant scenarios.

MIT licensed. Would love feedback!

GitHub: https://github.com/codenjwu/Runnable
```

---

## ?? Email to .NET Weekly / C# Digest

### Subject
```
New Library: Runnable - Multi-Tenant Pipelines for .NET
```

### Body
```
Hi [Editor Name],

I'd like to share a new open-source library that might interest your readers: Runnable.

**What is it?**
A composable pipeline library for .NET with first-class multi-tenancy support, inspired by LangChain's LCEL.

**Why it's interesting:**
- Solves a common pain point: managing tenant context in SaaS apps
- Per-tenant caching with automatic isolation
- Type-safe fluent API (0-16 parameters)
- Production-ready features (retry, timeout, circuit breaker)

**Example:**
```csharp
var pipeline = service
    .WithTenant(request.TenantId)
    .FilterContext((data, ctx) => data.TenantId == ctx.TenantId)
    .WithCachePerTenantTTL(TimeSpan.FromMinutes(10))
    .BranchByTenant("premium", premiumHandler);
```

**Links:**
- GitHub: https://github.com/codenjwu/Runnable
- NuGet: https://nuget.org/packages/Runnable
- Docs: [link]

Thanks for considering!

Best regards,
[Your Name]
```

---

## ?? Visual Assets Suggestions

### GitHub Social Preview (1280x640)

```
[Background: Gradient from #512BD4 (purple) to #239120 (green)]

Large Text:
"Runnable"

Subtitle:
"Multi-Tenant Pipelines for .NET"

Code Snippet:
```csharp
service
  .WithTenant(id)
  .FilterContext(...)
  .WithCachePerTenant()
```

Bottom:
".NET 5+ | MIT Licensed | github.com/codenjwu/Runnable"
```

### NuGet Icon (128x128)

- Purple/Green gradient background
- White "R" in modern font
- Small "pipeline" icon (three connected nodes)

---

## ?? Analytics Tracking

### UTM Parameters for Links

```
NuGet from Twitter: ?utm_source=twitter&utm_medium=social&utm_campaign=launch
GitHub from Reddit: ?utm_source=reddit&utm_medium=social&utm_campaign=launch
Docs from HN: ?utm_source=hackernews&utm_medium=social&utm_campaign=launch
```

---

## ?? Launch Checklist

### Pre-Launch
- [ ] Update README.md with final content
- [ ] Add CODE_OF_CONDUCT.md
- [ ] Add SECURITY.md
- [ ] Create GitHub repo topics/tags
- [ ] Set up GitHub Discussions
- [ ] Create release notes
- [ ] Build NuGet package
- [ ] Test NuGet package locally

### Launch Day
- [ ] Push to NuGet.org
- [ ] Create GitHub release with tag
- [ ] Post on Twitter (thread)
- [ ] Post on LinkedIn
- [ ] Post on Reddit (r/dotnet, r/csharp)
- [ ] Post on Hacker News
- [ ] Submit to Dev.to
- [ ] Email .NET Weekly
- [ ] Post in .NET Discord/Slack communities

### Post-Launch
- [ ] Monitor GitHub Issues/Discussions
- [ ] Respond to feedback
- [ ] Write follow-up blog post
- [ ] Create video tutorial
- [ ] Submit to Awesome .NET lists

---

**Good luck with your launch! ??**
