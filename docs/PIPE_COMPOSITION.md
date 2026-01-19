# Pipe - Function Composition for Runnables

## Overview

The `Pipe` method enables **function composition** by chaining runnables together. It feeds the output of one runnable into the input of another, creating powerful data transformation pipelines.

## Complete Support

? **Pipe is now available for all parameter counts (0-16)**

| First Runnable | Second Runnable | Result |
|---------------|----------------|--------|
| `IRunnable<TOutput>` (0 params) | `IRunnable<TOutput, TNext>` | `Runnable<TNext>` (0 params) |
| `IRunnable<T1, TOutput>` (1 param) | `IRunnable<TOutput, TNext>` | `Runnable<T1, TNext>` (1 param) |
| `IRunnable<T1, T2, TOutput>` (2 params) | `IRunnable<TOutput, TNext>` | `Runnable<T1, T2, TNext>` (2 params) |
| ... | ... | ... |
| `IRunnable<T1...T16, TOutput>` (16 params) | `IRunnable<TOutput, TNext>` | `Runnable<T1...T16, TNext>` (16 params) |

## Basic Usage

### Simple Composition (1 Parameter)

```csharp
var parseInt = RunnableLambda.Create<string, int>(s => int.Parse(s));
var double_ = RunnableLambda.Create<int, int>(x => x * 2);
var toString = RunnableLambda.Create<int, string>(x => $"Result: {x}");

// Compose: string -> int -> int -> string
var pipeline = parseInt.Pipe(double_).Pipe(toString);

Console.WriteLine(pipeline.Invoke("5")); // "Result: 10"
```

### Zero Parameters

```csharp
var getNumber = RunnableLambda.Create(() => 42);
var square = RunnableLambda.Create<int, int>(x => x * x);

var pipeline = getNumber.Pipe(square);
Console.WriteLine(pipeline.Invoke()); // 1764
```

### Two Parameters

```csharp
var add = RunnableLambda.Create<int, int, int>((a, b) => a + b);
var triple = RunnableLambda.Create<int, int>(x => x * 3);

var pipeline = add.Pipe(triple);
Console.WriteLine(pipeline.Invoke(5, 10)); // (5+10)*3 = 45
```

### Three Parameters

```csharp
var add3 = RunnableLambda.Create<int, int, int, int>((a, b, c) => a + b + c);
var double_ = RunnableLambda.Create<int, int>(x => x * 2);
var format = RunnableLambda.Create<int, string>(x => $"Result: {x}");

var pipeline = add3.Pipe(double_).Pipe(format);
Console.WriteLine(pipeline.Invoke(1, 2, 3)); // "Result: 12"
```

## How Pipe Works

```
©°©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©´        ©°©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©´        ©°©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©´
©¦  Runnable1  ©¦        ©¦  Runnable2  ©¦        ©¦  Runnable3  ©¦
©¦  (T1¡úT2)    ©¦ .Pipe  ©¦  (T2¡úT3)    ©¦ .Pipe  ©¦  (T3¡úT4)    ©¦
©¸©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¼  ©¤©¤©¤©¤> ©¸©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¼  ©¤©¤©¤©¤> ©¸©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¼
      ¡ý                       ¡ý                       ¡ý
      ©¸©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©Ø©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¼
                            Result
                      Runnable (T1¡úT4)
```

The output type of the first runnable must match the input type of the second runnable.

## Multi-Parameter Examples

### 5 Parameters

```csharp
var add5 = RunnableLambda.Create<int, int, int, int, int, int>(
    (a, b, c, d, e) => a + b + c + d + e);
var format = RunnableLambda.Create<int, string>(x => $"Total: {x:N0}");

var pipeline = add5.Pipe(format);
Console.WriteLine(pipeline.Invoke(10, 20, 30, 40, 50)); // "Total: 150"
```

### 8 Parameters

```csharp
var add8 = RunnableLambda.Create<int, int, int, int, int, int, int, int, int>(
    (a, b, c, d, e, f, g, h) => a + b + c + d + e + f + g + h);
var isEven = RunnableLambda.Create<int, bool>(x => x % 2 == 0);

var pipeline = add8.Pipe(isEven);
Console.WriteLine(pipeline.Invoke(1, 2, 3, 4, 5, 6, 7, 8)); // true (36 is even)
```

### 16 Parameters (Maximum)

