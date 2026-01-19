using Runnable;
using System;
using System.Threading.Tasks;

// .NET 8 Example - Comprehensive Runnable Library Showcase
Console.WriteLine("=== Runnable Library - .NET 8 Example ===\n");

// ==================== 1. Basic Runnable Creation ====================
Console.WriteLine("--- 1. Basic Runnable Creation ---");

var getRandomNumber = RunnableLambda.Create(() => Random.Shared.Next(1, 100));
Console.WriteLine($"Random number: {getRandomNumber.Invoke()}");

var square = RunnableLambda.Create<int, int>(x => x * x);
Console.WriteLine($"Square of 5: {square.Invoke(5)}");

var add = RunnableLambda.Create<int, int, int>((a, b) => a + b);
Console.WriteLine($"Add 10 + 20: {add.Invoke(10, 20)}");

var calculateVolume = RunnableLambda.Create<double, double, double, double>(
    (length, width, height) => length * width * height);
Console.WriteLine($"Volume (2x3x4): {calculateVolume.Invoke(2, 3, 4)}");

// ==================== 2. Pipe Composition ====================
Console.WriteLine("\n--- 2. Pipe Composition ---");

var parseInt = RunnableLambda.Create<string, int>(s => int.Parse(s));
var double_ = RunnableLambda.Create<int, int>(x => x * 2);
var toString = RunnableLambda.Create<int, string>(x => $"Result: {x}");

var pipeline = parseInt.Pipe(double_).Pipe(toString);
Console.WriteLine(pipeline.Invoke("42")); // "Result: 84"

// ==================== 3. Map and Tap ====================
Console.WriteLine("\n--- 3. Map and Tap (Transformations & Side Effects) ---");

var calculate = RunnableLambda.Create<int, int, int>((a, b) => a + b);
var enhanced = calculate
    .Map(sum => sum * 10)
    .Tap(result => Console.WriteLine($"  Intermediate result: {result}"))
    .Map(x => $"Final: {x}");

Console.WriteLine(enhanced.Invoke(5, 7)); // "Final: 120"

// ==================== 4. Error Handling ====================
Console.WriteLine("\n--- 4. Error Handling (WithFallback & WithRetry) ---");

var riskyDivision = RunnableLambda.Create<int, int, double>((a, b) => {
    if (b == 0) throw new DivideByZeroException("Cannot divide by zero");
    return (double)a / b;
});

var safeDivision = riskyDivision
    .WithFallbackValue(-1.0)
    .Tap(result => Console.WriteLine($"  Division result: {result}"));

Console.WriteLine($"10 / 2 = {safeDivision.Invoke(10, 2)}");
Console.WriteLine($"10 / 0 = {safeDivision.Invoke(10, 0)} (fallback)");

// WithRetry
int attemptCount = 0;
var unreliable = RunnableLambda.Create<int, int>(x => {
    attemptCount++;
    if (attemptCount < 3) throw new Exception("Temporary failure");
    return x * 2;
});

var reliable = unreliable.WithRetry(maxAttempts: 5);
attemptCount = 0;
Console.WriteLine($"With retry: {reliable.Invoke(21)} (took {attemptCount} attempts)");

// ==================== 5. Async Support ====================
Console.WriteLine("\n--- 5. Async Support ---");

var asyncFetch = RunnableLambda.Create<int, string>(
    id => $"Data{id}",
    async id => {
        await Task.Delay(50);
        return $"AsyncData{id}";
    });

var asyncPipeline = asyncFetch
    .Pipe(RunnableLambda.Create<string, string>(s => s.ToUpper()));

var asyncResult = await asyncPipeline.InvokeAsync(42);
Console.WriteLine($"Async result: {asyncResult}");

// ==================== 6. Batch Processing ====================
Console.WriteLine("\n--- 6. Batch Processing ---");

