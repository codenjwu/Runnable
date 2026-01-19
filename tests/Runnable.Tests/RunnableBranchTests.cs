using System;
using System.Threading.Tasks;
using Xunit;

namespace Runnable.Tests
{
    /// <summary>
    /// Unit tests for RunnableBranch functionality
    /// </summary>
    public class RunnableBranchTests
    {
        // ==================== Basic Functionality Tests ====================

        [Fact]
        public void Create_ZeroParameters_RoutesToCorrectBranch()
        {
            // Arrange
            var condition1Met = true;
            var branch1 = RunnableLambda.Create(() => "Branch1");
            var branch2 = RunnableLambda.Create(() => "Branch2");
            var defaultBranch = RunnableLambda.Create(() => "Default");

            var router = RunnableBranch.Create<string>(
                defaultBranch,
                (() => condition1Met, branch1),
                (() => false, branch2)
            );

            // Act
            var result = router.Invoke();

            // Assert
            Assert.Equal("Branch1", result);
        }

        [Fact]
        public void Create_ZeroParameters_UsesDefaultWhenNoMatch()
        {
            // Arrange
            var branch1 = RunnableLambda.Create(() => "Branch1");
            var defaultBranch = RunnableLambda.Create(() => "Default");

            var router = RunnableBranch.Create<string>(
                defaultBranch,
                (() => false, branch1)
            );

            // Act
            var result = router.Invoke();

            // Assert
            Assert.Equal("Default", result);
        }

        [Fact]
        public void Create_OneParameter_RoutesToCorrectBranch()
        {
            // Arrange
            var positive = RunnableLambda.Create<int, string>(x => $"Positive: {x}");
            var negative = RunnableLambda.Create<int, string>(x => $"Negative: {x}");
            var zero = RunnableLambda.Create<int, string>(x => "Zero");

            var router = RunnableBranch.Create<int, string>(
                zero,
                (x => x > 0, positive),
                (x => x < 0, negative)
            );

            // Act & Assert
            Assert.Equal("Positive: 42", router.Invoke(42));
            Assert.Equal("Negative: -7", router.Invoke(-7));
            Assert.Equal("Zero", router.Invoke(0));
        }

        [Fact]
        public void Create_TwoParameters_RoutesToCorrectBranch()
        {
            // Arrange
            var sumHandler = RunnableLambda.Create<int, int, string>(
                (a, b) => $"Sum: {a + b}");
            var productHandler = RunnableLambda.Create<int, int, string>(
                (a, b) => $"Product: {a * b}");
            var defaultHandler = RunnableLambda.Create<int, int, string>(
                (a, b) => "Default");

            var router = RunnableBranch.Create<int, int, string>(
                defaultHandler,
                ((a, b) => a > 0 && b > 0, sumHandler),
                ((a, b) => a < 0 || b < 0, productHandler)
            );

            // Act & Assert
            Assert.Equal("Sum: 15", router.Invoke(10, 5));
            Assert.Equal("Product: -15", router.Invoke(-5, 3));
            Assert.Equal("Default", router.Invoke(0, 0));
        }

        [Fact]
        public void Create_ThreeParameters_RoutesToCorrectBranch()
        {
            // Arrange
            var allPositive = RunnableLambda.Create<int, int, int, string>(
                (a, b, c) => $"All positive: {a + b + c}");
            var hasNegative = RunnableLambda.Create<int, int, int, string>(
                (a, b, c) => $"Has negative: {a * b * c}");
            var defaultBranch = RunnableLambda.Create<int, int, int, string>(
                (a, b, c) => "Default");

            var router = RunnableBranch.Create<int, int, int, string>(
                defaultBranch,
                ((a, b, c) => a > 0 && b > 0 && c > 0, allPositive),
                ((a, b, c) => a < 0 || b < 0 || c < 0, hasNegative)
            );

            // Act & Assert
            Assert.Equal("All positive: 12", router.Invoke(3, 4, 5));
            Assert.Equal("Has negative: -60", router.Invoke(-3, 4, 5));
        }

