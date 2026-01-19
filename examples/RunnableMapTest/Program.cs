using Runnable; 
using System;
using System.Linq;
using System.Threading.Tasks;

// RunnableMap Test - Parallel Execution
Console.WriteLine("=== RunnableMap Tests ===\n");

// ==================== 1. Basic Map (1 Parameter) ====================
Console.WriteLine("--- 1. Basic Map (1 Parameter) ---");

var square = RunnableLambda.Create<int, int>(x => x * x);
var cube = RunnableLambda.Create<int, int>(x => x * x * x);
var double_ = RunnableLambda.Create<int, int>(x => x * 2);

var mathMap = RunnableMap.Create<int, int>(
    ("square", square),
    ("cube", cube),
    ("double", double_)
);

var results = mathMap.Invoke(5);
Console.WriteLine($"Input: 5");
foreach (var (key, value) in results.OrderBy(x => x.Key))
{
    Console.WriteLine($"  {key}: {value}");
}

// ==================== 2. String Processing Map ====================
Console.WriteLine("\n--- 2. String Processing Map ---");

var upper = RunnableLambda.Create<string, string>(s => s.ToUpper());
var lower = RunnableLambda.Create<string, string>(s => s.ToLower());
var length = RunnableLambda.Create<string, int>(s => s.Length);
var reverse = RunnableLambda.Create<string, string>(s => new string(s.Reverse().ToArray()));

// Mixed output types - using object
var stringProcessors = RunnableMap.Create<string, string>(
    ("uppercase", upper),
    ("lowercase", lower),
    ("reversed", reverse)
);

var stringResults = stringProcessors.Invoke("Hello");
Console.WriteLine($"Input: 'Hello'");
foreach (var (key, value) in stringResults.OrderBy(x => x.Key))
{
    Console.WriteLine($"  {key}: {value}");
}

// ==================== 3. Data Enrichment ====================
Console.WriteLine("\n--- 3. Data Enrichment ---");

var calculateTax = RunnableLambda.Create<decimal, decimal>(amount => amount * 0.08m);
var calculateShipping = RunnableLambda.Create<decimal, decimal>(amount => 
    amount > 100m ? 0m : 9.99m);
var calculateDiscount = RunnableLambda.Create<decimal, decimal>(amount => 
    amount > 200m ? amount * 0.1m : 0m);

var priceMap = RunnableMap.Create<decimal, decimal>(
    ("tax", calculateTax),
    ("shipping", calculateShipping),
    ("discount", calculateDiscount)
);

var priceResults = priceMap.Invoke(150m);
Console.WriteLine($"Price: $150.00");
foreach (var (key, value) in priceResults.OrderBy(x => x.Key))
{
    Console.WriteLine($"  {key}: ${value:F2}");
}

var total = 150m + priceResults["tax"] + priceResults["shipping"] - priceResults["discount"];
Console.WriteLine($"  Total: ${total:F2}");

// ==================== 4. Async Parallel Execution ====================
Console.WriteLine("\n--- 4. Async Parallel Execution ---");

var asyncFetch1 = RunnableLambda.Create<string, string>(
    id => $"Data1-{id}",
    async id => {
        await Task.Delay(100);
        return $"AsyncData1-{id}";
    });

var asyncFetch2 = RunnableLambda.Create<string, string>(
    id => $"Data2-{id}",
    async id => {
        await Task.Delay(50);
        return $"AsyncData2-{id}";
    });

var asyncFetch3 = RunnableLambda.Create<string, string>(
    id => $"Data3-{id}",
    async id => {
        await Task.Delay(75);
        return $"AsyncData3-{id}";
    });

var asyncMap = RunnableMap.Create<string, string>(
    ("source1", asyncFetch1),
    ("source2", asyncFetch2),
    ("source3", asyncFetch3)
);

var startTime = DateTime.Now;
var asyncResults = await asyncMap.InvokeAsync("user123");
var elapsed = DateTime.Now - startTime;

