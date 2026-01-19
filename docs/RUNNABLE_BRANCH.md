# RunnableBranch - Conditional Routing

## Overview

`RunnableBranch` provides **conditional routing** for Runnables, inspired by LangChain's RunnableBranch. It allows you to evaluate conditions in order and execute the first matching runnable, with a default fallback.

## Key Features

? **First-Match-Wins** - Evaluates conditions in order, executes first match  
? **Default Fallback** - Always has a default branch when no condition matches  
? **0-8 Parameters** - Supports all common parameter arities  
? **Async Support** - Full async/await compatibility  
? **Composable** - Works with all other Runnable extensions  
? **Type-Safe** - Compile-time type checking for all branches  

## Basic Usage

### Single Parameter Branching

```csharp
var processPositive = RunnableLambda.Create<int, string>(x => $"Positive: {x}");
var processNegative = RunnableLambda.Create<int, string>(x => $"Negative: {x}");
var processZero = RunnableLambda.Create<int, string>(x => "Zero");

var branch = RunnableBranch.Create<int, string>(
    processZero,                    // Default branch
    (x => x > 0, processPositive),  // Condition 1
    (x => x < 0, processNegative)   // Condition 2
);

Console.WriteLine(branch.Invoke(42));   // "Positive: 42"
Console.WriteLine(branch.Invoke(-7));   // "Negative: -7"
Console.WriteLine(branch.Invoke(0));    // "Zero"
```

### How It Works

1. **Evaluates conditions in order** (top to bottom)
2. **Executes first matching runnable**
3. **Falls back to default** if no condition matches

```
Input ¡ú Condition 1? ¡ú Yes ¡ú Execute Runnable 1 ¡ú Output
        ¡ý No
        Condition 2? ¡ú Yes ¡ú Execute Runnable 2 ¡ú Output
        ¡ý No
        Default Branch ¡ú Output
```

## API Reference

### For 0 Parameters

```csharp
RunnableBranch.Create<TOutput>(
    IRunnable<TOutput> defaultBranch,
    params (Func<bool> condition, IRunnable<TOutput> runnable)[] branches)
```

### For 1 Parameter

```csharp
RunnableBranch.Create<TInput, TOutput>(
    IRunnable<TInput, TOutput> defaultBranch,
    params (Func<TInput, bool> condition, IRunnable<TInput, TOutput> runnable)[] branches)
```

### For 2 Parameters

```csharp
RunnableBranch.Create<T1, T2, TOutput>(
    IRunnable<T1, T2, TOutput> defaultBranch,
    params (Func<T1, T2, bool> condition, IRunnable<T1, T2, TOutput> runnable)[] branches)
```

### For 3+ Parameters

Pattern continues up to 8 parameters. See source code for full API.

## Examples

### Example 1: Classification

```csharp
var tiny = RunnableLambda.Create<int, string>(x => $"{x} is tiny");
var small = RunnableLambda.Create<int, string>(x => $"{x} is small");
var medium = RunnableLambda.Create<int, string>(x => $"{x} is medium");
var large = RunnableLambda.Create<int, string>(x => $"{x} is large");

var classifier = RunnableBranch.Create<int, string>(
    large,                              // Default: large
    (x => x >= 0 && x <= 10, tiny),     // 0-10: tiny
    (x => x >= 11 && x <= 50, small),   // 11-50: small
    (x => x >= 51 && x <= 100, medium)  // 51-100: medium
);

Console.WriteLine(classifier.Invoke(5));    // "5 is tiny"
Console.WriteLine(classifier.Invoke(150));  // "150 is large" (default)
```

### Example 2: User Role Routing

```csharp
var adminHandler = RunnableLambda.Create<string, string>(
    user => $"Admin access granted for {user}");
var moderatorHandler = RunnableLambda.Create<string, string>(
    user => $"Moderator access granted for {user}");
var userHandler = RunnableLambda.Create<string, string>(
    user => $"User access granted for {user}");
var guestHandler = RunnableLambda.Create<string, string>(
    user => $"Guest access for {user}");

var roleRouter = RunnableBranch.Create<string, string>(
    guestHandler,                                        // Default: guest
    (user => user.StartsWith("admin_"), adminHandler),   // admin_*
    (user => user.StartsWith("mod_"), moderatorHandler), // mod_*
    (user => user.StartsWith("user_"), userHandler)      // user_*
);

Console.WriteLine(roleRouter.Invoke("admin_alice"));  // Admin access
Console.WriteLine(roleRouter.Invoke("guest123"));     // Guest access
```