        [Fact]
        public void Create_EightParameters_RoutesToCorrectBranch()
        {
            // Arrange
            var allEven = RunnableLambda.Create<int, int, int, int, int, int, int, int, string>(
                (a, b, c, d, e, f, g, h) => "All even");
            var defaultBranch = RunnableLambda.Create<int, int, int, int, int, int, int, int, string>(
                (a, b, c, d, e, f, g, h) => "Default");

            var router = RunnableBranch.Create<int, int, int, int, int, int, int, int, string>(
                defaultBranch,
                ((a, b, c, d, e, f, g, h) => 
                    a % 2 == 0 && b % 2 == 0 && c % 2 == 0 && d % 2 == 0 &&
                    e % 2 == 0 && f % 2 == 0 && g % 2 == 0 && h % 2 == 0, allEven)
            );

            // Act & Assert
            Assert.Equal("All even", router.Invoke(2, 4, 6, 8, 10, 12, 14, 16));
            Assert.Equal("Default", router.Invoke(1, 2, 3, 4, 5, 6, 7, 8));
        }

        [Fact]
        public void Create_NineParameters_RoutesToCorrectBranch()
        {
            // Arrange
            var allPositive = RunnableLambda.Create<int, int, int, int, int, int, int, int, int, string>(
                (a, b, c, d, e, f, g, h, i) => "All positive");
            var defaultBranch = RunnableLambda.Create<int, int, int, int, int, int, int, int, int, string>(
                (a, b, c, d, e, f, g, h, i) => "Default");

            var router = RunnableBranch.Create<int, int, int, int, int, int, int, int, int, string>(
                defaultBranch,
                ((a, b, c, d, e, f, g, h, i) => 
                    a > 0 && b > 0 && c > 0 && d > 0 && e > 0 && f > 0 && g > 0 && h > 0 && i > 0, allPositive)
            );

            // Act & Assert
            Assert.Equal("All positive", router.Invoke(1, 2, 3, 4, 5, 6, 7, 8, 9));
            Assert.Equal("Default", router.Invoke(-1, 2, 3, 4, 5, 6, 7, 8, 9));
        }

