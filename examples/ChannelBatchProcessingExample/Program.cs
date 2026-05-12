using Runnable;
using Runnable.Components;
using Runnable.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

// ==================== Data Models ====================
record UserEvent(int UserId, string Action, DateTime Timestamp);
record BatchResult(int Count, DateTime ProcessedAt, bool Success, string Message = "") : IBatchResult;

// ==================== Simulated Database Service ====================
class DatabaseService
{
    private int _callCount = 0;

    public async Task<BatchResult> SaveBatchAsync(List<UserEvent> events)
    {
        _callCount++;
        Console.WriteLine($"[DB] Processing batch #{_callCount} with {events.Count} events...");
        
        if (_callCount % 5 == 0)
        {
            await Task.Delay(100);
            throw new InvalidOperationException("Simulated DB error: Connection timeout");
        }
        
        await Task.Delay(100);
        return new BatchResult(events.Count, DateTime.Now, true, $"Saved {events.Count} events");
    }
}

// ==================== Main Program ====================
class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("=== BatchingChannelConsumer Example ===\n");
        
        if (args.Length > 0 && args[0] == "2")
        {
            await RunSimpleExample();
        }
        else
        {
            await RunRunnableExample();
        }
    }

    // ==================== Example 1: Using Runnable (Recommended) ====================
    static async Task RunRunnableExample()
    {
        Console.WriteLine("--- Example 1: Using BatchingChannelConsumer with Runnable ---\n");
        
        var dbService = new DatabaseService();
        var channel = Channel.CreateUnbounded<UserEvent>();
        var cts = new CancellationTokenSource();
        
        var dbOperation = new Runnable<List<UserEvent>, BatchResult>(
            null,
            async (batch) => await dbService.SaveBatchAsync(batch)
        )
        .WithRetry(maxAttempts: 3, delay: TimeSpan.FromMilliseconds(500))
        .TapAsync(async result =>
        {
            if (result.Success)
            {
                Console.WriteLine($" [Tap] Logged: {result.Message}");
            }
            await Task.CompletedTask;
        });
        
        var consumer = new BatchingChannelConsumer<UserEvent>(
            batchSize: 10,
            timeoutSeconds: 10,
            maxRetries: 3,
            retryDelay: TimeSpan.FromMilliseconds(500)
        );
        
        consumer.Logger = async x => Console.WriteLine(x);
        consumer.OnSuccess = async result => { Console.WriteLine($"[Success] {result.Count} items\n"); await Task.CompletedTask; };
        consumer.OnFailure = async (batch, ex) => { Console.WriteLine($"[Failed] {batch.Count()} items: {ex.Message}\n"); await Task.CompletedTask; };
        
        await RunProducerConsumerAsync(channel, cts, async () =>
        {
            await consumer.ConsumeAsRunnableAsync(channel.Reader, dbOperation, cts.Token);
        });
        
        // Display statistics information
        Console.WriteLine("\n📊 === Statistics (Example 1) ===");
        Console.WriteLine($"Total batches: {consumer.Stats.TotalBatches}");
        Console.WriteLine($"Successful: {consumer.Stats.SuccessfulBatches}, Failed: {consumer.Stats.FailedBatches}");
        Console.WriteLine($"Success rate: {consumer.Stats.GetSuccessRate():F2}%");
        Console.WriteLine($"Total items: {consumer.Stats.TotalItemsProcessed}");
        Console.WriteLine($"Total duration: {consumer.Stats.TotalDuration.TotalSeconds:F2}s");
        Console.WriteLine($"Throughput: {consumer.Stats.GetThroughput():F2} items/s\n");
    }

    // ==================== Example 2: Using Delegate (Simple) ====================
    static async Task RunSimpleExample()
    {
        Console.WriteLine("--- Example 2: Using BatchingChannelConsumer with Delegate ---\n");
        
        var dbService = new DatabaseService();
        var channel = Channel.CreateUnbounded<UserEvent>();
        var cts = new CancellationTokenSource();
        
        var consumer = new BatchingChannelConsumer<UserEvent>(
            batchSize: 10,
            timeoutSeconds: 10,
            maxRetries: 3,
            retryDelay: TimeSpan.FromMilliseconds(200)
        );
        
        consumer.Logger = async x => Console.WriteLine(x);
        consumer.OnSuccess = async result => { Console.WriteLine($"[Success] {result.Count} items\n"); await Task.CompletedTask; };
        consumer.OnFailure = async (batch, ex) => { Console.WriteLine($"[Failed] {batch.Count()} items: {ex.Message}\n"); await Task.CompletedTask; };
        
        await RunProducerConsumerAsync(channel, cts, async () =>
        {
            await consumer.ConsumeAsync(
                channel.Reader,
                (batch, ct) => dbService.SaveBatchAsync(batch),
                cts.Token
            );
        });
        
        // Display statistics information
        Console.WriteLine("\n📊 === Statistics (Example 2) ===");
        Console.WriteLine($"Total batches: {consumer.Stats.TotalBatches}");
        Console.WriteLine($"Successful: {consumer.Stats.SuccessfulBatches}, Failed: {consumer.Stats.FailedBatches}");
        Console.WriteLine($"Success rate: {consumer.Stats.GetSuccessRate():F2}%");
        Console.WriteLine($"Total items: {consumer.Stats.TotalItemsProcessed}");
        Console.WriteLine($"Total duration: {consumer.Stats.TotalDuration.TotalSeconds:F2}s");
        Console.WriteLine($"Throughput: {consumer.Stats.GetThroughput():F2} items/s\n");
    }

    static async Task RunProducerConsumerAsync(
        Channel<UserEvent> channel,
        CancellationTokenSource cts,
        Func<Task> consumerTask)
    {
        Console.WriteLine("Starting producer and consumer...\n");
        
        var producerTask = ProduceEventsAsync(channel, cts);
        var consumeTask = consumerTask();
        
        try
        {
            await Task.WhenAll(producerTask, consumeTask);
            Console.WriteLine("\n=== Example Complete ===");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        finally
        {
            cts.Dispose();
        }
    }

    static async Task ProduceEventsAsync(Channel<UserEvent> channel, CancellationTokenSource cts)
    {
        var sampleActions = new[] { "Login", "Purchase", "ViewProduct", "AddToCart", "Logout" };
        
        for (int i = 0; i < 35; i++)
        {
            if (cts.Token.IsCancellationRequested) break;
            
            var evt = new UserEvent(
                UserId: i % 10 + 1,
                Action: sampleActions[i % sampleActions.Length],
                Timestamp: DateTime.Now
            );
            
            await channel.Writer.WriteAsync(evt, cts.Token);
            await Task.Delay(500);
            
            if (i == 14)
            {
                Console.WriteLine("\n[Producer] Starting to consume...\n");
            }
        }
        
        channel.Writer.Complete();
        Console.WriteLine("[Producer] All events sent\n");
    }
}
