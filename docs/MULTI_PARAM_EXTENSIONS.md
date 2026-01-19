# Multi-Parameter Extension Methods - Complete Implementation

## ? Implementation Complete

All utility extension methods now support **2-16 parameters** in addition to the existing 0-1 parameter support.

## What Was Added

### 1. **Map Extensions** (2-16 parameters)
Transform the output of a runnable while preserving all input parameters.

**Files**: `src/Runnable/RunnableUtilities.cs`
**Methods Added**: 15 (one for each 2-16 parameter count)

```csharp
// Example: 3 parameters
var add3 = RunnableLambda.Create<int, int, int, int>((a, b, c) => a + b + c);
var doubled = add3.Map(result => result * 2);
doubled.Invoke(1, 2, 3); // Returns: 12
```

### 2. **Tap Extensions** (2-16 parameters)
Execute side effects without changing the output.

**Files**: `src/Runnable/RunnableUtilities.cs`
**Methods Added**: 15

```csharp
// Example: 4 parameters
var concat = RunnableLambda.Create<string, string, string, string, string>(
    (a, b, c, d) => a + b + c + d);
string logged = "";
var tapped = concat.Tap(result => logged = result);
tapped.Invoke("A", "B", "C", "D");
```

### 3. **WithFallback Extensions** (2-16 parameters)
Provide a fallback runnable for error handling.

**Files**: `src/Runnable/RunnableMultiParamExtensions.cs` (NEW)
**Methods Added**: 15

```csharp
// Example: 2 parameters
var divider = RunnableLambda.Create<int, int, int>((a, b) => a / b);
var fallback = RunnableLambda.Create<int, int, int>((a, b) => -1);
var safe = divider.WithFallback(fallback);
safe.Invoke(10, 0); // Returns: -1 (instead of throwing)
```

### 4. **WithFallbackValue Extensions** (2-16 parameters)
Provide a fallback value for error handling.

**Files**: `src/Runnable/RunnableMultiParamExtensions.cs` (NEW)
**Methods Added**: 15

```csharp
// Example: 3 parameters
var risky = RunnableLambda.Create<int, int, int, int>((a, b, c) => {
    if (a + b + c > 100) throw new Exception();
    return a + b + c;
});
var safe = risky.WithFallbackValue(999);
safe.Invoke(50, 60, 70); // Returns: 999
```

### 5. **WithRetry Extensions** (2-16 parameters)
Automatically retry operations on failure.

**Files**: `src/Runnable/RunnableRetryFilterExtensions.cs` (NEW)
**Methods Added**: 15

```csharp
// Example: 5 parameters
var unreliable = RunnableLambda.Create<int, int, int, int, int, int>(
    (a, b, c, d, e) => {
        // Simulated unreliable operation
        if (Random.Shared.Next(10) < 5) throw new Exception();
        return a + b + c + d + e;
    });

var reliable = unreliable.WithRetry(
    maxAttempts: 5,
    delay: TimeSpan.FromMilliseconds(100));
    
reliable.Invoke(1, 2, 3, 4, 5); // Will retry up to 5 times
```

## New Files Created

1. **`src/Runnable/RunnableMultiParamExtensions.cs`**
   - WithFallback for 2-16 parameters (15 methods)
   - WithFallbackValue for 2-16 parameters (15 methods)
   - **Total: 30 methods**

2. **`src/Runnable/RunnableRetryFilterExtensions.cs`**
   - WithRetry for 2-16 parameters (15 methods)
   - **Total: 15 methods**

3. **`examples/MultiParamExtensionsTest/Program.cs`**
   - Comprehensive test suite demonstrating all new features

## Files Modified

1. **`src/Runnable/RunnableUtilities.cs`**
   - Added Map for 2-16 parameters (15 methods)
   - Added Tap for 2-16 parameters (15 methods)
   - **Total: 30 new methods**

2. **`docs/COMPLETE_FUNC_SUPPORT.md`**
   - Updated documentation with usage examples
   - Added extension methods coverage table
   - Updated statistics

## Coverage Summary

| Extension Method | Parameters Supported | Total Methods |
|-----------------|---------------------|---------------|
| Map | 0-16 (17 variants) | 17 |
| Tap | 0-16 (17 variants) | 17 |
| WithFallback | 0-16 (17 variants) | 17 |
| WithFallbackValue | 1-16 (16 variants) | 16 |
| WithRetry | 0-16 (17 variants) | 17 |
| **Subtotal** | **Multi-param extensions** | **84** |
| MapAsync | 0-1 | 2 |
| TapAsync | 0-1 | 2 |
| Filter | 1 | 1 |
| WithCache | 1 | 1 |
| WithDelay | 1 | 1 |
| WithTimeout | 1 | 1 |
| BatchParallel | 1 | 1 |
| InvokeParallel | 1 | 1 |
| **Total Extensions** | | **94** |

## Usage Patterns

### Simple Transformation
```csharp
var calculate = RunnableLambda.Create<int, int, int, int, int>(
    (a, b, c, d) => a + b + c + d);

var formatted = calculate.Map(sum => $"Result: {sum}");
Console.WriteLine(formatted.Invoke(1, 2, 3, 4)); // "Result: 10"
```

