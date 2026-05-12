using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

#nullable enable

namespace Runnable.Components
{
    /// <summary>
    /// Generic Channel batch consumer component
    /// Supports: batch processing, timeout control, automatic retries, error handling, statistics
    /// Part of the Runnable ecosystem for composable pipeline processing
    /// </summary>
    public class BatchingChannelConsumer<T>
    {
        private readonly int _batchSize;
        private readonly int _timeoutSeconds;
        private readonly int _maxRetries;
        private readonly TimeSpan _retryDelay;
        private readonly ConsumerStats _stats = new();
        private Stopwatch? _overallStopwatch;
        
        /// <summary>
        /// Batch processing delegate
        /// </summary>
        public delegate Task<TResult> BatchProcessorAsync<TResult>(List<T> batch, CancellationToken ct);
        
        /// <summary>
        /// Success callback
        /// </summary>
        public Func<IBatchResult, Task>? OnSuccess { get; set; }
        
        /// <summary>
        /// Failure callback
        /// </summary>
        public Func<IEnumerable<T>, Exception, Task>? OnFailure { get; set; }
        
        /// <summary>
        /// Async logging callback (supports async operations to avoid blocking)
        /// </summary>
        public Func<string, Task>? Logger { get; set; }
        
        /// <summary>
        /// Get consumer statistics
        /// </summary>
        public ConsumerStats Stats => _stats;

        public BatchingChannelConsumer(
            int batchSize = 10,
            int timeoutSeconds = 10,
            int maxRetries = 3,
            TimeSpan? retryDelay = null)
        {
            _batchSize = batchSize;
            _timeoutSeconds = timeoutSeconds;
            _maxRetries = maxRetries;
            _retryDelay = retryDelay ?? TimeSpan.FromMilliseconds(500);
        }

        /// <summary>
        /// Consume channel and perform batch processing
        /// </summary>
        public async Task ConsumeAsync<TResult>(
            ChannelReader<T> reader,
            BatchProcessorAsync<TResult> processor,
            CancellationToken cancellationToken = default)
        {
            _overallStopwatch = Stopwatch.StartNew();
            _stats.Reset();
            
            await ConsumeInternalAsync(
                reader,
                async batch => 
                {
                    await ProcessBatchWithRetryAsync(batch, processor, cancellationToken);
                },
                cancellationToken
            );
            
            if (_overallStopwatch != null)
            {
                _stats.TotalDuration = _overallStopwatch.Elapsed;
                _overallStopwatch.Stop();
            }
        }

