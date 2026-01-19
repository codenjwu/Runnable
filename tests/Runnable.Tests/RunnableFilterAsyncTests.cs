using System;
using System.Threading.Tasks;
using Xunit;

namespace Runnable.Tests
{
    /// <summary>
    /// Unit tests for FilterAsync extension functionality (0-16 parameters)
    /// </summary>
    public class RunnableFilterAsyncTests
    {
        // ==================== Basic Functionality Tests ====================

        [Fact]
        public async Task FilterAsync_ZeroParameters_WhenPredicateTrue_ExecutesRunnable()
        {
            // Arrange
            var executed = false;
            var runnable = RunnableLambda.Create(() => {
                executed = true;
                return "result";
            });

            var filtered = runnable.FilterAsync(async () => {
                await Task.Delay(10);
                return true;
            }, "default");

            // Act
            var result = await filtered.InvokeAsync();

            // Assert
            Assert.True(executed);
            Assert.Equal("result", result);
        }

        [Fact]
        public async Task FilterAsync_ZeroParameters_WhenPredicateFalse_ReturnsDefault()
        {
            // Arrange
            var executed = false;
            var runnable = RunnableLambda.Create(() => {
                executed = true;
                return "result";
            });

            var filtered = runnable.FilterAsync(async () => {
                await Task.Delay(10);
                return false;
            }, "default");

            // Act
            var result = await filtered.InvokeAsync();

            // Assert
            Assert.False(executed);
            Assert.Equal("default", result);
        }

        [Fact]
        public async Task FilterAsync_OneParameter_WhenPredicateTrue_ExecutesRunnable()
        {
            // Arrange
            var square = RunnableLambda.Create<int, int>(x => x * x);
            var filtered = square.FilterAsync(async x => {
                await Task.Delay(10);
                return x > 0;
            }, -1);

            // Act
            var result = await filtered.InvokeAsync(5);

            // Assert
            Assert.Equal(25, result);
        }

        [Fact]
        public async Task FilterAsync_OneParameter_WhenPredicateFalse_ReturnsDefault()
        {
            // Arrange
            var square = RunnableLambda.Create<int, int>(x => x * x);
            var filtered = square.FilterAsync(async x => {
                await Task.Delay(10);
                return x > 0;
            }, -1);

            // Act
            var result = await filtered.InvokeAsync(-5);

            // Assert
            Assert.Equal(-1, result);
        }

        [Fact]
        public async Task FilterAsync_TwoParameters_WhenPredicateTrue_ExecutesRunnable()
        {
            // Arrange
            var sum = RunnableLambda.Create<int, int, int>((a, b) => a + b);
            var filtered = sum.FilterAsync(async (a, b) => {
                await Task.Delay(10);
                return a > 0 && b > 0;
            }, 0);

            // Act
            var result = await filtered.InvokeAsync(10, 5);

            // Assert
            Assert.Equal(15, result);
        }

        [Fact]
        public async Task FilterAsync_TwoParameters_WhenPredicateFalse_ReturnsDefault()
        {
            // Arrange
            var sum = RunnableLambda.Create<int, int, int>((a, b) => a + b);
            var filtered = sum.FilterAsync(async (a, b) => {
                await Task.Delay(10);
                return a > 0 && b > 0;
            }, 0);

            // Act
            var result = await filtered.InvokeAsync(-10, 5);

            // Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public async Task FilterAsync_SixteenParameters_WorksCorrectly()
        {
            // Arrange
            var sum = RunnableLambda.Create<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16) =>
                    a1 + a2 + a3 + a4 + a5 + a6 + a7 + a8 + a9 + a10 + a11 + a12 + a13 + a14 + a15 + a16);
            
            var filtered = sum.FilterAsync(async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16) => {
                await Task.Delay(10);
                return a1 + a2 + a3 + a4 + a5 + a6 + a7 + a8 + a9 + a10 + a11 + a12 + a13 + a14 + a15 + a16 > 100;
            }, -1);

