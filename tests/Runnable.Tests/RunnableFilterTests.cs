using Runnable;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Runnable.Tests
{
    /// <summary>
    /// Unit tests for Filter extension functionality (0-16 parameters)
    /// </summary>
    public class RunnableFilterTests
    {
        // ==================== Basic Functionality Tests ====================

        [Fact]
        public void Filter_ZeroParameters_WhenPredicateTrue_ExecutesRunnable()
        {
            // Arrange
            var executed = false;
            var runnable = RunnableLambda.Create(() => {
                executed = true;
                return "result";
            });

            var filtered = runnable.Filter(() => true, "default");

            // Act
            var result = filtered.Invoke();

            // Assert
            Assert.True(executed);
            Assert.Equal("result", result);
        }

        [Fact]
        public void Filter_ZeroParameters_WhenPredicateFalse_ReturnsDefault()
        {
            // Arrange
            var executed = false;
            var runnable = RunnableLambda.Create(() => {
                executed = true;
                return "result";
            });

            var filtered = runnable.Filter(() => false, "default");

            // Act
            var result = filtered.Invoke();

            // Assert
            Assert.False(executed);
            Assert.Equal("default", result);
        }

        [Fact]
        public void Filter_OneParameter_WhenPredicateTrue_ExecutesRunnable()
        {
            // Arrange
            var square = RunnableLambda.Create<int, int>(x => x * x);
            var filtered = square.Filter(x => x > 0, -1);

            // Act
            var result = filtered.Invoke(5);

            // Assert
            Assert.Equal(25, result);
        }

        [Fact]
        public void Filter_OneParameter_WhenPredicateFalse_ReturnsDefault()
        {
            // Arrange
            var square = RunnableLambda.Create<int, int>(x => x * x);
            var filtered = square.Filter(x => x > 0, -1);

            // Act
            var result = filtered.Invoke(-5);

            // Assert
            Assert.Equal(-1, result);
        }

        [Fact]
        public void Filter_TwoParameters_WhenPredicateTrue_ExecutesRunnable()
        {
            // Arrange
            var sum = RunnableLambda.Create<int, int, int>((a, b) => a + b);
            var filtered = sum.Filter((a, b) => a > 0 && b > 0, 0);

            // Act
            var result = filtered.Invoke(10, 5);

            // Assert
            Assert.Equal(15, result);
        }

        [Fact]
        public void Filter_TwoParameters_WhenPredicateFalse_ReturnsDefault()
        {
            // Arrange
            var sum = RunnableLambda.Create<int, int, int>((a, b) => a + b);
            var filtered = sum.Filter((a, b) => a > 0 && b > 0, 0);

            // Act
            var result = filtered.Invoke(-10, 5);

            // Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public void Filter_ThreeParameters_WhenPredicateTrue_ExecutesRunnable()
        {
            // Arrange
            var sum = RunnableLambda.Create<int, int, int, int>((a, b, c) => a + b + c);
            var filtered = sum.Filter((a, b, c) => a > 0 && b > 0 && c > 0, -1);

            // Act
            var result = filtered.Invoke(1, 2, 3);

            // Assert
            Assert.Equal(6, result);
        }

        [Fact]
        public void Filter_ThreeParameters_WhenPredicateFalse_ReturnsDefault()
        {
            // Arrange
            var sum = RunnableLambda.Create<int, int, int, int>((a, b, c) => a + b + c);
            var filtered = sum.Filter((a, b, c) => a > 0 && b > 0 && c > 0, -1);

            // Act
            var result = filtered.Invoke(-1, 2, 3);

            // Assert
            Assert.Equal(-1, result);
        }

        [Fact]
        public void Filter_FourParameters_WorksCorrectly()
        {
            // Arrange
            var sum = RunnableLambda.Create<int, int, int, int, int>(
                (a, b, c, d) => a + b + c + d);
            var filtered = sum.Filter(
                (a, b, c, d) => a + b + c + d > 10, 0);

            // Act & Assert
            Assert.Equal(20, filtered.Invoke(5, 5, 5, 5));  // 20 > 10, executes
            Assert.Equal(0, filtered.Invoke(1, 1, 1, 1));   // 4 <= 10, default
        }

        [Fact]
        public void Filter_EightParameters_WorksCorrectly()
        {
            // Arrange
            var sum = RunnableLambda.Create<int, int, int, int, int, int, int, int, int>(
                (a, b, c, d, e, f, g, h) => a + b + c + d + e + f + g + h);
            var filtered = sum.Filter(
                (a, b, c, d, e, f, g, h) => a + b + c + d + e + f + g + h > 20, -1);

            // Act & Assert
            Assert.Equal(36, filtered.Invoke(1, 2, 3, 4, 5, 6, 7, 8));  // 36 > 20
            Assert.Equal(-1, filtered.Invoke(1, 1, 1, 1, 1, 1, 1, 1));  // 8 <= 20
        }

        [Fact]
        public void Filter_NineParameters_WorksCorrectly()
        {
            // Arrange
            var sum = RunnableLambda.Create<int, int, int, int, int, int, int, int, int, int>(
                (a, b, c, d, e, f, g, h, i) => a + b + c + d + e + f + g + h + i);
            var filtered = sum.Filter(
                (a, b, c, d, e, f, g, h, i) => a + b + c + d + e + f + g + h + i > 30, 0);

            // Act & Assert
            Assert.Equal(45, filtered.Invoke(1, 2, 3, 4, 5, 6, 7, 8, 9));  // 45 > 30
            Assert.Equal(0, filtered.Invoke(1, 1, 1, 1, 1, 1, 1, 1, 1));   // 9 <= 30
        }

        [Fact]
        public void Filter_SixteenParameters_WorksCorrectly()
        {
            // Arrange
            var sum = RunnableLambda.Create<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16) =>
                    a1 + a2 + a3 + a4 + a5 + a6 + a7 + a8 + a9 + a10 + a11 + a12 + a13 + a14 + a15 + a16);
            var filtered = sum.Filter(
                (a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16) =>
                    a1 + a2 + a3 + a4 + a5 + a6 + a7 + a8 + a9 + a10 + a11 + a12 + a13 + a14 + a15 + a16 > 100, 
                -1);

            // Act & Assert
            Assert.Equal(136, filtered.Invoke(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16));  // 136 > 100
            Assert.Equal(-1, filtered.Invoke(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1));          // 16 <= 100
        }

        // ==================== Async Tests ====================

        [Fact]
        public async Task Filter_Async_OneParameter_WhenPredicateTrue_ExecutesRunnable()
        {
            // Arrange
            var runnable = RunnableLambda.Create<int, int>(
                x => x * 2,
                async x => {
                    await Task.Delay(10);
                    return x * 2;
                });
            var filtered = runnable.Filter(x => x > 0, -1);

            // Act
            var result = await filtered.InvokeAsync(5);

            // Assert
            Assert.Equal(10, result);
        }

        [Fact]
        public async Task Filter_Async_OneParameter_WhenPredicateFalse_ReturnsDefault()
        {
            // Arrange
            var executed = false;
            var runnable = RunnableLambda.Create<int, int>(
                x => x * 2,
                async x => {
                    executed = true;
                    await Task.Delay(10);
                    return x * 2;
                });
            var filtered = runnable.Filter(x => x > 0, -1);

            // Act
            var result = await filtered.InvokeAsync(-5);

            // Assert
            Assert.False(executed);
            Assert.Equal(-1, result);
        }

        [Fact]
        public async Task Filter_Async_TwoParameters_WorksCorrectly()
        {
            // Arrange
            var runnable = RunnableLambda.Create<int, int, string>(
                (a, b) => $"Sum: {a + b}",
                async (a, b) => {
                    await Task.Delay(10);
                    return $"Async Sum: {a + b}";
                });
            var filtered = runnable.Filter((a, b) => a > 0 && b > 0, "default");

            // Act
            var result1 = await filtered.InvokeAsync(10, 5);
            var result2 = await filtered.InvokeAsync(-10, 5);

            // Assert
            Assert.Equal("Async Sum: 15", result1);
            Assert.Equal("default", result2);
        }

        // ==================== Default Value Tests ====================

        [Fact]
        public void Filter_WithNullDefaultValue_ReturnsNull()
        {
            // Arrange
            var runnable = RunnableLambda.Create<int, string>(x => "result");
            var filtered = runnable.Filter(x => x > 0, null);

            // Act
            var result = filtered.Invoke(-5);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void Filter_WithoutExplicitDefault_UsesDefaultOfType()
        {
            // Arrange
            var runnable = RunnableLambda.Create<int, int>(x => x * 2);
            var filtered = runnable.Filter(x => x > 0);  // No default specified

            // Act
            var result = filtered.Invoke(-5);

            // Assert
            Assert.Equal(0, result);  // default(int) is 0
        }

        [Fact]
        public void Filter_WithCustomDefaultValue_ReturnsCustomValue()
        {
            // Arrange
            var runnable = RunnableLambda.Create<int, string>(x => x.ToString());
            var filtered = runnable.Filter(x => x > 0, "FILTERED");

            // Act
            var result = filtered.Invoke(-5);

            // Assert
            Assert.Equal("FILTERED", result);
        }

        // ==================== Composition Tests ====================

        [Fact]
        public void Filter_CanBeComposedWithMap()
        {
            // Arrange
            var runnable = RunnableLambda.Create<int, int>(x => x * 2);
            var pipeline = runnable
                .Filter(x => x > 0, -1)
                .Map(x => x.ToString());

            // Act
            var result1 = pipeline.Invoke(5);
            var result2 = pipeline.Invoke(-5);

            // Assert
            Assert.Equal("10", result1);
            Assert.Equal("-1", result2);
        }

        [Fact]
        public void Filter_CanBeComposedWithTap()
        {
            // Arrange
            var tapped = false;
            var runnable = RunnableLambda.Create<int, int>(x => x * 2);
            var pipeline = runnable
                .Filter(x => x > 0, -1)
                .Tap(x => tapped = true);

            // Act
            pipeline.Invoke(5);

            // Assert
            Assert.True(tapped);
        }

        [Fact]
        public void Filter_CanBeChained()
        {
            // Arrange
            var runnable = RunnableLambda.Create<int, int>(x => x * 2);
            var pipeline = runnable
                .Filter(x => x > 0, -1)      // First filter: must be positive
                .Filter(x => x < 100, -2);   // Second filter: must be < 100

            // Act & Assert
            Assert.Equal(10, pipeline.Invoke(5));    // Passes both filters
            Assert.Equal(-1, pipeline.Invoke(-5));   // Fails first filter
            Assert.Equal(-2, pipeline.Invoke(200));  // Fails second filter
        }

        [Fact]
        public void Filter_WithPipe_WorksCorrectly()
        {
            // Arrange
            var double_ = RunnableLambda.Create<int, int>(x => x * 2);
            var toString = RunnableLambda.Create<int, string>(x => x.ToString());
            
            var pipeline = double_
                .Filter(x => x > 0, -1)
                .Pipe(toString);

            // Act
            var result1 = pipeline.Invoke(5);
            var result2 = pipeline.Invoke(-5);

            // Assert
            Assert.Equal("10", result1);
            Assert.Equal("-1", result2);
        }

        // ==================== Real-World Scenarios ====================

        [Fact]
        public void RealWorld_ValidateAndProcess_WorksCorrectly()
        {
            // Arrange
            var processEmail = RunnableLambda.Create<string, string>(
                email => $"Processed: {email.ToLower()}");

            var validatedProcessor = processEmail.Filter(
                email => email.Contains("@") && email.Contains("."),
                "INVALID_EMAIL");

            // Act & Assert
            Assert.Equal("Processed: alice@example.com", 
                validatedProcessor.Invoke("alice@example.com"));
            Assert.Equal("INVALID_EMAIL", 
                validatedProcessor.Invoke("invalid-email"));
        }

        [Fact]
        public void RealWorld_ConditionalCalculation_WorksCorrectly()
        {
            // Arrange
            var calculateDiscount = RunnableLambda.Create<decimal, int, decimal>(
                (price, quantity) => price * quantity * 0.1m);

            var discountWithMinimum = calculateDiscount.Filter(
                (price, quantity) => price * quantity >= 100m,
                0m);  // No discount if total < 100

            // Act & Assert
            Assert.Equal(15m, discountWithMinimum.Invoke(50m, 3));   // 150 >= 100: 15 discount
            Assert.Equal(0m, discountWithMinimum.Invoke(10m, 5));    // 50 < 100: no discount
        }

        [Fact]
        public void RealWorld_AgeBasedProcessing_WorksCorrectly()
        {
            // Arrange
            var processAdult = RunnableLambda.Create<string, int, string>(
                (name, age) => $"{name} is an adult ({age} years old)");

            var adultOnlyProcessor = processAdult.Filter(
                (name, age) => age >= 18,
                "Not eligible");

            // Act & Assert
            Assert.Equal("Alice is an adult (25 years old)", 
                adultOnlyProcessor.Invoke("Alice", 25));
            Assert.Equal("Not eligible", 
                adultOnlyProcessor.Invoke("Bob", 15));
        }

        [Fact]
        public void RealWorld_ConditionalDataTransformation_WorksCorrectly()
        {
            // Arrange
            var transformData = RunnableLambda.Create<int, string, string>(
                (id, data) => $"Transformed-{id}-{data.ToUpper()}");

            var conditionalTransform = transformData.Filter(
                (id, data) => !string.IsNullOrWhiteSpace(data),
                "EMPTY_DATA");

            // Act & Assert
            Assert.Equal("Transformed-123-HELLO", 
                conditionalTransform.Invoke(123, "hello"));
            Assert.Equal("EMPTY_DATA", 
                conditionalTransform.Invoke(123, ""));
            Assert.Equal("EMPTY_DATA", 
                conditionalTransform.Invoke(123, null));
        }

        // ==================== Edge Cases ====================

        [Fact]
        public void Filter_WithAlwaysTruePredicate_AlwaysExecutes()
        {
            // Arrange
            var runnable = RunnableLambda.Create<int, int>(x => x * 2);
            var filtered = runnable.Filter(x => true, -1);

            // Act & Assert
            Assert.Equal(10, filtered.Invoke(5));
            Assert.Equal(-10, filtered.Invoke(-5));
        }

        [Fact]
        public void Filter_WithAlwaysFalsePredicate_NeverExecutes()
        {
            // Arrange
            var executed = false;
            var runnable = RunnableLambda.Create<int, int>(x => {
                executed = true;
                return x * 2;
            });
            var filtered = runnable.Filter(x => false, -1);

            // Act
            var result = filtered.Invoke(5);

            // Assert
            Assert.False(executed);
            Assert.Equal(-1, result);
        }

        [Fact]
        public void Filter_WithComplexPredicate_WorksCorrectly()
        {
            // Arrange
            var process = RunnableLambda.Create<int, int, int, int>(
                (a, b, c) => a + b + c);

            var complexFiltered = process.Filter(
                (a, b, c) => {
                    var sum = a + b + c;
                    return sum > 10 && sum < 100 && sum % 2 == 0;
                },
                -1);

            // Act & Assert
            Assert.Equal(12, complexFiltered.Invoke(4, 4, 4));   // 12: even, 10 < 12 < 100
            Assert.Equal(-1, complexFiltered.Invoke(1, 1, 1));   // 3: < 10
            Assert.Equal(-1, complexFiltered.Invoke(50, 50, 1)); // 101: > 100
            Assert.Equal(-1, complexFiltered.Invoke(3, 3, 3));   // 9: odd
        }

        [Fact]
        public void Filter_PreservesTypeInformation()
        {
            // Arrange
            var runnable = RunnableLambda.Create<string, int>(s => s.Length);
            var filtered = runnable.Filter(s => !string.IsNullOrEmpty(s), 0);

            // Act
            var result = filtered.Invoke("Hello");

            // Assert
            Assert.IsType<int>(result);
            Assert.Equal(5, result);
        }
    }
}
