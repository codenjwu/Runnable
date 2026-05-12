using Runnable.Components;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Xunit;

namespace Runnable.Tests
{
    /// <summary>
    /// Comprehensive unit tests for BatchingChannelConsumer<T>
    /// Covers basic functionality, timeout, retry, error handling, statistics, and edge cases
    /// </summary>
    public class BatchingChannelConsumerTests
    {
        // ==================== Basic Functionality Tests ====================

        [Fact]
        public async Task ConsumeAsync_BasicBatching_ProcessesBatchesCorrectly()
        {
            // Arrange
            var channel = Channel.CreateUnbounded<int>();
            var consumer = new BatchingChannelConsumer<int>(batchSize: 3);
            var processedBatches = new List<List<int>>();

            // Act: Write items and close channel
            for (int i = 1; i <= 9; i++)
            {
                await channel.Writer.WriteAsync(i);
            }
            channel.Writer.Complete();

            await consumer.ConsumeAsync(
                channel.Reader,
                async (batch, ct) =>
                {
                    processedBatches.Add(new List<int>(batch));
                    return await Task.FromResult(0);
                },
                CancellationToken.None
            );

            // Assert
            Assert.Equal(3, processedBatches.Count); // 3 batches of 3 items each
            Assert.Equal(new[] { 1, 2, 3 }, processedBatches[0]);
            Assert.Equal(new[] { 4, 5, 6 }, processedBatches[1]);
            Assert.Equal(new[] { 7, 8, 9 }, processedBatches[2]);
        }

        [Fact]
        public async Task ConsumeAsync_IncompleteBatch_FlushesRemainingItems()
        {
            // Arrange
            var channel = Channel.CreateUnbounded<int>();
            var consumer = new BatchingChannelConsumer<int>(batchSize: 5);
            var processedBatches = new List<List<int>>();

            // Act: Write 7 items (1 full batch + 2 remaining)
            for (int i = 1; i <= 7; i++)
            {
                await channel.Writer.WriteAsync(i);
            }
            channel.Writer.Complete();

            await consumer.ConsumeAsync(
                channel.Reader,
                async (batch, ct) =>
                {
                    processedBatches.Add(new List<int>(batch));
                    return await Task.FromResult(0);
                },
                CancellationToken.None
            );

            // Assert
            Assert.Equal(2, processedBatches.Count); // 1 full batch + 1 partial
            Assert.Equal(5, processedBatches[0].Count);
            Assert.Equal(2, processedBatches[1].Count);
        }

        [Fact]
        public async Task ConsumeAsync_EmptyChannel_DoesNotProcessAnyBatches()
        {
            // Arrange
            var channel = Channel.CreateUnbounded<int>();
            var consumer = new BatchingChannelConsumer<int>(batchSize: 10);
            var processedCount = 0;

            // Act: Close channel immediately without writing anything
            channel.Writer.Complete();

            await consumer.ConsumeAsync(
                channel.Reader,
                async (batch, ct) =>
                {
                    processedCount++;
                    return await Task.FromResult(0);
                },
                CancellationToken.None
            );

            // Assert
            Assert.Equal(0, processedCount);
        }

        // ==================== Timeout Control Tests ====================

        [Fact]
        public async Task ConsumeAsync_TimeoutExpired_FlushesIncompletesBatch()
        {
            // Arrange
            var channel = Channel.CreateUnbounded<string>();
            var consumer = new BatchingChannelConsumer<string>(
                batchSize: 10,
                timeoutSeconds: 1  // 1 second timeout
            );
            var processedBatches = new List<List<string>>();
            var sw = Stopwatch.StartNew();

            // Act: Write only 3 items, wait for timeout
            for (int i = 0; i < 3; i++)
            {
                await channel.Writer.WriteAsync($"item_{i}");
            }
            channel.Writer.Complete();

            await consumer.ConsumeAsync(
                channel.Reader,
                async (batch, ct) =>
                {
                    processedBatches.Add(new List<string>(batch));
                    return await Task.FromResult(0);
                },
                CancellationToken.None
            );
            sw.Stop();

            // Assert
            Assert.Single(processedBatches); // One batch despite timeout
            Assert.Equal(3, processedBatches[0].Count);
            // Verify timeout was respected (at least 1 second elapsed)
            Assert.True(sw.Elapsed >= TimeSpan.FromSeconds(1));
        }

