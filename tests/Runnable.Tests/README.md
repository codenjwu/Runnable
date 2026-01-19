# Runnable.Tests

Comprehensive unit tests for the Runnable library using xUnit.

## Overview

This test project provides complete test coverage for the Runnable library's advanced features, including:

- **RunnableMap** - Parallel execution and result aggregation
- **RunnableBranch** - Conditional routing

## Test Structure

### RunnableMapTests.cs

Tests for parallel execution and dictionary-based result aggregation:

- ? **Basic Functionality** (8 tests)
  - Zero to eight parameter variations
  - Correct dictionary output
  - All runnables execute on same input

- ? **Async Execution** (3 tests)
  - Parallel execution with Task.WhenAll
  - Performance verification (proves parallel vs sequential)
  - Multiple parameter variations

- ? **String Processing** (1 test)
  - Multiple transformations on same input

- ? **Data Enrichment** (1 test)
  - Tax, shipping, discount calculations

- ? **Validation** (1 test)
  - Multiple validation checks

- ? **Error Handling** (4 tests)
  - Null/empty inputs
  - Exception propagation (sync & async)

- ? **Composition** (3 tests)
  - Integration with Map, Tap, Pipe

- ? **Edge Cases** (3 tests)
  - Single runnable
  - Duplicate keys
  - Different output types

- ? **Performance** (1 test)
  - Parallel execution verification for many runnables

- ? **Integration/Real-World** (3 tests)
  - API aggregation
  - Feature extraction
  - Multi-model AI simulation

**Total: 28 tests**

### RunnableBranchTests.cs

Tests for conditional routing:

- ? **Basic Functionality** (6 tests)
  - Zero to eight parameter variations
  - Correct branch selection
  - Default fallback

- ? **First Match Wins** (2 tests)
  - Stops at first true condition
  - Order matters (specific before general)

- ? **Async Execution** (2 tests)
  - Async routing for 1 and 2 parameters

- ? **Classification** (1 test)
  - Multi-level classification logic

- ? **User Role Routing** (1 test)
  - Role-based access control

- ? **Grade Calculation** (1 test)
  - Score-based grading

- ? **HTTP Status Code** (1 test)
  - Status code routing

- ? **Composition** (5 tests)
  - Integration with Map, Tap, Pipe, Retry, Fallback

- ? **Edge Cases** (3 tests)
  - No branches
  - Single branch
  - All conditions false

- ? **Real-World** (2 tests)
  - Email validation
  - Discount calculator

**Total: 22 tests**

## Running Tests

### Run All Tests

```bash
dotnet test tests/Runnable.Tests/Runnable.Tests.csproj
```

### Run with Verbose Output

```bash
dotnet test tests/Runnable.Tests/Runnable.Tests.csproj --verbosity normal
```

### Run Specific Test Class

```bash
dotnet test tests/Runnable.Tests/Runnable.Tests.csproj --filter FullyQualifiedName~RunnableMapTests
```

### Run Specific Test

```bash
dotnet test tests/Runnable.Tests/Runnable.Tests.csproj --filter FullyQualifiedName~InvokeAsync_OneParameter_ExecutesInParallel
```

### Generate Code Coverage

```bash
dotnet test tests/Runnable.Tests/Runnable.Tests.csproj --collect:"XPlat Code Coverage"
```

## Test Summary

| Category | RunnableMap | RunnableBranch | Total |
|----------|-------------|----------------|-------|
| Basic Functionality | 8 | 6 | 14 |
| Async | 3 | 2 | 5 |
| Classification/Routing | 1 | 4 | 5 |
| Validation | 1 | - | 1 |
| Error Handling | 4 | - | 4 |
| Composition | 3 | 5 | 8 |
| Edge Cases | 3 | 3 | 6 |
| Real-World | 5 | 2 | 7 |
| **Total** | **28** | **22** | **50** |

## Key Test Highlights

### RunnableMap Highlights

