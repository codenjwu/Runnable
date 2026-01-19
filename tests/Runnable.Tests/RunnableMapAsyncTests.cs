using Runnable;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Runnable.Tests
{
    /// <summary>
    /// Unit tests for MapAsync extension functionality (0-16 parameters)
    /// </summary>
    public class RunnableMapAsyncTests
    {
        // ==================== Basic Functionality Tests ====================

        [Fact]
        public async Task MapAsync_ZeroParameters_TransformsOutputAsync()
        {
            // Arrange
            var runnable = RunnableLambda.Create(() => 42);
            var mapped = runnable.MapAsync(async x => {
                await Task.Delay(10);
                return x.ToString();
            });

            // Act
            var result = await mapped.InvokeAsync();

            // Assert
            Assert.Equal("42", result);
        }

        [Fact]
        public async Task MapAsync_OneParameter_TransformsOutputAsync()
        {
            // Arrange
            var double_ = RunnableLambda.Create<int, int>(x => x * 2);
            var mapped = double_.MapAsync(async x => {
                await Task.Delay(10);
                return x.ToString();
            });

            // Act
            var result = await mapped.InvokeAsync(5);

            // Assert
            Assert.Equal("10", result);
        }

        [Fact]
        public async Task MapAsync_TwoParameters_TransformsOutputAsync()
        {
            // Arrange
            var sum = RunnableLambda.Create<int, int, int>((a, b) => a + b);
            var mapped = sum.MapAsync(async x => {
                await Task.Delay(10);
                return $"Sum: {x}";
            });

            // Act
            var result = await mapped.InvokeAsync(10, 5);

            // Assert
            Assert.Equal("Sum: 15", result);
        }

        [Fact]
        public async Task MapAsync_ThreeParameters_TransformsOutputAsync()
        {
            // Arrange
            var sum = RunnableLambda.Create<int, int, int, int>((a, b, c) => a + b + c);
            var mapped = sum.MapAsync(async x => {
                await Task.Delay(10);
                return $"Total: {x}";
            });

            // Act
            var result = await mapped.InvokeAsync(1, 2, 3);

            // Assert
            Assert.Equal("Total: 6", result);
        }

        [Fact]
        public async Task MapAsync_SixteenParameters_TransformsOutputAsync()
        {
            // Arrange
            var sum = RunnableLambda.Create<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16) =>
                    a1 + a2 + a3 + a4 + a5 + a6 + a7 + a8 + a9 + a10 + a11 + a12 + a13 + a14 + a15 + a16);

            var mapped = sum.MapAsync(async total => {
                await Task.Delay(10);
                return $"Sum of all: {total}";
            });

            // Act
            var result = await mapped.InvokeAsync(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);

            // Assert
            Assert.Equal("Sum of all: 136", result);
        }

        // ==================== Sync Invocation Tests ====================

        [Fact]
        public void MapAsync_CanBeInvokedSynchronously()
        {
            // Arrange
            var runnable = RunnableLambda.Create<int, int>(x => x * 2);
            var mapped = runnable.MapAsync(async x => {
                await Task.Delay(10);
                return x.ToString();
            });

            // Act
            var result = mapped.Invoke(5);

            // Assert
            Assert.Equal("10", result);
        }

        // ==================== Real-World Async Scenarios ====================

        [Fact]
        public async Task MapAsync_WithApiCall_WorksCorrectly()
        {
            // Arrange - Simulate API enrichment
            async Task<string> EnrichWithApiAsync(int userId)
            {
                await Task.Delay(20);  // Simulate API call
                return $"User-{userId}-enriched";
            }

            var getUserId = RunnableLambda.Create<string, int>(username => username.GetHashCode());
            var enriched = getUserId.MapAsync(async id => await EnrichWithApiAsync(id));

            // Act
            var result = await enriched.InvokeAsync("alice");

            // Assert
            Assert.Contains("enriched", result);
        }

        [Fact]
        public async Task MapAsync_WithDatabaseLookup_WorksCorrectly()
        {
            // Arrange - Simulate database lookup
            async Task<string> LookupInDatabaseAsync(int id)
            {
                await Task.Delay(20);  // Simulate DB query
                return $"Record-{id}";
            }

            var calculate = RunnableLambda.Create<int, int, int>((a, b) => a + b);
            var withLookup = calculate.MapAsync(async sum => await LookupInDatabaseAsync(sum));

            // Act
            var result = await withLookup.InvokeAsync(10, 5);

            // Assert
            Assert.Equal("Record-15", result);
        }

        [Fact]
        public async Task MapAsync_WithFileIO_WorksCorrectly()
        {
            // Arrange - Simulate file I/O
            async Task<string> WriteToFileAsync(string data)
            {
                await Task.Delay(15);  // Simulate file write
                return $"Saved: {data}";
            }

            var process = RunnableLambda.Create<string, string>(s => s.ToUpper());
            var withSave = process.MapAsync(async data => await WriteToFileAsync(data));

            // Act
            var result = await withSave.InvokeAsync("hello");

            // Assert
            Assert.Equal("Saved: HELLO", result);
        }

        // ==================== Chaining Tests ====================

        [Fact]
        public async Task MapAsync_CanBeChained()
        {
            // Arrange
            var runnable = RunnableLambda.Create<int, int>(x => x * 2);
            var pipeline = runnable
                .MapAsync(async x => {
                    await Task.Delay(10);
                    return x.ToString();
                })
                .MapAsync(async s => {
                    await Task.Delay(10);
                    return $"Result: {s}";
                });

            // Act
            var result = await pipeline.InvokeAsync(5);

            // Assert
            Assert.Equal("Result: 10", result);
        }

        [Fact]
        public async Task MapAsync_CanFollowMap()
        {
            // Arrange
            var runnable = RunnableLambda.Create<int, int>(x => x * 2);
            var pipeline = runnable
                .Map(x => x + 1)
                .MapAsync(async x => {
                    await Task.Delay(10);
                    return x.ToString();
                });

            // Act
            var result = await pipeline.InvokeAsync(5);

            // Assert
            Assert.Equal("11", result);  // (5 * 2) + 1 = 11
        }

        [Fact]
        public async Task MapAsync_CanPrecedeMap()
        {
            // Arrange
            var runnable = RunnableLambda.Create<int, int>(x => x * 2);
            var pipeline = runnable
                .MapAsync(async x => {
                    await Task.Delay(10);
                    return x + 1;
                })
                .Map(x => x.ToString());

            // Act
            var result = await pipeline.InvokeAsync(5);

            // Assert
            Assert.Equal("11", result);  // (5 * 2) + 1 = 11
        }

        // ==================== Composition Tests ====================

        [Fact]
        public async Task MapAsync_WithTap_WorksCorrectly()
        {
            // Arrange
            var tapped = false;
            var runnable = RunnableLambda.Create<int, int>(x => x * 2);
            var pipeline = runnable
                .MapAsync(async x => {
                    await Task.Delay(10);
                    return x.ToString();
                })
                .Tap(s => tapped = true);

            // Act
            await pipeline.InvokeAsync(5);

            // Assert
            Assert.True(tapped);
        }

        [Fact]
        public async Task MapAsync_WithFilter_WorksCorrectly()
        {
            // Arrange
            var runnable = RunnableLambda.Create<int, int>(x => x * 2);
            var pipeline = runnable
                .Filter(x => x >= 5, -1)  // Filter on input: keep if input >= 5
                .MapAsync(async x => {
                    await Task.Delay(10);
                    return x;
                });

            // Act & Assert
            Assert.Equal(10, await pipeline.InvokeAsync(5));   // Input 5 passes filter, 5*2=10
            Assert.Equal(-1, await pipeline.InvokeAsync(2));   // Input 2 fails filter, returns -1 default
        }

        [Fact]
        public async Task MapAsync_WithPipe_WorksCorrectly()
        {
            // Arrange
            var double_ = RunnableLambda.Create<int, int>(x => x * 2);
            var toString = RunnableLambda.Create<int, string>(x => x.ToString());

            var pipeline = double_
                .MapAsync(async x => {
                    await Task.Delay(10);
                    return x + 1;
                })
                .Pipe(toString);

            // Act
            var result = await pipeline.InvokeAsync(5);

            // Assert
            Assert.Equal("11", result);
        }

        // ==================== Type Transformation Tests ====================

        [Fact]
        public async Task MapAsync_IntToString_WorksCorrectly()
        {
            // Arrange
            var runnable = RunnableLambda.Create<int, int>(x => x * 2);
            var mapped = runnable.MapAsync(async x => {
                await Task.Delay(10);
                return x.ToString();
            });

            // Act
            var result = await mapped.InvokeAsync(42);

            // Assert
            Assert.IsType<string>(result);
            Assert.Equal("84", result);
        }

        [Fact]
        public async Task MapAsync_StringToInt_WorksCorrectly()
        {
            // Arrange
            var runnable = RunnableLambda.Create<string, string>(s => s.ToUpper());
            var mapped = runnable.MapAsync(async s => {
                await Task.Delay(10);
                return s.Length;
            });

            // Act
            var result = await mapped.InvokeAsync("hello");

            // Assert
            Assert.IsType<int>(result);
            Assert.Equal(5, result);
        }

        [Fact]
        public async Task MapAsync_ToComplexType_WorksCorrectly()
        {
            // Arrange
            var runnable = RunnableLambda.Create<int, int>(x => x * 2);
            var mapped = runnable.MapAsync(async x => {
                await Task.Delay(10);
                return new { Value = x, IsEven = x % 2 == 0 };
            });

            // Act
            var result = await mapped.InvokeAsync(5);

            // Assert
            Assert.Equal(10, result.Value);
            Assert.True(result.IsEven);
        }

        // ==================== Error Handling Tests ====================

        [Fact]
        public async Task MapAsync_WhenMapperThrows_PropagatesException()
        {
            // Arrange
            var runnable = RunnableLambda.Create<int, int>(x => x * 2);
            var mapped = runnable.MapAsync<int, int, string>(async x => {
                await Task.Delay(10);
                throw new InvalidOperationException("Mapper error");
            });

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => mapped.InvokeAsync(5));
        }

        [Fact]
        public async Task MapAsync_WhenRunnableThrows_PropagatesException()
        {
            // Arrange
            var runnable = RunnableLambda.Create<int, int>(x => throw new ArgumentException("Runnable error"));
            var mapped = runnable.MapAsync(async x => {
                await Task.Delay(10);
                return x.ToString();
            });

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => mapped.InvokeAsync(5));
        }

        // ==================== Edge Cases ====================

        [Fact]
        public async Task MapAsync_WithNullReturn_WorksCorrectly()
        {
            // Arrange
            var runnable = RunnableLambda.Create<int, int>(x => x * 2);
            var mapped = runnable.MapAsync(async x => {
                await Task.Delay(10);
                return (string)null;
            });

            // Act
            var result = await mapped.InvokeAsync(5);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task MapAsync_WithImmediateTask_WorksCorrectly()
        {
            // Arrange
            var runnable = RunnableLambda.Create<int, int>(x => x * 2);
            var mapped = runnable.MapAsync(x => Task.FromResult(x.ToString()));

            // Act
            var result = await mapped.InvokeAsync(5);

            // Assert
            Assert.Equal("10", result);
        }

        [Fact]
        public async Task MapAsync_MultipleParameters_WithComplexTransformation()
        {
            // Arrange
            var calculate = RunnableLambda.Create<int, int, int, int, int>(
                (a, b, c, d) => a + b + c + d);

            var enriched = calculate.MapAsync(async sum => {
                await Task.Delay(10);
                return new {
                    Sum = sum,
                    Average = sum / 4.0,
                    Category = sum > 20 ? "High" : "Low"
                };
            });

            // Act
            var result = await enriched.InvokeAsync(10, 5, 3, 2);

            // Assert
            Assert.Equal(20, result.Sum);
            Assert.Equal(5.0, result.Average);
            Assert.Equal("Low", result.Category);
        }

        // ==================== Performance Tests ====================

        [Fact]
        public async Task MapAsync_ExecutesAsynchronously()
        {
            // Arrange
            var runnable = RunnableLambda.Create<int, int>(x => x * 2);
            var delay = 50;
            var mapped = runnable.MapAsync(async x => {
                await Task.Delay(delay);
                return x.ToString();
            });

            // Act
            var startTime = DateTime.Now;
            var result = await mapped.InvokeAsync(5);
            var elapsed = DateTime.Now - startTime;

            // Assert
            Assert.Equal("10", result);
            Assert.True(elapsed.TotalMilliseconds >= delay - 10, 
                $"Should have taken at least {delay}ms, took {elapsed.TotalMilliseconds}ms");
        }

        // ==================== Real-World Pipeline ====================

        [Fact]
        public async Task RealWorld_DataProcessingPipeline_WorksCorrectly()
        {
            // Arrange - Simulate a data processing pipeline
            async Task<string> FetchFromDatabaseAsync(int id)
            {
                await Task.Delay(20);
                return $"Data-{id}";
            }

            async Task<string> EnrichWithExternalApiAsync(string data)
            {
                await Task.Delay(15);
                return $"{data}-Enriched";
            }

            var computeId = RunnableLambda.Create<string, int>(username => username.GetHashCode() % 1000);
            
            var pipeline = computeId
                .MapAsync(async id => await FetchFromDatabaseAsync(id))
                .MapAsync(async data => await EnrichWithExternalApiAsync(data))
                .Map(enrichedData => enrichedData.ToUpper());

            // Act
            var result = await pipeline.InvokeAsync("alice");

            // Assert
            Assert.Contains("DATA-", result);
            Assert.Contains("ENRICHED", result);
        }
    }
}
