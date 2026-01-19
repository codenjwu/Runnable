# Changelog

All notable changes to the Runnable project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Added
- Context-aware extension methods suite
  - `TapContext` - Observe with context access (0-15 parameters)
  - `MapContext` - Transform with context enrichment (0-15 parameters)
  - `FilterContext` - Filter with multi-tenant isolation (1-15 parameters)
  - `BranchContext` - Route based on context (1-15 parameters)
  - `CacheContext` - Per-tenant/user caching strategies
- Multi-tenant caching features
  - `WithCachePerTenant` - Isolated cache per tenant
  - `WithCachePerUser` - Isolated cache per user
  - `WithCachePerTenantTTL` - Tenant cache with expiration
  - `WithCachePerTenantLRU` - Tenant cache with LRU eviction
  - `WithCachePerTenantTTLAndLRU` - Combined TTL and LRU per tenant
  - `WithCacheContext` - Custom context-based cache key generation
- Branching enhancements
  - `BranchByTenant` - Route by tenant ID
  - `BranchByTenants` - Multi-tenant routing
  - `BranchByUser` - Route by user ID
  - `BranchByContextKey` - Route by context key existence
  - `BranchByContextValue` - Route by context value
  - `ABTestContext` - A/B testing support
  - `BranchByDebugMode` - Debug/production routing
  - `BranchAsyncContext` - Async predicate evaluation
- Comprehensive documentation
  - README.md with multi-tenant examples
  - CONTRIBUTING.md with coding guidelines
  - LICENSE file (MIT)
  - Examples directory with real-world scenarios
  - Multi-tenant SaaS complete example
- Test coverage for context extensions

### Changed
- N/A

### Deprecated
- N/A

### Removed
- N/A

### Fixed
- N/A

### Security
- Added tenant isolation validation in `FilterContext`
- Implemented secure context propagation

## [1.0.0] - 2024-XX-XX

### Added
- Initial release of Runnable library
- Core Runnable types
  - `RunnableLambda` - Function wrapper
  - `RunnableMap` - Transformation primitive
  - `RunnableBranch` - Conditional routing
  - `RunnablePassthrough` - Identity operation
- Extension methods
  - `.Map()` - Synchronous transformation
  - `.MapAsync()` - Async transformation
  - `.Tap()` - Side effects
  - `.TapAsync()` - Async side effects
  - `.Filter()` - Filtering
  - `.Pipe()` - Pipeline composition
  - `.Branch()` - Conditional branching
- Resilience patterns
  - `.WithRetry()` - Simple retry
  - `.WithExponentialBackoff()` - Exponential backoff retry
  - `.WithTimeout()` - Execution timeout
  - `.WithFallback()` - Fallback on error
- Caching support
  - `.WithCache()` - Simple memoization
  - `.WithCacheTTL()` - Time-based expiration
  - `.WithCacheLRU()` - LRU eviction
- Batch and stream processing
  - `.Batch()` - Batch execution
  - `.BatchAsync()` - Async batch execution
  - `.Stream()` - Streaming results
- Context management
  - `RunnableContext` - Thread-local context
  - `.WithContext()` - Set context values
  - `.WithCorrelationId()` - Distributed tracing
  - `.WithTenant()` - Multi-tenancy support
  - `.WithUser()` - User context
- Support for 0-16 parameter overloads
- Full async/await support
- .NET Standard 2.0+ compatibility
- Comprehensive unit test suite

---

## Release Notes Template

### [Version] - YYYY-MM-DD

#### Added
- New features and capabilities

#### Changed
- Changes to existing functionality

#### Deprecated
- Features marked for removal in future versions

#### Removed
- Features removed in this version

#### Fixed
- Bug fixes

#### Security
- Security improvements and fixes

---

## Versioning Strategy

We follow [Semantic Versioning](https://semver.org/):

- **MAJOR** version when making incompatible API changes
- **MINOR** version when adding functionality in a backwards-compatible manner
- **PATCH** version when making backwards-compatible bug fixes

## Migration Guides

### Upgrading to 2.0.0 (Future)

When we release version 2.0.0, migration notes will be added here.

---

For more details on each release, see the [GitHub Releases](https://github.com/codenjwu/Runnable/releases) page.
