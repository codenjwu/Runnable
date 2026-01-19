# RunnableMap - Parallel Execution & Aggregation

## Overview

`RunnableMap` enables **parallel execution** of multiple runnables on the same input, returning a **dictionary of named results**. Inspired by LangChain's RunnableMap, it's perfect for scenarios where you need to execute multiple operations simultaneously and aggregate their outputs.

## Key Features

? **Parallel Execution** - Runs multiple runnables concurrently (async mode)  
? **Named Results** - Returns `Dictionary<string, TOutput>` with named results  
? **0-8 Parameters** - Supports all common parameter arities  
? **Async with Task.WhenAll** - True parallel execution for async operations  
? **Type-Safe** - Compile-time type checking for all outputs  
? **Composable** - Works with all other Runnable extensions  

## Basic Usage

### Single Input, Multiple Processors

```csharp
var square = RunnableLambda.Create<int, int>(x => x * x);
var cube = RunnableLambda.Create<int, int>(x => x * x * x);
var double_ = RunnableLambda.Create<int, int>(x => x * 2);

var mathMap = RunnableMap.Create<int, int>(
    ("square", square),
    ("cube", cube),
    ("double", double_)
);

var results = mathMap.Invoke(5);
// results = {
//   "square": 25,
//   "cube": 125,
//   "double": 10
// }

Console.WriteLine(results["square"]);  // 25
Console.WriteLine(results["cube"]);    // 125
Console.WriteLine(results["double"]);  // 10
```

### How It Works

```
              ©°©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©´
Input (5) ©¤©¤©¤?©¦ RunnableMap ©¦
              ©¸©¤©¤©¤©¤©¤©¤©Ð©¤©¤©¤©¤©¤©¤©¼
                     ©¦
        ©°©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©à©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©´
        ©¦            ©¦            ©¦
        ¨‹            ¨‹            ¨‹
   ©°©¤©¤©¤©¤©¤©¤©¤©¤©´  ©°©¤©¤©¤©¤©¤©¤©¤©¤©´  ©°©¤©¤©¤©¤©¤©¤©¤©¤©´
   ©¦ Square ©¦  ©¦  Cube  ©¦  ©¦ Double ©¦
   ©¦ x ¡ú x? ©¦  ©¦x ¡ú x?  ©¦  ©¦ x ¡ú 2x ©¦
   ©¸©¤©¤©¤©¤©Ð©¤©¤©¤©¼  ©¸©¤©¤©¤©¤©Ð©¤©¤©¤©¼  ©¸©¤©¤©¤©¤©Ð©¤©¤©¤©¼
        ©¦            ©¦            ©¦
        ¨‹            ¨‹            ¨‹
       25          125           10
        ©¦            ©¦            ©¦
        ©¸©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©à©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¼
                     ©¦
                     ¨‹
         Dictionary<string, int>
         {
           "square": 25,
           "cube": 125,
           "double": 10
         }
```

## API Reference

### For 0 Parameters

```csharp
RunnableMap.Create<TOutput>(
    params (string key, IRunnable<TOutput> runnable)[] runnables)
```

### For 1 Parameter

```csharp
RunnableMap.Create<TInput, TOutput>(
    params (string key, IRunnable<TInput, TOutput> runnable)[] runnables)
```

### For 2 Parameters

```csharp
RunnableMap.Create<T1, T2, TOutput>(
    params (string key, IRunnable<T1, T2, TOutput> runnable)[] runnables)
```

### For 3+ Parameters

Pattern continues up to 8 parameters.

## Examples

### Example 1: Data Enrichment

```csharp
var calculateTax = RunnableLambda.Create<decimal, decimal>(
    amount => amount * 0.08m);

var calculateShipping = RunnableLambda.Create<decimal, decimal>(
    amount => amount > 100m ? 0m : 9.99m);

var calculateDiscount = RunnableLambda.Create<decimal, decimal>(
    amount => amount > 200m ? amount * 0.1m : 0m);

var priceMap = RunnableMap.Create<decimal, decimal>(
    ("tax", calculateTax),
    ("shipping", calculateShipping),
    ("discount", calculateDiscount)
);

var results = priceMap.Invoke(150m);
// results = {
//   "tax": 12.00,
//   "shipping": 0.00,
//   "discount": 0.00
// }

var total = 150m + results["tax"] + results["shipping"] - results["discount"];
Console.WriteLine($"Total: ${total:F2}");  // Total: $162.00
```