            // Act & Assert
            Assert.Equal(136, await filtered.InvokeAsync(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16));  // 136 > 100
            Assert.Equal(-1, await filtered.InvokeAsync(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1));          // 16 <= 100
        }

        // ==================== Sync Invocation Tests ====================

        [Fact]
        public void FilterAsync_CanBeInvokedSynchronously()
        {
            // Arrange
            var runnable = RunnableLambda.Create<int, int>(x => x * 2);
            var filtered = runnable.FilterAsync(async x => {
                await Task.Delay(10);
                return x > 0;
            }, -1);

            // Act
            var result = filtered.Invoke(5);

            // Assert
            Assert.Equal(10, result);
        }

        // ==================== Real-World Async Scenarios ====================

        [Fact]
        public async Task FilterAsync_WithDatabaseCheck_WorksCorrectly()
        {
            // Arrange - Simulate async database existence check
            async Task<bool> ExistsInDatabaseAsync(int id)
            {
                await Task.Delay(20);  // Simulate DB query
                return id > 0 && id < 1000;  // Valid ID range
            }

            var getUserData = RunnableLambda.Create<int, string>(id => $"User-{id}");
            var validated = getUserData.FilterAsync(
                async id => await ExistsInDatabaseAsync(id),
                "INVALID_USER");

            // Act & Assert
            Assert.Equal("User-123", await validated.InvokeAsync(123));     // Valid ID
            Assert.Equal("INVALID_USER", await validated.InvokeAsync(9999)); // Invalid ID
        }

        [Fact]
        public async Task FilterAsync_WithApiAuthorizationCheck_WorksCorrectly()
        {
            // Arrange - Simulate async API authorization check
            async Task<bool> IsAuthorizedAsync(string userId, string resource)
            {
                await Task.Delay(25);  // Simulate API call
                return userId == "admin" || resource == "public";
            }

            var processRequest = RunnableLambda.Create<string, string, string>(
                (userId, resource) => $"Processed {resource} for {userId}");

            var authorized = processRequest.FilterAsync(
                async (userId, resource) => await IsAuthorizedAsync(userId, resource),
                "ACCESS_DENIED");

            // Act & Assert
            Assert.Equal("Processed data for admin", await authorized.InvokeAsync("admin", "data"));
            Assert.Equal("Processed public for user1", await authorized.InvokeAsync("user1", "public"));
            Assert.Equal("ACCESS_DENIED", await authorized.InvokeAsync("user1", "private"));
        }

        [Fact]
        public async Task FilterAsync_WithRateLimitCheck_WorksCorrectly()
        {
            // Arrange - Simulate async rate limiter
            var requestCount = 0;
            
            async Task<bool> CheckRateLimitAsync(string clientId)
            {
                await Task.Delay(10);
                requestCount++;
                return requestCount <= 3;  // Allow first 3 requests
            }

            var handleRequest = RunnableLambda.Create<string, string>(clientId => $"Handled {clientId}");
            var rateLimited = handleRequest.FilterAsync(
                async clientId => await CheckRateLimitAsync(clientId),
                "RATE_LIMIT_EXCEEDED");

            // Act & Assert
            Assert.Equal("Handled client1", await rateLimited.InvokeAsync("client1"));
            Assert.Equal("Handled client1", await rateLimited.InvokeAsync("client1"));
            Assert.Equal("Handled client1", await rateLimited.InvokeAsync("client1"));
            Assert.Equal("RATE_LIMIT_EXCEEDED", await rateLimited.InvokeAsync("client1"));
        }

        // ==================== Chaining Tests ====================

        [Fact]
        public async Task FilterAsync_CanBeChained()
        {
            // Arrange
            var runnable = RunnableLambda.Create<int, int>(x => x * 2);
            var pipeline = runnable
                .FilterAsync(async x => {
                    await Task.Delay(10);
                    return x > 0;
                }, -1)
                .FilterAsync(async x => {
                    await Task.Delay(10);
                    return x < 100;
                }, -2);

            // Act & Assert
            Assert.Equal(10, await pipeline.InvokeAsync(5));    // Passes both filters
            Assert.Equal(-1, await pipeline.InvokeAsync(-5));   // Fails first filter
            Assert.Equal(-2, await pipeline.InvokeAsync(200));  // Fails second filter
        }

        [Fact]
        public async Task FilterAsync_CanFollowFilter()
        {
            // Arrange
            var runnable = RunnableLambda.Create<int, int>(x => x * 2);
            var pipeline = runnable
                .Filter(x => x > 0, -1)  // Sync filter
                .FilterAsync(async x => {
                    await Task.Delay(10);
                    return x < 100;
                }, -2);  // Async filter

            // Act & Assert
            Assert.Equal(10, await pipeline.InvokeAsync(5));    // Passes both
            Assert.Equal(-1, await pipeline.InvokeAsync(-5));   // Fails first
            Assert.Equal(-2, await pipeline.InvokeAsync(200));  // Fails second
        }

        // ==================== Composition Tests ====================

        [Fact]
        public async Task FilterAsync_WithMap_WorksCorrectly()
        {
            // Arrange
            var runnable = RunnableLambda.Create<int, int>(x => x * 2);
            var pipeline = runnable
                .FilterAsync(async x => {
                    await Task.Delay(10);
                    return x >= 5;
                }, -1)
                .Map(x => x.ToString());

            // Act & Assert
            Assert.Equal("10", await pipeline.InvokeAsync(5));
            Assert.Equal("-1", await pipeline.InvokeAsync(2));
        }

        [Fact]
        public async Task FilterAsync_WithTapAsync_WorksCorrectly()
        {
            // Arrange
            var logged = new System.Collections.Generic.List<int>();
            var runnable = RunnableLambda.Create<int, int>(x => x * 2);
            
            var pipeline = runnable
                .FilterAsync(async x => {
                    await Task.Delay(10);
                    return x >= 5;
                }, -1)
                .TapAsync(async x => {
                    await Task.Delay(10);
                    logged.Add(x);
                });

            // Act
            await pipeline.InvokeAsync(5);
            await pipeline.InvokeAsync(2);

            // Assert
            Assert.Equal(2, logged.Count);
            Assert.Contains(10, logged);  // Passed filter
            Assert.Contains(-1, logged);  // Failed filter, got default
        }

        [Fact]
        public async Task FilterAsync_WithPipe_WorksCorrectly()
        {
            // Arrange
            var validateInput = RunnableLambda.Create<int, int>(x => x);
            var processValue = RunnableLambda.Create<int, string>(x => $"Value: {x}");

            var pipeline = validateInput
                .FilterAsync(async x => {
                    await Task.Delay(10);
                    return x > 0;
                }, -1)
                .Pipe(processValue);

            // Act & Assert
            Assert.Equal("Value: 5", await pipeline.InvokeAsync(5));
            Assert.Equal("Value: -1", await pipeline.InvokeAsync(-5));
        }

        // ==================== Default Value Tests ====================

        [Fact]
        public async Task FilterAsync_WithNullDefaultValue_ReturnsNull()
        {
            // Arrange
            var runnable = RunnableLambda.Create<int, string>(x => "result");
            var filtered = runnable.FilterAsync(async x => {
                await Task.Delay(10);
                return x > 0;
            }, null);

            // Act
            var result = await filtered.InvokeAsync(-5);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task FilterAsync_WithoutExplicitDefault_UsesDefaultOfType()
        {
            // Arrange
            var runnable = RunnableLambda.Create<int, int>(x => x * 2);
            var filtered = runnable.FilterAsync(async x => {
                await Task.Delay(10);
                return x > 0;
            });  // No default specified

            // Act
            var result = await filtered.InvokeAsync(-5);

            // Assert
            Assert.Equal(0, result);  // default(int) is 0
        }

        // ==================== Error Handling Tests ====================

        [Fact]
        public async Task FilterAsync_WhenPredicateThrows_PropagatesException()
        {
            // Arrange
            var runnable = RunnableLambda.Create<int, int>(x => x * 2);
            var filtered = runnable.FilterAsync<int, int>(async x => {
                await Task.Delay(10);
                throw new InvalidOperationException("Predicate error");
            }, -1);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => filtered.InvokeAsync(5));
        }

        [Fact]
        public async Task FilterAsync_WhenRunnableThrows_PropagatesException()
        {
            // Arrange
            var runnable = RunnableLambda.Create<int, int>(x => throw new ArgumentException("Runnable error"));
            var filtered = runnable.FilterAsync(async x => {
                await Task.Delay(10);
                return true;
            }, -1);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => filtered.InvokeAsync(5));
        }

        // ==================== Edge Cases ====================

        [Fact]
        public async Task FilterAsync_WithAlwaysTruePredicate_AlwaysExecutes()
        {
            // Arrange
            var runnable = RunnableLambda.Create<int, int>(x => x * 2);
            var filtered = runnable.FilterAsync(async x => {
                await Task.Delay(10);
                return true;
            }, -1);

            // Act & Assert
            Assert.Equal(10, await filtered.InvokeAsync(5));
            Assert.Equal(-10, await filtered.InvokeAsync(-5));
        }

        [Fact]
        public async Task FilterAsync_WithAlwaysFalsePredicate_NeverExecutes()
        {
            // Arrange
            var executed = false;
            var runnable = RunnableLambda.Create<int, int>(x => {
                executed = true;
                return x * 2;
            });
            
            var filtered = runnable.FilterAsync(async x => {
                await Task.Delay(10);
                return false;
            }, -1);

            // Act
            var result = await filtered.InvokeAsync(5);

            // Assert
            Assert.False(executed);
            Assert.Equal(-1, result);
        }

        [Fact]
        public async Task FilterAsync_WithImmediateTask_WorksCorrectly()
        {
            // Arrange
            var runnable = RunnableLambda.Create<int, int>(x => x * 2);
            var filtered = runnable.FilterAsync(x => Task.FromResult(x > 0), -1);

            // Act
            var result = await filtered.InvokeAsync(5);

            // Assert
            Assert.Equal(10, result);
        }

        [Fact]
        public async Task FilterAsync_WithComplexAsyncPredicate_WorksCorrectly()
        {
            // Arrange
            var process = RunnableLambda.Create<int, int, int, int>(
                (a, b, c) => a + b + c);

            var filtered = process.FilterAsync(async (a, b, c) => {
                await Task.Delay(10);
                var sum = a + b + c;
                return sum > 10 && sum < 100 && sum % 2 == 0;
            }, -1);

            // Act & Assert
            Assert.Equal(12, await filtered.InvokeAsync(4, 4, 4));   // 12: even, 10 < 12 < 100
            Assert.Equal(-1, await filtered.InvokeAsync(1, 1, 1));   // 3: < 10
            Assert.Equal(-1, await filtered.InvokeAsync(50, 50, 1)); // 101: > 100
            Assert.Equal(-1, await filtered.InvokeAsync(3, 3, 3));   // 9: odd
        }

        // ==================== Performance Tests ====================

        [Fact]
        public async Task FilterAsync_ExecutesAsynchronously()
        {
            // Arrange
            var runnable = RunnableLambda.Create<int, int>(x => x * 2);
            var delay = 50;
            
            var filtered = runnable.FilterAsync(async x => {
                await Task.Delay(delay);
                return true;
            }, -1);

            // Act
            var startTime = DateTime.Now;
            var result = await filtered.InvokeAsync(5);
            var elapsed = DateTime.Now - startTime;

            // Assert
            Assert.Equal(10, result);
            Assert.True(elapsed.TotalMilliseconds >= delay - 10, 
                $"Should have taken at least {delay}ms, took {elapsed.TotalMilliseconds}ms");
        }

        // ==================== Real-World Pipeline ====================

        [Fact]
        public async Task RealWorld_ValidationPipeline_WorksCorrectly()
        {
            // Arrange - Multi-stage async validation
            async Task<bool> ValidateInDatabaseAsync(string email)
            {
                await Task.Delay(15);
                return email.Contains("@");
            }

            async Task<bool> CheckBlacklistAsync(string email)
            {
                await Task.Delay(12);
                return !email.Contains("spam");
            }

            var processEmail = RunnableLambda.Create<string, string>(
                email => $"Processed: {email.ToLower()}");

            var validated = processEmail
                .FilterAsync(async email => await ValidateInDatabaseAsync(email), "INVALID_FORMAT")
                .FilterAsync(async email => {
                    // Only check blacklist if previous filter passed
                    if (email == "INVALID_FORMAT") return true;  // Skip check for default
                    return await CheckBlacklistAsync(email);
                }, "BLACKLISTED");

            // Act & Assert
            Assert.Equal("Processed: alice@example.com", 
                await validated.InvokeAsync("alice@example.com"));
            Assert.Equal("INVALID_FORMAT", 
                await validated.InvokeAsync("invalid-email"));
            Assert.Equal("BLACKLISTED", 
                await validated.InvokeAsync("spam@example.com"));
        }
    }
}