var multiply = RunnableLambda.Create<int, int, int>((a, b) => a * b);
var inputs = new[] { (2, 3), (4, 5), (6, 7) };
var results = multiply.Batch(inputs);

Console.WriteLine("Batch results: " + string.Join(", ", results));

// ==================== 7. AsRunnable Extension ====================
Console.WriteLine("\n--- 7. AsRunnable Extension ---");

Func<string, string, string> concat = (a, b) => a + b;
var runnableConcat = concat.AsRunnable()
    .Map(result => result.ToUpper())
    .Tap(s => Console.WriteLine($"  Concatenated: {s}"));

runnableConcat.Invoke("Hello", " World");

// ==================== 8. Real-World Example: User Validation ====================
Console.WriteLine("\n--- 8. Real-World Example: User Validation Pipeline ---");

var parseUser = RunnableLambda.Create<string, string, string, (string Name, string Email, int Age)>(
    (name, email, ageStr) => (
        Name: name.Trim(),
        Email: email.Trim().ToLower(),
        Age: int.Parse(ageStr)
    ));

var validateUser = RunnableLambda.Create<(string Name, string Email, int Age), (string Name, string Email, int Age)>(
    user => {
        if (string.IsNullOrEmpty(user.Name)) throw new ArgumentException("Name required");
        if (!user.Email.Contains("@")) throw new ArgumentException("Invalid email");
        if (user.Age < 18) throw new ArgumentException("Must be 18+");
        return user;
    });

var formatUser = RunnableLambda.Create<(string Name, string Email, int Age), string>(
    user => $"? User: {user.Name} ({user.Email}), Age: {user.Age}");

var userPipeline = parseUser
    .Pipe(validateUser)
    .Pipe(formatUser)
    .WithRetry(2)
    .WithFallbackValue("? Invalid user data");

var validUser = userPipeline.Invoke("John Doe", "john@example.com", "25");
Console.WriteLine(validUser);

var invalidUser = userPipeline.Invoke("", "invalid", "15");
Console.WriteLine(invalidUser);

// ==================== 9. High Arity (8 parameters) ====================
Console.WriteLine("\n--- 9. High Arity Example (8 parameters) ---");

var sumMany = RunnableLambda.Create<int, int, int, int, int, int, int, int, int>(
    (a, b, c, d, e, f, g, h) => a + b + c + d + e + f + g + h);

var sumWithExtensions = sumMany
    .Map(sum => sum * 2)
    .Tap(result => Console.WriteLine($"  Sum ¡Á 2: {result}"));

sumWithExtensions.Invoke(1, 2, 3, 4, 5, 6, 7, 8);

// ==================== 10. Chaining Multiple Operations ====================
Console.WriteLine("\n--- 10. Chaining Multiple Operations ---");

var complexChain = RunnableLambda.Create<int, int, int, int>(
        (a, b, c) => a + b + c)
    .Map(sum => sum * 2)                    // Transform
    .Tap(x => Console.WriteLine($"  After double: {x}"))  // Log
    .Pipe(RunnableLambda.Create<int, string>(x => $"Result: {x}"))  // Compose
    .WithRetry(3)                           // Resilience
    .WithFallbackValue("Error occurred");  // Error handling

var chainResult = complexChain.Invoke(10, 20, 30);
Console.WriteLine(chainResult);

// ==================== 11. Stream Processing ====================
Console.WriteLine("\n--- 11. Stream Processing ---");

var numbers = new[] { 1, 2, 3, 4, 5 };
var squaredNumbers = numbers.Select(n => square.Invoke(n));
Console.WriteLine("Squared stream: " + string.Join(", ", squaredNumbers));

// ==================== Summary ====================
Console.WriteLine("\n=== .NET 8 Example Complete ===");
Console.WriteLine("? Demonstrated: Creation, Pipe, Map, Tap, Error Handling,");
Console.WriteLine("  Async, Batch, Extensions, and Real-World Scenarios");
Console.WriteLine("? Runnable library is fully functional on .NET 8!");