### Example 2: User Validation

```csharp
var validateEmail = RunnableLambda.Create<string, bool>(
    email => email.Contains("@") && email.Contains("."));

var validateLength = RunnableLambda.Create<string, bool>(
    email => email.Length >= 5 && email.Length <= 100);

var validateDomain = RunnableLambda.Create<string, bool>(
    email => email.EndsWith(".com") || email.EndsWith(".org"));

var validationMap = RunnableMap.Create<string, bool>(
    ("email_format", validateEmail),
    ("length_check", validateLength),
    ("domain_check", validateDomain)
);

var results = validationMap.Invoke("alice@example.com");
// results = {
//   "email_format": true,
//   "length_check": true,
//   "domain_check": true
// }

var isValid = results.Values.All(v => v);
Console.WriteLine($"Valid: {isValid}");  // Valid: true
```

### Example 3: String Processing

```csharp
var upper = RunnableLambda.Create<string, string>(s => s.ToUpper());
var lower = RunnableLambda.Create<string, string>(s => s.ToLower());
var reverse = RunnableLambda.Create<string, string>(
    s => new string(s.Reverse().ToArray()));
var length = RunnableLambda.Create<string, int>(s => s.Length);

var stringMap = RunnableMap.Create<string, string>(
    ("uppercase", upper),
    ("lowercase", lower),
    ("reversed", reverse)
);

var results = stringMap.Invoke("Hello");
// results = {
//   "uppercase": "HELLO",
//   "lowercase": "hello",
//   "reversed": "olleH"
// }
```

### Example 4: Async Parallel Execution

```csharp
var fetchUser = RunnableLambda.Create<string, string>(
    id => $"User-{id}",
    async id => {
        await Task.Delay(100);
        return $"{{name: 'User{id}', age: 30}}";
    });

var fetchPosts = RunnableLambda.Create<string, string>(
    id => $"Posts-{id}",
    async id => {
        await Task.Delay(75);
        return $"[Post1, Post2, Post3]";
    });

var fetchFollowers = RunnableLambda.Create<string, string>(
    id => $"Followers-{id}",
    async id => {
        await Task.Delay(50);
        return $"[User1, User2]";
    });

var apiMap = RunnableMap.Create<string, string>(
    ("user", fetchUser),
    ("posts", fetchPosts),
    ("followers", fetchFollowers)
);

var startTime = DateTime.Now;
var results = await apiMap.InvokeAsync("123");
var elapsed = DateTime.Now - startTime;

Console.WriteLine($"Execution time: {elapsed.TotalMilliseconds:F0}ms");
// Execution time: ~100ms (parallel, not 100+75+50=225ms sequential!)

foreach (var (key, value) in results)
{
    Console.WriteLine($"{key}: {value}");
}
```

### Example 5: Multi-Parameter (Order Processing)

```csharp
var calculateSubtotal = RunnableLambda.Create<decimal, int, decimal>(
    (price, qty) => price * qty);

var calculateBulkDiscount = RunnableLambda.Create<decimal, int, decimal>(
    (price, qty) => qty >= 10 ? price * qty * 0.1m : 0m);

var estimateShipping = RunnableLambda.Create<decimal, int, decimal>(
    (price, qty) => qty * 2.5m);

var orderMap = RunnableMap.Create<decimal, int, decimal>(
    ("subtotal", calculateSubtotal),
    ("discount", calculateBulkDiscount),
    ("shipping", estimateShipping)
);

var results = orderMap.Invoke(50m, 15);
// results = {
//   "subtotal": 750.00,
//   "discount": 75.00,
//   "shipping": 37.50
// }

var total = results["subtotal"] - results["discount"] + results["shipping"];
Console.WriteLine($"Total: ${total:F2}");  // Total: $712.50
```

### Example 6: Feature Extraction

```csharp
var wordCount = RunnableLambda.Create<string, int>(
    text => text.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length);

var charCount = RunnableLambda.Create<string, int>(text => text.Length);

var sentenceCount = RunnableLambda.Create<string, int>(
    text => text.Split('.', '!', '?', StringSplitOptions.RemoveEmptyEntries).Length);

var featureMap = RunnableMap.Create<string, int>(
    ("words", wordCount),
    ("characters", charCount),
    ("sentences", sentenceCount)
);

var text = "Hello world. This is a test!";
var features = featureMap.Invoke(text);
// features = {
//   "words": 6,
//   "characters": 29,
//   "sentences": 2
// }
```