Console.WriteLine($"Async execution time: {elapsed.TotalMilliseconds:F0}ms (parallel)");
foreach (var (key, value) in asyncResults.OrderBy(x => x.Key))
{
    Console.WriteLine($"  {key}: {value}");
}

// ==================== 5. Two Parameter Map ====================
Console.WriteLine("\n--- 5. Two Parameter Map ---");

var sum = RunnableLambda.Create<int, int, int>((a, b) => a + b);
var product = RunnableLambda.Create<int, int, int>((a, b) => a * b);
var difference = RunnableLambda.Create<int, int, int>((a, b) => Math.Abs(a - b));
var average = RunnableLambda.Create<int, int, double>((a, b) => (a + b) / 2.0);

var twoParamMap = RunnableMap.Create<int, int, int>(
    ("sum", sum),
    ("product", product),
    ("difference", difference)
);

var twoParamResults = twoParamMap.Invoke(10, 5);
Console.WriteLine($"Inputs: 10, 5");
foreach (var (key, value) in twoParamResults.OrderBy(x => x.Key))
{
    Console.WriteLine($"  {key}: {value}");
}

// ==================== 6. User Validation Map ====================
Console.WriteLine("\n--- 6. User Validation Map ---");

var validateEmail = RunnableLambda.Create<string, bool>(email => 
    email.Contains("@") && email.Contains("."));

var validateLength = RunnableLambda.Create<string, bool>(email => 
    email.Length >= 5 && email.Length <= 100);

var extractDomain = RunnableLambda.Create<string, string>(email => 
    email.Contains("@") ? email.Split('@')[1] : "invalid");

var validationMap = RunnableMap.Create<string, bool>(
    ("email_format", validateEmail),
    ("length_check", validateLength)
);

var validationResults = validationMap.Invoke("alice@example.com");
Console.WriteLine($"Email: 'alice@example.com'");
foreach (var (key, value) in validationResults.OrderBy(x => x.Key))
{
    Console.WriteLine($"  {key}: {value}");
}

// ==================== 7. Feature Extraction ====================
Console.WriteLine("\n--- 7. Feature Extraction ---");

var wordCount = RunnableLambda.Create<string, int>(text => 
    text.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length);

var charCount = RunnableLambda.Create<string, int>(text => text.Length);

var hasNumbers = RunnableLambda.Create<string, bool>(text => 
    text.Any(char.IsDigit));

var hasSpecialChars = RunnableLambda.Create<string, bool>(text => 
    text.Any(c => !char.IsLetterOrDigit(c) && !char.IsWhiteSpace(c)));

var featureMap = RunnableMap.Create<string, int>(
    ("word_count", wordCount),
    ("char_count", charCount)
);

var text = "Hello world! This is a test.";
var features = featureMap.Invoke(text);
Console.WriteLine($"Text: '{text}'");
foreach (var (key, value) in features.OrderBy(x => x.Key))
{
    Console.WriteLine($"  {key}: {value}");
}

// ==================== 8. Multi-Model Response ====================
Console.WriteLine("\n--- 8. Multi-Model Response Simulation ---");

var model1 = RunnableLambda.Create<string, string>(
    query => $"Model1: Processing '{query}'",
    async query => {
        await Task.Delay(30);
        return $"Model1: Async result for '{query}'";
    });

var model2 = RunnableLambda.Create<string, string>(
    query => $"Model2: Processing '{query}'",
    async query => {
        await Task.Delay(25);
        return $"Model2: Async result for '{query}'";
    });

var model3 = RunnableLambda.Create<string, string>(
    query => $"Model3: Processing '{query}'",
    async query => {
        await Task.Delay(20);
        return $"Model3: Async result for '{query}'";
    });

var multiModelMap = RunnableMap.Create<string, string>(
    ("gpt", model1),
    ("claude", model2),
    ("llama", model3)
);