### Example 3: Multi-Parameter (Discount Calculator)

```csharp
var vipDiscount = RunnableLambda.Create<decimal, int, string, string>(
    (price, qty, type) => {
        var total = price * qty * 0.8m; // 20% off
        return $"VIP: ${total:F2}";
    });

var memberDiscount = RunnableLambda.Create<decimal, int, string, string>(
    (price, qty, type) => {
        var total = price * qty * 0.9m; // 10% off
        return $"Member: ${total:F2}";
    });

var bulkDiscount = RunnableLambda.Create<decimal, int, string, string>(
    (price, qty, type) => {
        var total = price * qty * 0.95m; // 5% off
        return $"Bulk: ${total:F2}";
    });

var noDiscount = RunnableLambda.Create<decimal, int, string, string>(
    (price, qty, type) => {
        var total = price * qty;
        return $"Regular: ${total:F2}";
    });

var discountBranch = RunnableBranch.Create<decimal, int, string, string>(
    noDiscount,                                      // Default
    ((p, q, t) => t == "VIP", vipDiscount),         // VIP customers
    ((p, q, t) => t == "Member", memberDiscount),   // Members
    ((p, q, t) => q >= 10, bulkDiscount)            // Bulk orders
);

Console.WriteLine(discountBranch.Invoke(100m, 5, "VIP"));    // "VIP: $400.00"
Console.WriteLine(discountBranch.Invoke(100m, 15, "Guest")); // "Bulk: $1425.00"
```

### Example 4: HTTP Status Code Router

```csharp
var handleSuccess = RunnableLambda.Create<int, string, string>(
    (code, msg) => $"? Success ({code}): {msg}");

var handleRedirect = RunnableLambda.Create<int, string, string>(
    (code, msg) => $"¡ú Redirect ({code}): {msg}");

var handleClientError = RunnableLambda.Create<int, string, string>(
    (code, msg) => $"? Client Error ({code}): {msg}");

var handleServerError = RunnableLambda.Create<int, string, string>(
    (code, msg) => $"? Server Error ({code}): {msg}");

var handleUnknown = RunnableLambda.Create<int, string, string>(
    (code, msg) => $"? Unknown ({code}): {msg}");

var httpRouter = RunnableBranch.Create<int, string, string>(
    handleUnknown,                                          // Default
    ((c, m) => c >= 200 && c < 300, handleSuccess),        // 2xx
    ((c, m) => c >= 300 && c < 400, handleRedirect),       // 3xx
    ((c, m) => c >= 400 && c < 500, handleClientError),    // 4xx
    ((c, m) => c >= 500 && c < 600, handleServerError)     // 5xx
);

Console.WriteLine(httpRouter.Invoke(200, "OK"));              // "? Success (200): OK"
Console.WriteLine(httpRouter.Invoke(404, "Not Found"));      // "? Client Error (404): Not Found"
Console.WriteLine(httpRouter.Invoke(500, "Internal Error")); // "? Server Error (500): Internal Error"
```

### Example 5: Email Validation

```csharp
var validEmail = RunnableLambda.Create<string, string>(
    email => $"? Valid: {email.ToLower()}");
var invalidFormat = RunnableLambda.Create<string, string>(
    email => $"? Invalid format: {email}");
var emptyEmail = RunnableLambda.Create<string, string>(
    email => "? Email required");
var suspiciousEmail = RunnableLambda.Create<string, string>(
    email => $"? Suspicious: {email}");

var emailBranch = RunnableBranch.Create<string, string>(
    invalidFormat,                                              // Default
    (e => string.IsNullOrWhiteSpace(e), emptyEmail),           // Empty check
    (e => e.Contains("spam"), suspiciousEmail),                // Spam check
    (e => e.Contains("@") && e.Contains("."), validEmail)      // Format check
);

Console.WriteLine(emailBranch.Invoke("alice@example.com"));  // "? Valid: alice@example.com"
Console.WriteLine(emailBranch.Invoke("spam@test.com"));      // "? Suspicious: spam@test.com"
Console.WriteLine(emailBranch.Invoke(""));                   // "? Email required"
Console.WriteLine(emailBranch.Invoke("invalid"));            // "? Invalid format: invalid"
```