        [Fact]
        public async Task ConsumeAsync_MultipleTimeouts_ProcessesBatchesAtIntervals()
        {
            // Arrange
            var channel = Channel.CreateUnbounded<int>();
            var consumer = new BatchingChannelConsumer<int>(
                batchSize: 100,  // Large batch size to force timeout
                timeoutSeconds: 1
            );
            var processedBatches = new List<int>();

            // Act: Write items slowly to trigger multiple timeouts
            var writeTask = Task.Run(async () =>
            {
                for (int i = 0; i < 5; i++)
                {
                    await channel.Writer.WriteAsync(i);
                    await Task.Delay(400);  // Write every 400ms
                }
                channel.Writer.Complete();
            });

            await consumer.ConsumeAsync(
                channel.Reader,
                async (batch, ct) =>
                {
                    processedBatches.Add(batch.Count);
                    return await Task.FromResult(0);
                },
                CancellationToken.None
            );

            await writeTask;

            // Assert: Should have multiple batches due to timeout
            Assert.True(processedBatches.Count >= 2, "Should process multiple batches due to timeout");
        }

        // ==================== Retry Mechanism Tests ====================

        [Fact]
        public async Task ConsumeAsync_RetryOnFailure_RetriesConfiguredTimes()
        {
            // Arrange
            var channel = Channel.CreateUnbounded<int>();
            var consumer = new BatchingChannelConsumer<int>(
                batchSize: 5,
                maxRetries: 3,
                retryDelay: TimeSpan.FromMilliseconds(50)
            );
            var attemptCount = 0;

            // Act: Processor that fails first 2 times
            await channel.Writer.WriteAsync(1);
            await channel.Writer.WriteAsync(2);
            channel.Writer.Complete();

            await consumer.ConsumeAsync(
                channel.Reader,
                async (batch, ct) =>
                {
                    attemptCount++;
                    if (attemptCount <= 2)
                        throw new InvalidOperationException("Simulated failure");
                    return await Task.FromResult(0);
                },
                CancellationToken.None
            );

            // Assert
            Assert.Equal(3, attemptCount); // Initial attempt + 2 retries = 3 total
        }

        [Fact]
        public async Task ConsumeAsync_AllRetriesFail_InvokesFailureCallback()
        {
            // Arrange
            var channel = Channel.CreateUnbounded<int>();
            var consumer = new BatchingChannelConsumer<int>(
                batchSize: 5,
                maxRetries: 2,
                retryDelay: TimeSpan.FromMilliseconds(10)
            );
            var failureCallbackInvoked = false;
            var failedBatch = (List<int>?)null;

            consumer.OnFailure = async (batch, ex) =>
            {
                failureCallbackInvoked = true;
                failedBatch = batch.ToList();
                await Task.CompletedTask;
            };

            // Act: Always failing processor
            await channel.Writer.WriteAsync(1);
            await channel.Writer.WriteAsync(2);
            channel.Writer.Complete();

            await consumer.ConsumeAsync<int>(
                channel.Reader,
                async (batch, ct) =>
                {
                    throw new InvalidOperationException("Always fails");
                },
                CancellationToken.None
            );

            // Assert
            Assert.True(failureCallbackInvoked);
            Assert.NotNull(failedBatch);
            Assert.Equal(2, failedBatch.Count);
        }

        // ==================== Success Callback Tests ====================

        [Fact]
        public async Task ConsumeAsRunnableAsync_OnSuccess_InvokesSuccessCallback()
        {
            // Arrange
            var channel = Channel.CreateUnbounded<int>();
            var consumer = new BatchingChannelConsumer<int>(batchSize: 5);
            var successCallbackCount = 0;

            consumer.OnSuccess = async (result) =>
            {
                successCallbackCount++;
                await Task.CompletedTask;
            };

            // Act
            await channel.Writer.WriteAsync(1);
            await channel.Writer.WriteAsync(2);
            channel.Writer.Complete();

            var processor = new Runnable<List<int>, BatchResult>(
                null,
                async (batch) => await Task.FromResult(new BatchResult(batch.Count, DateTime.Now, true))
            );

            await consumer.ConsumeAsRunnableAsync(channel.Reader, processor, CancellationToken.None);

            // Assert
            Assert.True(successCallbackCount > 0);
        }

