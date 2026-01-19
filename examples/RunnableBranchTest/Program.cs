using Runnable; 
using System;
using System.Threading.Tasks;

// RunnableBranch Test - Conditional Routing
Console.WriteLine("=== RunnableBranch Tests ===\n");

// ==================== 1. Basic Branching (1 Parameter) ====================
Console.WriteLine("--- 1. Basic Branching (1 Parameter) ---");

var processPositive = RunnableLambda.Create<int, string>(x => $"Positive: {x}");
var processNegative = RunnableLambda.Create<int, string>(x => $"Negative: {x}");
var processZero = RunnableLambda.Create<int, string>(x => "Zero");

var numberBranch = RunnableBranch.Create<int, string>(
    processZero, // default
    (x => x > 0, processPositive),
    (x => x < 0, processNegative)
);

Console.WriteLine(numberBranch.Invoke(42));   // "Positive: 42"
Console.WriteLine(numberBranch.Invoke(-7));   // "Negative: -7"
Console.WriteLine(numberBranch.Invoke(0));    // "Zero"

// ==================== 2. Classification with Multiple Conditions ====================
Console.WriteLine("\n--- 2. Classification with Multiple Conditions ---");

var tiny = RunnableLambda.Create<int, string>(x => $"{x} is tiny (0-10)");
var small = RunnableLambda.Create<int, string>(x => $"{x} is small (11-50)");
var medium = RunnableLambda.Create<int, string>(x => $"{x} is medium (51-100)");
var large = RunnableLambda.Create<int, string>(x => $"{x} is large (100+)");

var classifier = RunnableBranch.Create<int, string>(
    large, // default
    (x => x >= 0 && x <= 10, tiny),
    (x => x >= 11 && x <= 50, small),
    (x => x >= 51 && x <= 100, medium)
);

Console.WriteLine(classifier.Invoke(5));
Console.WriteLine(classifier.Invoke(25));
Console.WriteLine(classifier.Invoke(75));
Console.WriteLine(classifier.Invoke(150));

// ==================== 3. User Role Based Routing ====================
Console.WriteLine("\n--- 3. User Role Based Routing ---");

var adminHandler = RunnableLambda.Create<string, string>(user => $"Admin access granted for {user}");
var moderatorHandler = RunnableLambda.Create<string, string>(user => $"Moderator access granted for {user}");
var userHandler = RunnableLambda.Create<string, string>(user => $"User access granted for {user}");
var guestHandler = RunnableLambda.Create<string, string>(user => $"Guest access for {user}");

var roleRouter = RunnableBranch.Create<string, string>(
    guestHandler, // default
    (user => user.StartsWith("admin_"), adminHandler),
    (user => user.StartsWith("mod_"), moderatorHandler),
    (user => user.StartsWith("user_"), userHandler)
);

Console.WriteLine(roleRouter.Invoke("admin_alice"));
Console.WriteLine(roleRouter.Invoke("mod_bob"));
Console.WriteLine(roleRouter.Invoke("user_charlie"));
Console.WriteLine(roleRouter.Invoke("guest123"));

// ==================== 4. Two Parameter Branching ====================
Console.WriteLine("\n--- 4. Two Parameter Branching ---");

var sumHandler = RunnableLambda.Create<int, int, string>((a, b) => $"Sum: {a + b}");
var productHandler = RunnableLambda.Create<int, int, string>((a, b) => $"Product: {a * b}");
var divideHandler = RunnableLambda.Create<int, int, string>((a, b) => $"Division: {(double)a / b:F2}");
var defaultHandler = RunnableLambda.Create<int, int, string>((a, b) => $"No operation for {a}, {b}");

var mathBranch = RunnableBranch.Create<int, int, string>(
    defaultHandler, // default
    ((a, b) => a > 0 && b > 0, sumHandler),
    ((a, b) => a < 0 || b < 0, productHandler),
    ((a, b) => b != 0 && a % b == 0, divideHandler)
);

