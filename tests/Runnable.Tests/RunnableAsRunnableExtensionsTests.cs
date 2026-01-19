using Runnable;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Runnable.Tests
{
    /// <summary>
    /// Unit tests for AsRunnable extension methods (0-16 parameters)
    /// Tests conversion of Func delegates to Runnable instances
    /// </summary>
    public class RunnableAsRunnableExtensionsTests
    {
        // ==================== 0 Parameters Tests ====================

        [Fact]
        public void AsRunnable_ZeroParameters_Sync_CreatesRunnableFromFunc()
        {
            // Arrange
            Func<string> func = () => "Hello, World!";

            // Act
            var runnable = func.AsRunnable();
            var result = runnable.Invoke();

            // Assert
            Assert.Equal("Hello, World!", result);
        }

        [Fact]
        public async Task AsRunnable_ZeroParameters_SyncAndAsync_ExecutesBothVersions()
        {
            // Arrange
            Func<int> syncFunc = () => 42;
            Func<Task<int>> asyncFunc = async () => {
                await Task.Delay(10);
                return 84;
            };

            // Act
            var runnable = syncFunc.AsRunnable(asyncFunc);
            var syncResult = runnable.Invoke();
            var asyncResult = await runnable.InvokeAsync();

            // Assert
            Assert.Equal(42, syncResult);
            Assert.Equal(84, asyncResult);
        }

        [Fact]
        public void AsRunnable_ZeroParameters_CanBeChained()
        {
            // Arrange
            Func<int> func = () => 10;

            // Act
            var result = func.AsRunnable()
                .Map(x => x * 2)
                .Map(x => x + 5)
                .Invoke();

            // Assert
            Assert.Equal(25, result);
        }

        // ==================== 1 Parameter Tests ====================

        [Fact]
        public void AsRunnable_OneParameter_Sync_CreatesRunnableFromFunc()
        {
            // Arrange
            Func<int, string> func = x => $"Value: {x}";

            // Act
            var runnable = func.AsRunnable();
            var result = runnable.Invoke(42);

            // Assert
            Assert.Equal("Value: 42", result);
        }

        [Fact]
        public async Task AsRunnable_OneParameter_SyncAndAsync_ExecutesBothVersions()
        {
            // Arrange
            Func<int, int> syncFunc = x => x * 2;
            Func<int, Task<int>> asyncFunc = async x => {
                await Task.Delay(10);
                return x * 3;
            };

            // Act
            var runnable = syncFunc.AsRunnable(asyncFunc);
            var syncResult = runnable.Invoke(5);
            var asyncResult = await runnable.InvokeAsync(5);

            // Assert
            Assert.Equal(10, syncResult);
            Assert.Equal(15, asyncResult);
        }

        [Fact]
        public void AsRunnable_OneParameter_CanBeChained()
        {
            // Arrange
            Func<int, int> func = x => x * 2;

            // Act
            var result = func.AsRunnable()
                .Map(x => x + 10)
                .Map(x => x.ToString())
                .Invoke(5);

            // Assert
            Assert.Equal("20", result);
        }

        [Fact]
        public void AsRunnable_OneParameter_PreservesType()
        {
            // Arrange
            Func<string, int> func = s => s.Length;

            // Act
            var runnable = func.AsRunnable();
            var result = runnable.Invoke("Hello");

            // Assert
            Assert.Equal(5, result);
            Assert.IsAssignableFrom<IRunnable<string, int>>(runnable);
        }

        // ==================== 2 Parameters Tests ====================

        [Fact]
        public void AsRunnable_TwoParameters_Sync_CreatesRunnableFromFunc()
        {
            // Arrange
            Func<int, int, int> func = (a, b) => a + b;

            // Act
            var runnable = func.AsRunnable();
            var result = runnable.Invoke(10, 20);

            // Assert
            Assert.Equal(30, result);
        }

        [Fact]
        public async Task AsRunnable_TwoParameters_SyncAndAsync_ExecutesBothVersions()
        {
            // Arrange
            Func<int, int, int> syncFunc = (a, b) => a + b;
            Func<int, int, Task<int>> asyncFunc = async (a, b) => {
                await Task.Delay(10);
                return a * b;
            };

            // Act
            var runnable = syncFunc.AsRunnable(asyncFunc);
            var syncResult = runnable.Invoke(5, 3);
            var asyncResult = await runnable.InvokeAsync(5, 3);

            // Assert
            Assert.Equal(8, syncResult);
            Assert.Equal(15, asyncResult);
        }

        [Fact]
        public void AsRunnable_TwoParameters_CanBeChained()
        {
            // Arrange
            Func<int, int, int> func = (a, b) => a + b;

            // Act
            var result = func.AsRunnable()
                .Map(sum => sum * 2)
                .Map(doubled => $"Result: {doubled}")
                .Invoke(5, 10);

            // Assert
            Assert.Equal("Result: 30", result);
        }

        // ==================== 3 Parameters Tests ====================

        [Fact]
        public void AsRunnable_ThreeParameters_Sync_CreatesRunnableFromFunc()
        {
            // Arrange
            Func<int, int, int, int> func = (a, b, c) => a + b + c;

            // Act
            var runnable = func.AsRunnable();
            var result = runnable.Invoke(10, 20, 30);

            // Assert
            Assert.Equal(60, result);
        }

        [Fact]
        public async Task AsRunnable_ThreeParameters_SyncAndAsync_ExecutesBothVersions()
        {
            // Arrange
            Func<int, int, int, string> syncFunc = (a, b, c) => $"{a},{b},{c}";
            Func<int, int, int, Task<string>> asyncFunc = async (a, b, c) => {
                await Task.Delay(10);
                return $"{a}+{b}+{c}";
            };

            // Act
            var runnable = syncFunc.AsRunnable(asyncFunc);
            var syncResult = runnable.Invoke(1, 2, 3);
            var asyncResult = await runnable.InvokeAsync(1, 2, 3);

            // Assert
            Assert.Equal("1,2,3", syncResult);
            Assert.Equal("1+2+3", asyncResult);
        }

        // ==================== 4 Parameters Tests ====================

        [Fact]
        public void AsRunnable_FourParameters_Sync_CreatesRunnableFromFunc()
        {
            // Arrange
            Func<int, int, int, int, int> func = (a, b, c, d) => a + b + c + d;

            // Act
            var runnable = func.AsRunnable();
            var result = runnable.Invoke(1, 2, 3, 4);

            // Assert
            Assert.Equal(10, result);
        }

        [Fact]
        public async Task AsRunnable_FourParameters_Async_WorksCorrectly()
        {
            // Arrange
            Func<int, int, int, int, int> syncFunc = (a, b, c, d) => a * b * c * d;
            Func<int, int, int, int, Task<int>> asyncFunc = async (a, b, c, d) => {
                await Task.Delay(10);
                return a * b * c * d;
            };

            // Act
            var runnable = syncFunc.AsRunnable(asyncFunc);
            var result = await runnable.InvokeAsync(2, 3, 4, 5);

            // Assert
            Assert.Equal(120, result);
        }

        // ==================== 5 Parameters Tests ====================

        [Fact]
        public void AsRunnable_FiveParameters_Sync_CreatesRunnableFromFunc()
        {
            // Arrange
            Func<int, int, int, int, int, int> func = (a, b, c, d, e) => a + b + c + d + e;

            // Act
            var runnable = func.AsRunnable();
            var result = runnable.Invoke(1, 2, 3, 4, 5);

            // Assert
            Assert.Equal(15, result);
        }

        // ==================== Higher Parameter Count Tests ====================

        [Fact]
        public void AsRunnable_TenParameters_Sync_CreatesRunnableFromFunc()
        {
            // Arrange
            Func<int, int, int, int, int, int, int, int, int, int, int> func = 
                (a, b, c, d, e, f, g, h, i, j) => a + b + c + d + e + f + g + h + i + j;

            // Act
            var runnable = func.AsRunnable();
            var result = runnable.Invoke(1, 2, 3, 4, 5, 6, 7, 8, 9, 10);

            // Assert
            Assert.Equal(55, result);
        }

        [Fact]
        public async Task AsRunnable_TenParameters_Async_WorksCorrectly()
        {
            // Arrange
            Func<int, int, int, int, int, int, int, int, int, int, string> syncFunc = 
                (a, b, c, d, e, f, g, h, i, j) => $"Sum: {a + b + c + d + e + f + g + h + i + j}";
            Func<int, int, int, int, int, int, int, int, int, int, Task<string>> asyncFunc = 
                async (a, b, c, d, e, f, g, h, i, j) => {
                    await Task.Delay(10);
                    return $"Sum: {a + b + c + d + e + f + g + h + i + j}";
                };

            // Act
            var runnable = syncFunc.AsRunnable(asyncFunc);
            var result = await runnable.InvokeAsync(1, 2, 3, 4, 5, 6, 7, 8, 9, 10);

            // Assert
            Assert.Equal("Sum: 55", result);
        }

        [Fact]
        public void AsRunnable_SixteenParameters_Sync_CreatesRunnableFromFunc()
        {
            // Arrange
            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int> func = 
                (a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p) => 
                    a + b + c + d + e + f + g + h + i + j + k + l + m + n + o + p;

            // Act
            var runnable = func.AsRunnable();
            var result = runnable.Invoke(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);

            // Assert
            Assert.Equal(136, result);
        }

        // ==================== Complex Type Tests ====================

        [Fact]
        public void AsRunnable_WithComplexTypes_WorksCorrectly()
        {
            // Arrange
            Func<string, int, DateTime, string> func = (name, age, date) => 
                $"{name} is {age} years old, born on {date:yyyy-MM-dd}";

            // Act
            var runnable = func.AsRunnable();
            var result = runnable.Invoke("Alice", 30, new DateTime(1993, 5, 15));

            // Assert
            Assert.Equal("Alice is 30 years old, born on 1993-05-15", result);
        }

        [Fact]
        public void AsRunnable_WithReferenceTypes_HandlesNull()
        {
            // Arrange
            Func<string?, string> func = s => s?.ToUpper() ?? "NULL";

            // Act
            var runnable = func.AsRunnable();
            var result1 = runnable.Invoke("hello");
            var result2 = runnable.Invoke(null!);

            // Assert
            Assert.Equal("HELLO", result1);
            Assert.Equal("NULL", result2);
        }

        // ==================== Integration with Other Extensions ====================

        [Fact]
        public void AsRunnable_IntegrationWithMap_WorksInPipeline()
        {
            // Arrange
            Func<int, int> add10 = x => x + 10;
            Func<int, int> multiply2 = x => x * 2;

            // Act
            var result = add10.AsRunnable()
                .Pipe(multiply2.AsRunnable())
                .Map(x => x.ToString())
                .Invoke(5);

            // Assert
            Assert.Equal("30", result);
        }

        [Fact]
        public void AsRunnable_IntegrationWithTap_WorksInPipeline()
        {
            // Arrange
            Func<int, int> func = x => x * 2;
            var sideEffectValue = 0;

            // Act
            var result = func.AsRunnable()
                .Tap(x => sideEffectValue = x)
                .Invoke(10);

            // Assert
            Assert.Equal(20, result);
            Assert.Equal(20, sideEffectValue);
        }

        [Fact]
        public void AsRunnable_IntegrationWithFilter_WorksInPipeline()
        {
            // Arrange
            Func<int, int> func = x => x * 2;

            // Act - Filter based on input parameter
            var runnable1 = func.AsRunnable();
            var result1 = runnable1
                .Filter((input) => input > 5)  // Predicate on input
                .Invoke(10); // 10 > 5, passes filter, returns 10 * 2 = 20

            var runnable2 = func.AsRunnable();
            var result2 = runnable2
                .Filter((input) => input > 5)  // Predicate on input
                .Invoke(3); // 3 <= 5, fails filter, returns default(int) = 0

            // Assert
            Assert.Equal(20, result1);
            Assert.Equal(0, result2);
        }

        [Fact]
        public void AsRunnable_IntegrationWithPipe_WorksInPipeline()
        {
            // Arrange
            Func<int, int> double_ = x => x * 2;
            Func<int, string> toString = x => $"Value: {x}";

            // Act
            var result = double_.AsRunnable()
                .Pipe(toString.AsRunnable())
                .Invoke(10);

            // Assert
            Assert.Equal("Value: 20", result);
        }

        // ==================== Error Handling Tests ====================

        [Fact]
        public void AsRunnable_WithException_ThrowsCorrectly()
        {
            // Arrange
            Func<int, int> func = x => throw new InvalidOperationException("Test exception");

            // Act
            var runnable = func.AsRunnable();

            // Assert
            Assert.Throws<InvalidOperationException>(() => runnable.Invoke(5));
        }

        [Fact]
        public async Task AsRunnable_AsyncWithException_ThrowsCorrectly()
        {
            // Arrange
            Func<int, int> syncFunc = x => x;
            Func<int, Task<int>> asyncFunc = x => throw new InvalidOperationException("Async test exception");

            // Act
            var runnable = syncFunc.AsRunnable(asyncFunc);

            // Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => runnable.InvokeAsync(5));
        }

        // ==================== Performance Tests ====================

        [Fact]
        public void AsRunnable_RepeatedInvocations_ConsistentResults()
        {
            // Arrange
            var counter = 0;
            Func<int, int> func = x => {
                counter++;
                return x * 2;
            };
            var runnable = func.AsRunnable();

            // Act
            var result1 = runnable.Invoke(5);
            var result2 = runnable.Invoke(5);
            var result3 = runnable.Invoke(5);

            // Assert
            Assert.Equal(10, result1);
            Assert.Equal(10, result2);
            Assert.Equal(10, result3);
            Assert.Equal(3, counter); // Should be invoked 3 times (no caching)
        }

        [Fact]
        public async Task AsRunnable_AsyncRepeatedInvocations_ConsistentResults()
        {
            // Arrange
            var counter = 0;
            Func<int, int> syncFunc = x => x;
            Func<int, Task<int>> asyncFunc = async x => {
                counter++;
                await Task.Delay(10);
                return x * 2;
            };
            var runnable = syncFunc.AsRunnable(asyncFunc);

            // Act
            var result1 = await runnable.InvokeAsync(5);
            var result2 = await runnable.InvokeAsync(5);
            var result3 = await runnable.InvokeAsync(5);

            // Assert
            Assert.Equal(10, result1);
            Assert.Equal(10, result2);
            Assert.Equal(10, result3);
            Assert.Equal(3, counter); // Should be invoked 3 times (no caching)
        }

        // ==================== Real-World Scenarios ====================

        [Fact]
        public void AsRunnable_StringManipulation_RealWorldScenario()
        {
            // Arrange: Convert a user registration function to runnable
            Func<string, string, string> createUserEmail = (firstName, lastName) => 
                $"{firstName.ToLower()}.{lastName.ToLower()}@company.com";

            // Act
            var result = createUserEmail.AsRunnable()
                .Map(email => email.Trim())
                .Invoke("John", "Doe");

            // Assert
            Assert.Equal("john.doe@company.com", result);
        }

        [Fact]
        public async Task AsRunnable_DataProcessing_RealWorldScenario()
        {
            // Arrange: Simulate data processing pipeline
            Func<int, int> calculateDiscount = amount => amount >= 100 ? amount * 90 / 100 : amount;
            Func<int, Task<int>> applyTaxAsync = async amount => {
                await Task.Delay(5); // Simulate async tax calculation
                return amount * 110 / 100;
            };

            // Act
            var pipeline = calculateDiscount.AsRunnable(applyTaxAsync);
            var result1 = await pipeline.InvokeAsync(150); // With discount: 150 * 0.9 * 1.1 = 148.5 (int division = 165)
            var result2 = await pipeline.InvokeAsync(50);  // No discount: 50 * 1.1 = 55

            // Assert
            Assert.Equal(165, result1); // 150 * 110 / 100 = 165
            Assert.Equal(55, result2);
        }

        [Fact]
        public void AsRunnable_ValidationPipeline_RealWorldScenario()
        {
            // Arrange: Email validation pipeline
            Func<string?, bool> validateEmail = email => 
                !string.IsNullOrEmpty(email) && email.Contains("@") && email.Contains(".");

            // Act
            var validator = validateEmail.AsRunnable()
                .Map(isValid => isValid ? "Valid" : "Invalid");

            var result1 = validator.Invoke("user@example.com");
            var result2 = validator.Invoke("invalid-email");
            var result3 = validator.Invoke(null!);

            // Assert
            Assert.Equal("Valid", result1);
            Assert.Equal("Invalid", result2);
            Assert.Equal("Invalid", result3);
        }

        [Fact]
        public void AsRunnable_CalculationPipeline_RealWorldScenario()
        {
            // Arrange: Mathematical calculation pipeline
            Func<double, double, double, double> calculateBMI = (weight, height, age) => {
                var bmi = weight / (height * height);
                var ageFactor = age > 40 ? 1.1 : 1.0;
                return bmi * ageFactor;
            };

            // Act
            var bmiCalculator = calculateBMI.AsRunnable()
                .Map(bmi => Math.Round(bmi, 2))
                .Map(bmi => $"BMI: {bmi}");

            var result1 = bmiCalculator.Invoke(70, 1.75, 30);  // Young person
            var result2 = bmiCalculator.Invoke(70, 1.75, 45);  // Older person

            // Assert
            Assert.Equal("BMI: 22.86", result1);
            Assert.Equal("BMI: 25.14", result2);
        }

        // ==================== Null Validation Tests ====================

        [Fact]
        public void AsRunnable_NullFunc_ThrowsArgumentNullException()
        {
            // Arrange
            Func<int> nullFunc = null!;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => nullFunc.AsRunnable());
        }

        [Fact]
        public void AsRunnable_NullSyncFunc_ThrowsArgumentNullException()
        {
            // Arrange
            Func<int> nullSync = null!;
            Func<Task<int>> asyncFunc = async () => await Task.FromResult(42);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => nullSync.AsRunnable(asyncFunc));
        }

        [Fact]
        public void AsRunnable_NullAsyncFunc_ThrowsArgumentNullException()
        {
            // Arrange
            Func<int> syncFunc = () => 42;
            Func<Task<int>> nullAsync = null!;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => syncFunc.AsRunnable(nullAsync));
        }

        [Fact]
        public void AsRunnable_OneParameter_NullFunc_ThrowsArgumentNullException()
        {
            // Arrange
            Func<int, string> nullFunc = null!;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => nullFunc.AsRunnable());
        }

        // ==================== Async-Only AsRunnableAsync Tests ====================

        [Fact]
        public async Task AsRunnableAsync_ZeroParameters_WorksForAsyncOnly()
        {
            // Arrange
            Func<Task<int>> asyncOnlyFunc = async () => {
                await Task.Delay(10);
                return 42;
            };

            // Act
            var runnable = asyncOnlyFunc.AsRunnableAsync();
            var result = await runnable.InvokeAsync();

            // Assert
            Assert.Equal(42, result);
        }

        [Fact]
        public void AsRunnableAsync_ZeroParameters_ThrowsForSync()
        {
            // Arrange
            Func<Task<int>> asyncOnlyFunc = async () => await Task.FromResult(42);
            var runnable = asyncOnlyFunc.AsRunnableAsync();

            // Act & Assert
            var ex = Assert.Throws<NotSupportedException>(() => runnable.Invoke());
            Assert.Contains("async execution", ex.Message);
            Assert.Contains("InvokeAsync", ex.Message);
        }

        [Fact]
        public async Task AsRunnableAsync_OneParameter_WorksForAsyncOnly()
        {
            // Arrange
            Func<int, Task<string>> asyncOnlyFunc = async x => {
                await Task.Delay(10);
                return $"Value: {x}";
            };

            // Act
            var runnable = asyncOnlyFunc.AsRunnableAsync();
            var result = await runnable.InvokeAsync(42);

            // Assert
            Assert.Equal("Value: 42", result);
        }

        [Fact]
        public void AsRunnableAsync_OneParameter_ThrowsForSync()
        {
            // Arrange
            Func<int, Task<string>> asyncOnlyFunc = async x => await Task.FromResult(x.ToString());
            var runnable = asyncOnlyFunc.AsRunnableAsync();

            // Act & Assert
            Assert.Throws<NotSupportedException>(() => runnable.Invoke(42));
        }

        [Fact]
        public void AsRunnableAsync_NullAsyncFunc_ThrowsArgumentNullException()
        {
            // Arrange
            Func<Task<int>> nullAsync = null!;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => nullAsync.AsRunnableAsync());
        }

        [Fact]
        public async Task AsRunnableAsync_CanBeChainedWithOtherExtensions()
        {
            // Arrange
            Func<Task<int>> asyncFunc = async () => {
                await Task.Delay(5);
                return 42;
            };

            // Act
            var result = await asyncFunc.AsRunnableAsync()
                .MapAsync(async x => {
                    await Task.Delay(5);
                    return x * 2;
                })
                .InvokeAsync();

            // Assert
            Assert.Equal(84, result);
        }

        // ==================== Action Support Tests ====================

        [Fact]
        public void AsRunnable_Action_ExecutesSideEffect()
        {
            // Arrange
            var counter = 0;
            Action incrementAction = () => counter++;

            // Act
            var runnable = incrementAction.AsRunnable();
            var result = runnable.Invoke();

            // Assert
            Assert.Equal(Unit.Default, result);
            Assert.Equal(1, counter);
        }

        [Fact]
        public void AsRunnable_Action_CanBeChained()
        {
            // Arrange
            var log = new System.Collections.Generic.List<string>();
            Action log1 = () => log.Add("First");
            Action log2 = () => log.Add("Second");

            // Act
            var result = log1.AsRunnable()
                .Tap(_ => log2())
                .Map(_ => log.Count)
                .Invoke();

            // Assert
            Assert.Equal(2, result);
            Assert.Equal(new[] { "First", "Second" }, log);
        }

        [Fact]
        public void AsRunnable_Action_NullThrowsArgumentNullException()
        {
            // Arrange
            Action nullAction = null!;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => nullAction.AsRunnable());
        }

        [Fact]
        public void AsRunnable_ActionT1_ExecutesSideEffect()
        {
            // Arrange
            var values = new System.Collections.Generic.List<int>();
            Action<int> addValue = x => values.Add(x);

            // Act
            var runnable = addValue.AsRunnable();
            var result = runnable.Invoke(42);

            // Assert
            Assert.Equal(Unit.Default, result);
            Assert.Contains(42, values);
        }

        [Fact]
        public void AsRunnable_ActionT1_CanBeChainedInPipeline()
        {
            // Arrange
            var output = 0;
            Action<int> setOutput = x => output = x;

            // Act
            var result = setOutput.AsRunnable()
                .Map(_ => output * 2)
                .Invoke(21);

            // Assert
            Assert.Equal(42, result);
            Assert.Equal(21, output);
        }

        [Fact]
        public async Task AsRunnableAsync_AsyncAction_ExecutesSideEffect()
        {
            // Arrange
            var executed = false;
            Func<Task> asyncAction = async () => {
                await Task.Delay(10);
                executed = true;
            };

            // Act
            var runnable = asyncAction.AsRunnableAsync();
            var result = await runnable.InvokeAsync();

            // Assert
            Assert.Equal(Unit.Default, result);
            Assert.True(executed);
        }

        [Fact]
        public void AsRunnableAsync_AsyncAction_ThrowsForSync()
        {
            // Arrange
            Func<Task> asyncAction = async () => await Task.Delay(10);
            var runnable = asyncAction.AsRunnableAsync();

            // Act & Assert
            Assert.Throws<NotSupportedException>(() => runnable.Invoke());
        }

        [Fact]
        public void AsRunnableAsync_AsyncAction_NullThrowsArgumentNullException()
        {
            // Arrange
            Func<Task> nullAsyncAction = null!;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => nullAsyncAction.AsRunnableAsync());
        }

        // ==================== Unit Type Tests ====================

        [Fact]
        public void Unit_DefaultValue_HasCorrectValue()
        {
            // Act
            var unit1 = Unit.Default;
            var unit2 = new Unit();

            // Assert
            Assert.Equal(unit1, unit2);
            Assert.Equal(unit1.GetHashCode(), unit2.GetHashCode());
        }

        [Fact]
        public void Unit_ToString_ReturnsCorrectValue()
        {
            // Act
            var result = Unit.Default.ToString();

            // Assert
            Assert.Equal("()", result);
        }

        [Fact]
        public void Unit_Equals_WorksCorrectly()
        {
            // Arrange
            var unit1 = Unit.Default;
            var unit2 = new Unit();

            // Act & Assert
            Assert.True(unit1.Equals(unit2));
            Assert.True(unit1.Equals((object)unit2));
            Assert.False(unit1.Equals("not a unit"));
            Assert.False(unit1.Equals(null));
        }

        // ==================== Integration Tests for New Features ====================

        [Fact]
        public async Task AsRunnableAsync_WithContextExtensions_WorksCorrectly()
        {
            // Arrange
            Func<int, Task<int>> asyncFunc = async x => {
                await Task.Delay(5);
                return x * 2;
            };

            RunnableContext.Current.TenantId = "test-tenant";

            // Act
            var result = await asyncFunc.AsRunnableAsync()
                .MapContext((output, ctx) => $"{output}-{ctx.TenantId}")
                .InvokeAsync(21);

            // Assert
            Assert.Equal("42-test-tenant", result);
        }

        [Fact]
        public void Action_WithTapContext_WorksCorrectly()
        {
            // Arrange
            var log = new System.Collections.Generic.List<string>();
            Action logAction = () => log.Add("Executed");
            RunnableContext.Current.TenantId = "tenant-123";

            // Act
            var result = logAction.AsRunnable()
                .Tap(_ => log.Add($"Tenant: {RunnableContext.Current.TenantId}"))
                .Invoke();

            // Assert
            Assert.Equal(Unit.Default, result);
            Assert.Equal(2, log.Count);
            Assert.Equal("Executed", log[0]);
            Assert.Equal("Tenant: tenant-123", log[1]);
        }
    }
}