1. **Parallel Execution Verification**
   ```csharp
   [Fact]
   public async Task InvokeAsync_OneParameter_ExecutesInParallel()
   {
       // Proves that 3 tasks with delays of 100ms, 75ms, 50ms
       // complete in ~100ms (parallel) not 225ms (sequential)
   }
   ```

2. **Real API Aggregation**
   ```csharp
   [Fact]
   public async Task RealWorld_APIAggregation_WorksCorrectly()
   {
       // Simulates fetching user, posts, followers in parallel
   }
   ```

3. **Multi-Model AI**
   ```csharp
   [Fact]
   public async Task RealWorld_MultiModelAI_SimulatesCorrectly()
   {
       // Simulates querying multiple AI models simultaneously
   }
   ```

### RunnableBranch Highlights

1. **First-Match-Wins**
   ```csharp
   [Fact]
   public void Create_FirstMatchWins_StopsAtFirstTrueCondition()
   {
       // Verifies only first matching branch executes
   }
   ```

2. **Order Matters**
   ```csharp
   [Fact]
   public void Create_OrderMatters_SpecificBeforeGeneral()
   {
       // Shows importance of condition ordering
   }
   ```

3. **Composition with Error Handling**
   ```csharp
   [Fact]
   public void Branch_CanBeComposedWithFallback()
   {
       // Branch + WithFallbackValue integration
   }
   ```

## Test Naming Convention

Tests follow the pattern: `MethodUnderTest_Scenario_ExpectedBehavior`

Examples:
- `Create_OneParameter_ExecutesAllRunnablesWithSameInput`
- `InvokeAsync_OneParameter_ExecutesInParallel`
- `Branch_CanBeComposedWithMap`

## Test Categories

### Unit Tests
Tests for individual features in isolation.

### Integration Tests
Tests for feature composition and real-world scenarios.

### Performance Tests
Tests that verify parallel execution performance (async tests).

## Dependencies

- **xUnit** 2.9.2 - Test framework
- **Microsoft.NET.Test.Sdk** 17.11.1 - Test SDK
- **coverlet.collector** 6.0.2 - Code coverage

## Code Coverage

All public APIs are covered:

- ? RunnableMap.Create (0-8 parameters)
- ? RunnableBranch.Create (0-8 parameters)
- ? Sync and async execution paths
- ? Error conditions
- ? Edge cases
- ? Composition with other Runnable features

## Continuous Integration

These tests are designed to run in CI/CD pipelines:

- Fast execution (~1-2 seconds total)
- No external dependencies
- Deterministic results
- Clear failure messages

## Adding New Tests

When adding new tests:

1. **Follow naming convention**: `MethodUnderTest_Scenario_ExpectedBehavior`
2. **Use AAA pattern**: Arrange, Act, Assert
3. **One assertion per test** (when possible)
4. **Clear test names** that describe the scenario
5. **Add comments** for complex scenarios

Example:
```csharp
[Fact]
public void MyMethod_WithValidInput_ReturnsExpectedOutput()
{
    // Arrange
    var input = "test";
    var expected = "TEST";

    // Act
    var result = MyMethod(input);

    // Assert
    Assert.Equal(expected, result);
}
```

## Test Execution Times

Average execution times:

- **RunnableMapTests**: ~600ms (includes parallel async tests)
- **RunnableBranchTests**: ~500ms
- **Total**: ~1100ms

## Future Test Additions

Planned test coverage expansions:

- [ ] 9-16 parameter variations (when implemented)
- [ ] Stress tests with large numbers of runnables
- [ ] Memory leak tests
- [ ] Thread safety tests
- [ ] Cancellation token support

## Contributing

When contributing tests:

1. Ensure all existing tests pass
2. Add tests for new features
3. Maintain or improve code coverage
4. Follow existing patterns and conventions
5. Update this README if adding new test categories

## License

Same as the main Runnable library.

---

**Test Status**: ? All 50 tests passing  
**Code Coverage**: Comprehensive coverage of RunnableMap and RunnableBranch  
**Last Updated**: 2024
