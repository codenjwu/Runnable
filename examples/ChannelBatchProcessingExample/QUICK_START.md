# Quick Start Guide

## Project Files

```
ChannelBatchProcessingExample/
├── Program.cs                              # Main program
├── BatchingChannelConsumer.cs              # Generic batch consumer
├── ChannelBatchProcessingExample.csproj    # Project configuration
├── README.md                               # Detailed documentation
├── QUICK_START.md                          # This file
└── IMPLEMENTATION_COMPARISON.md            # Comparison guide
```

## 5-Minute Quick Understanding

### Problem
Reading data from `Channel<T>` one by one, you want to **process in batches**:
- Process immediately when 10 items accumulated
- Process remaining items after 10 seconds even if less than 10
- Auto-retry DB operations 3 times on failure

### Solution

#### Step 1: Create Consumer
```csharp
var consumer = new BatchingChannelConsumer<UserEvent>(
    batchSize: 10,              // Process when reaching 10 items
    timeoutSeconds: 10,         // OR process after 10 seconds
    maxRetries: 3,              // Retry 3 times on DB failure
    retryDelay: TimeSpan.FromMilliseconds(500)
);

// Optional: Setup callbacks
consumer.Logger = async msg => Console.WriteLine(msg);
consumer.OnSuccess = async result => Console.WriteLine($"Saved {result.Count} items");
consumer.OnFailure = async (batch, ex) => Console.WriteLine($"Failed: {ex.Message}");
```

#### Step 2: Setup Database Operation (Runnable-based)
```csharp
var dbOperation = new Runnable<List<UserEvent>, BatchResult>(
    null,
    async (batch) => await dbService.SaveBatchAsync(batch)
);
```

#### Step 3: Start Processing
```csharp
await consumer.ConsumeAsRunnableAsync(
    channel.Reader,
    dbOperation,
    cancellationToken
);

// View statistics after completion
Console.WriteLine($"Total batches: {consumer.Stats.TotalBatches}");
Console.WriteLine($"Success rate: {consumer.Stats.GetSuccessRate():F2}%");
Console.WriteLine($"Throughput: {consumer.Stats.GetThroughput():F2} items/s");
```

## Two Usage Patterns

### Pattern 1: Runnable (Recommended)
```csharp
var operation = new Runnable<List<UserEvent>, BatchResult>(
    null,
    async batch => await db.SaveAsync(batch)
)
.WithRetry(3, TimeSpan.FromMilliseconds(500))  // Can add more extensions!
.TapAsync(result => { Console.WriteLine($"Saved {result.Count}"); return Task.CompletedTask; });

await consumer.ConsumeAsRunnableAsync(reader, operation, ct);
```

**Advantages**:
- Full Runnable framework support
- Can chain multiple extensions
- Type-safe result handling

### Pattern 2: Simple Delegate
```csharp
await consumer.ConsumeAsync(
    reader,
    async (batch, ct) => await db.SaveAsync(batch),
    ct
);
```

**Advantages**:
- Simple and straightforward
- No need to create Runnable wrapper
- Good for simple use cases

## Output Example

```
[Collector] Received item #1
[Collector] Received item #2
...
[Collector] Received item #10
[DB] Processing batch with 10 items... (Attempt 1/3)
[DB] Processing batch #1 with 10 events...
✓ [Success] Batch processed
[成功] 10 条

...

📊 === Statistics (Example 1) ===
Total batches: 4
Successful: 4, Failed: 0
Success rate: 100.00%
Total items: 35
Total duration: 17.83s
Throughput: 1.96 items/s
```

## Key Configuration Options

| Option | Default | Purpose |
|--------|---------|---------|
| `batchSize` | 10 | Items per batch |
| `timeoutSeconds` | 10 | Max wait time in seconds |
| `maxRetries` | 3 | Max retry attempts |
| `retryDelay` | 500ms | Delay between retries |

## Common Customizations

### Custom Success/Failure Handling
```csharp
consumer.OnSuccess = async result => {
    await logger.LogAsync($"Saved {result.Count} items");
    await metrics.RecordSuccess(result.Count);
};

consumer.OnFailure = async (batch, ex) => {
    await logger.LogAsync($"Failed: {ex.Message}");
    await deadLetterQueue.EnqueueAsync(batch);
};
```

### Async File Logging
```csharp
consumer.Logger = async msg => {
    await File.AppendAllTextAsync("batch_log.txt", $"{msg}\n");
};
```

### Access Statistics
```csharp
await consumer.ConsumeAsRunnableAsync(reader, op, ct);

var stats = consumer.Stats;
Console.WriteLine($"Processed {stats.TotalItemsProcessed} items in {stats.TotalBatches} batches");
Console.WriteLine($"Success rate: {stats.GetSuccessRate():P}");
Console.WriteLine($"Throughput: {stats.GetThroughput():F0} items/second");
```

## Running the Examples

```bash
# Example 1: Using Runnable framework
dotnet run

# Example 2: Using simple delegate
dotnet run 2
```

## Troubleshooting

**Q: Processing stops after first batch?**
- Make sure channel is properly closed: `channel.Writer.Complete()`
- Check if exception is being silently caught

**Q: Timeout doesn't seem to work?**
- Remember: consumer waits for EITHER 10 items OR 10 seconds
- The consumer exits when channel closes
- Use `CancellationToken` to stop early: `cts.Cancel()`

**Q: How to increase max retries?**
```csharp
var consumer = new BatchingChannelConsumer<T>(
    maxRetries: 5  // Changed from default 3
);
```

**Q: How to make logging non-blocking?**
```csharp
// Already built-in! Logger is async
consumer.Logger = async msg => {
    // This won't block the consumer thread
    await File.AppendAllTextAsync("log.txt", msg + "\n");
};
```

## Next Steps

1. Read [README.md](README.md) for detailed architecture
2. Check [IMPLEMENTATION_COMPARISON.md](IMPLEMENTATION_COMPARISON.md) to compare implementations
3. Explore extending with custom metrics or dead letter handling

## Performance Notes

- Single consumer/producer: ~2 items/second (due to 500ms delays in example)
- No blocking operations in the main loop
- Statistics collection has minimal overhead
- Async logging allows background processing

## Architecture Benefits

✅ **Timeout Precision**: Uses `Stopwatch + CancellationToken` for exact timing
✅ **Resource Safe**: Proper cleanup with `using` statements
✅ **Extensible**: Callbacks for success, failure, and logging
✅ **Observable**: Built-in statistics collection
✅ **Composable**: Works with Runnable framework extensions