Console.WriteLine(mathBranch.Invoke(10, 5));   // Sum
Console.WriteLine(mathBranch.Invoke(-5, 3));   // Product
Console.WriteLine(mathBranch.Invoke(20, 4));   // Division (first match wins)

// ==================== 5. Email Validation Branching ====================
Console.WriteLine("\n--- 5. Email Validation Branching ---");

var validEmailProcessor = RunnableLambda.Create<string, string>(email => 
    $"? Valid email: {email.ToLower()}");
var invalidFormatProcessor = RunnableLambda.Create<string, string>(email => 
    $"? Invalid format: {email}");
var emptyEmailProcessor = RunnableLambda.Create<string, string>(email => 
    "? Email is required");
var suspiciousEmailProcessor = RunnableLambda.Create<string, string>(email => 
    $"? Suspicious email: {email}");

var emailBranch = RunnableBranch.Create<string, string>(
    invalidFormatProcessor, // default
    (email => string.IsNullOrWhiteSpace(email), emptyEmailProcessor),
    (email => email.Contains("spam") || email.Contains("test"), suspiciousEmailProcessor),
    (email => email.Contains("@") && email.Contains("."), validEmailProcessor)
);

Console.WriteLine(emailBranch.Invoke("alice@example.com"));
Console.WriteLine(emailBranch.Invoke(""));
Console.WriteLine(emailBranch.Invoke("spam@test.com"));
Console.WriteLine(emailBranch.Invoke("invalid-email"));

// ==================== 6. Async Branching ====================
Console.WriteLine("\n--- 6. Async Branching ---");

var asyncFastPath = RunnableLambda.Create<int, string>(
    id => $"Fast: {id}",
    async id => {
        await Task.Delay(10);
        return $"Fast async: {id}";
    });

var asyncSlowPath = RunnableLambda.Create<int, string>(
    id => $"Slow: {id}",
    async id => {
        await Task.Delay(50);
        return $"Slow async: {id}";
    });

var asyncDefault = RunnableLambda.Create<int, string>(
    id => $"Default: {id}",
    async id => {
        await Task.Delay(5);
        return $"Default async: {id}";
    });

var asyncBranch = RunnableBranch.Create<int, string>(
    asyncDefault,
    (id => id < 10, asyncFastPath),
    (id => id >= 10, asyncSlowPath)
);

var result1 = await asyncBranch.InvokeAsync(5);
Console.WriteLine(result1);

var result2 = await asyncBranch.InvokeAsync(15);
Console.WriteLine(result2);

// ==================== 7. Grade Calculation Branching ====================
Console.WriteLine("\n--- 7. Grade Calculation Branching ---");

var gradeA = RunnableLambda.Create<int, string>(score => $"Grade A: {score}% - Excellent!");
var gradeB = RunnableLambda.Create<int, string>(score => $"Grade B: {score}% - Good!");
var gradeC = RunnableLambda.Create<int, string>(score => $"Grade C: {score}% - Average");
var gradeD = RunnableLambda.Create<int, string>(score => $"Grade D: {score}% - Below Average");
var gradeF = RunnableLambda.Create<int, string>(score => $"Grade F: {score}% - Failed");

var gradeBranch = RunnableBranch.Create<int, string>(
    gradeF, // default
    (score => score >= 90, gradeA),
    (score => score >= 80, gradeB),
    (score => score >= 70, gradeC),
    (score => score >= 60, gradeD)
);

Console.WriteLine(gradeBranch.Invoke(95));
Console.WriteLine(gradeBranch.Invoke(82));
Console.WriteLine(gradeBranch.Invoke(75));
Console.WriteLine(gradeBranch.Invoke(65));
Console.WriteLine(gradeBranch.Invoke(45));

// ==================== 8. Three Parameter Branching (Discount Calculator) ====================
Console.WriteLine("\n--- 8. Three Parameter Discount Calculator ---");

var vipDiscount = RunnableLambda.Create<decimal, int, string, string>(
    (price, qty, customerType) => {
        var total = price * qty * 0.8m; // 20% off
        return $"VIP: ${total:F2} (20% off)";
    });

