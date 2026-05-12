# Batch Consumer Integration Guide

## Overview

`BatchingChannelConsumer<T>` is now integrated as a core component of the Runnable framework.

**Location**: `src/Runnable/Components/BatchingChannelConsumer.cs`  
**Namespace**: `Runnable.Components`  
**Extensions**: `Runnable.Extensions.BatchConsumerExtensions`

## Architecture

```
Runnable Framework
├─ Core
│  ├─ Runnable.cs (compute pipelines)
│  └─ BaseRunnable.cs
├─ Extensions
│  ├─ WithRetry, TapAsync, etc. (pipeline features)
│  └─ BatchConsumerExtensions (integration)  ✨ NEW
└─ Components  ✨ NEW
   └─ BatchingChannelConsumer<T> (channel processing)
      ├─ ConsumeAsync (simple delegate pattern)
      ├─ ConsumeAsRunnableAsync (framework integration)
      ├─ IBatchResult (interface)
      └─ ConsumerStats (metrics)
```

## Key Differences from Previous Setup

| Aspect | Before | After |
|--------|--------|-------|
| **Location** | `examples/ChannelBatchProcessingExample/` | `src/Runnable/Components/` |
| **Namespace** | Global (no namespace) | `Runnable.Components` |
| **Usage** | Example-only | First-class framework component |
| **Discovery** | Hard to find | Easy to discover via NuGet docs |
| **Integration** | Manual | Built-in extensions |

## Usage Patterns

### Pattern 1: Simple Delegate (Lightweight)

```csharp
using Runnable.Components;

var consumer = new BatchingChannelConsumer<Order>(
    batchSize: 10,
    timeoutSeconds: 10,
    maxRetries: 3,
    retryDelay: TimeSpan.FromMilliseconds(500)
);

await consumer.ConsumeAsync(
    reader,
    async (batch, ct) => await database.SaveAsync(batch),
    cancellationToken
);
```

### Pattern 2: Runnable Integration (Recommended)

```csharp
using Runnable;
using Runnable.Components;
using Runnable.Extensions;

var consumer = new BatchingChannelConsumer<Order>(10, 10);

// Create a Runnable processor
var processor = new Runnable<List<Order>, BatchResult>(
    null,
    async (batch) => await database.SaveAsync(batch)
)
.WithRetry(maxAttempts: 3)
.TapAsync(async result => 
    await metrics.IncrementAsync($"orders.processed.{result.Count}")
);

// Consume with full Runnable integration
await consumer.ConsumeAsRunnableAsync(reader, processor, ct);
```

### Pattern 3: Fluent Configuration

```csharp
using Runnable.Components;
using Runnable.Extensions;

var consumer = new BatchingChannelConsumer<Order>(10, 10)
    .WithLogging(async msg => await Console.Out.WriteLineAsync(msg))
    .WithSuccessCallback(async result => 
        await metrics.IncrementAsync("batches.success"))
    .WithFailureCallback(async (batch, ex) => 
        await deadLetterQueue.SendAsync(batch));

await consumer.ConsumeAsync(reader, processor, ct);
```

## Integration Benefits

### ✅ Dual-Layer Retry Strategy

```
Layer 1 (Batch Collection):
  ├─ Timeout: 10 seconds
  ├─ Batches incomplete items
  └─ Cancellation-aware

Layer 2 (Processing via Runnable):
  ├─ Automatic retries: up to 3 attempts
  ├─ Exponential backoff
  ├─ All Runnable extensions (.WithRetry, .TapAsync, etc.)
  └─ Type-safe result handling
```

### ✅ Unified Extension Ecosystem

```csharp
// You can now combine:
processor
    .WithRetry(5)                    // Runnable extension
    .TapAsync(LogAsync)              // Runnable extension
    .CacheAsync("key")               // Runnable extension
    // Plus BatchConsumer features:
    // - Timeout control
    // - Statistics
    // - Callback-based error handling
```

### ✅ Consistent API Style

```csharp
// Same namespace hierarchy as other Runnable features
using Runnable;                    // Core
using Runnable.Components;         // Components (NEW)
using Runnable.Extensions;         // Extensions (updated)
```

## Configuration Methods

### Fluent Builder Pattern

```csharp
var consumer = new BatchingChannelConsumer<T>(
    batchSize: 10,           // items per batch
    timeoutSeconds: 10,      // max wait for incomplete batch
    maxRetries: 3,           // retry attempts on failure
    retryDelay: 500ms        // delay between retries
)
.WithLogging(logger)
.WithSuccessCallback(onSuccess)
.WithFailureCallback(onFailure);
```

### Configuration Dictionary