        /// <summary>
        /// Batch processing using Runnable (can use all Runnable extensions)
        /// </summary>
        public async Task ConsumeAsRunnableAsync<TResult>(
            ChannelReader<T> reader,
            IRunnable<List<T>, TResult> processor,
            CancellationToken cancellationToken = default) where TResult : IBatchResult
        {
            _overallStopwatch = Stopwatch.StartNew();
            _stats.Reset();
            
            await ConsumeInternalAsync(
                reader,
                async batch =>
                {
                    _stats.TotalBatches++;
                    
                    try
                    {
                        var result = await processor.InvokeAsync(batch);
                        _stats.SuccessfulBatches++;
                        _stats.TotalItemsProcessed += batch.Count;
                        
                        // Protect OnSuccess callback exceptions
                        if (OnSuccess != null)
                        {
                            try
                            {
                                await OnSuccess(result);
                            }
                            catch (Exception callbackEx)
                            {
                                await LogAsync($"⚠ [Callback Error] OnSuccess failed: {callbackEx.Message}");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _stats.FailedBatches++;
                        await LogAsync($"✗ [Failed] {ex.Message}");
                        
                        // Protect OnFailure callback from exceptions
                        if (OnFailure != null)
                        {
                            try
                            {
                                await OnFailure(batch, ex);
                            }
                            catch (Exception callbackEx)
                            {
                                await LogAsync($"⚠ [Callback Error] OnFailure failed: {callbackEx.Message}");
                            }
                        }
                    }
                },
                cancellationToken
            );
            
            if (_overallStopwatch != null)
            {
                _stats.TotalDuration = _overallStopwatch.Elapsed;
                _overallStopwatch.Stop();
            }
        }

        /// <summary>
        /// Internal consumption loop (common implementation)
        /// </summary>
        private async Task ConsumeInternalAsync(
            ChannelReader<T> reader,
            Func<List<T>, Task> batchHandler,
            CancellationToken cancellationToken)
        {
            while (!reader.Completion.IsCompleted || reader.Count > 0)
            {
                var batch = await CollectBatchAsync(reader, cancellationToken);
                
                if (batch.Count == 0)
                {
                    if (reader.Completion.IsCompleted)
                        break;
                    continue;
                }
                
                await batchHandler(batch);
            }
        }
        
        /// <summary>
        /// Async logging method (avoids blocking)
        /// </summary>
        private async Task LogAsync(string message)
        {
            if (Logger != null)
            {
                await Logger(message);
            }
        }

        /// <summary>
        /// Collect a batch of data
        /// </summary>
        private async Task<List<T>> CollectBatchAsync(
            ChannelReader<T> reader,
            CancellationToken cancellationToken)
        {
            var batch = new List<T>();
            var stopwatch = Stopwatch.StartNew();
            
            while (batch.Count < _batchSize)
            {
                // Check cancellation token
                if (cancellationToken.IsCancellationRequested)
                {
                    await LogAsync($"[Collector] Cancelled, batch size: {batch.Count}");
                    break;
                }
                
                var remainingTimeout = TimeSpan.FromSeconds(_timeoutSeconds) - stopwatch.Elapsed;
                
                if (remainingTimeout <= TimeSpan.Zero)
                {
                    await LogAsync($"[Collector] Timeout reached, batch size: {batch.Count}");
                    break;
                }
                
                try
                {
                    // Use 'using' to prevent CancellationTokenSource leak
                    using (var cts_read = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken))
                    {
                        cts_read.CancelAfter(remainingTimeout);
                        
                        if (await reader.WaitToReadAsync(cts_read.Token))
                        {
                            if (reader.TryRead(out var item))
                            {
                                batch.Add(item);
                                await LogAsync($"[Collector] Received item #{batch.Count}");
                            }
                        }
                        else
                        {
                            await LogAsync($"[Collector] Channel closed, batch size: {batch.Count}");
                            break;
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    if (batch.Count > 0)
                    {
                        await LogAsync($"[Collector] Timeout reached, batch size: {batch.Count}");
                    }
                    break;
                }
            }
            
            return batch;
        }

        /// <summary>
        /// Process batch (with retry)
        /// </summary>
        private async Task ProcessBatchWithRetryAsync<TResult>(
            List<T> batch,
            BatchProcessorAsync<TResult> processor,
            CancellationToken cancellationToken)
        {
            _stats.TotalBatches++;
            Exception? lastException = null;
            
            for (int attempt = 0; attempt < _maxRetries; attempt++)
            {
                try
                {
                    await LogAsync($"[DB] Processing batch with {batch.Count} items... (Attempt {attempt + 1}/{_maxRetries})");
                    var result = await processor(batch, cancellationToken);
                    
                    await LogAsync($"✓ [Success] Batch processed");
                    _stats.SuccessfulBatches++;
                    _stats.TotalItemsProcessed += batch.Count;
                    
                    // Protect OnSuccess callback from exceptions
                    if (OnSuccess != null && result is IBatchResult batchResult)
                    {
                        try
                        {
                            await OnSuccess(batchResult);
                        }
                        catch (Exception callbackEx)
                        {
                            await LogAsync($"⚠ [Callback Error] OnSuccess failed: {callbackEx.Message}");
                        }
                    }
                    return;
                }
                catch (Exception ex)
                {
                    lastException = ex;
                    await LogAsync($"⚠ [Retry] Attempt {attempt + 1} failed: {ex.Message}");
                    
                    if (attempt < _maxRetries - 1)
                    {
                        await Task.Delay(_retryDelay, cancellationToken);
                    }
                }
            }
            
            // All retries failed
            if (lastException != null)
            {
                _stats.FailedBatches++;
                await LogAsync($"✗ [Failed] After {_maxRetries} retries: {lastException.Message}");
                
                // Protect OnFailure callback exceptions
                if (OnFailure != null)
                {
                    try
                    {
                        await OnFailure(batch, lastException);
                    }
                    catch (Exception callbackEx)
                    {
                        await LogAsync($"⚠ [Callback Error] OnFailure failed: {callbackEx.Message}");
                    }
                }
            }
        }
    }

    /// <summary>
    /// Batch processing result interface
    /// </summary>
    public interface IBatchResult
    {
        int Count { get; }
        DateTime ProcessedAt { get; }
        bool Success { get; }
        string Message { get; }
    }

    /// <summary>
    /// Consumer statistics information
    /// </summary>
    public class ConsumerStats
    {
        /// <summary>
        /// Total number of batches
        /// </summary>
        public int TotalBatches { get; set; }
        
        /// <summary>
        /// Number of successful batches
        /// </summary>
        public int SuccessfulBatches { get; set; }
        
        /// <summary>
        /// Number of failed batches
        /// </summary>
        public int FailedBatches { get; set; }
        
        /// <summary>
        /// Total number of items processed
        /// </summary>
        public long TotalItemsProcessed { get; set; }
        
        /// <summary>
        /// Total duration
        /// </summary>
        public TimeSpan TotalDuration { get; set; }
        
        /// <summary>
        /// Reset statistics
        /// </summary>
        public void Reset()
        {
            TotalBatches = 0;
            SuccessfulBatches = 0;
            FailedBatches = 0;
            TotalItemsProcessed = 0;
            TotalDuration = TimeSpan.Zero;
        }
        
        /// <summary>
        /// Get success rate (percentage)
        /// </summary>
        public double GetSuccessRate() => TotalBatches > 0 ? (SuccessfulBatches * 100.0) / TotalBatches : 0;
        
        /// <summary>
        /// Get average processing speed (items/second)
        /// </summary>
        public double GetThroughput() => TotalDuration.TotalSeconds > 0 ? TotalItemsProcessed / TotalDuration.TotalSeconds : 0;
    }
}
