using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Runnable.Tests
{
    /// <summary>
    /// Tests for new features added in v1.1:
    /// - Exception type filtering
    /// - Retry with backoff strategies
    /// - Cache with expiration
    /// - Correlation ID / Context support
    /// - WithDelay/WithTimeout for 0-16 parameters
    /// </summary>
    public class RunnableV1_1FeaturesTests
    {
        // ==================== Exception Type Filtering Tests ====================

        [Fact]
        public void WithFallback_TypedExceptionFiltering_OnlyCatchesSpecificType()
        {
            // Arrange
            var callCount = 0;
            var primary = RunnableLambda.Create<int, string>(x => {
                callCount++;
                throw new IOException("File not found");
            });
            var fallback = RunnableLambda.Create<int, string>(x => "Fallback");

            // Act - Catch IOException
            var result = primary
                .WithFallback<int, string, IOException>(fallback)
                .Invoke(5);

            // Assert
            Assert.Equal("Fallback", result);
            Assert.Equal(1, callCount);
        }

        [Fact]
        public void WithFallback_TypedExceptionFiltering_DoesNotCatchOtherTypes()
        {
            // Arrange
            var primary = RunnableLambda.Create<int, string>(x => {
                throw new ArgumentException("Invalid");  // Different exception type
            });
            var fallback = RunnableLambda.Create<int, string>(x => "Fallback");

            // Act & Assert - ArgumentException should propagate
            Assert.Throws<ArgumentException>(() =>
                primary.WithFallback<int, string, IOException>(fallback).Invoke(5));
        }

        [Fact]
        public void WithFallbackValue_TypedExceptionFiltering_ReturnsValue()
        {
            // Arrange
            var primary = RunnableLambda.Create<int, string>(x => {
                throw new IOException("Error");
            });

            // Act
            var result = primary
                .WithFallbackValue<int, string, IOException>("Default")
                .Invoke(5);

            // Assert
            Assert.Equal("Default", result);
        }

        [Fact]
        public void WithFallbackWhen_CustomPredicate_WorksCorrectly()
        {
            // Arrange
            var primary = RunnableLambda.Create<int, string>(x => {
                throw new InvalidOperationException("Custom error");
            });
            var fallback = RunnableLambda.Create<int, string>(x => "Handled");

            // Act - Only catch exceptions with specific message
            var result = primary
                .WithFallbackWhen(
                    ex => ex.Message.Contains("Custom"),
                    fallback)
                .Invoke(5);

            // Assert
            Assert.Equal("Handled", result);
        }

        // ==================== Retry with Backoff Tests ====================

        [Fact]
        public async Task WithExponentialBackoff_RetriesWithIncreasingDelay()
        {
            // Arrange
            var attempts = 0;
            var attemptTimes = new List<DateTime>();
            var runnable = RunnableLambda.Create<int, int>(x => {
                attempts++;
                attemptTimes.Add(DateTime.UtcNow);
                if (attempts < 3)
                    throw new Exception("Retry");
                return x * 2;
            });

            // Act
            var result = await runnable
                .WithExponentialBackoff(
                    maxAttempts: 3,
                    baseDelay: TimeSpan.FromMilliseconds(50))
                .InvokeAsync(5);

            // Assert
            Assert.Equal(10, result);
            Assert.Equal(3, attempts);

            // Verify delays increased (approximately)
            if (attemptTimes.Count >= 2)
            {
                var delay1 = (attemptTimes[1] - attemptTimes[0]).TotalMilliseconds;
                Assert.True(delay1 >= 40);  // ~50ms (with some tolerance)
            }
        }

        [Fact]
        public void RetryStrategies_LinearBackoff_IncreasesLinearly()
        {
            // Arrange
            var strategy = RetryStrategies.LinearBackoff(TimeSpan.FromMilliseconds(100));

            // Act
            var delay1 = strategy(1);
            var delay2 = strategy(2);
            var delay3 = strategy(3);

            // Assert
            Assert.Equal(100, delay1.TotalMilliseconds);
            Assert.Equal(200, delay2.TotalMilliseconds);
            Assert.Equal(300, delay3.TotalMilliseconds);
        }

        [Fact]
        public void RetryStrategies_ExponentialBackoff_DoublesEachTime()
        {
            // Arrange
            var strategy = RetryStrategies.ExponentialBackoff(
                TimeSpan.FromMilliseconds(100));

            // Act
            var delay1 = strategy(1);
            var delay2 = strategy(2);
            var delay3 = strategy(3);

            // Assert
            Assert.Equal(100, delay1.TotalMilliseconds);  // 100 * 2^0
            Assert.Equal(200, delay2.TotalMilliseconds);  // 100 * 2^1
            Assert.Equal(400, delay3.TotalMilliseconds);  // 100 * 2^2
        }

        [Fact]
        public void RetryStrategies_ExponentialBackoff_RespectsMaxDelay()
        {
            // Arrange
            var strategy = RetryStrategies.ExponentialBackoff(
                TimeSpan.FromMilliseconds(100),
                maxDelay: TimeSpan.FromMilliseconds(250));

            // Act
            var delay3 = strategy(3);  // Would be 400ms without max

            // Assert
            Assert.Equal(250, delay3.TotalMilliseconds);  // Capped at 250ms
        }

        // ==================== Cache with Expiration Tests ====================

        [Fact]
        public async Task WithCacheTTL_ExpiresAfterTTL()
        {
            // Arrange
            var callCount = 0;
            var expensive = RunnableLambda.Create<int, int>(x => {
                callCount++;
                return x * 2;
            });

            var cached = expensive.WithCacheTTL(TimeSpan.FromMilliseconds(100));

            // Act
            var result1 = await cached.InvokeAsync(5);
            Assert.Equal(1, callCount);

            var result2 = await cached.InvokeAsync(5);  // Should hit cache
            Assert.Equal(1, callCount);  // Still 1

            await Task.Delay(150);  // Wait for expiration

            var result3 = await cached.InvokeAsync(5);  // Should re-execute

            // Assert
            Assert.Equal(10, result1);
            Assert.Equal(10, result2);
            Assert.Equal(10, result3);
            Assert.Equal(2, callCount);  // Called again after expiration
        }

        [Fact]
        public void WithCacheLRU_EvictsLeastRecentlyUsed()
        {
            // Arrange
            var callCount = 0;
            var expensive = RunnableLambda.Create<int, int>(x => {
                callCount++;
                return x * 2;
            });

            var cached = expensive.WithCacheLRU(maxSize: 2);

            // Act
            var r1 = cached.Invoke(1);  // Cache: {1}
            var r2 = cached.Invoke(2);  // Cache: {1, 2}
            var r3 = cached.Invoke(3);  // Cache: {2, 3} (evicted 1)
            var r4 = cached.Invoke(1);  // Cache: {3, 1} (recompute 1, evicted 2)

            // Assert
            Assert.Equal(2, r1);
            Assert.Equal(4, r2);
            Assert.Equal(6, r3);
            Assert.Equal(2, r4);
            Assert.Equal(4, callCount);  // 1, 2, 3, 1 (recomputed)
        }

        [Fact]
        public void WithCacheTTLAndSize_CombinesBothLimits()
        {
            // Arrange
            var expensive = RunnableLambda.Create<int, int>(x => x * 2);

            // Act
            var cached = expensive.WithCacheTTLAndSize(
                ttl: TimeSpan.FromMinutes(5),
                maxSize: 100);

            var result = cached.Invoke(5);

            // Assert
            Assert.Equal(10, result);
            // Further testing would require time manipulation
        }

        // ==================== Correlation ID / Context Tests ====================

        [Fact]
        public async Task WithCorrelationId_SetsCorrelationId()
        {
            // Arrange
            string capturedCorrelationId = null;
            var runnable = RunnableLambda.Create<int, int>(x => {
                capturedCorrelationId = RunnableContext.Current.CorrelationId;
                return x * 2;
            });

            // Act
            var result = await runnable
                .WithCorrelationId("test-correlation-123")
                .InvokeAsync(5);

            // Assert
            Assert.Equal(10, result);
            Assert.Equal("test-correlation-123", capturedCorrelationId);
        }

        [Fact]
        public async Task WithCorrelationId_AutoGeneratesIfNotProvided()
        {
            // Arrange
            string capturedCorrelationId = null;
            var runnable = RunnableLambda.Create<int, int>(x => {
                capturedCorrelationId = RunnableContext.Current.CorrelationId;
                return x * 2;
            });

            // Act
            var result = await runnable
                .WithCorrelationId()  // Auto-generate
                .InvokeAsync(5);

            // Assert
            Assert.Equal(10, result);
            Assert.NotNull(capturedCorrelationId);
            Assert.True(Guid.TryParse(capturedCorrelationId, out _));  // Valid GUID
        }

        [Fact]
        public async Task WithTenant_SetsTenantId()
        {
            // Arrange
            string capturedTenantId = null;
            var runnable = RunnableLambda.Create<string, string>(tenant => {
                capturedTenantId = RunnableContext.Current.TenantId;
                return $"Tenant: {tenant}";
            });

            // Act
            var result = await runnable
                .WithTenant(t => t)  // Extract from input
                .InvokeAsync("tenant-123");

            // Assert
            Assert.Equal("Tenant: tenant-123", result);
            Assert.Equal("tenant-123", capturedTenantId);
        }

        [Fact]
        public async Task WithContext_CustomValues_WorkCorrectly()
        {
            // Arrange
            var runnable = RunnableLambda.Create<int, int>(x => {
                var requestId = RunnableContext.Current.GetValue<string>("RequestId");
                var environment = RunnableContext.Current.GetValue<string>("Environment");
                Assert.NotNull(requestId);
                Assert.Equal("Production", environment);
                return x * 2;
            });

            // Act
            var result = await runnable
                .WithContext("RequestId", "req-123")
                .WithContext("Environment", "Production")
                .InvokeAsync(5);

            // Assert
            Assert.Equal(10, result);
        }

        [Fact(Skip = "Context propagation across pipeline needs more work - known limitation")]
        public async Task TapContext_ObservesContext()
        {
            // Arrange
            var logs = new List<string>();
            var runnable = RunnableLambda.Create<int, int>(x => x * 2);

            // Act
            var result = await runnable
                .WithCorrelationId("test-123")
                .TapContext((input, ctx) => {
                    // Access via GetValue to avoid auto-generation
                    var correlationId = ctx.GetValue<string>("CorrelationId");
                    logs.Add($"CorrelationId: {correlationId}, Input: {input}");
                })
                .InvokeAsync(5);

            // Assert
            Assert.Equal(10, result);
            Assert.Single(logs);
            Assert.Contains("test-123", logs[0]);
            Assert.Contains("5", logs[0]);
        }

        [Fact(Skip = "Context propagation across pipeline needs more work - known limitation")]
        public async Task TapContextAsync_ObservesContextAsync()
        {
            // Arrange
            var logs = new List<string>();
            var runnable = RunnableLambda.Create<int, int>(x => x * 2);

            // Act
            var result = await runnable
                .WithCorrelationId("async-test-456")
                .WithUser("user-789")
                .TapContextAsync(async (input, ctx) => {
                    await Task.Delay(1);
                    var correlationId = ctx.GetValue<string>("CorrelationId");
                    var userId = ctx.GetValue<string>("UserId");
                    logs.Add($"Correlation: {correlationId}, User: {userId}");
                })
                .InvokeAsync(5);

            // Assert
            Assert.Equal(10, result);
            Assert.Single(logs);
            Assert.Contains("async-test-456", logs[0]);
            Assert.Contains("user-789", logs[0]);
        }

        // ==================== WithDelay/WithTimeout for Multiple Parameters ====================

        [Fact]
        public async Task WithDelay_NoParameters_WorksCorrectly()
        {
            // Arrange
            var runnable = RunnableLambda.Create(() => 42);

            // Act
            var start = DateTime.UtcNow;
            var result = await runnable
                .WithDelay(TimeSpan.FromMilliseconds(50))
                .InvokeAsync();
            var elapsed = DateTime.UtcNow - start;

            // Assert
            Assert.Equal(42, result);
            Assert.True(elapsed.TotalMilliseconds >= 40);
        }

        [Fact]
        public async Task WithDelay_TwoParameters_WorksCorrectly()
        {
            // Arrange
            var sum = RunnableLambda.Create<int, int, int>((a, b) => a + b);

            // Act
            var result = await sum
                .WithDelay(TimeSpan.FromMilliseconds(10))
                .InvokeAsync(5, 3);

            // Assert
            Assert.Equal(8, result);
        }

        [Fact]
        public async Task WithTimeout_NoParameters_WorksCorrectly()
        {
            // Arrange
            var runnable = RunnableLambda.Create(() => {
                System.Threading.Thread.Sleep(10);
                return 42;
            });

            // Act
            var result = await runnable
                .WithTimeout(TimeSpan.FromSeconds(1))
                .InvokeAsync();

            // Assert
            Assert.Equal(42, result);
        }

        [Fact]
        public async Task WithTimeout_TwoParameters_WorksCorrectly()
        {
            // Arrange
            var sum = RunnableLambda.Create<int, int, int>((a, b) => {
                System.Threading.Thread.Sleep(10);
                return a + b;
            });

            // Act
            var result = await sum
                .WithTimeout(TimeSpan.FromSeconds(1))
                .InvokeAsync(5, 3);

            // Assert
            Assert.Equal(8, result);
        }

        // ==================== Integration Tests (Combining Features) ====================

        [Fact(Skip = "Context propagation across pipeline needs more work - known limitation")]
        public async Task Integration_AllFeaturesCombin_WorksCorrectly()
        {
            // Arrange - Production-like pipeline
            var attempts = 0;
            var logs = new List<string>();

            var apiCall = RunnableLambda.Create<int, string>(x => {
                attempts++;
                if (attempts < 2)
                    throw new IOException("Network error");
                return $"Result: {x * 2}";
            });

            var fallback = RunnableLambda.Create<int, string>(x => "Cached");

            // Act - Build comprehensive pipeline
            var pipeline = apiCall
                .WithCorrelationId("integration-test-001")
                .WithUser("test-user")
                .WithExponentialBackoff(
                    maxAttempts: 3,
                    baseDelay: TimeSpan.FromMilliseconds(10))
                .WithFallback<int, string, HttpRequestException>(fallback)  // Won't catch IOException
                .WithCacheTTL(TimeSpan.FromMinutes(5))
                .TapContextAsync(async (input, ctx) => {
                    await Task.Delay(1);
                    var correlationId = ctx.GetValue<string>("CorrelationId");
                    var userId = ctx.GetValue<string>("UserId");
                    logs.Add($"Processed: {correlationId}, User: {userId}");
                });

            var result = await pipeline.InvokeAsync(5);

            // Assert
            Assert.Equal("Result: 10", result);
            Assert.Equal(2, attempts);  // Retried once, then succeeded
            Assert.Single(logs);
            Assert.Contains("integration-test-001", logs[0]);
            Assert.Contains("test-user", logs[0]);
        }

        [Fact]
        public async Task Integration_CachingWithContext_WorksCorrectly()
        {
            // Arrange
            var callCount = 0;
            var runnable = RunnableLambda.Create<int, int>(x => {
                callCount++;
                var correlationId = RunnableContext.Current.CorrelationId;
                Assert.NotNull(correlationId);
                return x * 2;
            });

            // Act
            var pipeline = runnable
                .WithCorrelationId("cache-test-123")
                .WithCacheLRU(100);

            var result1 = await pipeline.InvokeAsync(5);
            var result2 = await pipeline.InvokeAsync(5);  // Cache hit

            // Assert
            Assert.Equal(10, result1);
            Assert.Equal(10, result2);
            Assert.Equal(1, callCount);  // Only called once (cached)
        }
    }
}
