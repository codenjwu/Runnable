# RunnableBranch Implementation Summary

## ? What Was Implemented

A complete **RunnableBranch** feature inspired by LangChain's RunnableBranch, providing conditional routing for the Runnable library.

## ?? Files Created

1. **src/Runnable/RunnableBranch.cs** - Core implementation
2. **examples/RunnableBranchTest/Program.cs** - Comprehensive tests (10 scenarios)
3. **examples/RunnableBranchTest/RunnableBranchTest.csproj** - Test project
4. **docs/RUNNABLE_BRANCH.md** - Complete documentation

## ?? Features

### Core Functionality

? **Conditional Routing** - Route execution based on predicates  
? **First-Match-Wins** - Evaluates conditions in order, executes first match  
? **Default Fallback** - Always has a default branch  
? **0-8 Parameters** - Support for all common parameter arities  
? **Async Support** - Full async/await compatibility  
? **Composable** - Works with all Runnable extensions  

### API Design

```csharp
// Basic pattern
var branch = RunnableBranch.Create<TInput, TOutput>(
    defaultBranch,                      // Fallback when no condition matches
    (condition1, runnable1),            // First condition to check
    (condition2, runnable2),            // Second condition to check
    // ... more conditions
);

// Usage
var result = branch.Invoke(input);
```

## ?? Test Coverage

**10 comprehensive test scenarios:**

1. ? Basic branching (1 parameter)
2. ? Classification with multiple conditions
3. ? User role-based routing
4. ? Two parameter branching
5. ? Email validation branching
6. ? Async branching
7. ? Grade calculation
8. ? Three parameter discount calculator
9. ? Combining with other extensions
10. ? Real-world HTTP status code routing

## ?? Real-World Examples

### Example 1: User Role Routing

```csharp
var roleRouter = RunnableBranch.Create<string, string>(
    guestHandler,                                        // Default
    (user => user.StartsWith("admin_"), adminHandler),   // Admin users
    (user => user.StartsWith("mod_"), moderatorHandler), // Moderators
    (user => user.StartsWith("user_"), userHandler)      // Regular users
);

roleRouter.Invoke("admin_alice");  // ¡ú "Admin access granted for admin_alice"
```

### Example 2: HTTP Status Code Routing

```csharp
var httpRouter = RunnableBranch.Create<int, string, string>(
    handleUnknown,                                       // Default
    ((c, m) => c >= 200 && c < 300, handleSuccess),     // 2xx
    ((c, m) => c >= 300 && c < 400, handleRedirect),    // 3xx
    ((c, m) => c >= 400 && c < 500, handleClientError), // 4xx
    ((c, m) => c >= 500 && c < 600, handleServerError)  // 5xx
);

httpRouter.Invoke(404, "Not Found");  // ¡ú "? Client Error (404): Not Found"
```

### Example 3: Discount Calculator (3 Parameters)

```csharp
var discountBranch = RunnableBranch.Create<decimal, int, string, string>(
    noDiscount,                                      // Default
    ((p, q, t) => t == "VIP", vipDiscount),         // 20% off for VIP
    ((p, q, t) => t == "Member", memberDiscount),   // 10% off for members
    ((p, q, t) => q >= 10, bulkDiscount)            // 5% off for bulk
);

discountBranch.Invoke(100m, 5, "VIP");  // ¡ú "VIP: $400.00 (20% off)"
```

## ?? How It Works

```
©°©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©´
©¦   Input     ©¦
©¸©¤©¤©¤©¤©¤©¤©Ð©¤©¤©¤©¤©¤©¤©¼
       ©¦
       ¨‹
©°©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©´
©¦ Condition 1?     ©¦©¤©¤©¤Yes©¤©¤? Execute Runnable 1 ©¤©¤©´
©¸©¤©¤©¤©¤©¤©¤©¤©¤©Ð©¤©¤©¤©¤©¤©¤©¤©¤©¤©¼                                ©¦
         ©¦ No                                       ©¦
         ¨‹                                          ©¦
©°©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©´                                ©¦
©¦ Condition 2?     ©¦©¤©¤©¤Yes©¤©¤? Execute Runnable 2 ©¤©¤©È
©¸©¤©¤©¤©¤©¤©¤©¤©¤©Ð©¤©¤©¤©¤©¤©¤©¤©¤©¤©¼                                ©¦
         ©¦ No                                       ¨‹
         ¨‹                                    ©°©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©´
©°©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©´                         ©¦  Output  ©¦
©¦ Default Branch   ©¦©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤?©¸©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¼
©¸©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¤©¼
```

