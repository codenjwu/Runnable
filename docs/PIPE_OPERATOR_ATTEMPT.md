# ? Pipe Operator (`|`) - Implementation Attempt Result

## Summary

**Result**: **CANNOT BE IMPLEMENTED** due to C# language limitations.

## What We Tried

We attempted to implement the `|` operator for Runnable composition:

```csharp
// Goal (DOESN'T WORK):
var result = parseInt | double_ | toString;
var output = result.Invoke("5");

// Instead, use .Pipe() method (WORKS PERFECTLY):
var result = parseInt.Pipe(double_).Pipe(toString);
var output = result.Invoke("5");
```

## Why It Failed

### C# Operator Constraint

**C# operators cannot have generic type parameters.**

```csharp
// ? This does NOT compile:
public static Runnable<T1, TNext> operator |<TNext>(  // ¡û Can't add <TNext> here!
    Runnable<T1, TOutput> first,
    IRunnable<TOutput, TNext> second)
{
    return first.Pipe(second);
}

// Error: CS1003: Syntax error, '<' unexpected
//        CS1519: Invalid token in class, struct, or interface member
```

The C# language specification explicitly states that operator overloads **cannot** introduce new generic type parameters. They can only use type parameters that are already defined on the class.

### Why This Is a Problem for Pipe

For pipe composition, we need to know the **output type** of the second runnable:

```
Runnable<T1, TOutput> | IRunnable<TOutput, TNext> ¡ú Runnable<T1, TNext>
                                              ¡ü                      ¡ü
                                         We need this type parameter here!
```

Without generics on the operator, there's no way to specify `TNext` at compile time.

## ? The Solution: Use `.Pipe()` Method

The `.Pipe()` method **already exists and works perfectly**:

```csharp
// ? This works - clear, type-safe, and idiomatic C#
var pipeline = parseInt
    .Pipe(double_)
    .Pipe(toString)
    .Map(s => s.ToUpper())
    .Tap(s => Console.WriteLine(s))
    .WithRetry(3);

var result = pipeline.Invoke("5");
```

### Advantages of `.Pipe()` over a hypothetical `|` operator:

| Aspect | Operator `|` | Method `.Pipe()` |
|--------|--------------|------------------|
| **Works in C#** | ? No | ? Yes |
| **Type Safety** | ? Can't implement | ? Full type inference |
| **IntelliSense** | ? Limited | ? Complete support |
| **Clarity** | ? Ambiguous (`|` = bitwise OR?) | ? Crystal clear |
| **Chainability** | ? N/A | ? Perfect chaining |
| **C# Idioms** | ? Foreign | ? Idiomatic |

## Real-World Usage

The `.Pipe()` method provides everything you need for clean composition:

### Example 1: Data Processing Pipeline
```csharp
var dataPipeline = loadFromDatabase
    .Pipe(validateData)
    .Pipe(transformData)
    .Pipe(saveToCache)
    .WithRetry(3)
    .WithFallbackValue(defaultData);

var result = dataPipeline.Invoke(userId);
```

### Example 2: String Processing
```csharp
var process = trim
    .Pipe(toLowerCase)
    .Pipe(removeSpecialChars)
    .Pipe(validateFormat);

var cleaned = process.Invoke(userInput);
```

### Example 3: Mathematical Composition
```csharp
// f(x) = x + 5, g(x) = x * 2, h(x) = x?
var composed = addFive
    .Pipe(double_)
    .Pipe(square);

var result = composed.Invoke(3);  // (2*(3+5))? = 256
```

## Other Languages

### F# - Has `|>` Operator (Built Into Language)
```fsharp
let result = 
    5
    |> addFive
    |> double
    |> square
```

F# can do this because:
- The `|>` operator is **built into the language**
- F# has different rules for operators
- It's designed for functional composition

### C# - Use Method Chaining (The C# Way)
```csharp
var result = RunnableLambda.Create(() => 5)
    .Pipe(addFive)
    .Pipe(double_)
    .Pipe(square)
    .Invoke();
```

C#'s approach is actually **more powerful** because you can mix different operations:

```csharp
var result = source
    .Pipe(step1)           // Composition
    .Map(x => x * 2)       // Transformation
    .Tap(Log)              // Side effect
    .Pipe(step2)           // More composition
    .WithRetry(3)          // Resilience
    .WithFallbackValue(0); // Error handling
```

## Conclusion

- ? **Cannot** implement `|` operator (C# language limitation)
- ? **Can** use `.Pipe()` method (already implemented!)
- ? `.Pipe()` is actually **better** than an operator would be
- ? This is the **idiomatic C# way**

**The `.Pipe()` method is not a workaround - it's the proper C# solution!**

## Documentation

For more details, see:
- `docs/PIPE_OPERATOR_LIMITATION.md` - Full explanation of the limitation
- `docs/PIPE_COMPOSITION.md` - Complete `.Pipe()` usage guide
- `examples/PipeTest/Program.cs` - Working examples

## Bottom Line

**Use `.Pipe()` - it's perfect for C#!** ??

```csharp
// This is beautiful C# code:
var result = parseInt
    .Pipe(validate)
    .Pipe(transform)
    .Pipe(format)
    .Invoke(input);
```

No operator needed - method chaining is the C# way, and it's excellent!