## Async Support

All branches support async execution:

```csharp
var asyncFast = RunnableLambda.Create<int, string>(
    id => $"Fast: {id}",
    async id => {
        await Task.Delay(10);
        return $"Async Fast: {id}";
    });

var asyncSlow = RunnableLambda.Create<int, string>(
    id => $"Slow: {id}",
    async id => {
        await Task.Delay(50);
        return $"Async Slow: {id}";
    });

var asyncDefault = RunnableLambda.Create<int, string>(
    id => $"Default: {id}",
    async id => await Task.FromResult($"Async Default: {id}"));

var asyncBranch = RunnableBranch.Create<int, string>(
    asyncDefault,
    (id => id < 10, asyncFast),
    (id => id >= 10, asyncSlow)
);

var result = await asyncBranch.InvokeAsync(5);  // "Async Fast: 5"
```

## Composing with Other Extensions

Branch plays nicely with all Runnable extensions:

```csharp
var branch = RunnableBranch.Create<int, string>(
        defaultBranch,
        (x => x > 0, positiveBranch),
        (x => x < 0, negativeBranch)
    )
    .Map(result => result.ToUpper())           // Transform output
    .Tap(result => Log(result))                // Side effect
    .WithRetry(3)                              // Add retry
    .WithFallbackValue("Error occurred");      // Add fallback

var result = branch.Invoke(42);
```

## Order Matters!

Conditions are evaluated **in order**, and the **first match wins**:

```csharp
// ? BAD: Later condition will never match
var badBranch = RunnableBranch.Create<int, string>(
    defaultBranch,
    (x => x > 0, allPositive),      // This matches ALL positive
    (x => x > 10, largePositive)    // This will NEVER execute!
);

// ? GOOD: Most specific conditions first
var goodBranch = RunnableBranch.Create<int, string>(
    defaultBranch,
    (x => x > 10, largePositive),   // Check large first
    (x => x > 0, allPositive)       // Then general positive
);
```

## Best Practices

### 1. Order Conditions from Most to Least Specific

```csharp
// ? Good: Specific ¡ú General
RunnableBranch.Create<int, string>(
    defaultBranch,
    (x => x == 0, zeroHandler),           // Most specific
    (x => x > 100, largeHandler),         // Very specific
    (x => x > 10, mediumHandler),         // Less specific
    (x => x > 0, smallHandler)            // Least specific
);
```

### 2. Always Provide a Meaningful Default

```csharp
// ? Good: Explicit default with clear meaning
var defaultHandler = RunnableLambda.Create<int, string>(
    x => $"Unhandled value: {x}");

// ? Bad: Silent default
var badDefault = RunnableLambda.Create<int, string>(x => "");
```

### 3. Use Descriptive Variable Names

```csharp
// ? Good: Clear intent
var handleAdminUser = RunnableLambda.Create<string, string>(...);
var handleGuestUser = RunnableLambda.Create<string, string>(...);

// ? Bad: Unclear
var handler1 = RunnableLambda.Create<string, string>(...);
var handler2 = RunnableLambda.Create<string, string>(...);
```

### 4. Keep Conditions Simple

```csharp
// ? Good: Simple, readable conditions
(user => user.IsAdmin, adminHandler)
(user => user.IsPremium, premiumHandler)

// ? Bad: Complex inline logic
(user => user.Permissions.Any(p => p.Level > 5 && p.Active) && 
         user.CreatedDate > DateTime.Now.AddYears(-1), complexHandler)

// ? Better: Extract complex logic
bool IsEligibleForFeature(User user) => 
    user.Permissions.Any(p => p.Level > 5 && p.Active) && 
    user.CreatedDate > DateTime.Now.AddYears(-1);

(user => IsEligibleForFeature(user), featureHandler)
```

## Comparison with Other Patterns

