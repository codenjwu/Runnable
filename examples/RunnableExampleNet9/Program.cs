using Runnable;
using System;
using System.Linq;
using System.Threading.Tasks;

// .NET 9 Example - Advanced Runnable Library Features
Console.WriteLine("=== Runnable Library - .NET 9 Example ===\n");

// ==================== 1. Advanced Pipe Composition ====================
Console.WriteLine("--- 1. Advanced Pipe Composition ---");

var parseJson = RunnableLambda.Create<string, (int Id, string Name)>(json => {
    // Simplified JSON parsing
    var parts = json.Trim('[', ']').Split(',');
    return (Id: int.Parse(parts[0]), Name: parts[1].Trim('"'));
});

var enrichData = RunnableLambda.Create<(int Id, string Name), (int Id, string Name, string Status)>(
    data => (data.Id, data.Name, Status: data.Id > 100 ? "Premium" : "Standard"));

var formatOutput = RunnableLambda.Create<(int Id, string Name, string Status), string>(
    data => $"[{data.Id}] {data.Name} ({data.Status})");

var dataPipeline = parseJson
    .Pipe(enrichData)
    .Pipe(formatOutput)
    .WithFallbackValue("Invalid data");

Console.WriteLine(dataPipeline.Invoke("[101,\"Alice\"]"));
Console.WriteLine(dataPipeline.Invoke("[50,\"Bob\"]"));

// ==================== 2. Pattern Matching with Runnables ====================
Console.WriteLine("\n--- 2. Pattern Matching with Runnables ---");

var classifyNumber = RunnableLambda.Create<int, string>(n => n switch {
    < 0 => "Negative",
    0 => "Zero",
    > 0 and <= 10 => "Small positive",
    > 10 and <= 100 => "Medium positive",
    _ => "Large positive"
});

var numbers = new[] { -5, 0, 5, 50, 500 };
foreach (var num in numbers)
{
    Console.WriteLine($"{num,4} => {classifyNumber.Invoke(num)}");
}

// ==================== 3. Record Types with Runnables ====================
Console.WriteLine("\n--- 3. Record Types with Runnables ---");

var createPerson = RunnableLambda.Create<string, string, int, (string FirstName, string LastName, int Age)>(
    (first, last, age) => (FirstName: first, LastName: last, Age: age));

var promoteToEmployee = RunnableLambda.Create<(string FirstName, string LastName, int Age), (string FirstName, string LastName, int Age, string Department, decimal Salary)>(
    person => (
        person.FirstName,
        person.LastName,
        person.Age,
        Department: "Engineering",
        Salary: 50000m + (person.Age * 1000m)));

var formatEmployee = RunnableLambda.Create<(string FirstName, string LastName, int Age, string Department, decimal Salary), string>(
    emp => $"{emp.FirstName} {emp.LastName}, {emp.Age}y, {emp.Department}, ${emp.Salary:N0}");

var employeePipeline = createPerson
    .Pipe(promoteToEmployee)
    .Pipe(formatEmployee);

Console.WriteLine(employeePipeline.Invoke("Alice", "Smith", 28));
Console.WriteLine(employeePipeline.Invoke("Bob", "Jones", 35));

// ==================== 4. Collection Expressions (NET 9) ====================
Console.WriteLine("\n--- 4. Collection Expressions ---");

var processCollection = RunnableLambda.Create<int[], string>(
    nums => $"Sum: {nums.Sum()}, Avg: {nums.Average():F1}, Max: {nums.Max()}");

int[] data = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];
Console.WriteLine(processCollection.Invoke(data));

// ==================== 5. Async Batch Processing ====================
Console.WriteLine("\n--- 5. Async Batch Processing ---");

var asyncProcessor = RunnableLambda.Create<int, string>(
    id => $"Item{id}",
    async id => {
        await Task.Delay(10);
        return $"AsyncItem{id}";
    });

var asyncItems = new[] { 1, 2, 3, 4, 5 };
var asyncResultsList = new List<string>();
foreach (var item in asyncItems)
{
    asyncResultsList.Add(await asyncProcessor.InvokeAsync(item));
}
Console.WriteLine("Async batch: " + string.Join(", ", asyncResultsList));

// ==================== 6. Complex Error Handling Pipeline ====================
Console.WriteLine("\n--- 6. Complex Error Handling ---");

var validateInput = RunnableLambda.Create<string, int>(input => {
    if (string.IsNullOrWhiteSpace(input))
        throw new ArgumentException("Input cannot be empty");
    if (!int.TryParse(input, out var num))
        throw new FormatException("Invalid number format");
    if (num < 0)
        throw new ArgumentOutOfRangeException("Number must be positive");
    return num;
});

var processNumber = RunnableLambda.Create<int, string>(num => 
    num % 2 == 0 ? $"✓ Even: {num}" : $"✓ Odd: {num}");

