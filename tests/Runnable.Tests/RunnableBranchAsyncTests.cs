using System;
using System.Threading.Tasks;
using Xunit;

namespace Runnable.Tests
{
    /// <summary>
    /// Unit tests for RunnableBranch.CreateAsync functionality (0-16 parameters)
    /// </summary>
    public class RunnableBranchAsyncTests
    {
        // ==================== Basic Functionality Tests ====================

        [Fact]
        public async Task CreateAsync_ZeroParameters_RoutesToFirstMatchingBranch()
        {
            // Arrange
            var flag = true;
            var branch1 = RunnableLambda.Create(() => "Branch1");
            var branch2 = RunnableLambda.Create(() => "Branch2");
            var defaultBranch = RunnableLambda.Create(() => "Default");

            var router = RunnableBranch.CreateAsync(
                defaultBranch,
                (async () => {
                    await Task.Delay(10);
                    return flag;
                }, branch1),
                (async () => {
                    await Task.Delay(10);
                    return false;
                }, branch2)
            );

            // Act
            var result = await router.InvokeAsync();

            // Assert
            Assert.Equal("Branch1", result);
        }

        [Fact]
        public async Task CreateAsync_ZeroParameters_RoutesToDefault_WhenNoMatch()
        {
            // Arrange
            var branch1 = RunnableLambda.Create(() => "Branch1");
            var defaultBranch = RunnableLambda.Create(() => "Default");

            var router = RunnableBranch.CreateAsync(
                defaultBranch,
                (async () => {
                    await Task.Delay(10);
                    return false;
                }, branch1)
            );

            // Act
            var result = await router.InvokeAsync();

            // Assert
            Assert.Equal("Default", result);
        }

        [Fact]
        public async Task CreateAsync_OneParameter_RoutesBasedOnAsyncCondition()
        {
            // Arrange
            var positiveBranch = RunnableLambda.Create<int, string>(x => $"Positive: {x}");
            var negativeBranch = RunnableLambda.Create<int, string>(x => $"Negative: {x}");
            var zeroBranch = RunnableLambda.Create<int, string>(x => "Zero");

            var router = RunnableBranch.CreateAsync(
                zeroBranch,
                (async x => {
                    await Task.Delay(10);
                    return x > 0;
                }, positiveBranch),
                (async x => {
                    await Task.Delay(10);
                    return x < 0;
                }, negativeBranch)
            );

            // Act & Assert
            Assert.Equal("Positive: 5", await router.InvokeAsync(5));
            Assert.Equal("Negative: -3", await router.InvokeAsync(-3));
            Assert.Equal("Zero", await router.InvokeAsync(0));
        }

        [Fact]
        public async Task CreateAsync_TwoParameters_RoutesCorrectly()
        {
            // Arrange
            var sumBranch = RunnableLambda.Create<int, int, string>((a, b) => $"Sum: {a + b}");
            var productBranch = RunnableLambda.Create<int, int, string>((a, b) => $"Product: {a * b}");
            var defaultBranch = RunnableLambda.Create<int, int, string>((a, b) => "Default");

            var router = RunnableBranch.CreateAsync(
                defaultBranch,
                (async (a, b) => {
                    await Task.Delay(10);
                    return a + b > 10;
                }, sumBranch),
                (async (a, b) => {
                    await Task.Delay(10);
                    return a * b > 20;
                }, productBranch)
            );

            // Act & Assert
            Assert.Equal("Sum: 15", await router.InvokeAsync(10, 5));
            Assert.Equal("Product: 24", await router.InvokeAsync(4, 6));
            Assert.Equal("Default", await router.InvokeAsync(1, 1));
        }