```csharp
consumer.Configure(c => 
{
    c.Logger = async msg => await LogAsync(msg);
    c.OnSuccess = async result => await TrackAsync(result);
    c.OnFailure = async (batch, ex) => await HandleFailureAsync(batch, ex);
});
```

## Extension Methods

| Method | Purpose | Example |
|--------|---------|---------|
| `WithLogging` | Set async logger | `WithLogging(async m => Console.WriteLine(m))` |
| `WithSuccessCallback` | On batch success | `WithSuccessCallback(async r => ...)` |
| `WithFailureCallback` | On batch failure | `WithFailureCallback(async (b, e) => ...)` |
| `Configure` | Fluent config | `.Configure(c => { c.Logger = ...; })` |

## Statistics Access

```csharp
// After consumption completes:
var stats = consumer.Stats;

stats.TotalBatches              // 4
stats.SuccessfulBatches         // 3
stats.FailedBatches             // 1
stats.TotalItemsProcessed       // 35
stats.TotalDuration             // TimeSpan
stats.GetSuccessRate()          // 75.0%
stats.GetThroughput()           // 1.96 items/sec
```

## Error Handling Strategy

```
Layer 1 (Callback Protection):
├─ OnSuccess exceptions → caught & logged
├─ OnFailure exceptions → caught & logged
└─ Logger exceptions → caught & swallowed

Layer 2 (Batch Collection):
└─ Timeout handling → batch flushed after timeout
   Cancellation → batch discarded

Layer 3 (Processing Retry):
├─ Attempt 1: fails → wait 500ms
├─ Attempt 2: fails → wait 500ms
├─ Attempt 3: fails → invoke OnFailure
└─ All protected against exceptions
```

## Migration from Example

If you were using BatchingChannelConsumer from the examples directory:

**Before**:
```csharp
// BatchingChannelConsumer was in examples, no namespace
```

**After**:
```csharp
using Runnable.Components;  // NEW - officially part of framework

// Everything else stays the same!
var consumer = new BatchingChannelConsumer<Order>(10, 10);
```

## Namespace Reference

```csharp
// Core Runnable functionality
using Runnable;                          // IRunnable, Runnable<>, etc.

// NEW: Batch processing component
using Runnable.Components;               // BatchingChannelConsumer<T>
                                         // IBatchResult, ConsumerStats

// Framework extensions
using Runnable.Extensions;               // WithRetry, TapAsync, etc.
                                         // NEW: BatchConsumerExtensions
```

## Interaction with Other Components

### With Caching Extension

```csharp
var processor = new Runnable<List<Order>, BatchResult>(...)
    .CacheAsync("batch-cache")
    .WithRetry(3)
    .TapAsync(LogAsync);

await consumer.ConsumeAsRunnableAsync(reader, processor, ct);

// Batches are cached before processing
// Reduces database load for duplicate batches
```

### With Tracing Extension

```csharp
var processor = new Runnable<List<Order>, BatchResult>(...)
    .WithContext(contextProvider)
    .TapAsync(async result =>
    {
        await tracer.LogAsync($"Processed {result.Count} orders");
    });

await consumer.ConsumeAsRunnableAsync(reader, processor, ct);
```

## Performance Considerations

### Batch Size Tuning

```csharp
// High Throughput (realtime)
new BatchingChannelConsumer<T>(
    batchSize: 100,       // Larger batches
    timeoutSeconds: 1,    // Quick flush
    maxRetries: 5
)

// Cost Optimization (batch processing)
new BatchingChannelConsumer<T>(
    batchSize: 1000,      // Maximize batch size
    timeoutSeconds: 60,   // Long timeouts OK
    maxRetries: 3
)

// High Reliability (financial)
new BatchingChannelConsumer<T>(
    batchSize: 50,        // Moderate batches
    timeoutSeconds: 5,    // Responsive
    maxRetries: 10        // Maximum retries
)
```

### Memory Impact

- **Per Instance**: ~5KB + callback delegates
- **Per Batch**: `BatchSize × sizeof(T)` bytes
- **Statistics**: Negligible overhead

## Testing

The component is fully tested with:
- ✅ Timeout control accuracy
- ✅ Retry logic under failures
- ✅ Statistics collection
- ✅ Callback exception safety
- ✅ Cancellation token handling

See `examples/ChannelBatchProcessingExample/Program.cs` for working examples.

## Status

**✅ Production Ready**
- Fully integrated with Runnable framework
- Comprehensive error handling
- Built-in statistics
- Callback-based extensibility
- Framework-compliant API design

---

**For complete examples, see**:
- [QUICK_START.md](../ChannelBatchProcessingExample/QUICK_START.md)
- [README.md](../ChannelBatchProcessingExample/README.md)
- [Program.cs](../ChannelBatchProcessingExample/Program.cs) (working examples)