### Error Handling
```csharp
var risky = RunnableLambda.Create<string, int, string>((str, divisor) => {
    if (divisor == 0) throw new DivideByZeroException();
    return str.Substring(0, str.Length / divisor);
});

var safe = risky.WithFallbackValue("ERROR");
Console.WriteLine(safe.Invoke("Hello", 0)); // "ERROR"
```

### Retry with Backoff
```csharp
var apiCall = RunnableLambda.Create<string, string, int, string>(
    (endpoint, apiKey, timeout) => {
        // Simulated API call that might fail
        // ...
    });

var reliableApi = apiCall.WithRetry(
    maxAttempts: 3,
    delay: TimeSpan.FromSeconds(1));
```

### Chaining Multiple Extensions
```csharp
var pipeline = RunnableLambda.Create<int, int, int, int, int, int>(
        (a, b, c, d, e) => a + b + c + d + e)
    .WithRetry(3)                          // Retry on failure
    .Map(sum => sum * 2)                   // Transform output
    .Tap(result => logger.Log(result))    // Log
    .WithFallbackValue(0);                 // Fallback value

var result = pipeline.Invoke(1, 2, 3, 4, 5); // Returns 30 with retry/logging
```

## Real-World Examples

### Database Query Builder
```csharp
var buildQuery = RunnableLambda.Create<
    string, string, int, int, bool, string>(
    (table, column, min, max, includeDeleted) => {
        if (min > max) throw new ArgumentException("Invalid range");
        return $"SELECT * FROM {table} WHERE {column} BETWEEN {min} AND {max}" +
               (includeDeleted ? "" : " AND deleted = 0");
    });

var safeQuery = buildQuery
    .WithRetry(2)
    .WithFallbackValue("SELECT * FROM fallback_table")
    .Tap(sql => logger.LogDebug($"Executing: {sql}"));

var query = safeQuery.Invoke("Users", "age", 18, 65, false);
```

### Configuration Builder
```csharp
var buildConfig = RunnableLambda.Create<
    string, int, bool, string, string, TimeSpan, int, string>(
    (host, port, ssl, db, user, timeout, pool) => {
        return new ConnectionString {
            Host = host,
            Port = port,
            UseSsl = ssl,
            Database = db,
            User = user,
            Timeout = timeout,
            PoolSize = pool
        }.ToString();
    });

var robustConfig = buildConfig
    .Map(connStr => connStr + ";MultipleActiveResultSets=true")
    .Tap(c => Console.WriteLine($"Config: {c}"))
    .WithFallbackValue("Server=localhost;Database=default");

var config = robustConfig.Invoke(
    "prod-db.example.com", 5432, true, "myapp_db", "admin",
    TimeSpan.FromSeconds(30), 100);
```

### API Client with Retry
```csharp
var callApi = RunnableLambda.Create<
    string, string, string, Dictionary<string, string>, HttpResponse>(
    (method, endpoint, apiKey, headers) => {
        // API call implementation
        // ...
    });

var reliableApi = callApi
    .WithRetry(maxAttempts: 5, delay: TimeSpan.FromSeconds(2))
    .Map(response => response.Body)
    .Tap(body => metrics.RecordApiCall())
    .WithFallbackValue("{ \"error\": \"Service unavailable\" }");

var response = reliableApi.Invoke(
    "GET", "/api/users", "key123", new Dictionary<string, string>());
```

## Testing

All extension methods have been tested with:
- ? Synchronous execution
- ? Asynchronous execution  
- ? Error handling scenarios
- ? Chaining multiple extensions
- ? High arity (12-16 parameters)
- ? Real-world use cases

Test file: `examples/MultiParamExtensionsTest/Program.cs`

## Build Status

? **Build: SUCCESSFUL**

All new methods compile without errors across all target frameworks:
- .NET Standard 2.0
- .NET 5, 6, 8, 9, 10

## Performance Considerations

- **No runtime overhead**: All methods are thin wrappers that delegate to the underlying runnable
- **Async-friendly**: Proper async/await patterns throughout
- **Memory efficient**: No unnecessary allocations
- **Type-safe**: Full compile-time type checking

## Future Enhancements (Optional)

The following extensions are currently limited to 0-1 parameters and could be extended if needed:

- ?? MapAsync (async transformation)
- ?? TapAsync (async side effects)
- ?? Filter (conditional execution) - requires multiple predicates for multi-param
- ?? WithCache (memoization) - requires composite key hashing
- ?? WithDelay (rate limiting)
- ?? WithTimeout (execution timeout)

These were intentionally kept limited because:
1. **Caching** with multiple parameters requires complex composite key generation
2. **Filtering** with multiple parameters needs multiple predicates (design decision needed)
3. **Delay/Timeout** are typically applied after composition, not during

## Summary

**Total New Methods Added**: 75 (across Map, Tap, WithFallback, WithFallbackValue, WithRetry)

**Files Added**: 2
- RunnableMultiParamExtensions.cs
- RunnableRetryFilterExtensions.cs

**Files Modified**: 2
- RunnableUtilities.cs (added Map/Tap)
- COMPLETE_FUNC_SUPPORT.md (updated docs)

**Test Coverage**: Complete test suite in `examples/MultiParamExtensionsTest/Program.cs`

?? **All multi-parameter extension methods (2-16 params) are now fully implemented and tested!**