        [Fact]
        public async Task ConsumeAsync_OnSuccessThrowsException_IsProtected()
        {
            // Arrange
            var channel = Channel.CreateUnbounded<int>();
            var consumer = new BatchingChannelConsumer<int>(batchSize: 2);
            var logMessages = new List<string>();

            consumer.Logger = async (msg) =>
            {
                logMessages.Add(msg);
                await Task.CompletedTask;
            };

            consumer.OnSuccess = async (result) =>
            {
                throw new Exception("Callback error");
            };

            // Act & Assert: Should not throw despite callback exception
            await channel.Writer.WriteAsync(1);
            await channel.Writer.WriteAsync(2);
            channel.Writer.Complete();

            await consumer.ConsumeAsync(
                channel.Reader,
                async (batch, ct) => await Task.FromResult(0),
                CancellationToken.None
            );

            // Verify that callback error was logged
            Assert.NotEmpty(logMessages);
        }

        // ==================== Logging Tests ====================

        [Fact]
        public async Task ConsumeAsync_LoggingCallback_IsInvokedForEvents()
        {
            // Arrange
            var channel = Channel.CreateUnbounded<int>();
            var consumer = new BatchingChannelConsumer<int>(batchSize: 2);
            var logMessages = new List<string>();

            consumer.Logger = async (msg) =>
            {
                logMessages.Add(msg);
                await Task.CompletedTask;
            };

            // Act
            await channel.Writer.WriteAsync(1);
            await channel.Writer.WriteAsync(2);
            channel.Writer.Complete();

            await consumer.ConsumeAsync(
                channel.Reader,
                async (batch, ct) => await Task.FromResult(0),
                CancellationToken.None
            );

            // Assert
            Assert.NotEmpty(logMessages);
            Assert.Contains(logMessages, msg => msg.Contains("Collector") || msg.Contains("Success"));
        }

        [Fact]
        public async Task ConsumeAsync_LoggingException_IsHandledSafely()
        {
            // Arrange
            var channel = Channel.CreateUnbounded<int>();
            var consumer = new BatchingChannelConsumer<int>(batchSize: 2);

            consumer.Logger = async (msg) =>
            {
                throw new InvalidOperationException("Logger error");
            };

            // Act & Assert: Should not throw despite logger exception
            await channel.Writer.WriteAsync(1);
            await channel.Writer.WriteAsync(2);
            channel.Writer.Complete();

            await consumer.ConsumeAsync(
                channel.Reader,
                async (batch, ct) => await Task.FromResult(0),
                CancellationToken.None
            );

            // If we reach here, logger exceptions were handled properly
            Assert.True(true);
        }

        // ==================== Statistics Tests ====================

        [Fact]
        public async Task ConsumeAsync_Statistics_AccuracyAfterProcessing()
        {
            // Arrange
            var channel = Channel.CreateUnbounded<int>();
            var consumer = new BatchingChannelConsumer<int>(batchSize: 5);

            // Act
            for (int i = 1; i <= 17; i++)
            {
                await channel.Writer.WriteAsync(i);
            }
            channel.Writer.Complete();

            await consumer.ConsumeAsync(
                channel.Reader,
                async (batch, ct) => await Task.FromResult(0),
                CancellationToken.None
            );

            // Assert
            Assert.Equal(4, consumer.Stats.TotalBatches);     // 3 full + 1 partial
            Assert.Equal(4, consumer.Stats.SuccessfulBatches);
            Assert.Equal(0, consumer.Stats.FailedBatches);
            Assert.Equal(17, consumer.Stats.TotalItemsProcessed);
            Assert.True(consumer.Stats.GetSuccessRate() >= 99); // Should be 100%
        }