### vs Switch Expressions

```csharp
// C# Switch Expression
string result = value switch {
    > 100 => "Large",
    > 50 => "Medium",
    > 0 => "Small",
    _ => "Other"
};

// RunnableBranch (more flexible)
var branch = RunnableBranch.Create<int, string>(
    otherHandler,
    (x => x > 100, largeHandler),   // Can be complex Runnable
    (x => x > 50, mediumHandler),   // Can have async logic
    (x => x > 0, smallHandler)      // Can compose with other Runnables
);
```

**Advantages of RunnableBranch:**
- ? Each branch can be a complex Runnable with its own logic
- ? Full async/await support
- ? Can compose with other Runnable extensions
- ? Branches can be reused across multiple contexts

### vs If-Else Chains

```csharp
// Traditional if-else
string Process(int value) {
    if (value > 100)
        return largeHandler.Invoke(value);
    else if (value > 50)
        return mediumHandler.Invoke(value);
    else if (value > 0)
        return smallHandler.Invoke(value);
    else
        return defaultHandler.Invoke(value);
}

// RunnableBranch (declarative)
var branch = RunnableBranch.Create<int, string>(
    defaultHandler,
    (x => x > 100, largeHandler),
    (x => x > 50, mediumHandler),
    (x => x > 0, smallHandler)
);
```

**Advantages:**
- ? More declarative and composable
- ? Can be stored and reused
- ? Works with Runnable ecosystem
- ? Cleaner syntax for complex routing

## Performance Considerations

- **Condition Evaluation**: Conditions are evaluated sequentially until a match
- **Early Exit**: Stops at first match (no unnecessary evaluations)
- **Overhead**: Minimal - just a foreach loop checking conditions
- **Async**: No blocking - proper async/await throughout

## Common Use Cases

1. **User Role Routing** - Route based on user permissions
2. **HTTP Status Handling** - Different handlers for different status codes
3. **Validation Pipelines** - Different validation paths
4. **A/B Testing** - Route to different implementations
5. **Feature Flags** - Enable/disable features based on conditions
6. **Multi-Tenant Routing** - Different logic per tenant
7. **Error Handling** - Different recovery strategies
8. **Data Classification** - Route data to appropriate processors

## Complete Example

```csharp
// Define handlers for different user types
var adminFlow = RunnableLambda.Create<string, string>(
    user => $"Admin dashboard for {user}");

var premiumFlow = RunnableLambda.Create<string, string>(
    user => $"Premium features for {user}");

var basicFlow = RunnableLambda.Create<string, string>(
    user => $"Basic features for {user}");

var suspendedFlow = RunnableLambda.Create<string, string>(
    user => $"Account suspended: {user}");

// Create routing logic
var userRouter = RunnableBranch.Create<string, string>(
    basicFlow,                                      // Default: basic user
    (u => u.Contains("_SUSPENDED"), suspendedFlow), // Check suspension first
    (u => u.StartsWith("admin_"), adminFlow),       // Then admin
    (u => u.StartsWith("premium_"), premiumFlow)    // Then premium
);

// Add logging and error handling
var completeFlow = userRouter
    .Tap(result => Console.WriteLine($"Routing: {result}"))
    .WithRetry(2)
    .WithFallbackValue("System error - please try again");

// Use it
Console.WriteLine(completeFlow.Invoke("admin_alice"));      // Admin dashboard
Console.WriteLine(completeFlow.Invoke("premium_bob"));      // Premium features
Console.WriteLine(completeFlow.Invoke("user_charlie"));     // Basic features
Console.WriteLine(completeFlow.Invoke("user_SUSPENDED"));   // Account suspended
```

## Summary

**RunnableBranch** brings powerful conditional routing to the Runnable ecosystem:

- ? **Declarative** routing logic
- ? **First-match-wins** evaluation
- ? **Type-safe** conditions and handlers
- ? **Async support** throughout
- ? **Composable** with all Runnable extensions
- ? **Clean API** for complex routing scenarios

**Perfect for**: User routing, status handling, validation pipelines, feature flags, and any scenario where you need conditional execution paths with complex logic.

?? **Get started with `RunnableBranch.Create()` and simplify your conditional logic!**