## Real-World Scenarios

### Scenario 1: API Aggregation

Fetch data from multiple APIs in parallel:

```csharp
var fetchProfile = RunnableLambda.Create<string, string>(
    userId => /* fetch from API 1 */,
    async userId => await Api1.GetProfileAsync(userId));

var fetchOrders = RunnableLambda.Create<string, string>(
    userId => /* fetch from API 2 */,
    async userId => await Api2.GetOrdersAsync(userId));

var fetchPreferences = RunnableLambda.Create<string, string>(
    userId => /* fetch from API 3 */,
    async userId => await Api3.GetPreferencesAsync(userId));

var aggregator = RunnableMap.Create<string, string>(
    ("profile", fetchProfile),
    ("orders", fetchOrders),
    ("preferences", fetchPreferences)
);

// All 3 APIs called in parallel!
var userData = await aggregator.InvokeAsync(userId);
```

### Scenario 2: Multi-Model AI Response

Query multiple AI models simultaneously:

```csharp
var gpt = RunnableLambda.Create<string, string>(
    query => /* GPT call */,
    async query => await GptApi.GenerateAsync(query));

var claude = RunnableLambda.Create<string, string>(
    query => /* Claude call */,
    async query => await ClaudeApi.GenerateAsync(query));

var llama = RunnableLambda.Create<string, string>(
    query => /* Llama call */,
    async query => await LlamaApi.GenerateAsync(query));

var multiModel = RunnableMap.Create<string, string>(
    ("gpt", gpt),
    ("claude", claude),
    ("llama", llama)
);

var responses = await multiModel.InvokeAsync("Explain AI");
// Get responses from all 3 models simultaneously

// Compare or aggregate responses
var bestResponse = ChooseBest(responses.Values);
```

### Scenario 3: Data Validation Pipeline

```csharp
var checkFormat = RunnableLambda.Create<string, bool>(
    email => email.Contains("@"));

var checkDomain = RunnableLambda.Create<string, bool>(
    email => email.Split('@')[1].Contains("."));

var checkLength = RunnableLambda.Create<string, bool>(
    email => email.Length <= 100);

var validationPipeline = RunnableMap.Create<string, bool>(
    ("format", checkFormat),
    ("domain", checkDomain),
    ("length", checkLength)
)
.Map(results => new {
    IsValid = results.Values.All(v => v),
    Failures = results.Where(kv => !kv.Value).Select(kv => kv.Key).ToList()
});

var validation = validationPipeline.Invoke("user@example.com");
Console.WriteLine($"Valid: {validation.IsValid}");
```

### Scenario 4: Price Comparison

```csharp
var amazonPrice = RunnableLambda.Create<string, decimal>(
    productId => /* Amazon API */,
    async productId => await AmazonApi.GetPriceAsync(productId));

var ebayPrice = RunnableLambda.Create<string, decimal>(
    productId => /* eBay API */,
    async productId => await EbayApi.GetPriceAsync(productId));

var walmartPrice = RunnableLambda.Create<string, decimal>(
    productId => /* Walmart API */,
    async productId => await WalmartApi.GetPriceAsync(productId));

var priceComparison = RunnableMap.Create<string, decimal>(
    ("amazon", amazonPrice),
    ("ebay", ebayPrice),
    ("walmart", walmartPrice)
);

var prices = await priceComparison.InvokeAsync(productId);
var lowestPrice = prices.MinBy(p => p.Value);
Console.WriteLine($"Best deal: {lowestPrice.Key} at ${lowestPrice.Value}");
```

## Async Performance

**Sequential vs Parallel:**

```csharp
// Sequential (slow): 100ms + 75ms + 50ms = 225ms
var result1 = await runnable1.InvokeAsync(input);
var result2 = await runnable2.InvokeAsync(input);
var result3 = await runnable3.InvokeAsync(input);

// Parallel (fast): max(100ms, 75ms, 50ms) = 100ms
var results = await RunnableMap.Create<string, string>(
    ("r1", runnable1),
    ("r2", runnable2),
    ("r3", runnable3)
).InvokeAsync(input);
```

**Under the hood:**