var memberDiscount = RunnableLambda.Create<decimal, int, string, string>(
    (price, qty, customerType) => {
        var total = price * qty * 0.9m; // 10% off
        return $"Member: ${total:F2} (10% off)";
    });

var bulkDiscount = RunnableLambda.Create<decimal, int, string, string>(
    (price, qty, customerType) => {
        var total = price * qty * 0.95m; // 5% off for bulk
        return $"Bulk: ${total:F2} (5% off for 10+ items)";
    });

var noDiscount = RunnableLambda.Create<decimal, int, string, string>(
    (price, qty, customerType) => {
        var total = price * qty;
        return $"Regular: ${total:F2} (no discount)";
    });

var discountBranch = RunnableBranch.Create<decimal, int, string, string>(
    noDiscount, // default
    ((price, qty, type) => type == "VIP", vipDiscount),
    ((price, qty, type) => type == "Member", memberDiscount),
    ((price, qty, type) => qty >= 10, bulkDiscount)
);

Console.WriteLine(discountBranch.Invoke(100m, 5, "VIP"));
Console.WriteLine(discountBranch.Invoke(100m, 5, "Member"));
Console.WriteLine(discountBranch.Invoke(100m, 15, "Guest"));
Console.WriteLine(discountBranch.Invoke(100m, 3, "Guest"));

// ==================== 9. Combining Branch with Other Extensions ====================
Console.WriteLine("\n--- 9. Combining Branch with Extensions ---");

var complexBranch = RunnableBranch.Create<int, string>(
        RunnableLambda.Create<int, string>(x => $"Default: {x}"),
        (x => x > 0, RunnableLambda.Create<int, string>(x => $"Positive: {x}")),
        (x => x < 0, RunnableLambda.Create<int, string>(x => $"Negative: {x}"))
    )
    .Map(result => result.ToUpper())
    .Tap(result => Console.WriteLine($"  Processing: {result}"))
    .WithRetry(2);

complexBranch.Invoke(42);
complexBranch.Invoke(-7);

// ==================== 10. Real-World: HTTP Status Code Routing ====================
Console.WriteLine("\n--- 10. Real-World: HTTP Status Code Routing ---");

var handleSuccess = RunnableLambda.Create<int, string, string>(
    (code, message) => $"? Success ({code}): {message}");

var handleRedirect = RunnableLambda.Create<int, string, string>(
    (code, message) => $"¡ú Redirect ({code}): {message}");

var handleClientError = RunnableLambda.Create<int, string, string>(
    (code, message) => $"? Client Error ({code}): {message}");

var handleServerError = RunnableLambda.Create<int, string, string>(
    (code, message) => $"? Server Error ({code}): {message}");

var handleUnknown = RunnableLambda.Create<int, string, string>(
    (code, message) => $"? Unknown ({code}): {message}");

var httpRouter = RunnableBranch.Create<int, string, string>(
    handleUnknown, // default
    ((code, msg) => code >= 200 && code < 300, handleSuccess),
    ((code, msg) => code >= 300 && code < 400, handleRedirect),
    ((code, msg) => code >= 400 && code < 500, handleClientError),
    ((code, msg) => code >= 500 && code < 600, handleServerError)
);

Console.WriteLine(httpRouter.Invoke(200, "OK"));
Console.WriteLine(httpRouter.Invoke(301, "Moved Permanently"));
Console.WriteLine(httpRouter.Invoke(404, "Not Found"));
Console.WriteLine(httpRouter.Invoke(500, "Internal Server Error"));
Console.WriteLine(httpRouter.Invoke(999, "Custom Status"));

// ==================== Summary ====================
Console.WriteLine("\n=== RunnableBranch Tests Complete ===");
Console.WriteLine("? Conditional routing for 0-8 parameters");
Console.WriteLine("? First-match-wins evaluation order");
Console.WriteLine("? Async support");
Console.WriteLine("? Composable with other Runnable extensions");
Console.WriteLine("? Real-world routing scenarios demonstrated");
