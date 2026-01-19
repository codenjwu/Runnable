using Runnable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Runnable.Tests
{
    /// <summary>
    /// Tests for CancellationToken support and Telemetry features (Items 6, 9, 10)
    /// </summary>
    public class RunnableV1_1_Part2FeaturesTests
    {
        // ==================== CancellationToken Support Tests ====================

        [Fact]
        public async Task InvokeAsync_WithCancellationToken_SupportsCancellation()
        {
            // Arrange
            var runnable = RunnableLambda.Create<int, int>(x => {
                Thread.Sleep(500);  // Long operation
                return x * 2;
            });

            using var cts = new CancellationTokenSource();
            cts.CancelAfter(TimeSpan.FromMilliseconds(50));

            // Act & Assert
            await Assert.ThrowsAsync<TaskCanceledException>(async () =>
                await runnable.InvokeAsync(5, cts.Token));
        }

        [Fact]
        public async Task InvokeAsync_WithCancellationToken_CompletesNormally()
        {
            // Arrange
            var runnable = RunnableLambda.Create<int, int>(x => x * 2);
            using var cts = new CancellationTokenSource();

            // Act
            var result = await runnable.InvokeAsync(5, cts.Token);

            // Assert
            Assert.Equal(10, result);
        }

        [Fact]
        public async Task InvokeAsync_WithTimeoutAndCancellation_WorksCorrectly()
        {
            // Arrange
            var runnable = RunnableLambda.Create<int, int>(x => {
                Thread.Sleep(10);
                return x * 2;
            });

            // Act
            var result = await runnable.InvokeAsync(5, TimeSpan.FromSeconds(1));

            // Assert
            Assert.Equal(10, result);
        }

        [Fact]
        public async Task WithAutoCancellation_CancelsAfterTimeout()
        {
            // Arrange
            var runnable = RunnableLambda.Create<int, int>(x => {
                Thread.Sleep(500);  // Long operation
                return x * 2;
            });

            // Act & Assert
            await Assert.ThrowsAnyAsync<Exception>(async () =>
                await runnable
                    .WithAutoCancellation(TimeSpan.FromMilliseconds(50))
                    .InvokeAsync(5));
        }

        // ==================== Telemetry Tests ====================

        [Fact]
        public async Task WithTelemetry_TracksSuccessfulExecution()
        {
            // Arrange
            var telemetryData = new List<RunnableTelemetryExtensions.TelemetryData>();
            var runnable = RunnableLambda.Create<int, int>(x => x * 2);

            // Act
            var result = await runnable
                .WithTelemetry(telemetryData.Add, "TestOperation")
                .InvokeAsync(5);

            // Assert
            Assert.Equal(10, result);
            Assert.Single(telemetryData);
            Assert.True(telemetryData[0].Success);
            Assert.Equal("TestOperation", telemetryData[0].OperationName);
            Assert.True(telemetryData[0].Duration > TimeSpan.Zero);
            Assert.Null(telemetryData[0].Exception);
        }

        [Fact]
        public void WithTelemetry_TracksFailedExecution()
        {
            // Arrange
            var telemetryData = new List<RunnableTelemetryExtensions.TelemetryData>();
            var runnable = RunnableLambda.Create<int, int>(x => throw new Exception("Test error"));

            // Act & Assert
            Assert.Throws<Exception>(() => runnable
                .WithTelemetry(telemetryData.Add, "FailingOperation")
                .Invoke(5));

            Assert.Single(telemetryData);
            Assert.False(telemetryData[0].Success);
            Assert.NotNull(telemetryData[0].Exception);
            Assert.Equal("Test error", telemetryData[0].Exception.Message);
        }

        [Fact]
        public async Task WithDurationTracking_TracksExecutionTime()
        {
            // Arrange
            var durations = new List<TimeSpan>();
            var runnable = RunnableLambda.Create<int, int>(x => {
                Thread.Sleep(10);
                return x * 2;
            });

            // Act
            var result = await runnable
                .WithDurationTracking(durations.Add)
                .InvokeAsync(5);

            // Assert
            Assert.Equal(10, result);
            Assert.Single(durations);
            Assert.True(durations[0].TotalMilliseconds >= 5);
        }

        [Fact]
        public async Task WithDurationTracking_WithInput_TracksCorrectly()
        {
            // Arrange
            var trackedInputs = new List<(int input, TimeSpan duration)>();
            var runnable = RunnableLambda.Create<int, int>(x => {
                Thread.Sleep(10);
                return x * 2;
            });

            // Act
            var result = await runnable
                .WithDurationTracking((input, duration) => trackedInputs.Add((input, duration)))
                .InvokeAsync(5);

            // Assert
            Assert.Equal(10, result);
            Assert.Single(trackedInputs);
            Assert.Equal(5, trackedInputs[0].input);
            Assert.True(trackedInputs[0].duration.TotalMilliseconds >= 5);
        }

        [Fact]
        public void WithExceptionTracking_TracksExceptions()
        {
            // Arrange
            var exceptions = new List<Exception>();
            var runnable = RunnableLambda.Create<int, int>(x => throw new InvalidOperationException("Test"));

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => runnable
                .WithExceptionTracking(exceptions.Add)
                .Invoke(5));

            Assert.Single(exceptions);
            Assert.IsType<InvalidOperationException>(exceptions[0]);
        }

        [Fact]
        public async Task WithSuccessRateTracking_TracksSuccessAndFailure()
        {
            // Arrange
            var results = new List<bool>();
            var attempts = 0;
            
            var runnable = RunnableLambda.Create<int, int>(x => {
                attempts++;
                if (attempts <= 2)
                    throw new Exception("Fail");
                return x * 2;
            });

            // Act
            try { await runnable.WithSuccessRateTracking(results.Add).InvokeAsync(5); } catch { }
            try { await runnable.WithSuccessRateTracking(results.Add).InvokeAsync(5); } catch { }
            await runnable.WithSuccessRateTracking(results.Add).InvokeAsync(5);

            // Assert
            Assert.Equal(3, results.Count);
            Assert.False(results[0]);  // Failed
            Assert.False(results[1]);  // Failed
            Assert.True(results[2]);   // Success
        }

        [Fact]
        public async Task WithPerformanceThreshold_LogsSlowOperations()
        {
            // Arrange
            var slowOps = new List<(int input, TimeSpan duration)>();
            var runnable = RunnableLambda.Create<int, int>(x => {
                Thread.Sleep(50);  // Intentionally slow
                return x * 2;
            });

            // Act
            var result = await runnable
                .WithPerformanceThreshold(
                    TimeSpan.FromMilliseconds(20),
                    (input, duration) => slowOps.Add((input, duration)))
                .InvokeAsync(5);

            // Assert
            Assert.Equal(10, result);
            Assert.Single(slowOps);
            Assert.Equal(5, slowOps[0].input);
            Assert.True(slowOps[0].duration.TotalMilliseconds >= 40);
        }

        [Fact]
        public async Task WithMetrics_CollectsComprehensiveMetrics()
        {
            // Arrange
            var metrics = new RunnableTelemetryExtensions.MetricsCollector();
            var attempts = 0;
            
            var runnable = RunnableLambda.Create<int, int>(x => {
                attempts++;
                Thread.Sleep(10);
                if (attempts == 2)
                    throw new Exception("Test failure");
                return x * 2;
            });

            var instrumentedRunnable = runnable.WithMetrics(metrics);

            // Act
            await instrumentedRunnable.InvokeAsync(5);  // Success
            try { await instrumentedRunnable.InvokeAsync(5); } catch { }  // Failure
            await instrumentedRunnable.InvokeAsync(5);  // Success

            // Assert
            Assert.Equal(3, metrics.TotalInvocations);
            Assert.Equal(2, metrics.SuccessfulInvocations);
            Assert.Equal(1, metrics.FailedInvocations);
            Assert.True(metrics.SuccessRate > 60);  // 2/3 = 66.67%
            Assert.True(metrics.AverageDuration > 0);
            Assert.True(metrics.MinDuration > TimeSpan.Zero);
            Assert.True(metrics.MaxDuration > metrics.MinDuration);
        }

        // ==================== Integration Tests ====================

        [Fact]
        public async Task Integration_TelemetryWithRetryAndCache_WorksCorrectly()
        {
            // Arrange
            var telemetryData = new List<RunnableTelemetryExtensions.TelemetryData>();
            var metrics = new RunnableTelemetryExtensions.MetricsCollector();
            var attempts = 0;

            var runnable = RunnableLambda.Create<int, int>(x => {
                attempts++;
                if (attempts == 1)
                    throw new Exception("First attempt fails");
                Thread.Sleep(10);
                return x * 2;
            });

            // Act
            var pipeline = runnable
                .WithRetry(2)  // Retry once
                .WithTelemetry(telemetryData.Add, "IntegrationTest")
                .WithMetrics(metrics)
                .WithCache();

            var result1 = await pipeline.InvokeAsync(5);
            var result2 = await pipeline.InvokeAsync(5);  // Cache hit

            // Assert
            Assert.Equal(10, result1);
            Assert.Equal(10, result2);
            Assert.Equal(2, attempts);  // Retried once
            
            // Telemetry captured
            Assert.Equal(2, telemetryData.Count);  // Both attempts (including retry)
            
            // Metrics collected
            Assert.Equal(2, metrics.TotalInvocations);
            Assert.True(metrics.SuccessfulInvocations >= 1);
        }

        [Fact]
        public async Task Integration_CancellationWithTimeout_WorksCorrectly()
        {
            // Arrange
            var runnable = RunnableLambda.Create<int, int>(x => {
                Thread.Sleep(200);
                return x * 2;
            });

            using var cts = new CancellationTokenSource();
            cts.CancelAfter(TimeSpan.FromMilliseconds(50));

            // Act & Assert - Should cancel
            await Assert.ThrowsAnyAsync<Exception>(async () =>
                await runnable
                    .WithTimeout(TimeSpan.FromMilliseconds(100))
                    .InvokeAsync(5, cts.Token));
        }

        [Fact]
        public async Task Integration_CompletePipeline_WithAllFeatures_WorksCorrectly()
        {
            // Arrange
            var metrics = new RunnableTelemetryExtensions.MetricsCollector();
            var slowOps = new List<(int, TimeSpan)>();
            var attempts = 0;

            var runnable = RunnableLambda.Create<int, int>(x => {
                attempts++;
                Thread.Sleep(20);
                if (attempts == 1)
                    throw new Exception("Transient error");
                return x * 2;
            });

            // Act - Complete pipeline with all v1.1 features
            var pipeline = runnable
                .WithExponentialBackoff(
                    maxAttempts: 3,
                    baseDelay: TimeSpan.FromMilliseconds(10))
                .WithMetrics(metrics)
                .WithPerformanceThreshold(
                    TimeSpan.FromMilliseconds(15),
                    (input, duration) => slowOps.Add((input, duration)))
                .WithCacheLRU(100)
                .WithTimeout(TimeSpan.FromSeconds(1));

            using var cts = new CancellationTokenSource();
            var result = await pipeline.InvokeAsync(5, cts.Token);

            // Assert
            Assert.Equal(10, result);
            Assert.Equal(2, attempts);  // Retried once
            Assert.True(metrics.TotalInvocations > 0);
            Assert.NotEmpty(slowOps);  // Slow operations detected
        }
    }
}
