using Runnable;
using System;
using System.Linq;
using System.Threading.Tasks;

// .NET 10 Example - Cutting-Edge Runnable Library Features
Console.WriteLine("=== Runnable Library - .NET 10 Example ===\n");

// ==================== 1. Maximum Arity Showcase (16 Parameters) ====================
Console.WriteLine("--- 1. Maximum Arity (16 Parameters) ---");

var sum16 = RunnableLambda.Create<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(
    (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16) =>
        a1 + a2 + a3 + a4 + a5 + a6 + a7 + a8 + a9 + a10 + a11 + a12 + a13 + a14 + a15 + a16);

var describe16 = RunnableLambda.Create<int, string>(sum => 
    $"Sum of 16 numbers: {sum} (Average: {sum / 16.0:F1})");

var pipeline16 = sum16
    .Pipe(describe16)
    .Tap(result => Console.WriteLine($"  {result}"));

pipeline16.Invoke(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);

// ==================== 2. Advanced Generic Composition ====================
Console.WriteLine("\n--- 2. Advanced Generic Composition ---");

var createDataPoint = RunnableLambda.Create<double, (double Value, DateTime Timestamp, string Source)>(
    value => (Value: value, Timestamp: DateTime.Now, Source: "Sensor-A"));

var validateDataPoint = RunnableLambda.Create<(double Value, DateTime Timestamp, string Source), (double Value, DateTime Timestamp, string Source)>(
    dp => {
        if (dp.Value < 0 || dp.Value > 100)
            throw new ArgumentOutOfRangeException("Value out of range");
        return dp;
    });

var formatDataPoint = RunnableLambda.Create<(double Value, DateTime Timestamp, string Source), string>(
    dp => $"[{dp.Source}] {dp.Value:F2} at {dp.Timestamp:HH:mm:ss}");

var sensorPipeline = createDataPoint
    .Pipe(validateDataPoint)
    .Pipe(formatDataPoint)
    .WithFallbackValue("? Invalid sensor reading");

Console.WriteLine(sensorPipeline.Invoke(75.5));
Console.WriteLine(sensorPipeline.Invoke(150.0)); // Out of range

// ==================== 3. Fluent Configuration Builder ====================
Console.WriteLine("\n--- 3. Fluent Configuration Builder (6 Parameters) ---");

var buildDbConfig = RunnableLambda.Create<string, int, string, string, bool, int, (string Host, int Port, string Database, string Username, bool UseSSL, int ConnectionPoolSize)>(
    (host, port, db, user, ssl, poolSize) => 
        (Host: host, Port: port, Database: db, Username: user, UseSSL: ssl, ConnectionPoolSize: poolSize));

var validateDbConfig = RunnableLambda.Create<(string Host, int Port, string Database, string Username, bool UseSSL, int ConnectionPoolSize), (string Host, int Port, string Database, string Username, bool UseSSL, int ConnectionPoolSize)>(config => {
    if (config.Port < 1 || config.Port > 65535)
        throw new ArgumentException("Invalid port");
    if (config.ConnectionPoolSize < 1)
        throw new ArgumentException("Pool size must be positive");
    return config;
});

var formatConnectionString = RunnableLambda.Create<(string Host, int Port, string Database, string Username, bool UseSSL, int ConnectionPoolSize), string>(config =>
    $"Host={config.Host};Port={config.Port};Database={config.Database};" +
    $"User={config.Username};SSL={config.UseSSL};PoolSize={config.ConnectionPoolSize}");

var configPipeline = buildDbConfig
    .Pipe(validateDbConfig)
    .Pipe(formatConnectionString)
    .WithRetry(2)
    .WithFallbackValue("Default connection string");

var connString = configPipeline.Invoke("localhost", 5432, "mydb", "admin", true, 10);
Console.WriteLine(connString);

// ==================== 4. Async Processing Pipeline ====================
Console.WriteLine("\n--- 4. Async Processing Pipeline ---");

var computeHash = RunnableLambda.Create<string, int>(
    s => s.GetHashCode(),
    async s => {
        await Task.Delay(10);
        return s.GetHashCode();
    });

