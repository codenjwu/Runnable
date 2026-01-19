using Runnable;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Runnable.Tests
{
    /// <summary>
    /// Unit tests for TapAsync extension functionality (0-16 parameters)
    /// </summary>
    public class RunnableTapAsyncTests
    {
        // ==================== Basic Functionality Tests ====================

        [Fact]
        public async Task TapAsync_ZeroParameters_ExecutesAsyncSideEffect()
        {
            // Arrange
            var sideEffectExecuted = false;
            var runnable = RunnableLambda.Create(() => 42);
            
            var tapped = runnable.TapAsync(async result => {
                await Task.Delay(10);
                sideEffectExecuted = true;
            });

            // Act
            var result = await tapped.InvokeAsync();

            // Assert
            Assert.True(sideEffectExecuted);
            Assert.Equal(42, result);
        }

        [Fact]
        public async Task TapAsync_OneParameter_ExecutesAsyncSideEffect()
        {
            // Arrange
            var sideEffectValue = 0;
            var runnable = RunnableLambda.Create<int, int>(x => x * 2);
            
            var tapped = runnable.TapAsync(async result => {
                await Task.Delay(10);
                sideEffectValue = result;
            });

            // Act
            var result = await tapped.InvokeAsync(5);

            // Assert
            Assert.Equal(10, sideEffectValue);
            Assert.Equal(10, result);
        }

        [Fact]
        public async Task TapAsync_TwoParameters_ExecutesAsyncSideEffect()
        {
            // Arrange
            var logged = "";
            var runnable = RunnableLambda.Create<int, int, int>((a, b) => a + b);
            
            var tapped = runnable.TapAsync(async sum => {
                await Task.Delay(10);
                logged = $"Sum: {sum}";
            });

            // Act
            var result = await tapped.InvokeAsync(10, 5);

            // Assert
            Assert.Equal("Sum: 15", logged);
            Assert.Equal(15, result);
        }

        [Fact]
        public async Task TapAsync_SixteenParameters_ExecutesAsyncSideEffect()
        {
            // Arrange
            var sideEffectValue = 0;
            var runnable = RunnableLambda.Create<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16) =>
                    a1 + a2 + a3 + a4 + a5 + a6 + a7 + a8 + a9 + a10 + a11 + a12 + a13 + a14 + a15 + a16);
            
            var tapped = runnable.TapAsync(async sum => {
                await Task.Delay(10);
                sideEffectValue = sum;
            });

            // Act
            var result = await tapped.InvokeAsync(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);

            // Assert
            Assert.Equal(136, sideEffectValue);
            Assert.Equal(136, result);
        }

        // ==================== Does Not Modify Output ====================

        [Fact]
        public async Task TapAsync_DoesNotModifyOutput()
        {
            // Arrange
            var runnable = RunnableLambda.Create<int, string>(x => $"Value: {x}");
            
            var tapped = runnable.TapAsync(async _ => {
                await Task.Delay(10);
                // Side effect doesn't change result
            });

            // Act
            var result = await tapped.InvokeAsync(42);

            // Assert
            Assert.Equal("Value: 42", result);
        }

        // ==================== Sync Invocation Tests ====================

        [Fact]
        public void TapAsync_CanBeInvokedSynchronously()
        {
            // Arrange
            var sideEffectExecuted = false;
            var runnable = RunnableLambda.Create<int, int>(x => x * 2);
            
            var tapped = runnable.TapAsync(async result => {
                await Task.Delay(10);
                sideEffectExecuted = true;
            });

            // Act
            var result = tapped.Invoke(5);

            // Assert
            Assert.True(sideEffectExecuted);
            Assert.Equal(10, result);
        }

        // ==================== Real-World Async Scenarios ====================

        [Fact]
        public async Task TapAsync_WithAsyncLogging_WorksCorrectly()
        {
            // Arrange - Simulate async logging
            var logEntries = new System.Collections.Generic.List<string>();
            
            async Task LogAsync(string message)
            {
                await Task.Delay(10);  // Simulate I/O
                logEntries.Add(message);
            }

            var calculate = RunnableLambda.Create<int, int, int>((a, b) => a + b);
            var logged = calculate.TapAsync(async sum => await LogAsync($"Result: {sum}"));

            // Act
            var result = await logged.InvokeAsync(10, 5);

            // Assert
            Assert.Equal(15, result);
            Assert.Single(logEntries);
            Assert.Equal("Result: 15", logEntries[0]);
        }

        [Fact]
        public async Task TapAsync_WithDatabaseInsert_WorksCorrectly()
        {
            // Arrange - Simulate database insert
            var savedValues = new System.Collections.Generic.List<int>();
            
            async Task SaveToDatabaseAsync(int value)
            {
                await Task.Delay(15);  // Simulate DB write
                savedValues.Add(value);
            }

            var process = RunnableLambda.Create<string, int>(s => s.Length);
            var withSave = process.TapAsync(async length => await SaveToDatabaseAsync(length));

            // Act
            var result = await withSave.InvokeAsync("Hello");

            // Assert
            Assert.Equal(5, result);
            Assert.Single(savedValues);
            Assert.Equal(5, savedValues[0]);
        }

        [Fact]
        public async Task TapAsync_WithMessageQueue_WorksCorrectly()
        {
            // Arrange - Simulate message queue publish
            var publishedMessages = new System.Collections.Generic.List<string>();
            
            async Task PublishAsync(string message)
            {
                await Task.Delay(12);  // Simulate network call
                publishedMessages.Add(message);
            }

            var generate = RunnableLambda.Create<int, string>(id => $"User-{id}");
            var withPublish = generate.TapAsync(async msg => await PublishAsync(msg));

            // Act
            var result = await withPublish.InvokeAsync(123);

            // Assert
            Assert.Equal("User-123", result);
            Assert.Single(publishedMessages);
            Assert.Equal("User-123", publishedMessages[0]);
        }

        // ==================== Chaining Tests ====================

        [Fact]
        public async Task TapAsync_CanBeChained()
        {
            // Arrange
            var log1 = "";
            var log2 = "";
            
            var runnable = RunnableLambda.Create<int, int>(x => x * 2);
            var pipeline = runnable
                .TapAsync(async x => {
                    await Task.Delay(10);
                    log1 = $"First: {x}";
                })
                .TapAsync(async x => {
                    await Task.Delay(10);
                    log2 = $"Second: {x}";
                });

            // Act
            var result = await pipeline.InvokeAsync(5);

            // Assert
            Assert.Equal(10, result);
            Assert.Equal("First: 10", log1);
            Assert.Equal("Second: 10", log2);
        }

        [Fact]
        public async Task TapAsync_CanFollowTap()
        {
            // Arrange
            var syncLog = "";
            var asyncLog = "";
            
            var runnable = RunnableLambda.Create<int, int>(x => x * 2);
            var pipeline = runnable
                .Tap(x => syncLog = $"Sync: {x}")
                .TapAsync(async x => {
                    await Task.Delay(10);
                    asyncLog = $"Async: {x}";
                });

            // Act
            var result = await pipeline.InvokeAsync(5);

            // Assert
            Assert.Equal(10, result);
            Assert.Equal("Sync: 10", syncLog);
            Assert.Equal("Async: 10", asyncLog);
        }

        [Fact]
        public async Task TapAsync_CanPrecedeTap()
        {
            // Arrange
            var asyncLog = "";
            var syncLog = "";
            
            var runnable = RunnableLambda.Create<int, int>(x => x * 2);
            var pipeline = runnable
                .TapAsync(async x => {
                    await Task.Delay(10);
                    asyncLog = $"Async: {x}";
                })
                .Tap(x => syncLog = $"Sync: {x}");

            // Act
            var result = await pipeline.InvokeAsync(5);

            // Assert
            Assert.Equal(10, result);
            Assert.Equal("Async: 10", asyncLog);
            Assert.Equal("Sync: 10", syncLog);
        }

        // ==================== Composition Tests ====================

        [Fact]
        public async Task TapAsync_WithMap_WorksCorrectly()
        {
            // Arrange
            var logged = 0;
            var runnable = RunnableLambda.Create<int, int>(x => x * 2);
            var pipeline = runnable
                .TapAsync(async x => {
                    await Task.Delay(10);
                    logged = x;
                })
                .Map(x => x.ToString());

            // Act
            var result = await pipeline.InvokeAsync(5);

            // Assert
            Assert.Equal("10", result);
            Assert.Equal(10, logged);
        }

        [Fact]
        public async Task TapAsync_WithFilter_WorksCorrectly()
        {
            // Arrange
            var logged = new System.Collections.Generic.List<int>();
            var runnable = RunnableLambda.Create<int, int>(x => x * 2);
            var pipeline = runnable
                .Filter(x => x >= 5, -1)
                .TapAsync(async x => {
                    await Task.Delay(10);
                    logged.Add(x);
                });

            // Act & Assert
            Assert.Equal(10, await pipeline.InvokeAsync(5));  // Passes filter
            Assert.Equal(-1, await pipeline.InvokeAsync(2));  // Fails filter

            Assert.Equal(2, logged.Count);
            Assert.Contains(10, logged);
            Assert.Contains(-1, logged);
        }

        [Fact]
        public async Task TapAsync_WithPipe_WorksCorrectly()
        {
            // Arrange
            var logged = "";
            var double_ = RunnableLambda.Create<int, int>(x => x * 2);
            var toString = RunnableLambda.Create<int, string>(x => x.ToString());

            var pipeline = double_
                .TapAsync(async x => {
                    await Task.Delay(10);
                    logged = $"After double: {x}";
                })
                .Pipe(toString);

            // Act
            var result = await pipeline.InvokeAsync(5);

            // Assert
            Assert.Equal("10", result);
            Assert.Equal("After double: 10", logged);
        }

        // ==================== Error Handling Tests ====================

        [Fact]
        public async Task TapAsync_WhenSideEffectThrows_PropagatesException()
        {
            // Arrange
            var runnable = RunnableLambda.Create<int, int>(x => x * 2);
            var tapped = runnable.TapAsync<int, int>(async x => {
                await Task.Delay(10);
                throw new InvalidOperationException("Side effect error");
            });

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => tapped.InvokeAsync(5));
        }

        [Fact]
        public async Task TapAsync_WhenRunnableThrows_PropagatesException()
        {
            // Arrange
            var runnable = RunnableLambda.Create<int, int>(x => throw new ArgumentException("Runnable error"));
            var tapped = runnable.TapAsync(async x => await Task.Delay(10));

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => tapped.InvokeAsync(5));
        }

        // ==================== Edge Cases ====================

        [Fact]
        public async Task TapAsync_WithMultipleSideEffects_AllExecute()
        {
            // Arrange
            var counter = 0;
            var runnable = RunnableLambda.Create<int, int>(x => x * 2);
            
            var tapped = runnable
                .TapAsync(async x => { await Task.Delay(5); counter++; })
                .TapAsync(async x => { await Task.Delay(5); counter++; })
                .TapAsync(async x => { await Task.Delay(5); counter++; });

            // Act
            var result = await tapped.InvokeAsync(5);

            // Assert
            Assert.Equal(10, result);
            Assert.Equal(3, counter);
        }

        [Fact]
        public async Task TapAsync_WithImmediateTask_WorksCorrectly()
        {
            // Arrange
            var executed = false;
            var runnable = RunnableLambda.Create<int, int>(x => x * 2);
            var tapped = runnable.TapAsync(x => {
                executed = true;
                return Task.CompletedTask;
            });

            // Act
            var result = await tapped.InvokeAsync(5);

            // Assert
            Assert.True(executed);
            Assert.Equal(10, result);
        }

        // ==================== Performance Tests ====================

        [Fact]
        public async Task TapAsync_ExecutesAsynchronously()
        {
            // Arrange
            var runnable = RunnableLambda.Create<int, int>(x => x * 2);
            var delay = 50;
            var tapped = runnable.TapAsync(async x => await Task.Delay(delay));

            // Act
            var startTime = DateTime.Now;
            var result = await tapped.InvokeAsync(5);
            var elapsed = DateTime.Now - startTime;

            // Assert
            Assert.Equal(10, result);
            Assert.True(elapsed.TotalMilliseconds >= delay - 10, 
                $"Should have taken at least {delay}ms, took {elapsed.TotalMilliseconds}ms");
        }

        // ==================== Real-World Pipeline ====================

        [Fact]
        public async Task RealWorld_LoggingPipeline_WorksCorrectly()
        {
            // Arrange - Simulate a logging pipeline
            var logs = new System.Collections.Generic.List<string>();
            
            async Task LogAsync(string stage, object value)
            {
                await Task.Delay(10);
                logs.Add($"[{stage}] {value}");
            }

            var process = RunnableLambda.Create<string, int>(s => s.Length);
            
            var pipeline = process
                .TapAsync(async len => await LogAsync("AfterLength", len))
                .Map(len => len * 2)
                .TapAsync(async doubled => await LogAsync("AfterDouble", doubled))
                .Map(x => x + 10)
                .TapAsync(async final => await LogAsync("Final", final));

            // Act
            var result = await pipeline.InvokeAsync("Hello");

            // Assert
            Assert.Equal(20, result);  // (5 * 2) + 10
            Assert.Equal(3, logs.Count);
            Assert.Contains("[AfterLength] 5", logs);
            Assert.Contains("[AfterDouble] 10", logs);
            Assert.Contains("[Final] 20", logs);
        }
    }
}
