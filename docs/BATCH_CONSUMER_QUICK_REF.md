# Runnable + BatchingChannelConsumer Quick Reference

## What Changed?

BatchingChannelConsumer is now integrated into the Runnable framework as a first-class component.

| Aspect | Before | After |
|--------|--------|-------|
| Location | `examples/ChannelBatchProcessingExample/` | `src/Runnable/Components/` |
| Namespace | None (global) | `Runnable.Components` |
| Integration | None | Full Runnable framework support |
| Status | Example code | Official component |

## Installation

Already included in Runnable NuGet package. No additional dependencies needed.

## Quick Start

### 1. Basic Usage (Same as Before)
```csharp
using Runnable.Components;

var consumer = new BatchingChannelConsumer<Order>(10, 10);
await consumer.ConsumeAsync(reader, processor, ct);
```

### 2. With Runnable Framework
```csharp
using Runnable;
using Runnable.Components;

var pipeline = new Runnable<List<Order>, BatchResult>(...)
    .WithRetry(3)
    .TapAsync(LogAsync);

await consumer.ConsumeAsRunnableAsync(reader, pipeline, ct);
```

### 3. Fluent Configuration
```csharp
using Runnable.Extensions;

consumer
    .WithLogging(async msg => await File.AppendAllTextAsync("log.txt", msg))
    .WithSuccessCallback(async r => await metrics.Track(r))
    .WithFailureCallback(async (b, e) => await deadletter.Send(b));
```

## Namespaces

```csharp
using Runnable;                  // Core (Runnable, IRunnable)
using Runnable.Components;       // NEW: BatchingChannelConsumer<T>, etc.
using Runnable.Extensions;       // Extensions (WithRetry, TapAsync, NEW: BatchConsumerExtensions)
```

## New Extension Methods

| Method | Use Case |
|--------|----------|
| `.WithLogging(logger)` | Set async logger callback |
| `.WithSuccessCallback(callback)` | Handle successful batch |
| `.WithFailureCallback(callback)` | Handle failed batch |
| `.Configure(action)` | Fluent configuration |

## Common Patterns

### High-Throughput Real-time Processing
```csharp
new BatchingChannelConsumer<Event>(
    batchSize: 100,
    timeoutSeconds: 1,
    maxRetries: 5
)
```

### Cost-Optimized Batch Processing
```csharp
new BatchingChannelConsumer<Record>(
    batchSize: 1000,
    timeoutSeconds: 60,
    maxRetries: 3
)
```

### High-Reliability Financial Processing
```csharp
new BatchingChannelConsumer<Transaction>(
    batchSize: 50,
    timeoutSeconds: 5,
    maxRetries: 10
)
```

## Examples

See [INTEGRATION_GUIDE.md](../src/Runnable/Components/INTEGRATION_GUIDE.md) for complete documentation.

See `examples/ChannelBatchProcessingExample/Program.cs` for working code examples.

## Statistics

```csharp
var stats = consumer.Stats;
stats.TotalBatches           // Total batches processed
stats.SuccessfulBatches      // Successful batches
stats.FailedBatches          // Failed batches
stats.TotalItemsProcessed    // Total items processed
stats.GetSuccessRate()       // Success percentage
stats.GetThroughput()        // Items per second
```

## Architecture

```
Runnable Framework
├─ Core: Runnable<TIn, TOut>, BaseRunnable
├─ Extensions: WithRetry, TapAsync, CacheAsync, etc.
└─ Components: BatchingChannelConsumer<T> ← NEW
```

## Key Features

✅ Dual-layer error handling (timeout + retry)
✅ Built-in statistics collection
✅ Callback-based extensibility
✅ Async logging (non-blocking)
✅ Cancellation-aware
✅ Resource-safe (using statements)
✅ Full Runnable framework integration

## Breaking Changes

**None!** Existing code continues to work with updated namespaces.

---

**For details**: See [SCHEME_C_IMPLEMENTATION.md](../docs/SCHEME_C_IMPLEMENTATION.md)
