# Runnable Utilities - LangChain-Inspired Features

This document describes the enhanced features added to the Runnable library to provide a LangChain-like experience in C#.

## Overview

The Runnable library now includes comprehensive utilities for functional composition, error handling, caching, and more. These features make it easier to build robust, composable data processing pipelines similar to LangChain's Runnable protocol.

## Table of Contents

1. [Basic Composition - Pipe](#pipe)
2. [Lambda Creation](#runnable-lambda)
3. [Passthrough](#runnable-passthrough)
4. [Transformation - Map](#map)
5. [Side Effects - Tap](#tap)
6. [Error Handling - WithFallback](#withfallback)
7. [Retry Logic - WithRetry](#withretry)
8. [Filtering](#filter)
9. [Parallel Execution](#parallel-execution)
10. [Caching](#caching)
11. [Delay & Timeout](#delay--timeout)

---

## Pipe

**Purpose**: Compose two runnables into a single pipeline.

### Examples

```csharp
using Runnable;

// Basic pipe
var addOne = new Runnable<int, int>(x => x + 1);
var double = new Runnable<int, int>(x => x * 2);
var toString = new Runnable<int, string>(x => $"Result: {x}");

var pipeline = addOne.Pipe(double).Pipe(toString);
Console.WriteLine(pipeline.Invoke(5)); // Output: "Result: 12"

// Multi-parameter pipe
var concat = new Runnable<string, string, string>((a, b) => a + b);
var getLength = new Runnable<string, int>(s => s.Length);
var formatLength = new Runnable<int, string>(len => $"Length: {len}");

var complexPipeline = concat.Pipe(getLength).Pipe(formatLength);
Console.WriteLine(complexPipeline.Invoke("Hello", "World")); 
// Output: "Length: 10"
```

---

## Runnable Lambda

**Purpose**: Quick creation of runnables from lambda functions.

### Examples

```csharp
using Runnable;

// Simple lambda
var square = RunnableLambda.Create<int, int>(x => x * x);
Console.WriteLine(square.Invoke(5)); // Output: 25

// With async support
var asyncDouble = RunnableLambda.Create<int, int>(
    x => x * 2,
    async x => await Task.FromResult(x * 2)
);
var result = await asyncDouble.InvokeAsync(10); // Output: 20

// Multi-parameter
var add = RunnableLambda.Create<int, int, int>((a, b) => a + b);
Console.WriteLine(add.Invoke(3, 4)); // Output: 7
```

---

## Runnable Passthrough

**Purpose**: Pass input through unchanged, optionally with side effects.

### Examples

```csharp
using Runnable;

// Simple passthrough
var passthrough = RunnablePassthrough.Create<int>();
Console.WriteLine(passthrough.Invoke(42)); // Output: 42

// Passthrough with side effect (logging)
var loggedPassthrough = RunnablePassthrough.Create<string>(
    x => Console.WriteLine($"Processing: {x}")
);
var result = loggedPassthrough.Invoke("Hello");
// Logs: "Processing: Hello"
// Returns: "Hello"

// Async side effect
var asyncPassthrough = RunnablePassthrough.Create<int>(
    async x => await File.WriteAllTextAsync("log.txt", x.ToString())
);
```

---

## Map

**Purpose**: Transform the output of a runnable without changing the input signature.

### Examples

```csharp
using Runnable;

// Map to different type
var getNumber = new Runnable<string, int>(s => s.Length);
var mapped = getNumber.Map(len => $"Length is {len}");
Console.WriteLine(mapped.Invoke("Hello")); // Output: "Length is 5"

// Chain multiple maps
var pipeline = getNumber
    .Map(len => len * 2)
    .Map(doubled => $"Doubled: {doubled}");

// Async map
var asyncMapped = getNumber.MapAsync(async len =>
{
    await Task.Delay(100);
    return $"Async result: {len}";
});
```

---

## Tap

**Purpose**: Execute side effects without modifying the output.

### Examples

```csharp
using Runnable;

var processor = new Runnable<string, string>(s => s.ToUpper());

// Add logging without changing behavior
var logged = processor.Tap(result => 
    Console.WriteLine($"Result: {result}")
);

Console.WriteLine(logged.Invoke("hello"));
// Logs: "Result: HELLO"
// Returns: "HELLO"

// Multiple taps in a pipeline
var pipeline = processor
    .Tap(r => Console.WriteLine($"Step 1: {r}"))
    .Map(r => r + "!")
    .Tap(r => Console.WriteLine($"Step 2: {r}"));

// Async tap
var asyncTapped = processor.TapAsync(async result =>
{
    await File.AppendAllTextAsync("log.txt", result + "\n");
});
```

---

## WithFallback

**Purpose**: Provide error handling with fallback runnables or values.

### Examples

```csharp
using Runnable;

// Fallback to another runnable
var risky = new Runnable<int, string>(x => 
{
    if (x < 0) throw new ArgumentException("Negative not allowed");
    return x.ToString();
});

var safe = new Runnable<int, string>(x => "default");
var withFallback = risky.WithFallback(safe);

Console.WriteLine(withFallback.Invoke(5));   // Output: "5"
Console.WriteLine(withFallback.Invoke(-1));  // Output: "default"

// Fallback to a value
var withValue = risky.WithFallbackValue("error");
Console.WriteLine(withValue.Invoke(-1)); // Output: "error"

// Async fallback
var asyncRisky = new Runnable<int, string>(
    x => x.ToString(),
    async x => 
    {
        await Task.Delay(10);
        if (x < 0) throw new Exception();
        return x.ToString();
    }
);
var asyncSafe = asyncRisky.WithFallbackValue("failed");
```

---

## WithRetry

**Purpose**: Automatically retry failed operations.

### Examples

```csharp
using Runnable;

int attempts = 0;
var flaky = new Runnable<int, string>(x =>
{
    attempts++;
    if (attempts < 3) throw new Exception("Not ready");
    return $"Success on attempt {attempts}";
});

// Retry up to 3 times
var retryable = flaky.WithRetry(maxAttempts: 3);
Console.WriteLine(retryable.Invoke(0)); 
// Output: "Success on attempt 3"

// With custom delay
var withDelay = flaky.WithRetry(
    maxAttempts: 5,
    delay: TimeSpan.FromMilliseconds(500)
);

// Async retry
var asyncFlaky = new Runnable<int, string>(
    x => "sync",
    async x =>
    {
        await Task.Delay(10);
        if (DateTime.Now.Millisecond % 2 == 0) 
            throw new Exception();
        return "success";
    }
);
var asyncRetryable = asyncFlaky.WithRetry(maxAttempts: 3);
```

---

## Filter

**Purpose**: Conditionally execute based on a predicate.

### Examples

```csharp
using Runnable;

var process = new Runnable<int, string>(x => $"Processed: {x}");

// Only process positive numbers
var filtered = process.Filter(
    x => x > 0,
    defaultValue: "skipped"
);

Console.WriteLine(filtered.Invoke(5));   // Output: "Processed: 5"
Console.WriteLine(filtered.Invoke(-1));  // Output: "skipped"

// Complex filter
var evenOnly = process.Filter(
    x => x % 2 == 0,
    defaultValue: "not even"
);
```

---

## Parallel Execution

**Purpose**: Execute multiple runnables or inputs in parallel.

### Examples

```csharp
using Runnable;

var slowProcess = new Runnable<int, string>(
    x => x.ToString(),
    async x =>
    {
        await Task.Delay(100);
        return $"Processed: {x}";
    }
);

// Run multiple inputs in parallel
var inputs = new[] { 1, 2, 3, 4, 5 };
var results = await slowProcess.BatchParallel(inputs);
// Much faster than sequential batch!

// Run multiple different runnables in parallel
var runnables = new[]
{
    new Runnable<int, string>(x => $"A: {x}"),
    new Runnable<int, string>(x => $"B: {x}"),
    new Runnable<int, string>(x => $"C: {x}")
};
var parallelResults = await runnables.InvokeParallel(42);
// Returns: ["A: 42", "B: 42", "C: 42"]
```

---

## Caching

**Purpose**: Cache results to avoid redundant computations.

### Examples

```csharp
using Runnable;

int callCount = 0;
var expensive = new Runnable<int, string>(x =>
{
    callCount++;
    Thread.Sleep(1000); // Simulate expensive operation
    return $"Result for {x}";
});

var cached = expensive.WithCache();

Console.WriteLine(cached.Invoke(5)); // Takes 1 second, callCount = 1
Console.WriteLine(cached.Invoke(5)); // Instant!, callCount = 1
Console.WriteLine(cached.Invoke(10)); // Takes 1 second, callCount = 2
Console.WriteLine(cached.Invoke(5)); // Instant!, callCount = 2

// Works with async too
var result1 = await cached.InvokeAsync(5); // Uses cache
var result2 = await cached.InvokeAsync(10); // Uses cache
```

---

## Delay & Timeout

**Purpose**: Add delays or timeouts to execution.

### Examples

```csharp
using Runnable;

// Add delay between invocations (rate limiting)
var process = new Runnable<int, string>(x => x.ToString());
var delayed = process.WithDelay(TimeSpan.FromMilliseconds(500));

var start = DateTime.Now;
delayed.Invoke(1);
delayed.Invoke(2);
delayed.Invoke(3);
var elapsed = DateTime.Now - start;
// elapsed will be ~1.5 seconds (3 invocations with 0.5s delay each)

// Add timeout to async operations
var slow = new Runnable<int, string>(
    x => x.ToString(),
    async x =>
    {
        await Task.Delay(5000); // Very slow
        return x.ToString();
    }
);

var timedOut = slow.WithTimeout(TimeSpan.FromSeconds(1));

try
{
    await timedOut.InvokeAsync(42);
}
catch (TimeoutException ex)
{
    Console.WriteLine("Operation timed out!");
}
```

---

## Complete Example: Real-World Pipeline

Here's a comprehensive example combining multiple features:

```csharp
using Runnable;

// Build a robust data processing pipeline
var parseData = RunnableLambda.Create<string, int>(s => int.Parse(s));

var validateData = parseData
    .Filter(x => x >= 0, defaultValue: 0) // Only positive numbers
    .Tap(x => Console.WriteLine($"Validated: {x}")); // Log

var processData = validateData
    .Map(x => x * 2) // Transform
    .WithCache() // Cache results
    .WithRetry(maxAttempts: 3) // Retry on failure
    .WithFallbackValue(0) // Fallback if all retries fail
    .WithTimeout(TimeSpan.FromSeconds(5)); // Timeout safety

// Use the pipeline
var inputs = new[] { "10", "20", "-5", "30" };
var results = await processData.BatchParallel(inputs);
// Results: [20, 40, 0, 60]
// Logs: "Validated: 10", "Validated: 20", "Validated: 30"

// The pipeline is:
// 1. Parse string to int
// 2. Filter out negative numbers
// 3. Log validation
// 4. Double the value
// 5. Cache results
// 6. Retry up to 3 times on failure
// 7. Use 0 if all retries fail
// 8. Timeout after 5 seconds
// 9. Process all inputs in parallel
```

---

## Extension Method Summary

All extension methods are available in the `Runnable` namespace:

| Method | Purpose | Applies To |
|--------|---------|-----------|
| `Pipe` | Compose runnables | All runnables |
| `Map` | Transform output | All runnables |
| `MapAsync` | Async transform | All runnables |
| `Tap` | Side effects | All runnables |
| `TapAsync` | Async side effects | All runnables |
| `WithFallback` | Error handling | All runnables |
| `WithFallbackValue` | Error handling with value | All runnables |
| `WithRetry` | Retry on failure | All runnables |
| `Filter` | Conditional execution | All runnables |
| `WithCache` | Result caching | All runnables |
| `WithDelay` | Rate limiting | All runnables |
| `WithTimeout` | Timeout protection | All runnables |
| `BatchParallel` | Parallel batch processing | All runnables |
| `InvokeParallel` | Run multiple runnables | IEnumerable<IRunnable> |

---

## Tips and Best Practices

1. **Chain operations**: Most methods return a new Runnable, allowing fluent chaining
2. **Error handling**: Use `WithFallback` and `WithRetry` together for robust pipelines
3. **Performance**: Use `WithCache` for expensive computations, `BatchParallel` for I/O operations
4. **Debugging**: Use `Tap` to add logging without modifying your pipeline logic
5. **Type safety**: All operations maintain strong typing throughout the pipeline

---

## Comparison with LangChain

| LangChain Feature | C# Runnable Equivalent |
|-------------------|------------------------|
| `pipe()` | `Pipe()` |
| `map()` | `Map()` |
| `with_fallbacks()` | `WithFallback()` |
| `with_retry()` | `WithRetry()` |
| `with_config()` | N/A (use .NET configuration) |
| `RunnableLambda` | `RunnableLambda.Create()` |
| `RunnablePassthrough` | `RunnablePassthrough.Create()` |
| `batch()` | `Batch()` / `BatchAsync()` |
| `stream()` | `Stream()` |
| `ainvoke()` | `InvokeAsync()` |
| `abatch()` | `BatchAsync()` |

---

## Future Enhancements

Potential additions:
- **RunnableParallel**: Explicit parallel composition
- **RunnableBranch**: Conditional branching
- **RunnableRouter**: Route based on input
- **Config system**: Runtime configuration override
- **Tracing/Observability**: Built-in tracing support