```csharp
var sum16 = RunnableLambda.Create<int, int, int, int, int, int, int, int, 
    int, int, int, int, int, int, int, int, int>(
    (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16) =>
        a1 + a2 + a3 + a4 + a5 + a6 + a7 + a8 + a9 + 
        a10 + a11 + a12 + a13 + a14 + a15 + a16);

var analyze = RunnableLambda.Create<int, string>(sum => 
    sum < 50 ? "Low" : sum < 100 ? "Medium" : "High");

var pipeline = sum16.Pipe(analyze);
var result = pipeline.Invoke(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);
Console.WriteLine(result); // "High" (136 > 100)
```

## Combining Pipe with Other Extensions

Pipe works seamlessly with all other extension methods:

```csharp
var multiply = RunnableLambda.Create<int, int, int>((a, b) => a * b);
var addTen = RunnableLambda.Create<int, int>(x => x + 10);

var pipeline = multiply
    .Pipe(addTen)                              // Composition
    .Map(result => result * 100)               // Transformation
    .Tap(result => logger.Log(result))        // Side effect
    .WithRetry(3)                              // Retry logic
    .WithFallbackValue(0);                    // Error handling

var result = pipeline.Invoke(5, 6); // ((5*6)+10)*100 = 4000
```

## Async Support

Pipe fully supports async operations:

```csharp
var fetchData = RunnableLambda.Create<int, string>(
    id => $"Data{id}",
    async id => {
        await Task.Delay(100);
        return await httpClient.GetStringAsync($"/api/data/{id}");
    });

var processData = RunnableLambda.Create<string, string>(
    s => s.ToUpper(),
    async s => {
        await Task.Delay(50);
        return await ProcessAsync(s);
    });

var asyncPipeline = fetchData.Pipe(processData);
var result = await asyncPipeline.InvokeAsync(42);
```

## Real-World Examples

### Data Transformation Pipeline

```csharp
// Parse -> Validate -> Transform -> Format
var parseInput = RunnableLambda.Create<string, int>(s => int.Parse(s));
var validateRange = RunnableLambda.Create<int, int>(x => {
    if (x < 0 || x > 100) throw new ArgumentOutOfRangeException();
    return x;
});
var calculatePercentage = RunnableLambda.Create<int, double>(x => x / 100.0);
var formatPercentage = RunnableLambda.Create<double, string>(x => $"{x:P0}");

var pipeline = parseInput
    .Pipe(validateRange)
    .Pipe(calculatePercentage)
    .Pipe(formatPercentage)
    .WithFallbackValue("Invalid");

Console.WriteLine(pipeline.Invoke("75"));  // "75%"
Console.WriteLine(pipeline.Invoke("150")); // "Invalid"
```

### Configuration Builder (6 Parameters)

```csharp
var buildConfig = RunnableLambda.Create<string, int, bool, string, int, TimeSpan, string>(
    (host, port, useSsl, db, poolSize, timeout) =>
        $"Server={host}:{port};Database={db};SSL={useSsl};" +
        $"PoolSize={poolSize};Timeout={timeout.TotalSeconds}");

var validateConfig = RunnableLambda.Create<string, bool>(config => 
    config.Contains("Server=") && config.Contains("Database="));

var formatResult = RunnableLambda.Create<bool, string>(valid =>
    valid ? "? Valid configuration" : "? Invalid configuration");

var pipeline = buildConfig
    .Pipe(validateConfig)
    .Pipe(formatResult);

var result = pipeline.Invoke(
    "localhost", 5432, true, "mydb", 10, TimeSpan.FromSeconds(30));
Console.WriteLine(result); // "? Valid configuration"
```

### Data Loading from Multiple Sources (4 Parameters)

```csharp
// Step 1: Load data from 4 sources
var loadData = RunnableLambda.Create<string, string, string, string, string>(
    (source1, source2, source3, source4) => 
        $"{source1},{source2},{source3},{source4}");

// Step 2: Parse CSV
var parseCsv = RunnableLambda.Create<string, string[]>(
    csv => csv.Split(','));

// Step 3: Count items
var count = RunnableLambda.Create<string[], int>(arr => arr.Length);

// Step 4: Categorize
var categorize = RunnableLambda.Create<int, string>(n => 
    n < 3 ? "Few" : n < 6 ? "Some" : "Many");

var pipeline = loadData
    .Pipe(parseCsv)
    .Pipe(count)
    .Pipe(categorize)
    .WithFallbackValue("Error");

var category = pipeline.Invoke("item1", "item2", "item3", "item4");
Console.WriteLine(category); // "Some"
```

