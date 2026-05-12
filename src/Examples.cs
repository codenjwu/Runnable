using System;
using System.Threading.Tasks;

namespace Runnable.Examples
{
    /// <summary>
    /// Runnable Usage Examples
    /// </summary>
    public class RunnableExamples
    {
        // Example 1: Simple string processing pipeline
        public static void StringProcessingExample()
        {
            Console.WriteLine("=== String Processing Pipeline ===");

            // Create processing steps
            var toUpper = new Func<string, string>(s => s.ToUpper())
                .AsRunnable();

            var addPrefix = new Func<string, string>(s => ">>> " + s)
                .AsRunnable();

            var reverse = new Func<string, string>(s =>
            {
                var chars = s.ToCharArray();
                Array.Reverse(chars);
                return new string(chars);
            }).AsRunnable();

            // Use pipe operator | to compose
            var pipeline = toUpper | addPrefix | reverse;

            // Execute
            var result = pipeline.Invoke("hello world");
            Console.WriteLine($"Input: hello world");
            Console.WriteLine($"Output: {result}");
            Console.WriteLine();
        }

        // Example 2: Numeric processing pipeline
        public static void NumericProcessingExample()
        {
            Console.WriteLine("=== Numeric Processing Pipeline ===");

            var doubleNumber = new Func<int, int>(x => x * 2)
                .AsRunnable();

            var addTen = new Func<int, int>(x => x + 10)
                .AsRunnable();

            var formatAsString = new Func<int, string>(x => $"Result: {x}")
                .AsRunnable();

            var pipeline = doubleNumber | addTen | formatAsString;

            var result = pipeline.Invoke(5);
            Console.WriteLine($"Input: 5");
            Console.WriteLine($"Processing: (5 * 2) + 10 = 20");
            Console.WriteLine($"Output: {result}");
            Console.WriteLine();
        }

        // Example 3: Async pipeline
        public static async Task AsyncPipelineExample()
        {
            Console.WriteLine("=== Async Pipeline ===");

            var fetchData = new Func<int, Task<string>>(async id =>
            {
                await Task.Delay(500); // Simulate network request
                return $"User Data-{id}";
            });

            var processData = new Func<string, Task<string>>(async data =>
            {
                await Task.Delay(300);
                return $"Processed: {data}";
            });

            var saveData = new Func<string, Task<string>>(async data =>
            {
                await Task.Delay(200);
                return $"Saved: {data}";
            });

            var pipeline = fetchData.AsRunnable(async x => await fetchData(x))
                | processData.AsRunnable(async x => await processData(x))
                | saveData.AsRunnable(async x => await saveData(x));

            var result = await pipeline.InvokeAsync(1);
            Console.WriteLine($"Output: {result}");
            Console.WriteLine();
        }

        // Example 4: Batch processing
        public static void BatchProcessingExample()
        {
            Console.WriteLine("=== Batch Processing ===");

            var square = new Func<int, int>(x => x * x)
                .AsRunnable();

            var addOne = new Func<int, int>(x => x + 1)
                .AsRunnable();

            var pipeline = square | addOne;

            var inputs = new[] { 1, 2, 3, 4, 5 };
            var results = pipeline.Batch(inputs);

            Console.WriteLine("Input: [1, 2, 3, 4, 5]");
            Console.Write("Output: [");
            Console.Write(string.Join(", ", results));
            Console.WriteLine("]");
            Console.WriteLine();
        }

        // Example 5: Custom Runnable class
        public class TextAnalyzer : BaseRunnable<string, int>
        {
            public override int Invoke(string input)
            {
                return input.Length;
            }
        }

        public static void CustomRunnableExample()
        {
            Console.WriteLine("=== Custom Runnable ===");

            var analyzer = new TextAnalyzer();
            var format = new Func<int, string>(x => $"Text Length: {x}")
                .AsRunnable();

            var pipeline = analyzer | format;
            var result = pipeline.Invoke("Hello, World!");

            Console.WriteLine($"Input: Hello, World!");
            Console.WriteLine($"Output: {result}");
            Console.WriteLine();
        }

        public static async Task Main(string[] args)
        {
            StringProcessingExample();
            NumericProcessingExample();
            await AsyncPipelineExample();
            BatchProcessingExample();
            CustomRunnableExample();

            Console.WriteLine("All examples completed!");
        }
    }
}