var robustPipeline = validateInput
    .Pipe(processNumber)
    .WithRetry(maxAttempts: 3)
    .WithFallbackValue("✗ Processing failed");

var testInputs = new[] { "42", "-10", "abc", "", "7" };
foreach (var input in testInputs)
{
    Console.WriteLine($"  Input '{input}': {robustPipeline.Invoke(input)}");
}

// ==================== 7. Multi-Parameter Composition ====================
Console.WriteLine("\n--- 7. Multi-Parameter Composition ---");

var calculateDiscount = RunnableLambda.Create<decimal, int, string, decimal>(
    (price, quantity, customerType) => {
        var subtotal = price * quantity;
        var discount = customerType switch {
            "VIP" => 0.20m,
            "Member" => 0.10m,
            _ => 0m
        };
        return subtotal * (1 - discount);
    });

var formatPrice = RunnableLambda.Create<decimal, string>(
    price => $"${price:F2}");

var pricingPipeline = calculateDiscount
    .Pipe(formatPrice)
    .Tap(result => Console.WriteLine($"  Final price: {result}"));

pricingPipeline.Invoke(100m, 3, "VIP");    // $240.00 (20% off)
pricingPipeline.Invoke(100m, 3, "Member"); // $270.00 (10% off)
pricingPipeline.Invoke(100m, 3, "Guest");  // $300.00 (no discount)

// ==================== 8. Functional Composition Patterns ====================
Console.WriteLine("\n--- 8. Functional Composition Patterns ---");

// Compose multiple small functions
var trim = RunnableLambda.Create<string, string>(s => s.Trim());
var toLower = RunnableLambda.Create<string, string>(s => s.ToLower());
var removeSpaces = RunnableLambda.Create<string, string>(s => s.Replace(" ", ""));
var reverse = RunnableLambda.Create<string, string>(s => 
    new string(s.Reverse().ToArray()));

var textPipeline = trim
    .Pipe(toLower)
    .Pipe(removeSpaces)
    .Pipe(reverse);

Console.WriteLine(textPipeline.Invoke("  Hello World  ")); // "dlrowolleh"

// ==================== 9. High-Performance Batch Processing ====================
Console.WriteLine("\n--- 9. Batch Processing ---");

var expensiveOperation = RunnableLambda.Create<int, int>(
    x => {
        Thread.Sleep(5);
        return x * x;
    },
    async x => {
        await Task.Delay(5);
        return x * x;
    });

var batchInputs = Enumerable.Range(1, 10).ToList();
var startTime = DateTime.Now;
var batchResults = new List<int>();
foreach (var input in batchInputs)
{
    batchResults.Add(await expensiveOperation.InvokeAsync(input));
}
var elapsed = DateTime.Now - startTime;

Console.WriteLine($"Processed {batchResults.Count} items in {elapsed.TotalMilliseconds:F0}ms");
Console.WriteLine($"Results: {string.Join(", ", batchResults.Take(5))}...");

// ==================== 10. Real-World: Data Transformation Pipeline ====================
Console.WriteLine("\n--- 10. Real-World: ETL Pipeline ---");

var parseRawData = RunnableLambda.Create<string, string, string, (string Timestamp, string Value, string Status)>(
    (ts, val, status) => (Timestamp: ts, Value: val, Status: status));

var transformData = RunnableLambda.Create<(string Timestamp, string Value, string Status), (DateTime Time, double NumericValue, bool IsValid)>(raw => 
    (Time: DateTime.Parse(raw.Timestamp),
     NumericValue: double.Parse(raw.Value),
     IsValid: raw.Status == "OK"));

var aggregateData = RunnableLambda.Create<(DateTime Time, double NumericValue, bool IsValid), (DateTime Time, double Value, bool IsValid, string Summary)>(data =>
    (data.Time,
     data.NumericValue,
     data.IsValid,
     Summary: data.IsValid 
        ? $"Valid reading: {data.NumericValue:F2}" 
        : "Invalid reading"));

var formatReport = RunnableLambda.Create<(DateTime Time, double Value, bool IsValid, string Summary), string>(agg =>
    $"[{agg.Time:HH:mm:ss}] {agg.Summary}");

var etlPipeline = parseRawData
    .Pipe(transformData)
    .Pipe(aggregateData)
    .Pipe(formatReport)
    .WithRetry(2)
    .WithFallbackValue("⚠ Data processing error");

Console.WriteLine(etlPipeline.Invoke("2024-01-15 10:30:00", "42.5", "OK"));
Console.WriteLine(etlPipeline.Invoke("2024-01-15 10:31:00", "38.2", "ERROR"));

// ==================== Summary ====================
Console.WriteLine("\n=== .NET 9 Example Complete ===");
Console.WriteLine("✓ Showcased: Advanced composition, pattern matching, records,");
Console.WriteLine("  collection expressions, async streams, and ETL pipelines");
Console.WriteLine("✓ Runnable library leverages modern C# 13 features on .NET 9!");