```csharp
// Sync: Sequential execution
foreach (var (key, runnable) in runnables)
{
    results[key] = runnable.Invoke(input);
}

// Async: Parallel with Task.WhenAll
var tasks = runnables.Select(async r => {
    var result = await r.runnable.InvokeAsync(input);
    return (r.key, result);
});
var results = await Task.WhenAll(tasks);
```

## Composing with Other Extensions

Map plays nicely with all Runnable features:

```csharp
var enriched = RunnableMap.Create<int, int>(
        ("square", square),
        ("cube", cube)
    )
    .Map(dict => {
        // Transform dictionary to summary
        var sum = dict.Values.Sum();
        return $"Total: {sum}";
    })
    .Tap(summary => Log(summary))
    .WithRetry(3)
    .WithFallbackValue("Calculation failed");

var result = enriched.Invoke(5);
```

## Best Practices

### 1. Use Meaningful Keys

```csharp
// ? Good: Descriptive keys
RunnableMap.Create<string, bool>(
    ("email_format_valid", validateEmail),
    ("length_within_limits", validateLength)
);

// ? Bad: Generic keys
RunnableMap.Create<string, bool>(
    ("check1", validateEmail),
    ("check2", validateLength)
);
```

### 2. Keep Output Types Consistent

```csharp
// ? Good: All outputs same type
RunnableMap.Create<int, int>(
    ("square", square),
    ("cube", cube),
    ("double", double_)
);

// ?? If you need mixed types, use a common base or object
```

### 3. Async for I/O Operations

```csharp
// ? Good: Use async for true parallelism
var results = await RunnableMap.Create<string, Data>(...)
    .InvokeAsync(userId);

// ? Avoid: Sync blocks threads
var results = RunnableMap.Create<string, Data>(...)
    .Invoke(userId);
```

### 4. Handle Partial Failures

```csharp
// Wrap individual runnables with fallbacks
var safeApi1 = api1.WithFallbackValue(defaultData1);
var safeApi2 = api2.WithFallbackValue(defaultData2);

var map = RunnableMap.Create<string, Data>(
    ("api1", safeApi1),
    ("api2", safeApi2)
);
```

## Comparison with Alternatives

### vs Manual Dictionary Building

```csharp
// Manual (verbose)
var results = new Dictionary<string, int>();
results["square"] = square.Invoke(5);
results["cube"] = cube.Invoke(5);
results["double"] = double_.Invoke(5);

// RunnableMap (concise)
var results = RunnableMap.Create<int, int>(
    ("square", square),
    ("cube", cube),
    ("double", double_)
).Invoke(5);
```

### vs Task.WhenAll

```csharp
// Manual Task.WhenAll (more code)
var task1 = Task.Run(() => square.Invoke(5));
var task2 = Task.Run(() => cube.Invoke(5));
var results = await Task.WhenAll(task1, task2);
// Need to manually create dictionary

// RunnableMap (cleaner)
var results = await RunnableMap.Create<int, int>(
    ("square", square),
    ("cube", cube)
).InvokeAsync(5);
// Dictionary ready to use
```

## Performance Considerations

- **Sync mode**: Sequential execution, no parallelism
- **Async mode**: True parallel execution with `Task.WhenAll`
- **Overhead**: Minimal - just dictionary construction
- **Memory**: One dictionary allocation per invocation

## Common Use Cases

1. **API Aggregation** - Fetch from multiple sources in parallel
2. **Multi-Model AI** - Query multiple LLMs simultaneously
3. **Data Enrichment** - Calculate multiple derived values
4. **Validation** - Run multiple validation checks
5. **Feature Extraction** - Extract multiple features from data
6. **Price Comparison** - Check prices across platforms
7. **A/B Testing** - Run multiple variations
8. **Metrics Collection** - Gather multiple metrics

## Summary

**RunnableMap** provides powerful parallel execution and result aggregation:

- ? **Parallel execution** with async/await
- ? **Named results** in dictionaries
- ? **Type-safe** composition
- ? **Async optimization** with Task.WhenAll
- ? **Composable** with all Runnable extensions
- ? **Clean API** for complex scenarios

**Perfect for**: API aggregation, multi-model AI, data enrichment, parallel validation, and any scenario requiring simultaneous execution of multiple operations!

?? **Get started with `RunnableMap.Create()` and parallelize your workflows!**
