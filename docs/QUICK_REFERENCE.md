# Runnable Quick Reference

## Creating Runnables

```csharp
// From lambda
var r1 = RunnableLambda.Create<int, int>(x => x * 2);

// From Func extension
Func<string, int> getLength = s => s.Length;
var r2 = getLength.AsRunnable();

// Direct construction
var r3 = new Runnable<int, string>(x => x.ToString());

// With async
var r4 = new Runnable<int, int>(
    x => x * 2,
    async x => await Task.FromResult(x * 2)
);

// Multi-parameter (2-5 params)
var add = RunnableLambda.Create<int, int, int>((a, b) => a + b);
var add3 = RunnableLambda.Create<int, int, int, int>((a, b, c) => a + b + c);
```

## Composition

```csharp
// Pipe (chain runnables)
var pipeline = r1.Pipe(r2).Pipe(r3);
result = pipeline.Invoke(input);

// Or using operator (if available)
var pipeline = r1 | r2 | r3;
```

## Transformation

```csharp
// Map (transform output)
var mapped = runnable.Map(x => x.ToString());
var asyncMapped = runnable.MapAsync(async x => await TransformAsync(x));
```

## Side Effects

```csharp
// Tap (logging, monitoring)
var logged = runnable.Tap(x => Console.WriteLine($"Value: {x}"));
var asyncTapped = runnable.TapAsync(async x => await LogAsync(x));

// Passthrough (identity with side effect)
var pass = RunnablePassthrough.Create<int>(x => Log(x));
```

## Error Handling

```csharp
// Fallback to another runnable
var safe = risky.WithFallback(fallbackRunnable);

// Fallback to a value
var safe = risky.WithFallbackValue(defaultValue);

// Retry on failure
var robust = flaky.WithRetry(
    maxAttempts: 3,
    delay: TimeSpan.FromMilliseconds(100)
);
```

## Conditional Execution

```csharp
// Filter (only execute if predicate is true)
var evenOnly = runnable.Filter(
    x => x % 2 == 0,
    defaultValue: 0
);
```

## Performance

```csharp
// Cache results
var cached = expensive.WithCache();

// Parallel batch processing
var results = await runnable.BatchParallel(inputs);

// Run multiple runnables in parallel
var runnables = new[] { r1, r2, r3 };
var results = await runnables.InvokeParallel(input);
```

## Resource Management

```csharp
// Add delay between calls (rate limiting)
var limited = runnable.WithDelay(TimeSpan.FromMilliseconds(500));

// Timeout for async operations
var timedOut = runnable.WithTimeout(TimeSpan.FromSeconds(5));
```

## Complete Example

```csharp
using Runnable;

// Build a robust pipeline
var parseData = RunnableLambda.Create<string, int>(s => int.Parse(s));

var pipeline = parseData
    .Filter(x => x >= 0, defaultValue: 0)          // Validate
    .Tap(x => Console.WriteLine($"Valid: {x}"))    // Log
    .Map(x => x * 2)                                // Transform
    .WithCache()                                    // Cache
    .WithRetry(maxAttempts: 3)                      // Retry
    .WithFallbackValue(0)                           // Fallback
    .WithTimeout(TimeSpan.FromSeconds(5))           // Timeout
    .Map(x => $"Result: {x}");                      // Format

// Use it
var result = pipeline.Invoke("42");
var results = await pipeline.BatchParallel(new[] { "1", "2", "3" });
```

## Method Chaining Patterns

```csharp
// Error handling chain
var safe = runnable
    .WithRetry(3)
    .WithFallbackValue(default);

// Transform chain
var transformed = runnable
    .Map(x => x.Process())
    .Map(x => x.ToString())
    .Map(x => x.ToUpper());

// Monitoring chain
var monitored = runnable
    .Tap(x => Log($"Input: {x}"))
    .Map(x => Transform(x))
    .Tap(x => Log($"Output: {x}"));

// Performance chain
var optimized = runnable
    .WithCache()
    .WithDelay(TimeSpan.FromMilliseconds(100))
    .WithTimeout(TimeSpan.FromSeconds(30));
```

## Common Patterns

### Validation Pipeline
```csharp
var validator = RunnableLambda.Create<Data, Data>(d => Validate(d))
    .Filter(d => d.IsValid, defaultValue: Data.Empty)
    .Tap(d => Log($"Validated: {d}"));
```

### Resilient API Call
```csharp
var apiCall = RunnableLambda.Create<Request, Response>(r => CallApi(r))
    .WithRetry(maxAttempts: 3, delay: TimeSpan.FromSeconds(1))
    .WithTimeout(TimeSpan.FromSeconds(30))
    .WithFallbackValue(Response.Error);
```

### Cached Computation
```csharp
var compute = RunnableLambda.Create<int, Result>(x => Expensive(x))
    .WithCache()
    .Tap(r => Console.WriteLine($"Cache: {r}"));
```

### Data Processing Pipeline
```csharp
var process = parseInput
    .Pipe(validate)
    .Pipe(transform)
    .Pipe(enrich)
    .WithRetry(2)
    .WithFallbackValue(default);

var results = await process.BatchParallel(inputs);
```

## Tips

1. **Chain liberally** - Most methods return new Runnables for fluent chaining
2. **Combine features** - Use WithRetry + WithFallback for robust error handling
3. **Cache strategically** - Use WithCache for expensive, pure computations
4. **Log with Tap** - Add logging without breaking the pipeline flow
5. **Parallelize I/O** - Use BatchParallel for I/O-bound operations
6. **Type safety** - All operations maintain full generic type information

## Extension Method Reference

| Method | Purpose | Example |
|--------|---------|---------|
| `Pipe<TNext>()` | Chain runnables | `r1.Pipe(r2)` |
| `Map<TNext>()` | Transform output | `.Map(x => x.ToString())` |
| `MapAsync<TNext>()` | Async transform | `.MapAsync(async x => await F(x))` |
| `Tap()` | Side effects | `.Tap(x => Log(x))` |
| `TapAsync()` | Async side effects | `.TapAsync(async x => await Log(x))` |
| `WithFallback()` | Fallback runnable | `.WithFallback(safeRunnable)` |
| `WithFallbackValue()` | Fallback value | `.WithFallbackValue(0)` |
| `WithRetry()` | Retry on failure | `.WithRetry(3)` |
| `Filter()` | Conditional exec | `.Filter(x => x > 0, 0)` |
| `WithCache()` | Cache results | `.WithCache()` |
| `WithDelay()` | Rate limit | `.WithDelay(TimeSpan.FromMilliseconds(100))` |
| `WithTimeout()` | Timeout async | `.WithTimeout(TimeSpan.FromSeconds(5))` |
| `BatchParallel()` | Parallel batch | `await r.BatchParallel(inputs)` |
| `InvokeParallel()` | Parallel invoke | `await runnables.InvokeParallel(input)` |