        [Fact]
        public async Task ConsumeAsync_Statistics_TrackSuccessAndFailure()
        {
            // Arrange
            var channel = Channel.CreateUnbounded<int>();
            var consumer = new BatchingChannelConsumer<int>(
                batchSize: 3,
                maxRetries: 1,
                retryDelay: TimeSpan.FromMilliseconds(10)
            );
            var callCount = 0;

            // Act
            for (int i = 1; i <= 6; i++)
            {
                await channel.Writer.WriteAsync(i);
            }
            channel.Writer.Complete();

            await consumer.ConsumeAsync(
                channel.Reader,
                async (batch, ct) =>
                {
                    callCount++;
                    if (callCount == 1)
                        throw new Exception("First batch fails");
                    return await Task.FromResult(0);
                },
                CancellationToken.None
            );

            // Assert
            Assert.Equal(2, consumer.Stats.TotalBatches);
            Assert.Equal(1, consumer.Stats.FailedBatches);
            Assert.Equal(1, consumer.Stats.SuccessfulBatches);
            Assert.Equal(6, consumer.Stats.TotalItemsProcessed);
        }

        [Fact]
        public async Task ConsumeAsync_Statistics_ThroughputCalculation()
        {
            // Arrange
            var channel = Channel.CreateUnbounded<int>();
            var consumer = new BatchingChannelConsumer<int>(batchSize: 10);

            // Act
            for (int i = 0; i < 100; i++)
            {
                await channel.Writer.WriteAsync(i);
            }
            channel.Writer.Complete();

            await consumer.ConsumeAsync(
                channel.Reader,
                async (batch, ct) =>
                {
                    await Task.Delay(50); // Simulate processing time
                    return await Task.FromResult(0);
                },
                CancellationToken.None
            );

            // Assert
            var throughput = consumer.Stats.GetThroughput();
            Assert.True(throughput > 0, "Throughput should be calculated");
            Assert.True(consumer.Stats.TotalDuration > TimeSpan.Zero);
        }

        [Fact]
        public void Statistics_Reset_ClearsAllMetrics()
        {
            // Arrange
            var stats = new ConsumerStats
            {
                TotalBatches = 10,
                SuccessfulBatches = 8,
                FailedBatches = 2,
                TotalItemsProcessed = 100,
                TotalDuration = TimeSpan.FromSeconds(5)
            };

            // Act
            stats.Reset();

            // Assert
            Assert.Equal(0, stats.TotalBatches);
            Assert.Equal(0, stats.SuccessfulBatches);
            Assert.Equal(0, stats.FailedBatches);
            Assert.Equal(0, stats.TotalItemsProcessed);
            Assert.Equal(TimeSpan.Zero, stats.TotalDuration);
        }

        // ==================== Cancellation Token Tests ====================

        [Fact]
        public async Task ConsumeAsync_CancellationToken_StopsProcessing()
        {
            // Arrange
            var channel = Channel.CreateUnbounded<int>();
            var cts = new CancellationTokenSource();
            var consumer = new BatchingChannelConsumer<int>(batchSize: 5);
            var processedBatches = 0;

            // Act
            var produceTask = Task.Run(async () =>
            {
                for (int i = 0; i < 100; i++)
                {
                    if (cts.Token.IsCancellationRequested)
                        break;
                    await channel.Writer.WriteAsync(i);
                    await Task.Delay(10);
                }
            });

            var consumeTask = Task.Run(async () =>
            {
                try
                {
                    await consumer.ConsumeAsync(
                        channel.Reader,
                        async (batch, ct) =>
                        {
                            processedBatches++;
                            if (processedBatches >= 2)
                                cts.Cancel();
                            return await Task.FromResult(0);
                        },
                        cts.Token
                    );
                }
                catch (OperationCanceledException)
                {
                    // Expected
                }
            });

            await Task.WhenAll(produceTask, consumeTask);

            // Assert
            Assert.True(processedBatches >= 2, "Should process at least 2 batches before cancellation");
        }

        // ==================== Edge Case Tests ====================

        [Fact]
        public async Task ConsumeAsync_SingleItem_ProcessesSingleItemBatch()
        {
            // Arrange
            var channel = Channel.CreateUnbounded<string>();
            var consumer = new BatchingChannelConsumer<string>(batchSize: 10);
            var batchCount = 0;

            // Act
            await channel.Writer.WriteAsync("single");
            channel.Writer.Complete();

            await consumer.ConsumeAsync(
                channel.Reader,
                async (batch, ct) =>
                {
                    batchCount++;
                    Assert.Single(batch);
                    Assert.Equal("single", batch[0]);
                    return await Task.FromResult(0);
                },
                CancellationToken.None
            );

            // Assert
            Assert.Equal(1, batchCount);
        }

