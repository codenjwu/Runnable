using System;
using System.Threading.Tasks;

namespace Runnable.Examples
{
    /// <summary>
    /// Runnable 使用示例
    /// </summary>
    public class RunnableExamples
    {
        // 示例 1: 简单的字符串处理管道
        public static void StringProcessingExample()
        {
            Console.WriteLine("=== 字符串处理管道 ===");

            // 创建各个处理步骤
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

            // 使用管道操作符 | 组合
            var pipeline = toUpper | addPrefix | reverse;

            // 执行
            var result = pipeline.Invoke("hello world");
            Console.WriteLine($"输入: hello world");
            Console.WriteLine($"输出: {result}");
            Console.WriteLine();
        }

        // 示例 2: 数值转换管道
        public static void NumericProcessingExample()
        {
            Console.WriteLine("=== 数值处理管道 ===");

            var doubleNumber = new Func<int, int>(x => x * 2)
                .AsRunnable();

            var addTen = new Func<int, int>(x => x + 10)
                .AsRunnable();

            var formatAsString = new Func<int, string>(x => $"结果: {x}")
                .AsRunnable();

            var pipeline = doubleNumber | addTen | formatAsString;

            var result = pipeline.Invoke(5);
            Console.WriteLine($"输入: 5");
            Console.WriteLine($"处理: (5 * 2) + 10 = 20");
            Console.WriteLine($"输出: {result}");
            Console.WriteLine();
        }

        // 示例 3: 异步管道
        public static async Task AsyncPipelineExample()
        {
            Console.WriteLine("=== 异步管道 ===");

            var fetchData = new Func<int, Task<string>>(async id =>
            {
                await Task.Delay(500); // 模拟网络请求
                return $"用户数据-{id}";
            });

            var processData = new Func<string, Task<string>>(async data =>
            {
                await Task.Delay(300);
                return $"已处理: {data}";
            });

            var saveData = new Func<string, Task<string>>(async data =>
            {
                await Task.Delay(200);
                return $"已保存: {data}";
            });

            var pipeline = fetchData.AsRunnable(async x => await fetchData(x))
                | processData.AsRunnable(async x => await processData(x))
                | saveData.AsRunnable(async x => await saveData(x));

            var result = await pipeline.InvokeAsync(1);
            Console.WriteLine($"输出: {result}");
            Console.WriteLine();
        }

        // 示例 4: 批量处理
        public static void BatchProcessingExample()
        {
            Console.WriteLine("=== 批量处理 ===");

            var square = new Func<int, int>(x => x * x)
                .AsRunnable();

            var addOne = new Func<int, int>(x => x + 1)
                .AsRunnable();

            var pipeline = square | addOne;

            var inputs = new[] { 1, 2, 3, 4, 5 };
            var results = pipeline.Batch(inputs);

            Console.WriteLine("输入: [1, 2, 3, 4, 5]");
            Console.Write("输出: [");
            Console.Write(string.Join(", ", results));
            Console.WriteLine("]");
            Console.WriteLine();
        }

        // 示例 5: 自定义 Runnable 类
        public class TextAnalyzer : BaseRunnable<string, int>
        {
            public override int Invoke(string input)
            {
                return input.Length;
            }
        }

        public static void CustomRunnableExample()
        {
            Console.WriteLine("=== 自定义 Runnable ===");

            var analyzer = new TextAnalyzer();
            var format = new Func<int, string>(x => $"文本长度: {x}")
                .AsRunnable();

            var pipeline = analyzer | format;
            var result = pipeline.Invoke("Hello, World!");

            Console.WriteLine($"输入: Hello, World!");
            Console.WriteLine($"输出: {result}");
            Console.WriteLine();
        }

        public static async Task Main(string[] args)
        {
            StringProcessingExample();
            NumericProcessingExample();
            await AsyncPipelineExample();
            BatchProcessingExample();
            CustomRunnableExample();

            Console.WriteLine("所有示例执行完毕！");
        }
    }
}
