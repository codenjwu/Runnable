using Runnable;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Runnable.Tests
{
    /// <summary>
    /// Unit tests for RunnableMap functionality
    /// </summary>
    public class RunnableMapTests
    {
        // ==================== Basic Functionality Tests ====================

        [Fact]
        public void Create_ZeroParameters_ExecutesAllRunnables()
        {
            // Arrange
            var value1 = 10;
            var value2 = 20;
            var value3 = 30;

            var runnable1 = RunnableLambda.Create(() => value1);
            var runnable2 = RunnableLambda.Create(() => value2);
            var runnable3 = RunnableLambda.Create(() => value3);

            var map = RunnableMap.Create<int>(
                ("r1", runnable1),
                ("r2", runnable2),
                ("r3", runnable3)
            );

            // Act
            var results = map.Invoke();

            // Assert
            Assert.Equal(3, results.Count);
            Assert.Equal(value1, results["r1"]);
            Assert.Equal(value2, results["r2"]);
            Assert.Equal(value3, results["r3"]);
        }

        [Fact]
        public void Create_OneParameter_ExecutesAllRunnablesWithSameInput()
        {
            // Arrange
            var square = RunnableLambda.Create<int, int>(x => x * x);
            var cube = RunnableLambda.Create<int, int>(x => x * x * x);
            var double_ = RunnableLambda.Create<int, int>(x => x * 2);

            var map = RunnableMap.Create<int, int>(
                ("square", square),
                ("cube", cube),
                ("double", double_)
            );

            // Act
            var results = map.Invoke(5);

            // Assert
            Assert.Equal(3, results.Count);
            Assert.Equal(25, results["square"]);
            Assert.Equal(125, results["cube"]);
            Assert.Equal(10, results["double"]);
        }

        [Fact]
        public void Create_TwoParameters_ExecutesAllRunnablesWithSameInputs()
        {
            // Arrange
            var sum = RunnableLambda.Create<int, int, int>((a, b) => a + b);
            var product = RunnableLambda.Create<int, int, int>((a, b) => a * b);
            var difference = RunnableLambda.Create<int, int, int>((a, b) => Math.Abs(a - b));

            var map = RunnableMap.Create<int, int, int>(
                ("sum", sum),
                ("product", product),
                ("difference", difference)
            );

            // Act
            var results = map.Invoke(10, 5);

            // Assert
            Assert.Equal(3, results.Count);
            Assert.Equal(15, results["sum"]);
            Assert.Equal(50, results["product"]);
            Assert.Equal(5, results["difference"]);
        }

        [Fact]
        public void Create_ThreeParameters_ExecutesCorrectly()
        {
            // Arrange
            var sum3 = RunnableLambda.Create<int, int, int, int>((a, b, c) => a + b + c);
            var product3 = RunnableLambda.Create<int, int, int, int>((a, b, c) => a * b * c);

            var map = RunnableMap.Create<int, int, int, int>(
                ("sum", sum3),
                ("product", product3)
            );

            // Act
            var results = map.Invoke(2, 3, 4);

            // Assert
            Assert.Equal(2, results.Count);
            Assert.Equal(9, results["sum"]);
            Assert.Equal(24, results["product"]);
        }

        [Fact]
        public void Create_FourParameters_ExecutesCorrectly()
        {
            // Arrange
            var sum4 = RunnableLambda.Create<int, int, int, int, int>((a, b, c, d) => a + b + c + d);
            var product4 = RunnableLambda.Create<int, int, int, int, int>((a, b, c, d) => a * b * c * d);

            var map = RunnableMap.Create<int, int, int, int, int>(
                ("sum", sum4),
                ("product", product4)
            );

            // Act
            var results = map.Invoke(2, 3, 4, 5);

            // Assert
            Assert.Equal(2, results.Count);
            Assert.Equal(14, results["sum"]);
            Assert.Equal(120, results["product"]);
        }

        [Fact]
        public void Create_EightParameters_ExecutesCorrectly()
        {
            // Arrange
            var sum8 = RunnableLambda.Create<int, int, int, int, int, int, int, int, int>(
                (a, b, c, d, e, f, g, h) => a + b + c + d + e + f + g + h);

            var map = RunnableMap.Create<int, int, int, int, int, int, int, int, int>(
                ("sum", sum8)
            );

            // Act
            var results = map.Invoke(1, 2, 3, 4, 5, 6, 7, 8);

            // Assert
            Assert.Single(results);
            Assert.Equal(36, results["sum"]);
        }

        [Fact]
        public void Create_NineParameters_ExecutesCorrectly()
        {
            // Arrange
            var sum9 = RunnableLambda.Create<int, int, int, int, int, int, int, int, int, int>(
                (a, b, c, d, e, f, g, h, i) => a + b + c + d + e + f + g + h + i);

            var map = RunnableMap.Create<int, int, int, int, int, int, int, int, int, int>(
                ("sum", sum9)
            );

            // Act
            var results = map.Invoke(1, 2, 3, 4, 5, 6, 7, 8, 9);

            // Assert
            Assert.Single(results);
            Assert.Equal(45, results["sum"]);
        }

        [Fact]
        public void Create_TenParameters_ExecutesCorrectly()
        {
            // Arrange
            var sum10 = RunnableLambda.Create<int, int, int, int, int, int, int, int, int, int, int>(
                (a, b, c, d, e, f, g, h, i, j) => a + b + c + d + e + f + g + h + i + j);

            var map = RunnableMap.Create<int, int, int, int, int, int, int, int, int, int, int>(
                ("sum", sum10)
            );

            // Act
            var results = map.Invoke(1, 2, 3, 4, 5, 6, 7, 8, 9, 10);

            // Assert
            Assert.Single(results);
            Assert.Equal(55, results["sum"]);
        }

        [Fact]
        public void Create_SixteenParameters_ExecutesCorrectly()
        {
            // Arrange
            var sum16 = RunnableLambda.Create<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16) => 
                    a1 + a2 + a3 + a4 + a5 + a6 + a7 + a8 + a9 + a10 + a11 + a12 + a13 + a14 + a15 + a16);

            var map = RunnableMap.Create<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(
                ("sum", sum16)
            );

            // Act
            var results = map.Invoke(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);

            // Assert
            Assert.Single(results);
            Assert.Equal(136, results["sum"]);
        }

        // ==================== Async Tests ====================

        [Fact]
        public async Task InvokeAsync_OneParameter_ExecutesInParallel()
        {
            // Arrange
            var delay1 = 100;
            var delay2 = 75;
            var delay3 = 50;

            var runnable1 = RunnableLambda.Create<int, string>(
                x => $"Result1-{x}",
                async x => {
                    await Task.Delay(delay1);
                    return $"AsyncResult1-{x}";
                });

            var runnable2 = RunnableLambda.Create<int, string>(
                x => $"Result2-{x}",
                async x => {
                    await Task.Delay(delay2);
                    return $"AsyncResult2-{x}";
                });

            var runnable3 = RunnableLambda.Create<int, string>(
                x => $"Result3-{x}",
                async x => {
                    await Task.Delay(delay3);
                    return $"AsyncResult3-{x}";
                });

            var map = RunnableMap.Create<int, string>(
                ("r1", runnable1),
                ("r2", runnable2),
                ("r3", runnable3)
            );

            // Act
            var stopwatch = Stopwatch.StartNew();
            var results = await map.InvokeAsync(42);
            stopwatch.Stop();

            // Assert
            Assert.Equal(3, results.Count);
            Assert.Equal("AsyncResult1-42", results["r1"]);
            Assert.Equal("AsyncResult2-42", results["r2"]);
            Assert.Equal("AsyncResult3-42", results["r3"]);

            // Verify parallel execution (should be closer to max delay, not sum)
            Assert.True(stopwatch.ElapsedMilliseconds < delay1 + delay2 + delay3,
                $"Expected parallel execution (~{delay1}ms), but took {stopwatch.ElapsedMilliseconds}ms");
        }

        [Fact]
        public async Task InvokeAsync_ZeroParameters_ExecutesInParallel()
        {
            // Arrange
            var runnable1 = RunnableLambda.Create(
                () => "Sync1",
                async () => {
                    await Task.Delay(50);
                    return "Async1";
                });

            var runnable2 = RunnableLambda.Create(
                () => "Sync2",
                async () => {
                    await Task.Delay(40);
                    return "Async2";
                });

            var map = RunnableMap.Create<string>(
                ("r1", runnable1),
                ("r2", runnable2)
            );

            // Act
            var results = await map.InvokeAsync();

            // Assert
            Assert.Equal(2, results.Count);
            Assert.Equal("Async1", results["r1"]);
            Assert.Equal("Async2", results["r2"]);
        }

        [Fact]
        public async Task InvokeAsync_TwoParameters_ExecutesInParallel()
        {
            // Arrange
            var runnable1 = RunnableLambda.Create<int, int, int>(
                (a, b) => a + b,
                async (a, b) => {
                    await Task.Delay(30);
                    return a + b;
                });

            var runnable2 = RunnableLambda.Create<int, int, int>(
                (a, b) => a * b,
                async (a, b) => {
                    await Task.Delay(25);
                    return a * b;
                });

            var map = RunnableMap.Create<int, int, int>(
                ("sum", runnable1),
                ("product", runnable2)
            );

            // Act
            var results = await map.InvokeAsync(10, 5);

            // Assert
            Assert.Equal(2, results.Count);
            Assert.Equal(15, results["sum"]);
            Assert.Equal(50, results["product"]);
        }

        // ==================== String Processing Tests ====================

        [Fact]
        public void Create_StringProcessing_ReturnsCorrectResults()
        {
            // Arrange
            var upper = RunnableLambda.Create<string, string>(s => s.ToUpper());
            var lower = RunnableLambda.Create<string, string>(s => s.ToLower());
            var reverse = RunnableLambda.Create<string, string>(s => new string(s.Reverse().ToArray()));

            var map = RunnableMap.Create<string, string>(
                ("uppercase", upper),
                ("lowercase", lower),
                ("reversed", reverse)
            );

            // Act
            var results = map.Invoke("Hello");

            // Assert
            Assert.Equal(3, results.Count);
            Assert.Equal("HELLO", results["uppercase"]);
            Assert.Equal("hello", results["lowercase"]);
            Assert.Equal("olleH", results["reversed"]);
        }

        // ==================== Data Enrichment Tests ====================

        [Fact]
        public void Create_DataEnrichment_CalculatesCorrectly()
        {
            // Arrange
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

            // Act
            var results = priceMap.Invoke(150m);

            // Assert
            Assert.Equal(3, results.Count);
            Assert.Equal(12.00m, results["tax"]);
            Assert.Equal(0m, results["shipping"]);
            Assert.Equal(0m, results["discount"]);
        }

        // ==================== Validation Tests ====================

        [Fact]
        public void Create_Validation_ChecksMultipleConditions()
        {
            // Arrange
            var validateEmail = RunnableLambda.Create<string, bool>(
                email => email.Contains("@") && email.Contains("."));
            var validateLength = RunnableLambda.Create<string, bool>(
                email => email.Length >= 5 && email.Length <= 100);
            var validateDomain = RunnableLambda.Create<string, bool>(
                email => email.EndsWith(".com") || email.EndsWith(".org"));

            var validationMap = RunnableMap.Create<string, bool>(
                ("email_format", validateEmail),
                ("length_check", validateLength),
                ("domain_check", validateDomain)
            );

            // Act
            var results = validationMap.Invoke("alice@example.com");

            // Assert
            Assert.Equal(3, results.Count);
            Assert.True(results["email_format"]);
            Assert.True(results["length_check"]);
            Assert.True(results["domain_check"]);
        }

        // ==================== Error Handling Tests ====================

        [Fact]
        public void Create_WithNullRunnables_ThrowsArgumentException()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => 
                RunnableMap.Create<int, int>(null!));
        }

        [Fact]
        public void Create_WithEmptyRunnables_ThrowsArgumentException()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => 
                RunnableMap.Create<int, int>(Array.Empty<(string, IRunnable<int, int>)>()));
        }

        [Fact]
        public void Invoke_WhenOneRunnableThrows_PropagatesException()
        {
            // Arrange
            var good = RunnableLambda.Create<int, int>(x => x * 2);
            var bad = RunnableLambda.Create<int, int>(x => throw new InvalidOperationException("Test error"));

            var map = RunnableMap.Create<int, int>(
                ("good", good),
                ("bad", bad)
            );

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => map.Invoke(5));
        }

        [Fact]
        public async Task InvokeAsync_WhenOneRunnableThrows_PropagatesException()
        {
            // Arrange
            var good = RunnableLambda.Create<int, int>(
                x => x * 2,
                async x => await Task.FromResult(x * 2));
            var bad = RunnableLambda.Create<int, int>(
                x => x,
                async x => throw new InvalidOperationException("Test async error"));

            var map = RunnableMap.Create<int, int>(
                ("good", good),
                ("bad", bad)
            );

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => map.InvokeAsync(5));
        }

        // ==================== Composition Tests ====================

        [Fact]
        public void Map_CanBeComposedWithOtherExtensions()
        {
            // Arrange
            var square = RunnableLambda.Create<int, int>(x => x * x);
            var cube = RunnableLambda.Create<int, int>(x => x * x * x);

            var composedMap = RunnableMap.Create<int, int>(
                    ("square", square),
                    ("cube", cube)
                )
                .Map(dict => dict.Values.Sum())
                .Map(sum => $"Total: {sum}");

            // Act
            var result = composedMap.Invoke(3);

            // Assert
            Assert.Equal("Total: 36", result); // 9 + 27 = 36
        }

        [Fact]
        public void Map_CanUseTap()
        {
            // Arrange
            var tapped = false;
            var square = RunnableLambda.Create<int, int>(x => x * x);

            var map = RunnableMap.Create<int, int>(
                    ("square", square)
                )
                .Tap(dict => tapped = true);

            // Act
            map.Invoke(5);

            // Assert
            Assert.True(tapped);
        }

        [Fact]
        public void Map_CanBePiped()
        {
            // Arrange
            var square = RunnableLambda.Create<int, int>(x => x * x);
            var cube = RunnableLambda.Create<int, int>(x => x * x * x);

            var map = RunnableMap.Create<int, int>(
                ("square", square),
                ("cube", cube)
            );

            var sumResults = RunnableLambda.Create<Dictionary<string, int>, int>(
                dict => dict.Values.Sum());

            var pipeline = map.Pipe(sumResults);

            // Act
            var result = pipeline.Invoke(3);

            // Assert
            Assert.Equal(36, result); // 9 + 27 = 36
        }

        // ==================== Edge Cases ====================

        [Fact]
        public void Create_WithSingleRunnable_Works()
        {
            // Arrange
            var single = RunnableLambda.Create<int, int>(x => x * 2);
            var map = RunnableMap.Create<int, int>(("only", single));

            // Act
            var results = map.Invoke(5);

            // Assert
            Assert.Single(results);
            Assert.Equal(10, results["only"]);
        }

        [Fact]
        public void Create_WithDuplicateKeys_LastValueWins()
        {
            // Arrange
            var first = RunnableLambda.Create<int, int>(x => x * 2);
            var second = RunnableLambda.Create<int, int>(x => x * 3);

            var map = RunnableMap.Create<int, int>(
                ("duplicate", first),
                ("duplicate", second)
            );

            // Act
            var results = map.Invoke(5);

            // Assert
            Assert.Single(results);
            Assert.Equal(15, results["duplicate"]); // Second one wins
        }

        [Fact]
        public void Create_WithDifferentOutputTypes_Works()
        {
            // Arrange - All outputs must be same type for a single map
            var length = RunnableLambda.Create<string, int>(s => s.Length);
            var wordCount = RunnableLambda.Create<string, int>(s => s.Split(' ').Length);

            var map = RunnableMap.Create<string, int>(
                ("length", length),
                ("words", wordCount)
            );

            // Act
            var results = map.Invoke("Hello World");

            // Assert
            Assert.Equal(2, results.Count);
            Assert.Equal(11, results["length"]);
            Assert.Equal(2, results["words"]);
        }

        // ==================== Performance Tests ====================

        [Fact]
        public async Task InvokeAsync_ManyRunnables_ExecutesInParallel()
        {
            // Arrange
            var runnables = Enumerable.Range(1, 10)
                .Select(i => (
                    key: $"r{i}",
                    runnable: (IRunnable<int, int>)RunnableLambda.Create<int, int>(
                        x => x * i,
                        async x => {
                            await Task.Delay(20);
                            return x * i;
                        })
                ))
                .ToArray();

            var map = RunnableMap.Create<int, int>(runnables);

            // Act
            var stopwatch = Stopwatch.StartNew();
            var results = await map.InvokeAsync(5);
            stopwatch.Stop();

            // Assert
            Assert.Equal(10, results.Count);
            for (int i = 1; i <= 10; i++)
            {
                Assert.Equal(5 * i, results[$"r{i}"]);
            }

            // Should take ~20ms for parallel, not ~200ms for sequential
            Assert.True(stopwatch.ElapsedMilliseconds < 150,
                $"Expected parallel execution, but took {stopwatch.ElapsedMilliseconds}ms");
        }

        // ==================== Integration Tests ====================

        [Fact]
        public async Task RealWorld_APIAggregation_WorksCorrectly()
        {
            // Arrange - Simulate fetching from multiple APIs
            var fetchUser = RunnableLambda.Create<string, string>(
                id => $"User-{id}",
                async id => {
                    await Task.Delay(40);
                    return $"{{userId: '{id}', name: 'User{id}'}}";
                });

            var fetchPosts = RunnableLambda.Create<string, string>(
                id => $"Posts-{id}",
                async id => {
                    await Task.Delay(35);
                    return $"[{{postId: 1}}, {{postId: 2}}]";
                });

            var fetchFollowers = RunnableLambda.Create<string, string>(
                id => $"Followers-{id}",
                async id => {
                    await Task.Delay(30);
                    return $"[User1, User2, User3]";
                });

            var aggregator = RunnableMap.Create<string, string>(
                ("user", fetchUser),
                ("posts", fetchPosts),
                ("followers", fetchFollowers)
            );

            // Act
            var results = await aggregator.InvokeAsync("123");

            // Assert
            Assert.Equal(3, results.Count);
            Assert.Contains("123", results["user"]);
            Assert.Contains("postId", results["posts"]);
            Assert.Contains("User1", results["followers"]);
        }

        [Fact]
        public void RealWorld_FeatureExtraction_WorksCorrectly()
        {
            // Arrange
            var wordCount = RunnableLambda.Create<string, int>(
                text => text.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length);
            var charCount = RunnableLambda.Create<string, int>(text => text.Length);
            var hasNumbers = RunnableLambda.Create<string, bool>(
                text => text.Any(char.IsDigit));

            // Need separate maps for different output types
            var intFeatures = RunnableMap.Create<string, int>(
                ("word_count", wordCount),
                ("char_count", charCount)
            );

            // Act
            var text = "Hello world! This is a test.";
            var features = intFeatures.Invoke(text);

            // Assert
            Assert.Equal(2, features.Count);
            Assert.Equal(6, features["word_count"]);
            Assert.Equal(28, features["char_count"]);
        }

        [Fact]
        public async Task RealWorld_MultiModelAI_SimulatesCorrectly()
        {
            // Arrange - Simulate multiple AI models
            var model1 = RunnableLambda.Create<string, string>(
                query => $"Model1: {query}",
                async query => {
                    await Task.Delay(30);
                    return $"Model1 response to: {query}";
                });

            var model2 = RunnableLambda.Create<string, string>(
                query => $"Model2: {query}",
                async query => {
                    await Task.Delay(25);
                    return $"Model2 response to: {query}";
                });

            var model3 = RunnableLambda.Create<string, string>(
                query => $"Model3: {query}",
                async query => {
                    await Task.Delay(20);
                    return $"Model3 response to: {query}";
                });

            var multiModel = RunnableMap.Create<string, string>(
                ("gpt", model1),
                ("claude", model2),
                ("llama", model3)
            );

            // Act
            var responses = await multiModel.InvokeAsync("What is AI?");

            // Assert
            Assert.Equal(3, responses.Count);
            Assert.All(responses.Values, response => Assert.Contains("What is AI?", response));
        }
    }
}