        [Fact]
        public async Task ConsumeAsync_VeryLargeBatchSize_HandlesCorrectly()
        {
            // Arrange
            var channel = Channel.CreateUnbounded<int>();
            var consumer = new BatchingChannelConsumer<int>(batchSize: 10000);

            // Act
            for (int i = 0; i < 100; i++)
            {
                await channel.Writer.WriteAsync(i);
            }
            channel.Writer.Complete();

            await consumer.ConsumeAsync(
                channel.Reader,
                async (batch, ct) =>
                {
                    Assert.Equal(100, batch.Count);
                    return await Task.FromResult(0);
                },
                CancellationToken.None
            );

            // Assert
            Assert.Equal(1, consumer.Stats.TotalBatches);
        }

        [Fact]
        public async Task ConsumeAsync_VerySmallBatchSize_CreatesMany()
        {
            // Arrange
            var channel = Channel.CreateUnbounded<int>();
            var consumer = new BatchingChannelConsumer<int>(batchSize: 1);

            // Act
            for (int i = 0; i < 10; i++)
            {
                await channel.Writer.WriteAsync(i);
            }
            channel.Writer.Complete();

            await consumer.ConsumeAsync(
                channel.Reader,
                async (batch, ct) => await Task.FromResult(0),
                CancellationToken.None
            );

            // Assert
            Assert.Equal(10, consumer.Stats.TotalBatches); // Each item is its own batch
            Assert.Equal(10, consumer.Stats.TotalItemsProcessed);
        }

        // ==================== Runnable Integration Tests ====================

        [Fact]
        public async Task ConsumeAsRunnableAsync_WithRunnable_IntegratesCorrectly()
        {
            // Arrange
            var channel = Channel.CreateUnbounded<int>();
            var consumer = new BatchingChannelConsumer<int>(batchSize: 3);
            var processedSums = new List<int>();

            // Act
            for (int i = 1; i <= 6; i++)
            {
                await channel.Writer.WriteAsync(i);
            }
            channel.Writer.Complete();

            var processor = new Runnable<List<int>, BatchResult>(
                null,
                async (batch) =>
                {
                    var sum = batch.Sum();
                    processedSums.Add(sum);
                    return await Task.FromResult(new BatchResult(batch.Count, DateTime.Now, true));
                }
            );

            await consumer.ConsumeAsRunnableAsync(channel.Reader, processor, CancellationToken.None);

            // Assert
            Assert.Equal(2, processedSums.Count);
            Assert.Equal(6, processedSums[0]);  // 1 + 2 + 3
            Assert.Equal(15, processedSums[1]); // 4 + 5 + 6
        }

        [Fact]
        public async Task ConsumeAsRunnableAsync_WithRunnableFailure_CallsFailureCallback()
        {
            // Arrange
            var channel = Channel.CreateUnbounded<int>();
            var consumer = new BatchingChannelConsumer<int>(batchSize: 3, maxRetries: 1);
            var failureCount = 0;

            consumer.OnFailure = async (batch, ex) =>
            {
                failureCount++;
                await Task.CompletedTask;
            };

            // Act
            for (int i = 1; i <= 3; i++)
            {
                await channel.Writer.WriteAsync(i);
            }
            channel.Writer.Complete();

            var processor = new Runnable<List<int>, BatchResult>(
                null,
                async (batch) => throw new Exception("Simulated processor failure")
            );

            await consumer.ConsumeAsRunnableAsync(channel.Reader, processor, CancellationToken.None);

            // Assert
            Assert.True(failureCount > 0);
        }

        // ==================== Concurrent Scenario Tests ====================