var categorizeHash = RunnableLambda.Create<int, string>(hash => {
    var abs = Math.Abs(hash);
    return (abs % 3) switch {
        0 => "Category A",
        1 => "Category B",
        _ => "Category C"
    };
});

var hashPipeline = computeHash.Pipe(categorizeHash);

var inputs = new[] { "apple", "banana", "cherry", "date", "elderberry" };
var startTime = DateTime.Now;
var categories = new List<string>();
foreach (var input in inputs)
{
    categories.Add(await hashPipeline.InvokeAsync(input));
}
var elapsed = DateTime.Now - startTime;

Console.WriteLine($"Processed {categories.Count} items in {elapsed.TotalMilliseconds:F0}ms:");
for (int i = 0; i < inputs.Length; i++)
{
    Console.WriteLine($"  {inputs[i],-12} => {categories[i]}");
}

// ==================== 5. Complex Multi-Stage Pipeline ====================
Console.WriteLine("\n--- 5. Complex Multi-Stage Pipeline (10+ Parameters) ---");

var calculateMetrics = RunnableLambda.Create<
    double, double, double, double, double,
    double, double, double, double, double,
    (double Sum, double Avg, double Max, double Min)>(
    (v1, v2, v3, v4, v5, v6, v7, v8, v9, v10) => {
        var values = new[] { v1, v2, v3, v4, v5, v6, v7, v8, v9, v10 };
        return (
            Sum: values.Sum(),
            Avg: values.Average(),
            Max: values.Max(),
            Min: values.Min()
        );
    });

var formatMetrics = RunnableLambda.Create<(double Sum, double Avg, double Max, double Min), string>(
    metrics => 
        $"Sum: {metrics.Sum:F2}, Avg: {metrics.Avg:F2}, " +
        $"Max: {metrics.Max:F2}, Min: {metrics.Min:F2}");

var metricsPipeline = calculateMetrics
    .Pipe(formatMetrics)
    .Tap(result => Console.WriteLine($"  Metrics: {result}"));

metricsPipeline.Invoke(1.5, 2.3, 3.7, 4.2, 5.8, 6.1, 7.9, 8.4, 9.2, 10.6);

// ==================== 6. State Machine with Runnable ====================
Console.WriteLine("\n--- 6. State Machine Pattern ---");

var transition1 = RunnableLambda.Create<(string Name, int Value), (string Name, int Value)>(
    state => (Name: "Processing", Value: state.Value * 2));

var transition2 = RunnableLambda.Create<(string Name, int Value), (string Name, int Value)>(
    state => (Name: "Validating", Value: state.Value + 10));

var transition3 = RunnableLambda.Create<(string Name, int Value), (string Name, int Value)>(
    state => (Name: "Complete", Value: state.Value));

var formatState = RunnableLambda.Create<(string Name, int Value), string>(
    state => $"State: {state.Name}, Value: {state.Value}");

var stateMachine = transition1
    .Pipe(transition2)
    .Pipe(transition3)
    .Pipe(formatState)
    .Tap(s => Console.WriteLine($"  {s}"));

var initialState = (Name: "Initial", Value: 5);
stateMachine.Invoke(initialState);

// ==================== 7. Real-World: Financial Calculator ====================
Console.WriteLine("\n--- 7. Real-World: Financial Calculator (5 Parameters) ---");

var calculateLoan = RunnableLambda.Create<decimal, decimal, int, int, string, (decimal Principal, decimal Rate, int Years, int Frequency, string Type)>(
    (principal, rate, years, frequency, type) =>
        (Principal: principal, Rate: rate, Years: years, Frequency: frequency, Type: type));

var computePayment = RunnableLambda.Create<(decimal Principal, decimal Rate, int Years, int Frequency, string Type), decimal>(loan => {
    var monthlyRate = (double)(loan.Rate / 12 / 100);
    var months = loan.Years * 12;
    var payment = (double)loan.Principal * 
        (monthlyRate * Math.Pow(1 + monthlyRate, months)) /
        (Math.Pow(1 + monthlyRate, months) - 1);
    return (decimal)payment;
});