## ?? Key Design Decisions

### 1. First-Match-Wins Evaluation

Conditions are evaluated **in order** until the first match:

```csharp
// ? Order matters! Most specific first
RunnableBranch.Create<int, string>(
    defaultBranch,
    (x => x > 100, veryLargeHandler),  // Check most specific first
    (x => x > 10, largeHandler),       // Then less specific
    (x => x > 0, positiveHandler)      // Then general
);
```

### 2. Mandatory Default Branch

Always requires a default to ensure all cases are handled:

```csharp
RunnableBranch.Create<int, string>(
    defaultBranch,  // ¡û REQUIRED: Fallback when no condition matches
    (condition1, handler1),
    (condition2, handler2)
);
```

### 3. Parameter Arity Support

Supports 0-8 parameters (most common use cases):

- 0 params: Global state branching
- 1 param: Most common (item routing)
- 2-3 params: Common business logic
- 4-8 params: Complex scenarios

### 4. Async Throughout

Both sync and async paths fully supported:

```csharp
// Sync
var result = branch.Invoke(input);

// Async
var result = await branch.InvokeAsync(input);
```

## ?? Integration with Runnable Ecosystem

Works seamlessly with all other Runnable features:

```csharp
var pipeline = RunnableBranch.Create<int, string>(...)
    .Map(result => result.ToUpper())        // Transform
    .Tap(result => Log(result))             // Side effect
    .Pipe(nextRunnable)                     // Compose
    .WithRetry(3)                           // Resilience
    .WithFallbackValue("Error occurred");   // Error handling
```

## ?? Performance

- **Minimal overhead**: Just a foreach loop checking conditions
- **Early exit**: Stops at first match
- **No reflection**: Direct method calls
- **Async-friendly**: Proper async/await, no blocking

## ?? Comparison with Alternatives

| Feature | RunnableBranch | Switch Expression | If-Else Chain |
|---------|----------------|-------------------|---------------|
| **Declarative** | ? | ? | ? |
| **Reusable** | ? | ? | ? |
| **Composable** | ? | ? | ? |
| **Async** | ? | ?? | ? |
| **Complex Logic** | ? | ? | ? |
| **Type Safe** | ? | ? | ? |

## ?? Documentation

Complete documentation includes:

- **API Reference** - All method signatures
- **Usage Examples** - 10+ real-world scenarios
- **Best Practices** - How to order conditions, name variables, etc.
- **Performance Tips** - Optimization guidelines
- **Common Use Cases** - When to use RunnableBranch

See: `docs/RUNNABLE_BRANCH.md`

## ? Build Status

**BUILD: SUCCESSFUL**  
**TESTS: ALL PASSING**

```
=== RunnableBranch Tests Complete ===
? Conditional routing for 0-8 parameters
? First-match-wins evaluation order
? Async support
? Composable with other Runnable extensions
? Real-world routing scenarios demonstrated
```

## ?? Usage

```csharp
using Runnable;

// Create branches
var branch = RunnableBranch.Create<int, string>(
    defaultHandler,
    (x => x > 0, positiveHandler),
    (x => x < 0, negativeHandler)
);

// Use it
var result = branch.Invoke(42);
Console.WriteLine(result);  // Output from positiveHandler
```

## ?? Summary

**RunnableBranch** is a powerful addition to the Runnable library that:

- ? Brings LangChain-style conditional routing to C#
- ? Supports 0-8 parameters
- ? Has full async support
- ? Composes with all Runnable extensions
- ? Is type-safe and performant
- ? Includes comprehensive tests and documentation

**Perfect for**: User routing, status handling, validation pipelines, feature flags, A/B testing, and any scenario requiring conditional execution paths!

?? **RunnableBranch is ready for production use!**