        [Fact]
        public void Create_SixteenParameters_RoutesToCorrectBranch()
        {
            // Arrange
            var sumGreaterThan100 = RunnableLambda.Create<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, string>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16) => "Sum > 100");
            var defaultBranch = RunnableLambda.Create<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, string>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16) => "Default");

            var router = RunnableBranch.Create<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, string>(
                defaultBranch,
                ((a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16) => 
                    a1 + a2 + a3 + a4 + a5 + a6 + a7 + a8 + a9 + a10 + a11 + a12 + a13 + a14 + a15 + a16 > 100, sumGreaterThan100)
            );

            // Act & Assert
            Assert.Equal("Sum > 100", router.Invoke(10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10)); // 160
            Assert.Equal("Default", router.Invoke(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1)); // 16
        }

        // ==================== First Match Wins Tests ====================

        [Fact]
        public void Create_FirstMatchWins_StopsAtFirstTrueCondition()
        {
            // Arrange
            var executionLog = new System.Collections.Generic.List<string>();

            var branch1 = RunnableLambda.Create<int, string>(x => {
                executionLog.Add("branch1");
                return "Branch1";
            });

            var branch2 = RunnableLambda.Create<int, string>(x => {
                executionLog.Add("branch2");
                return "Branch2";
            });

            var defaultBranch = RunnableLambda.Create<int, string>(x => {
                executionLog.Add("default");
                return "Default";
            });

            var router = RunnableBranch.Create<int, string>(
                defaultBranch,
                (x => x > 0, branch1),  // This will match
                (x => x > 0, branch2)   // This also would match but shouldn't execute
            );

            // Act
            var result = router.Invoke(5);

            // Assert
            Assert.Equal("Branch1", result);
            Assert.Single(executionLog);
            Assert.Equal("branch1", executionLog[0]);
        }

        [Fact]
        public void Create_OrderMatters_SpecificBeforeGeneral()
        {
            // Arrange
            var veryLarge = RunnableLambda.Create<int, string>(x => "Very large (>100)");
            var large = RunnableLambda.Create<int, string>(x => "Large (>10)");
            var small = RunnableLambda.Create<int, string>(x => "Small");

            // Correct order: most specific first
            var correctRouter = RunnableBranch.Create<int, string>(
                small,
                (x => x > 100, veryLarge),  // Most specific
                (x => x > 10, large)        // Less specific
            );

            // Act & Assert
            Assert.Equal("Very large (>100)", correctRouter.Invoke(150));
            Assert.Equal("Large (>10)", correctRouter.Invoke(50));
            Assert.Equal("Small", correctRouter.Invoke(5));
        }

        // ==================== Async Tests ====================

        [Fact]
        public async Task InvokeAsync_OneParameter_RoutesCorrectly()
        {
            // Arrange
            var asyncPositive = RunnableLambda.Create<int, string>(
                x => $"Positive: {x}",
                async x => {
                    await Task.Delay(20);
                    return $"Async Positive: {x}";
                });

            var asyncNegative = RunnableLambda.Create<int, string>(
                x => $"Negative: {x}",
                async x => {
                    await Task.Delay(20);
                    return $"Async Negative: {x}";
                });

            var asyncDefault = RunnableLambda.Create<int, string>(
                x => "Default",
                async x => {
                    await Task.Delay(20);
                    return "Async Default";
                });

            var router = RunnableBranch.Create<int, string>(
                asyncDefault,
                (x => x > 0, asyncPositive),
                (x => x < 0, asyncNegative)
            );

            // Act & Assert
            Assert.Equal("Async Positive: 42", await router.InvokeAsync(42));
            Assert.Equal("Async Negative: -7", await router.InvokeAsync(-7));
            Assert.Equal("Async Default", await router.InvokeAsync(0));
        }

        [Fact]
        public async Task InvokeAsync_TwoParameters_RoutesCorrectly()
        {
            // Arrange
            var asyncSum = RunnableLambda.Create<int, int, string>(
                (a, b) => $"Sum: {a + b}",
                async (a, b) => {
                    await Task.Delay(10);
                    return $"Async Sum: {a + b}";
                });

            var asyncDefault = RunnableLambda.Create<int, int, string>(
                (a, b) => "Default",
                async (a, b) => await Task.FromResult("Async Default"));

            var router = RunnableBranch.Create<int, int, string>(
                asyncDefault,
                ((a, b) => a > 0 && b > 0, asyncSum)
            );

            // Act & Assert
            Assert.Equal("Async Sum: 15", await router.InvokeAsync(10, 5));
            Assert.Equal("Async Default", await router.InvokeAsync(-10, 5));
        }

        // ==================== Classification Tests ====================

        [Fact]
        public void Create_Classification_RoutesCorrectly()
        {
            // Arrange
            var tiny = RunnableLambda.Create<int, string>(x => $"{x} is tiny");
            var small = RunnableLambda.Create<int, string>(x => $"{x} is small");
            var medium = RunnableLambda.Create<int, string>(x => $"{x} is medium");
            var large = RunnableLambda.Create<int, string>(x => $"{x} is large");

            var classifier = RunnableBranch.Create<int, string>(
                large,                              // Default: large
                (x => x >= 0 && x <= 10, tiny),
                (x => x >= 11 && x <= 50, small),
                (x => x >= 51 && x <= 100, medium)
            );

            // Act & Assert
            Assert.Equal("5 is tiny", classifier.Invoke(5));
            Assert.Equal("25 is small", classifier.Invoke(25));
            Assert.Equal("75 is medium", classifier.Invoke(75));
            Assert.Equal("150 is large", classifier.Invoke(150));
        }

        // ==================== User Role Routing Tests ====================

        [Fact]
        public void Create_UserRoleRouting_RoutesCorrectly()
        {
            // Arrange
            var adminHandler = RunnableLambda.Create<string, string>(
                user => $"Admin access for {user}");
            var modHandler = RunnableLambda.Create<string, string>(
                user => $"Moderator access for {user}");
            var userHandler = RunnableLambda.Create<string, string>(
                user => $"User access for {user}");
            var guestHandler = RunnableLambda.Create<string, string>(
                user => $"Guest access for {user}");

            var roleRouter = RunnableBranch.Create<string, string>(
                guestHandler,
                (user => user.StartsWith("admin_"), adminHandler),
                (user => user.StartsWith("mod_"), modHandler),
                (user => user.StartsWith("user_"), userHandler)
            );

            // Act & Assert
            Assert.Equal("Admin access for admin_alice", roleRouter.Invoke("admin_alice"));
            Assert.Equal("Moderator access for mod_bob", roleRouter.Invoke("mod_bob"));
            Assert.Equal("User access for user_charlie", roleRouter.Invoke("user_charlie"));
            Assert.Equal("Guest access for guest123", roleRouter.Invoke("guest123"));
        }

        // ==================== Grade Calculation Tests ====================

        [Fact]
        public void Create_GradeCalculation_RoutesCorrectly()
        {
            // Arrange
            var gradeA = RunnableLambda.Create<int, string>(score => $"Grade A: {score}%");
            var gradeB = RunnableLambda.Create<int, string>(score => $"Grade B: {score}%");
            var gradeC = RunnableLambda.Create<int, string>(score => $"Grade C: {score}%");
            var gradeF = RunnableLambda.Create<int, string>(score => $"Grade F: {score}%");

            var gradeRouter = RunnableBranch.Create<int, string>(
                gradeF,
                (score => score >= 90, gradeA),
                (score => score >= 80, gradeB),
                (score => score >= 70, gradeC)
            );

            // Act & Assert
            Assert.Equal("Grade A: 95%", gradeRouter.Invoke(95));
            Assert.Equal("Grade B: 85%", gradeRouter.Invoke(85));
            Assert.Equal("Grade C: 75%", gradeRouter.Invoke(75));
            Assert.Equal("Grade F: 65%", gradeRouter.Invoke(65));
        }

        // ==================== HTTP Status Code Tests ====================

        [Fact]
        public void Create_HTTPStatusRouting_RoutesCorrectly()
        {
            // Arrange
            var handleSuccess = RunnableLambda.Create<int, string, string>(
                (code, msg) => $"Success ({code}): {msg}");
            var handleRedirect = RunnableLambda.Create<int, string, string>(
                (code, msg) => $"Redirect ({code}): {msg}");
            var handleClientError = RunnableLambda.Create<int, string, string>(
                (code, msg) => $"Client Error ({code}): {msg}");
            var handleServerError = RunnableLambda.Create<int, string, string>(
                (code, msg) => $"Server Error ({code}): {msg}");
            var handleUnknown = RunnableLambda.Create<int, string, string>(
                (code, msg) => $"Unknown ({code}): {msg}");

            var httpRouter = RunnableBranch.Create<int, string, string>(
                handleUnknown,
                ((c, m) => c >= 200 && c < 300, handleSuccess),
                ((c, m) => c >= 300 && c < 400, handleRedirect),
                ((c, m) => c >= 400 && c < 500, handleClientError),
                ((c, m) => c >= 500 && c < 600, handleServerError)
            );

            // Act & Assert
            Assert.Equal("Success (200): OK", httpRouter.Invoke(200, "OK"));
            Assert.Equal("Redirect (301): Moved", httpRouter.Invoke(301, "Moved"));
            Assert.Equal("Client Error (404): Not Found", httpRouter.Invoke(404, "Not Found"));
            Assert.Equal("Server Error (500): Error", httpRouter.Invoke(500, "Error"));
            Assert.Equal("Unknown (999): Custom", httpRouter.Invoke(999, "Custom"));
        }

        // ==================== Composition Tests ====================

        [Fact]
        public void Branch_CanBeComposedWithMap()
        {
            // Arrange
            var positive = RunnableLambda.Create<int, string>(x => $"positive:{x}");
            var negative = RunnableLambda.Create<int, string>(x => $"negative:{x}");
            var zero = RunnableLambda.Create<int, string>(x => "zero");

            var branch = RunnableBranch.Create<int, string>(
                    zero,
                    (x => x > 0, positive),
                    (x => x < 0, negative)
                )
                .Map(result => result.ToUpper());

            // Act & Assert
            Assert.Equal("POSITIVE:42", branch.Invoke(42));
            Assert.Equal("NEGATIVE:-7", branch.Invoke(-7));
            Assert.Equal("ZERO", branch.Invoke(0));
        }

        [Fact]
        public void Branch_CanBeComposedWithTap()
        {
            // Arrange
            var tapped = false;
            var positive = RunnableLambda.Create<int, string>(x => "Positive");
            var defaultBranch = RunnableLambda.Create<int, string>(x => "Default");

            var branch = RunnableBranch.Create<int, string>(
                    defaultBranch,
                    (x => x > 0, positive)
                )
                .Tap(result => tapped = true);

            // Act
            branch.Invoke(5);

            // Assert
            Assert.True(tapped);
        }

        [Fact]
        public void Branch_CanBeComposedWithPipe()
        {
            // Arrange
            var positive = RunnableLambda.Create<int, string>(x => "positive");
            var negative = RunnableLambda.Create<int, string>(x => "negative");
            var zero = RunnableLambda.Create<int, string>(x => "zero");

            var branch = RunnableBranch.Create<int, string>(
                zero,
                (x => x > 0, positive),
                (x => x < 0, negative)
            );

            var toUpper = RunnableLambda.Create<string, string>(s => s.ToUpper());
            var pipeline = branch.Pipe(toUpper);

            // Act & Assert
            Assert.Equal("POSITIVE", pipeline.Invoke(42));
        }

        [Fact]
        public void Branch_CanBeComposedWithRetry()
        {
            // Arrange
            var positive = RunnableLambda.Create<int, string>(x => "Positive");
            var defaultBranch = RunnableLambda.Create<int, string>(x => "Default");

            var branch = RunnableBranch.Create<int, string>(
                    defaultBranch,
                    (x => x > 0, positive)
                )
                .WithRetry(3);

            // Act
            var result = branch.Invoke(5);

            // Assert
            Assert.Equal("Positive", result);
        }

        [Fact]
        public void Branch_CanBeComposedWithFallback()
        {
            // Arrange
            var throwing = RunnableLambda.Create<int, string>(
                x => throw new InvalidOperationException("Error"));
            var defaultBranch = RunnableLambda.Create<int, string>(x => "Default");

            var branch = RunnableBranch.Create<int, string>(
                    defaultBranch,
                    (x => x > 0, throwing)
                )
                .WithFallbackValue("Fallback");

            // Act
            var result = branch.Invoke(5);

            // Assert
            Assert.Equal("Fallback", result);
        }

        // ==================== Edge Cases ====================

        [Fact]
        public void Create_WithNoBranches_UsesDefaultOnly()
        {
            // Arrange
            var defaultBranch = RunnableLambda.Create<int, string>(x => "Default");
            var router = RunnableBranch.Create<int, string>(defaultBranch);

            // Act
            var result = router.Invoke(42);

            // Assert
            Assert.Equal("Default", result);
        }

        [Fact]
        public void Create_WithSingleBranch_WorksCorrectly()
        {
            // Arrange
            var branch = RunnableLambda.Create<int, string>(x => "Branch");
            var defaultBranch = RunnableLambda.Create<int, string>(x => "Default");

            var router = RunnableBranch.Create<int, string>(
                defaultBranch,
                (x => x > 0, branch)
            );

            // Act & Assert
            Assert.Equal("Branch", router.Invoke(5));
            Assert.Equal("Default", router.Invoke(-5));
        }

        [Fact]
        public void Create_AllConditionsFalse_UsesDefault()
        {
            // Arrange
            var branch1 = RunnableLambda.Create<int, string>(x => "Branch1");
            var branch2 = RunnableLambda.Create<int, string>(x => "Branch2");
            var defaultBranch = RunnableLambda.Create<int, string>(x => "Default");

            var router = RunnableBranch.Create<int, string>(
                defaultBranch,
                (x => x > 100, branch1),
                (x => x < -100, branch2)
            );

            // Act
            var result = router.Invoke(50);

            // Assert
            Assert.Equal("Default", result);
        }

        // ==================== Real-World Tests ====================

        [Fact]
        public void RealWorld_EmailValidation_WorksCorrectly()
        {
            // Arrange
            var validEmail = RunnableLambda.Create<string, string>(
                email => $"Valid: {email.ToLower()}");
            var invalidFormat = RunnableLambda.Create<string, string>(
                email => $"Invalid format: {email}");
            var emptyEmail = RunnableLambda.Create<string, string>(
                email => "Email required");
            var suspicious = RunnableLambda.Create<string, string>(
                email => $"Suspicious: {email}");

            var emailRouter = RunnableBranch.Create<string, string>(
                invalidFormat,
                (e => string.IsNullOrWhiteSpace(e), emptyEmail),
                (e => e.Contains("spam"), suspicious),
                (e => e.Contains("@") && e.Contains("."), validEmail)
            );

            // Act & Assert
            Assert.Equal("Valid: alice@example.com", emailRouter.Invoke("alice@example.com"));
            Assert.Equal("Email required", emailRouter.Invoke(""));
            Assert.Equal("Suspicious: spam@test.com", emailRouter.Invoke("spam@test.com"));
            Assert.Equal("Invalid format: invalid", emailRouter.Invoke("invalid"));
        }

        [Fact]
        public void RealWorld_DiscountCalculator_WorksCorrectly()
        {
            // Arrange
            var vipDiscount = RunnableLambda.Create<decimal, int, string, string>(
                (price, qty, type) => {
                    var total = price * qty * 0.8m;
                    return $"VIP: ${total:F2}";
                });

            var memberDiscount = RunnableLambda.Create<decimal, int, string, string>(
                (price, qty, type) => {
                    var total = price * qty * 0.9m;
                    return $"Member: ${total:F2}";
                });

            var bulkDiscount = RunnableLambda.Create<decimal, int, string, string>(
                (price, qty, type) => {
                    var total = price * qty * 0.95m;
                    return $"Bulk: ${total:F2}";
                });

            var noDiscount = RunnableLambda.Create<decimal, int, string, string>(
                (price, qty, type) => {
                    var total = price * qty;
                    return $"Regular: ${total:F2}";
                });

            var discountRouter = RunnableBranch.Create<decimal, int, string, string>(
                noDiscount,
                ((p, q, t) => t == "VIP", vipDiscount),
                ((p, q, t) => t == "Member", memberDiscount),
                ((p, q, t) => q >= 10, bulkDiscount)
            );

            // Act & Assert
            Assert.Equal("VIP: $400.00", discountRouter.Invoke(100m, 5, "VIP"));
            Assert.Equal("Member: $450.00", discountRouter.Invoke(100m, 5, "Member"));
            Assert.Equal("Bulk: $1425.00", discountRouter.Invoke(100m, 15, "Guest"));
            Assert.Equal("Regular: $300.00", discountRouter.Invoke(100m, 3, "Guest"));
        }
    }
}
