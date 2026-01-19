# ? Pipe Operator (`|`) - Why It Can't Be Implemented

## The Problem

You **cannot** implement the `|` (or any custom operator) for Runnable pipe composition in C# due to fundamental language limitations.

## Why It Doesn't Work

### Reason 1: Operators Cannot Have Generic Type Parameters

```csharp
// ? THIS DOESN'T COMPILE - operators can't be generic!
public static Runnable<T1, TNext> operator |<TNext>(
    Runnable<T1, TOutput> first,
    IRunnable<TOutput, TNext> second)
{
    return first.Pipe(second);
}

// Error: CS1519: Invalid token '<' in class, struct, or interface member declaration
```

C# requires operator overloads to have **fixed type signatures** - you cannot add generic type parameters to operators.

### Reason 2: Return Type Must Match Operand Types

```csharp
// Even without generics, this won't work:
public static object operator |(
    Runnable<T1, TOutput> first,
    IRunnable<TOutput, ???> second)  // ¡û What type goes here?
{
    return first.Pipe(second);
}
```

The operator needs to know `TNext` at compile time, but we can't specify it without generics.

### Reason 3: The `|` Operator is Already Defined

```csharp
//In C#, | is bitwise OR operator:
int result = 5 | 3;  // Binary: 101 | 011 = 111 (result: 7)

// Using | for pipes would conflict with this built-in behavior
```

## ? What We CAN Do

### Option 1: Use `.Pipe()` Method (RECOMMENDED)

```csharp
// This IS the C# way - clear, explicit, type-safe
var result = parseInt
    .Pipe(double_)
    .Pipe(toString)
    .Invoke("5");

// Reads naturally: parseInt, pipe to double, pipe to toString
```

**Advantages:**
- ? Type-safe with full IntelliSense
- ? Idiomatic C# code
- ? Works with method chaining
- ? No confusion with bitwise operators
- ? Already implemented and working!

### Option 2: Use LINQ-Style Query Syntax

```csharp
// We could add a Select method (like LINQ)
var result = from x in parseInt
             select x * 2 into doubled
             select doubled.ToString();

// But .Pipe() is clearer for composition!
```

### Option 3: Extension Method with Different Name

```csharp
// Add a >> method (still can't be an operator, but looks similar)
public static class PipeOperatorExtensions
{
    public static Runnable<T1, TNext> Then<T1, TOutput, TNext>(
        this Runnable<T1, TOutput> first,
        IRunnable<TOutput, TNext> second)
    {
        return first.Pipe(second);
    }
}

// Usage:
var result = parseInt
    .Then(double_)
    .Then(toString);

// But this is just renaming .Pipe() - no real benefit
```

## ?? Comparison with Other Languages

### F# (Has `|>` operator)
```fsharp
let result = 
    "5"
    |> parseInt
    |> double
    |> toString
```

F# can do this because:
- It has **real** pipe operators built into the language
- Function application works differently
- It's designed for functional composition

### Kotlin (Has extension functions)
```kotlin
infix fun <T, R> T.pipe(f: (T) -> R): R = f(this)

// Usage:
val result = "5" pipe parseInt pipe double pipe toString
```

Kotlin's `infix` functions look like operators but are still methods.

### C# (Method Chaining)
```csharp
// This is the C# way - embrace it!
var result = "5"
    .AsRunnable()
    .Pipe(parseInt)
    .Pipe(double_)
    .Pipe(toString)
    .Invoke();
```

## ?? Best Practice for C#

**Use `.Pipe()` - it's perfect for C#:**

```csharp
// ? Clear composition pipeline
var dataPipeline = loadData
    .Pipe(validateData)
    .Pipe(transformData)
    .Pipe(saveData)
    .WithRetry(3)
    .WithFallbackValue(defaultResult);

// ? Works beautifully with other extensions
var result = calculate
    .Pipe(double_)
    .Map(x => x + 10)
    .Tap(x => Console.WriteLine(x))
    .WithRetry(3);

// ? Reads like English:
// "calculate, pipe to doubler, map to add 10, tap to log, with retry 3 times"
```

## ?? Performance

**Good news**: `.Pipe()` has ZERO performance overhead compared to what a hypothetical operator would have:

```csharp
// These would compile to essentially the same IL:
var withOperator = a | b | c;  // (if it were possible)
var withMethod = a.Pipe(b).Pipe(c);  // ¡û Just as efficient!
```

Both are thin wrappers that delegate directly to the underlying functions.

## ?? Why `.Pipe()` is Actually Better

| Aspect | Operator `|` | Method `.Pipe()` |
|--------|--------------|------------------|
| **Clarity** | Ambiguous (bitwise or pipe?) | ? Crystal clear |
| **IntelliSense** | Limited | ? Full support |
| **Discoverability** | Hidden | ? Visible in method list |
| **Chainability** | Limited | ? Chains perfectly |
| **Type Safety** | Would need hacks | ? Fully type-safe |
| **C# Idioms** | Foreign to C# | ? Idiomatic |

## ?? Embrace The C# Way

C# is NOT F# or Haskell - and that's okay! C#'s method chaining is actually **more powerful** because:

```csharp
// You can mix and match different operations naturally:
var result = runnable
    .Pipe(step1)           // Composition
    .Map(x => x * 2)       // Transformation
    .Tap(Log)              // Side effect
    .Pipe(step2)           // More composition  
    .WithRetry(3)          // Resilience
    .WithTimeout(...)      // Timeout
    .WithFallbackValue(0); // Error handling

// Try doing THAT with just operators!
```

## ? Conclusion

- ? **Cannot** implement `|` operator (C# language limitation)
- ? **Can** use `.Pipe()` method (already implemented, works perfectly!)
- ? `.Pipe()` is actually **better** for C# than an operator would be
- ? Embrace idiomatic C# - method chaining is powerful and clear

**The `.Pipe()` method is not a compromise - it's the C# way, and it's excellent!** ??
