# Contributing to Runnable ??

Thank you for considering contributing to Runnable! We welcome contributions from the community and are pleased to have you join us.

## ?? Table of Contents

- [Code of Conduct](#code-of-conduct)
- [Getting Started](#getting-started)
- [How to Contribute](#how-to-contribute)
- [Development Setup](#development-setup)
- [Coding Guidelines](#coding-guidelines)
- [Testing](#testing)
- [Pull Request Process](#pull-request-process)
- [Reporting Bugs](#reporting-bugs)
- [Suggesting Features](#suggesting-features)

## ?? Code of Conduct

This project adheres to a Code of Conduct that all contributors are expected to follow. Please be respectful and constructive in all interactions.

### Our Standards

- ? Be welcoming and inclusive
- ? Be respectful of differing viewpoints
- ? Accept constructive criticism gracefully
- ? Focus on what's best for the community
- ? Show empathy towards other community members

## ?? Getting Started

1. **Fork the repository** on GitHub
2. **Clone your fork** locally
   ```bash
   git clone https://github.com/YOUR_USERNAME/Runnable.git
   cd Runnable
   ```
3. **Add upstream remote**
   ```bash
   git remote add upstream https://github.com/codenjwu/Runnable.git
   ```
4. **Create a branch** for your changes
   ```bash
   git checkout -b feature/my-awesome-feature
   ```

## ?? How to Contribute

There are many ways to contribute to Runnable:

### ?? Bug Fixes
Found a bug? We'd love a pull request! Please include:
- Clear description of the bug
- Steps to reproduce
- Expected vs actual behavior
- Unit test that demonstrates the fix

### ? New Features
Have an idea for a new feature? Great! Please:
- Open an issue first to discuss the feature
- Wait for maintainer feedback before implementing
- Follow the existing code patterns
- Include comprehensive tests
- Update documentation

### ?? Documentation
Documentation improvements are always welcome:
- Fix typos or unclear explanations
- Add examples for existing features
- Improve API documentation
- Create tutorials or guides

### ?? Tests
Help improve test coverage:
- Add missing test cases
- Improve existing tests
- Add integration tests
- Add performance benchmarks

## ??? Development Setup

### Prerequisites

- .NET 8.0 SDK or later
- Visual Studio 2022 / VS Code / Rider
- Git

### Building the Project

```bash
# Restore dependencies
dotnet restore

# Build the solution
dotnet build

# Run tests
dotnet test

# Run tests with coverage
dotnet test /p:CollectCoverage=true
```

### Project Structure

```
Runnable/
©À©¤©¤ src/
©¦   ©¸©¤©¤ Runnable/              # Main library
©¦       ©À©¤©¤ Context/           # Context-aware extensions
©¦       ©À©¤©¤ Extensions/        # Core extensions
©¦       ©¸©¤©¤ *.cs              # Core types
©À©¤©¤ tests/
©¦   ©¸©¤©¤ Runnable.Tests/        # Unit tests
©À©¤©¤ examples/                  # Example applications
©¸©¤©¤ docs/                      # Documentation
```

## ?? Coding Guidelines

### C# Style Guide

We follow standard C# conventions with some additions:

#### Naming Conventions

```csharp
// ? Good
public class RunnableContext { }
public interface IRunnable<TOutput> { }
public static class RunnableExtensions { }
private readonly Dictionary<string, object> _data;
public string TenantId { get; set; }
public TOutput Invoke(TInput input);

// ? Bad
public class runnable_context { }
public interface Runnable<TOutput> { }  // Interface should start with 'I'
private Dictionary<string, object> data;  // Missing underscore
public string tenantid { get; set; }  // Should be PascalCase
```

#### Code Organization

```csharp
// ? Good - Organize by feature/purpose
namespace Runnable
{
    /// <summary>
    /// Clear XML documentation
    /// </summary>
    public static class RunnableMapContextExtensions
    {
        // Group related methods together
        
        // ==================== MapContext ====================
        
        public static Runnable<TInput, TNewOutput> MapContext<TInput, TOutput, TNewOutput>(
            this IRunnable<TInput, TOutput> runnable,
            Func<TOutput, RunnableContext, TNewOutput> mapper)
        {
            // Implementation
        }
    }
}
```

#### Comments and Documentation

- ? **DO** add XML documentation for public APIs
- ? **DO** explain *why* in comments, not *what*
- ? **DO** add examples in XML docs for complex APIs
- ? **DON'T** add obvious comments

```csharp
// ? Good
/// <summary>
/// Maps output with context access for multi-tenant enrichment.
/// </summary>
/// <example>
/// <code>
/// processor.MapContext((result, ctx) => new Response {
///     Data = result,
///     TenantId = ctx.TenantId
/// });
/// </code>
/// </example>
public static Runnable<TInput, TNewOutput> MapContext<TInput, TOutput, TNewOutput>(...)

// ? Bad
// This method maps the output
public static Runnable<TInput, TNewOutput> MapContext<TInput, TOutput, TNewOutput>(...)
```

### Extension Method Guidelines

When adding new extension methods:

1. **Parameter Coverage**: Support 0-15 parameters (up to C# limits)
2. **Async Support**: Provide both sync and async versions where appropriate
3. **Context Awareness**: Consider if a context-aware version would be valuable
4. **Naming**: Follow existing patterns (e.g., `WithX`, `MapX`, `FilterX`)

```csharp
// ? Good - Complete pattern
public static Runnable<TInput, TOutput> MyFeature<TInput, TOutput>(...)
public static Runnable<TInput, TOutput> MyFeatureAsync<TInput, TOutput>(...)
public static Runnable<TInput, TOutput> MyFeatureContext<TInput, TOutput>(...)
```

### Performance Considerations

- ? Avoid allocations in hot paths
- ? Use `ValueTask<T>` for frequently-executed async code
- ? Consider caching expensive computations
- ? Profile before optimizing

```csharp
// ? Good - Reuse instances
private static readonly Dictionary<string, TOutput> _cache = new();

// ? Bad - Creates new instance on every call
public TOutput Process() => new Dictionary<string, TOutput>();
```

## ?? Testing

### Test Requirements

All contributions must include tests:

- ? **Unit tests** for new functionality
- ? **Integration tests** for complex features
- ? **Edge case coverage** (null inputs, empty collections, etc.)
- ? **Context-aware tests** for context extensions

### Test Structure

```csharp
[TestClass]
public class RunnableMapContextExtensionsTests
{
    [TestMethod]
    public void MapContext_WithTenantId_EnrichesOutput()
    {
        // Arrange
        RunnableContext.Current.TenantId = "test-tenant";
        var runnable = RunnableLambda.Create<string, string>(x => x.ToUpper());
        
        // Act
        var result = runnable
            .MapContext((output, ctx) => $"{output}-{ctx.TenantId}")
            .Invoke("hello");
        
        // Assert
        Assert.AreEqual("HELLO-test-tenant", result);
    }
    
    [TestMethod]
    public async Task MapContext_Async_WorksCorrectly()
    {
        // Arrange
        RunnableContext.Current.TenantId = "test-tenant";
        var runnable = RunnableLambda.Create<string, Task<string>>(
            async x => await Task.FromResult(x.ToUpper()));
        
        // Act
        var result = await runnable
            .MapContext((output, ctx) => $"{output}-{ctx.TenantId}")
            .InvokeAsync("hello");
        
        // Assert
        Assert.AreEqual("HELLO-test-tenant", result);
    }
}
```

### Running Tests

```bash
# Run all tests
dotnet test

# Run specific test class
dotnet test --filter "FullyQualifiedName~RunnableMapContextExtensionsTests"

# Run tests with coverage
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover

# Run tests in watch mode
dotnet watch test
```

## ?? Pull Request Process

1. **Update documentation** for any API changes
2. **Add tests** for new functionality
3. **Update CHANGELOG.md** with your changes
4. **Ensure all tests pass** locally
5. **Ensure code builds** without warnings
6. **Follow commit message conventions**

### Commit Message Format

We follow [Conventional Commits](https://www.conventionalcommits.org/):

```
<type>(<scope>): <subject>

<body>

<footer>
```

**Types:**
- `feat`: New feature
- `fix`: Bug fix
- `docs`: Documentation changes
- `test`: Adding or updating tests
- `refactor`: Code refactoring
- `perf`: Performance improvements
- `chore`: Build process or auxiliary tool changes

**Examples:**

```bash
feat(context): add WithCachePerTenant extension

Add context-aware caching that isolates cache per tenant.
Includes TTL and LRU variants for memory management.

Closes #123
```

```bash
fix(branch): correct predicate evaluation in BranchContext

Predicate was being evaluated twice in async path.
Now evaluated once and cached.

Fixes #456
```

### Pull Request Template

When creating a PR, please include:

```markdown
## Description
Brief description of changes

## Type of Change
- [ ] Bug fix
- [ ] New feature
- [ ] Breaking change
- [ ] Documentation update

## Testing
- [ ] Tests added/updated
- [ ] All tests pass
- [ ] Manual testing performed

## Checklist
- [ ] Code follows project style guidelines
- [ ] Self-review completed
- [ ] Documentation updated
- [ ] No new warnings introduced
```

## ?? Reporting Bugs

Before creating bug reports, please check existing issues. When creating a bug report, include:

### Bug Report Template

```markdown
**Describe the bug**
A clear description of what the bug is.

**To Reproduce**
Steps to reproduce the behavior:
1. Create pipeline with '...'
2. Set context value '...'
3. Invoke with '...'
4. See error

**Expected behavior**
What you expected to happen.

**Actual behavior**
What actually happened.

**Code Sample**
```csharp
// Minimal reproducible example
var pipeline = RunnableLambda.Create<string, string>(x => x)
    .MapContext((output, ctx) => ...);
```

**Environment**
- OS: [e.g., Windows 11]
- .NET Version: [e.g., .NET 8.0]
- Runnable Version: [e.g., 1.0.0]

**Additional context**
Any other relevant information.
```

## ?? Suggesting Features

We love feature suggestions! Please:

1. **Check existing issues** to avoid duplicates
2. **Provide use cases** for the feature
3. **Consider backwards compatibility**
4. **Think about API design**

### Feature Request Template

```markdown
**Is your feature request related to a problem?**
A clear description of what the problem is.

**Describe the solution you'd like**
What you want to happen.

**Describe alternatives you've considered**
Other solutions or features you've considered.

**Example Usage**
```csharp
// Show how you'd like to use the feature
var pipeline = processor
    .YourNewFeature(...)
    .Map(x => ...);
```

**Additional context**
Any other relevant information.
```

## ?? Recognition

Contributors will be:
- ? Listed in CONTRIBUTORS.md
- ??? Credited in release notes
- ?? Mentioned in relevant documentation

## ?? Getting Help

- ?? [GitHub Discussions](https://github.com/codenjwu/Runnable/discussions) - Ask questions
- ?? [GitHub Issues](https://github.com/codenjwu/Runnable/issues) - Report bugs
- ?? Email maintainers for private concerns

## ?? Additional Resources

- [C# Coding Conventions](https://docs.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions)
- [Conventional Commits](https://www.conventionalcommits.org/)
- [Semantic Versioning](https://semver.org/)

---

**Thank you for contributing to Runnable! ??**

Your contributions make this project better for everyone in the .NET community!