var modelStart = DateTime.Now;
var modelResults = await multiModelMap.InvokeAsync("What is AI?");
var modelElapsed = DateTime.Now - modelStart;

Console.WriteLine($"Multi-model execution: {modelElapsed.TotalMilliseconds:F0}ms");
foreach (var (key, value) in modelResults.OrderBy(x => x.Key))
{
    Console.WriteLine($"  {key}: {value}");
}

// ==================== 9. Composition with Other Extensions ====================
Console.WriteLine("\n--- 9. Composition with Extensions ---");

var enrichedMap = RunnableMap.Create<int, int>(
        ("square", square),
        ("cube", cube)
    )
    .Map(dict => {
        var sum = dict.Values.Sum();
        return $"Sum of all results: {sum}";
    })
    .Tap(result => Console.WriteLine($"  Processing: {result}"));

enrichedMap.Invoke(3);

// ==================== 10. Real-World: API Aggregation ====================
Console.WriteLine("\n--- 10. Real-World: API Aggregation ---");

var fetchUserProfile = RunnableLambda.Create<string, string>(
    userId => $"Profile data for {userId}",
    async userId => {
        await Task.Delay(40);
        return $"{{name: 'User{userId}', age: 30}}";
    });

var fetchUserPosts = RunnableLambda.Create<string, string>(
    userId => $"Posts for {userId}",
    async userId => {
        await Task.Delay(35);
        return $"[Post1, Post2, Post3]";
    });

var fetchUserFollowers = RunnableLambda.Create<string, string>(
    userId => $"Followers for {userId}",
    async userId => {
        await Task.Delay(30);
        return $"[User1, User2, User3]";
    });

var fetchUserSettings = RunnableLambda.Create<string, string>(
    userId => $"Settings for {userId}",
    async userId => {
        await Task.Delay(25);
        return $"{{theme: 'dark', lang: 'en'}}";
    });

var apiAggregator = RunnableMap.Create<string, string>(
    ("profile", fetchUserProfile),
    ("posts", fetchUserPosts),
    ("followers", fetchUserFollowers),
    ("settings", fetchUserSettings)
);

var apiStart = DateTime.Now;
var apiResults = await apiAggregator.InvokeAsync("123");
var apiElapsed = DateTime.Now - apiStart;

Console.WriteLine($"API aggregation time: {apiElapsed.TotalMilliseconds:F0}ms (4 parallel calls)");
foreach (var (key, value) in apiResults.OrderBy(x => x.Key))
{
    Console.WriteLine($"  {key}: {value}");
}

// ==================== 11. Zero Parameter Map ====================
Console.WriteLine("\n--- 11. Zero Parameter Map ---");

var getCurrentTime = RunnableLambda.Create(() => DateTime.Now.ToString("HH:mm:ss"));
var getRandomNumber = RunnableLambda.Create(() => Random.Shared.Next(1, 100));
var getGuid = RunnableLambda.Create(() => Guid.NewGuid().ToString().Substring(0, 8));

var zeroParamMap = RunnableMap.Create<string>(
    ("time", getCurrentTime),
    ("guid", getGuid)
);

var zeroResults = zeroParamMap.Invoke();
Console.WriteLine("Zero parameter map:");
foreach (var (key, value) in zeroResults.OrderBy(x => x.Key))
{
    Console.WriteLine($"  {key}: {value}");
}

// ==================== Summary ====================
Console.WriteLine("\n=== RunnableMap Tests Complete ===");
Console.WriteLine("? Parallel execution of multiple runnables");
Console.WriteLine("? Dictionary-based output with named results");
Console.WriteLine("? Async support with Task.WhenAll");
Console.WriteLine("? Support for 0-8 parameters");
Console.WriteLine("? Composable with other Runnable extensions");
Console.WriteLine("? Real-world API aggregation scenarios");