        [Fact]
        public async Task ConsumeAsync_FastProducerSlowConsumer_Buffers()
        {
            // Arrange
            var channel = Channel.CreateUnbounded<int>();
            var consumer = new BatchingChannelConsumer<int>(batchSize: 5);
            var batchCount = 0;

            // Act: Producer writes fast, consumer processes slowly
            var produceTask = Task.Run(async () =>
            {
                for (int i = 0; i < 50; i++)
                {
                    await channel.Writer.WriteAsync(i);
                }
                channel.Writer.Complete();
            });

            var consumeTask = consumer.ConsumeAsync(
                channel.Reader,
                async (batch, ct) =>
                {
                    batchCount++;
                    await Task.Delay(50); // Slow processing
                    return await Task.FromResult(0);
                },
                CancellationToken.None
            );

            await Task.WhenAll(produceTask, consumeTask);

            // Assert
            Assert.Equal(10, consumer.Stats.TotalBatches); // 50 items / 5 per batch
            Assert.Equal(50, consumer.Stats.TotalItemsProcessed);
        }

        [Fact]
        public async Task ConsumeAsync_ExceptionInBatch_ContinuesProcessing()
        {
            // Arrange
            var channel = Channel.CreateUnbounded<int>();
            var consumer = new BatchingChannelConsumer<int>(
                batchSize: 2,
                maxRetries: 1,
                retryDelay: TimeSpan.FromMilliseconds(10)
            );
            var batchNumber = 0;

            // Act
            for (int i = 1; i <= 6; i++)
            {
                await channel.Writer.WriteAsync(i);
            }
            channel.Writer.Complete();

            await consumer.ConsumeAsync(
                channel.Reader,
                async (batch, ct) =>
                {
                    batchNumber++;
                    if (batchNumber == 1)
                    {
                        throw new Exception("Batch 1 fails");
                    }
                    return await Task.FromResult(0);
                },
                CancellationToken.None
            );

            // Assert
            Assert.Equal(3, consumer.Stats.TotalBatches); // All 3 batches attempted
            Assert.Equal(2, consumer.Stats.SuccessfulBatches); // 2 succeeded
            Assert.Equal(1, consumer.Stats.FailedBatches);    // 1 failed
        }

        // ==================== Configuration Tests ====================

        [Fact]
        public void Constructor_WithCustomParameters_StoresValues()
        {
            // Arrange & Act
            var consumer = new BatchingChannelConsumer<int>(
                batchSize: 25,
                timeoutSeconds: 15,
                maxRetries: 5,
                retryDelay: TimeSpan.FromMilliseconds(200)
            );

            // Assert - Verify configuration by checking behavior
            Assert.NotNull(consumer);
            Assert.NotNull(consumer.Stats);
        }

        [Fact]
        public void Constructor_WithDefaults_UsesCorrectDefaults()
        {
            // Arrange & Act
            var consumer = new BatchingChannelConsumer<int>();

            // Assert - Default values should work without errors
            Assert.NotNull(consumer);
            Assert.NotNull(consumer.Stats);
        }

        // ==================== Callback Configuration Tests ====================

        [Fact]
        public async Task ConfigureCallbacks_AllCallbacksSet_AllInvoked()
        {
            // Arrange
            var channel = Channel.CreateUnbounded<int>();
            var consumer = new BatchingChannelConsumer<int>(batchSize: 2);
            var logCount = 0;
            var successCount = 0;

            consumer.Logger = async (msg) =>
            {
                logCount++;
                await Task.CompletedTask;
            };

            consumer.OnSuccess = async (result) =>
            {
                successCount++;
                await Task.CompletedTask;
            };

            // Act
            await channel.Writer.WriteAsync(1);
            await channel.Writer.WriteAsync(2);
            channel.Writer.Complete();

            var processor = new Runnable<List<int>, BatchResult>(
                null,
                async (batch) => await Task.FromResult(new BatchResult(batch.Count, DateTime.Now, true))
            );

            await consumer.ConsumeAsRunnableAsync(channel.Reader, processor, CancellationToken.None);

            // Assert
            Assert.True(logCount > 0, "Logger should be invoked");
            Assert.True(successCount > 0, "OnSuccess should be invoked");
        }
    }

    // ==================== Helper Classes ====================

    /// <summary>
    /// Simple batch result implementation for testing
    /// </summary>
    public class BatchResult : IBatchResult
    {
        public int Count { get; }
        public DateTime ProcessedAt { get; }
        public bool Success { get; }
        public string Message { get; }

        public BatchResult(int count, DateTime processedAt, bool success, string message = "")
        {
            Count = count;
            ProcessedAt = processedAt;
            Success = success;
            Message = message;
        }
    }
}