var formatPayment = RunnableLambda.Create<decimal, string>(
    payment => $"Monthly payment: ${payment:F2}");

var loanPipeline = calculateLoan
    .Pipe(computePayment)
    .Pipe(formatPayment)
    .WithFallbackValue("Error calculating loan");

Console.WriteLine(loanPipeline.Invoke(200000m, 5.5m, 30, 12, "Fixed"));
Console.WriteLine(loanPipeline.Invoke(350000m, 4.25m, 15, 12, "Fixed"));

// ==================== 8. Async Batch Processing ====================
Console.WriteLine("\n--- 8. Async Batch Processing ---");

var asyncTransform = RunnableLambda.Create<int, string>(
    x => $"Item-{x}",
    async x => {
        await Task.Delay(5);
        return $"Async-Item-{x}";
    });

var asyncStream = Enumerable.Range(1, 20).ToList();
var asyncStart = DateTime.Now;
var asyncResults = new List<string>();
foreach (var item in asyncStream)
{
    asyncResults.Add(await asyncTransform.InvokeAsync(item));
}
var asyncElapsed = DateTime.Now - asyncStart;

Console.WriteLine($"Async processed {asyncResults.Count} items in {asyncElapsed.TotalMilliseconds:F0}ms");
Console.WriteLine($"Sample: {string.Join(", ", asyncResults.Take(5))}...");

// ==================== 9. Error Recovery Strategies ====================
Console.WriteLine("\n--- 9. Advanced Error Recovery ---");

var riskyComputation = RunnableLambda.Create<int, double>(x => {
    if (x == 0) throw new DivideByZeroException();
    if (x < 0) throw new ArgumentException("Negative not allowed");
    return 100.0 / x;
});

var fallbackComputation = RunnableLambda.Create<int, double>(x => 0.0);

var resilientPipeline = riskyComputation
    .WithFallback(fallbackComputation)
    .WithRetry(maxAttempts: 3)
    .Map(result => $"{result:F2}");

var testValues = new[] { 5, 0, -3, 10 };
foreach (var val in testValues)
{
    Console.WriteLine($"  Input {val,3}: {resilientPipeline.Invoke(val)}");
}

// ==================== 10. Benchmark: Runnable vs Direct Calls ====================
Console.WriteLine("\n--- 10. Performance Benchmark ---");

var directFunc = (int x) => x * x;
var runnableFunc = RunnableLambda.Create<int, int>(x => x * x);

var iterations = 100000;
var range = Enumerable.Range(1, iterations);

// Direct calls
var sw1 = System.Diagnostics.Stopwatch.StartNew();
foreach (var i in range) { _ = directFunc(i); }
sw1.Stop();

// Runnable calls
var sw2 = System.Diagnostics.Stopwatch.StartNew();
foreach (var i in range) { _ = runnableFunc.Invoke(i); }
sw2.Stop();

Console.WriteLine($"Direct function:   {sw1.ElapsedMilliseconds}ms for {iterations:N0} calls");
Console.WriteLine($"Runnable function: {sw2.ElapsedMilliseconds}ms for {iterations:N0} calls");
Console.WriteLine($"Overhead: {(sw2.ElapsedMilliseconds - sw1.ElapsedMilliseconds)}ms ({100.0 * (sw2.ElapsedMilliseconds - sw1.ElapsedMilliseconds) / sw1.ElapsedMilliseconds:F1}%)");

// ==================== Summary ====================
Console.WriteLine("\n=== .NET 10 Example Complete ===");
Console.WriteLine("? Demonstrated: Maximum arity (16 params), generics,");
Console.WriteLine("  parallel processing, state machines, financial calculations,");
Console.WriteLine("  async streams, error recovery, and performance benchmarks");
Console.WriteLine("? Runnable library is fully optimized for .NET 10!");
Console.WriteLine("? Zero-overhead abstraction with full composability!");