        [Fact]
        public async Task CreateAsync_SixteenParameters_WorksCorrectly()
        {
            // Arrange
            var highBranch = RunnableLambda.Create<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, string>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16) => "High");
            var lowBranch = RunnableLambda.Create<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, string>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16) => "Low");
            var defaultBranch = RunnableLambda.Create<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, string>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16) => "Default");

            var router = RunnableBranch.CreateAsync(
                defaultBranch,
                (async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16) => {
                    await Task.Delay(10);
                    return a1 + a2 + a3 + a4 + a5 + a6 + a7 + a8 + a9 + a10 + a11 + a12 + a13 + a14 + a15 + a16 > 100;
                }, highBranch),
                (async (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16) => {
                    await Task.Delay(10);
                    return a1 + a2 + a3 + a4 + a5 + a6 + a7 + a8 + a9 + a10 + a11 + a12 + a13 + a14 + a15 + a16 < 20;
                }, lowBranch)
            );

            // Act & Assert
            Assert.Equal("High", await router.InvokeAsync(10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10)); // 160 > 100
            Assert.Equal("Low", await router.InvokeAsync(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1));                   // 16 < 20
            Assert.Equal("Default", await router.InvokeAsync(2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2));              // 32: 20 <= 32 <= 100
        }

        // ==================== Sync Invocation Tests ====================

        [Fact]
        public void CreateAsync_CanBeInvokedSynchronously()
        {
            // Arrange
            var positiveBranch = RunnableLambda.Create<int, string>(x => "Positive");
            var defaultBranch = RunnableLambda.Create<int, string>(x => "Default");

            var router = RunnableBranch.CreateAsync(
                defaultBranch,
                (async x => {
                    await Task.Delay(10);
                    return x > 0;
                }, positiveBranch)
            );

            // Act
            var result = router.Invoke(5);

            // Assert
            Assert.Equal("Positive", result);
        }

        // ==================== Real-World Async Scenarios ====================

        [Fact]
        public async Task CreateAsync_WithDatabaseCheck_WorksCorrectly()
        {
            // Arrange - Simulate async database checks
            async Task<bool> IsPremiumUserAsync(int userId)
            {
                await Task.Delay(15);
                return userId >= 1000;
            }

            async Task<bool> IsActiveUserAsync(int userId)
            {
                await Task.Delay(12);
                return userId >= 100;
            }

            var premiumHandler = RunnableLambda.Create<int, string>(id => $"Premium user: {id}");
            var activeHandler = RunnableLambda.Create<int, string>(id => $"Active user: {id}");
            var inactiveHandler = RunnableLambda.Create<int, string>(id => $"Inactive user: {id}");

            var router = RunnableBranch.CreateAsync(
                inactiveHandler,
                (async userId => await IsPremiumUserAsync(userId), premiumHandler),
                (async userId => await IsActiveUserAsync(userId), activeHandler)
            );

            // Act & Assert
            Assert.Equal("Premium user: 1500", await router.InvokeAsync(1500));
            Assert.Equal("Active user: 500", await router.InvokeAsync(500));
            Assert.Equal("Inactive user: 50", await router.InvokeAsync(50));
        }

        [Fact]
        public async Task CreateAsync_WithApiAuthorizationCheck_WorksCorrectly()
        {
            // Arrange - Simulate async API authorization
            async Task<bool> IsAdminAsync(string userId, string resource)
            {
                await Task.Delay(20);
                return userId == "admin";
            }

            async Task<bool> HasAccessAsync(string userId, string resource)
            {
                await Task.Delay(18);
                return resource == "public";
            }

            var adminHandler = RunnableLambda.Create<string, string, string>(
                (userId, resource) => $"Admin access: {resource}");
            var userHandler = RunnableLambda.Create<string, string, string>(
                (userId, resource) => $"User access: {resource}");
            var deniedHandler = RunnableLambda.Create<string, string, string>(
                (userId, resource) => "Access denied");

            var router = RunnableBranch.CreateAsync(
                deniedHandler,
                (async (userId, resource) => await IsAdminAsync(userId, resource), adminHandler),
                (async (userId, resource) => await HasAccessAsync(userId, resource), userHandler)
            );

            // Act & Assert
            Assert.Equal("Admin access: data", await router.InvokeAsync("admin", "data"));
            Assert.Equal("User access: public", await router.InvokeAsync("user1", "public"));
            Assert.Equal("Access denied", await router.InvokeAsync("user1", "private"));
        }

        [Fact]
        public async Task CreateAsync_WithFeatureFlagCheck_WorksCorrectly()
        {
            // Arrange - Simulate async feature flag service
            var featureFlags = new System.Collections.Generic.Dictionary<string, bool>
            {
                ["new-ui"] = true,
                ["beta-features"] = false
            };

            async Task<bool> IsFeatureEnabledAsync(string feature)
            {
                await Task.Delay(10);
                return featureFlags.GetValueOrDefault(feature, false);
            }

            var newUiHandler = RunnableLambda.Create<string, string>(user => $"New UI for {user}");
            var betaHandler = RunnableLambda.Create<string, string>(user => $"Beta for {user}");
            var defaultHandler = RunnableLambda.Create<string, string>(user => $"Default UI for {user}");

            var router = RunnableBranch.CreateAsync(
                defaultHandler,
                (async _ => await IsFeatureEnabledAsync("new-ui"), newUiHandler),
                (async _ => await IsFeatureEnabledAsync("beta-features"), betaHandler)
            );

            // Act
            var result = await router.InvokeAsync("alice");

            // Assert
            Assert.Equal("New UI for alice", result);
        }

        // ==================== Chaining and Composition Tests ====================

        [Fact]
        public async Task CreateAsync_WithMap_WorksCorrectly()
        {
            // Arrange
            var positiveBranch = RunnableLambda.Create<int, int>(x => x * 2);
            var negativeBranch = RunnableLambda.Create<int, int>(x => x * -1);
            var zeroBranch = RunnableLambda.Create<int, int>(x => 0);

            var router = RunnableBranch.CreateAsync(
                zeroBranch,
                (async x => {
                    await Task.Delay(10);
                    return x > 0;
                }, positiveBranch),
                (async x => {
                    await Task.Delay(10);
                    return x < 0;
                }, negativeBranch)
            );

            var pipeline = router.Map(x => $"Result: {x}");

            // Act & Assert
            Assert.Equal("Result: 10", await pipeline.InvokeAsync(5));
            Assert.Equal("Result: 5", await pipeline.InvokeAsync(-5));
            Assert.Equal("Result: 0", await pipeline.InvokeAsync(0));
        }

        [Fact]
        public async Task CreateAsync_WithTapAsync_WorksCorrectly()
        {
            // Arrange
            var logged = "";
            var branch1 = RunnableLambda.Create<int, string>(x => "Branch1");
            var defaultBranch = RunnableLambda.Create<int, string>(x => "Default");

            var router = RunnableBranch.CreateAsync(
                defaultBranch,
                (async x => {
                    await Task.Delay(10);
                    return x > 0;
                }, branch1)
            );

            var pipeline = router.TapAsync(async result => {
                await Task.Delay(10);
                logged = result;
            });

            // Act
            await pipeline.InvokeAsync(5);

            // Assert
            Assert.Equal("Branch1", logged);
        }

        [Fact]
        public async Task CreateAsync_WithFilterAsync_WorksCorrectly()
        {
            // Arrange
            var branch1 = RunnableLambda.Create<int, int>(x => x * 2);
            var defaultBranch = RunnableLambda.Create<int, int>(x => x);

            var router = RunnableBranch.CreateAsync(
                defaultBranch,
                (async x => {
                    await Task.Delay(10);
                    return x > 0;
                }, branch1)
            );

            var pipeline = router.FilterAsync(async x => {
                await Task.Delay(10);
                return x >= 5;
            }, -1);

            // Act & Assert
            Assert.Equal(10, await pipeline.InvokeAsync(5));   // Goes to branch1 (5*2=10), passes filter
            Assert.Equal(-1, await pipeline.InvokeAsync(2));   // Goes to branch1 (2*2=4), fails filter
            Assert.Equal(-1, await pipeline.InvokeAsync(-5));  // Goes to default (-5), fails filter
        }

        // ==================== First Match Semantics ====================

        [Fact]
        public async Task CreateAsync_StopsAtFirstMatch()
        {
            // Arrange
            var executed = new System.Collections.Generic.List<string>();
            
            var branch1 = RunnableLambda.Create<int, string>(x => "Branch1");
            var branch2 = RunnableLambda.Create<int, string>(x => "Branch2");
            var defaultBranch = RunnableLambda.Create<int, string>(x => "Default");

            var router = RunnableBranch.CreateAsync(
                defaultBranch,
                (async x => {
                    await Task.Delay(10);
                    executed.Add("Condition1");
                    return true;
                }, branch1),
                (async x => {
                    await Task.Delay(10);
                    executed.Add("Condition2");  // Should NOT be called
                    return true;
                }, branch2)
            );

            // Act
            var result = await router.InvokeAsync(5);

            // Assert
            Assert.Equal("Branch1", result);
            Assert.Single(executed);
            Assert.Equal("Condition1", executed[0]);
        }

        // ==================== Error Handling Tests ====================

        [Fact]
        public async Task CreateAsync_WhenConditionThrows_PropagatesException()
        {
            // Arrange
            var branch1 = RunnableLambda.Create<int, string>(x => "Branch1");
            var defaultBranch = RunnableLambda.Create<int, string>(x => "Default");

            var router = RunnableBranch.CreateAsync(
                defaultBranch,
                (async Task<bool> (int x) => {
                    await Task.Delay(10);
                    throw new InvalidOperationException("Condition error");
                }, branch1)
            );

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => router.InvokeAsync(5));
        }

        [Fact]
        public async Task CreateAsync_WhenBranchThrows_PropagatesException()
        {
            // Arrange
            var throwingBranch = RunnableLambda.Create<int, string>(x => throw new ArgumentException("Branch error"));
            var defaultBranch = RunnableLambda.Create<int, string>(x => "Default");

            var router = RunnableBranch.CreateAsync(
                defaultBranch,
                (async x => {
                    await Task.Delay(10);
                    return true;
                }, throwingBranch)
            );

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => router.InvokeAsync(5));
        }

        // ==================== Edge Cases ====================

        [Fact]
        public async Task CreateAsync_WithNoBranches_AlwaysUsesDefault()
        {
            // Arrange
            var defaultBranch = RunnableLambda.Create<int, string>(x => "Default");
            var router = RunnableBranch.CreateAsync<int, string>(defaultBranch);

            // Act
            var result = await router.InvokeAsync(42);

            // Assert
            Assert.Equal("Default", result);
        }

        [Fact]
        public async Task CreateAsync_WithImmediateTask_WorksCorrectly()
        {
            // Arrange
            var branch1 = RunnableLambda.Create<int, string>(x => "Branch1");
            var defaultBranch = RunnableLambda.Create<int, string>(x => "Default");

            var router = RunnableBranch.CreateAsync(
                defaultBranch,
                (x => Task.FromResult(x > 0), branch1)
            );

            // Act
            var result = await router.InvokeAsync(5);

            // Assert
            Assert.Equal("Branch1", result);
        }

        [Fact]
        public async Task CreateAsync_WithComplexAsyncCondition_WorksCorrectly()
        {
            // Arrange
            async Task<bool> ComplexCheckAsync(int a, int b, int c)
            {
                await Task.Delay(10);
                var sum = a + b + c;
                return sum > 10 && sum < 100 && sum % 2 == 0;
            }

            var evenBranch = RunnableLambda.Create<int, int, int, string>((a, b, c) => "Even");
            var defaultBranch = RunnableLambda.Create<int, int, int, string>((a, b, c) => "Default");

            var router = RunnableBranch.CreateAsync(
                defaultBranch,
                (async (a, b, c) => await ComplexCheckAsync(a, b, c), evenBranch)
            );

            // Act & Assert
            Assert.Equal("Even", await router.InvokeAsync(4, 4, 4));    // 12: even, 10 < 12 < 100
            Assert.Equal("Default", await router.InvokeAsync(1, 1, 1)); // 3: < 10
            Assert.Equal("Default", await router.InvokeAsync(50, 50, 1)); // 101: > 100
            Assert.Equal("Default", await router.InvokeAsync(3, 3, 3)); // 9: odd
        }

        // ==================== Performance Tests ====================

        [Fact]
        public async Task CreateAsync_ExecutesAsynchronously()
        {
            // Arrange
            var delay = 50;
            var branch1 = RunnableLambda.Create<int, string>(x => "Branch1");
            var defaultBranch = RunnableLambda.Create<int, string>(x => "Default");

            var router = RunnableBranch.CreateAsync(
                defaultBranch,
                (async x => {
                    await Task.Delay(delay);
                    return true;
                }, branch1)
            );

            // Act
            var startTime = DateTime.Now;
            var result = await router.InvokeAsync(5);
            var elapsed = DateTime.Now - startTime;

            // Assert
            Assert.Equal("Branch1", result);
            Assert.True(elapsed.TotalMilliseconds >= delay - 10, 
                $"Should have taken at least {delay}ms, took {elapsed.TotalMilliseconds}ms");
        }

        // ==================== Real-World Pipelines ====================

        [Fact]
        public async Task RealWorld_AuthenticationRouter_WorksCorrectly()
        {
            // Arrange - Simulate authentication routing
            var authenticatedUsers = new System.Collections.Generic.HashSet<string> { "alice", "bob" };
            var adminUsers = new System.Collections.Generic.HashSet<string> { "admin" };

            async Task<bool> IsAdminAsync(string username)
            {
                await Task.Delay(15);
                return adminUsers.Contains(username);
            }

            async Task<bool> IsAuthenticatedAsync(string username)
            {
                await Task.Delay(12);
                return authenticatedUsers.Contains(username);
            }

            var adminHandler = RunnableLambda.Create<string, string>(u => $"Admin dashboard for {u}");
            var userHandler = RunnableLambda.Create<string, string>(u => $"User dashboard for {u}");
            var loginHandler = RunnableLambda.Create<string, string>(u => "Please login");

            var router = RunnableBranch.CreateAsync(
                loginHandler,
                (async u => await IsAdminAsync(u), adminHandler),
                (async u => await IsAuthenticatedAsync(u), userHandler)
            );

            // Act & Assert
            Assert.Equal("Admin dashboard for admin", await router.InvokeAsync("admin"));
            Assert.Equal("User dashboard for alice", await router.InvokeAsync("alice"));
            Assert.Equal("Please login", await router.InvokeAsync("guest"));
        }

        [Fact]
        public async Task RealWorld_ContentRouter_WorksCorrectly()
        {
            // Arrange - Simulate content delivery routing
            var premiumContent = new System.Collections.Generic.HashSet<int> { 100, 200, 300 };

            async Task<bool> IsPremiumContentAsync(int contentId)
            {
                await Task.Delay(10);
                return premiumContent.Contains(contentId);
            }

            async Task<bool> IsAvailableAsync(int contentId)
            {
                await Task.Delay(8);
                return contentId > 0;
            }

            var premiumHandler = RunnableLambda.Create<int, string>(id => $"Premium content {id}");
            var freeHandler = RunnableLambda.Create<int, string>(id => $"Free content {id}");
            var notFoundHandler = RunnableLambda.Create<int, string>(id => "Content not found");

            var router = RunnableBranch.CreateAsync(
                notFoundHandler,
                (async id => await IsPremiumContentAsync(id), premiumHandler),
                (async id => await IsAvailableAsync(id), freeHandler)
            );

            var pipeline = router
                .TapAsync(async result => {
                    await Task.Delay(5);
                    // Log access
                })
                .Map(result => result.ToUpper());

            // Act & Assert
            Assert.Equal("PREMIUM CONTENT 100", await pipeline.InvokeAsync(100));
            Assert.Equal("FREE CONTENT 50", await pipeline.InvokeAsync(50));
            Assert.Equal("CONTENT NOT FOUND", await pipeline.InvokeAsync(-1));
        }
    }
}