### Mathematical Function Composition

```csharp
// f(x) = x + 5
var addFive = RunnableLambda.Create<int, int>(x => x + 5);

// g(x) = x * 2
var double_ = RunnableLambda.Create<int, int>(x => x * 2);

// h(x) = x?
var square = RunnableLambda.Create<int, int>(x => x * x);

// Composition: h(g(f(x))) = (2(x+5))?
var composed = addFive.Pipe(double_).Pipe(square);

// For x = 3: f(3) = 8, g(8) = 16, h(16) = 256
Console.WriteLine(composed.Invoke(3)); // 256
```

## Type Safety

Pipe is fully type-safe at compile time:

```csharp
var intToString = RunnableLambda.Create<int, string>(x => x.ToString());
var stringLength = RunnableLambda.Create<string, int>(s => s.Length);

// ? This compiles - types match
var valid = intToString.Pipe(stringLength);

// ? This won't compile - type mismatch
// var invalid = stringLength.Pipe(intToString); 
// Error: Can't pipe int -> string to string -> int
```

## Performance

- **Zero overhead**: Pipe creates a thin wrapper that directly delegates to underlying runnables
- **Async-friendly**: Proper async/await patterns throughout
- **Lazy evaluation**: Composition happens at definition time, execution at invoke time
- **Memory efficient**: No unnecessary allocations or closures

## Method Signature

```csharp
// 0 parameters
public static Runnable<TNext> Pipe<TOutput, TNext>(
    this IRunnable<TOutput> first,
    IRunnable<TOutput, TNext> second)

// 1 parameter (classic composition)
public static Runnable<TInput, TNext> Pipe<TInput, TOutput, TNext>(
    this IRunnable<TInput, TOutput> first,
    IRunnable<TOutput, TNext> second)

// 2 parameters
public static Runnable<T1, T2, TNext> Pipe<T1, T2, TOutput, TNext>(
    this IRunnable<T1, T2, TOutput> first,
    IRunnable<TOutput, TNext> second)

// ... and so on for 3-16 parameters
```

## Best Practices

### 1. **Chain Related Transformations**
```csharp
var pipeline = loadData
    .Pipe(parse)
    .Pipe(validate)
    .Pipe(transform)
    .Pipe(format);
```

### 2. **Combine with Error Handling**
```csharp
var safeP pipeline = risky1
    .Pipe(risky2)
    .WithRetry(3)
    .WithFallbackValue(defaultValue);
```

### 3. **Use for Readability**
```csharp
// ? Nested calls (hard to read)
var result = format(transform(validate(parse(loadData(input)))));

// ? Pipeline (easy to read)
var pipeline = loadData.Pipe(parse).Pipe(validate).Pipe(transform).Pipe(format);
var result = pipeline.Invoke(input);
```

### 4. **Reuse Pipelines**
```csharp
var commonPipeline = parse.Pipe(validate).Pipe(transform);

var pipeline1 = commonPipeline.Pipe(formatJson);
var pipeline2 = commonPipeline.Pipe(formatXml);
var pipeline3 = commonPipeline.Pipe(formatCsv);
```

## Comparison with Map

| Feature | Pipe | Map |
|---------|------|-----|
| Purpose | Compose two runnables | Transform output within one runnable |
| Input | Another runnable | A mapper function |
| Type signature | `first.Pipe(second)` | `runnable.Map(mapper)` |
| Use case | Multi-step workflows | Simple transformations |

```csharp
// Map: Simple transformation
var doubled = add.Map(sum => sum * 2);

// Pipe: Composition with another runnable
var doubled = add.Pipe(RunnableLambda.Create<int, int>(x => x * 2));
```

## Testing

Comprehensive test suite available in `examples/PipeTest/Program.cs` covering:
- ? All parameter counts (0-16)
- ? Synchronous and asynchronous execution
- ? Type safety
- ? Chaining multiple pipes
- ? Combining with other extensions
- ? Real-world scenarios

## Summary

**Pipe Methods Added**: 17 (one for each 0-16 parameter count)
**File**: `src/Runnable/Pipe.cs`
**Build Status**: ? Successful

The Pipe method provides **mathematical function composition** for all Runnable arities, enabling clean, type-safe, and composable data transformation pipelines.

?? **Full function composition support for 0-16 parameters is now available!**
