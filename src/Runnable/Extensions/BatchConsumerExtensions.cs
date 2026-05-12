using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Runnable.Extensions
{
    /// <summary>
    /// Extension methods for BatchingChannelConsumer integration with Runnable framework
    /// Enables seamless composition of batch processing with Runnable pipelines
    /// </summary>
    public static class BatchConsumerExtensions
    {
        /// <summary>
        /// Create a Runnable from a BatchingChannelConsumer
        /// Allows using all Runnable extensions (WithRetry, TapAsync, CacheAsync, etc.)
        /// </summary>
        /// <example>
        /// var consumer = new BatchingChannelConsumer&lt;Order&gt;(10, 10);
        /// var pipeline = consumer
        ///     .AsRunnable(reader, ct)
        ///     .WithRetry(3)
        ///     .TapAsync(LogBatchAsync)
        ///     .CacheAsync("batch_key");
        /// </example>
        public static IRunnable<List<T>, TResult> AsRunnable<T, TResult>(
            this Components.BatchingChannelConsumer<T> consumer,
            ChannelReader<T> reader,
            CancellationToken ct = default) where TResult : Components.IBatchResult
        {
            return new Runnable<List<T>, TResult>(
                null,
                async batch =>
                {
                    // This method signature bridges BatchingChannelConsumer with Runnable
                    // In practice, use ConsumeAsRunnableAsync for full integration
                    throw new NotImplementedException("Use ConsumeAsRunnableAsync instead for channel-based consumption");
                }
            );
        }

        /// <summary>
        /// Enable batch processing with Runnable integration
        /// Combines BatchingChannelConsumer's timeout + retry capabilities with Runnable pipeline features
        /// </summary>
        /// <example>
        /// var consumer = new BatchingChannelConsumer&lt;Order&gt;(10, 10, 3, 500ms);
        /// 
        /// var processor = new Runnable&lt;List&lt;Order&gt;, BatchResult&gt;(...)
        ///     .WithRetry(3)
        ///     .TapAsync(LogAsync);
        /// 
        /// // Consumer applies its own timeout + retry at batch collection level
        /// // Runnable applies timeout + retry at processing level
        /// // This creates a robust two-layer retry strategy
        /// await consumer.ConsumeAsRunnableAsync(reader, processor, ct);
        /// </example>
        public static async Task ConsumeAsRunnableAsync<T, TResult>(
            this Components.BatchingChannelConsumer<T> consumer,
            ChannelReader<T> reader,
            IRunnable<List<T>, TResult> processor,
            CancellationToken cancellationToken = default) where TResult : Components.IBatchResult
        {
            await consumer.ConsumeAsRunnableAsync(reader, processor, cancellationToken);
        }

        /// <summary>
        /// Enable simple batch processing without Runnable framework
        /// </summary>
        /// <example>
        /// var consumer = new BatchingChannelConsumer&lt;Order&gt;(10, 10);
        /// 
        /// await consumer.ConsumeAsync(
        ///     reader,
        ///     async (batch, ct) => await database.SaveAsync(batch),
        ///     ct
        /// );
        /// </example>
        public static async Task ConsumeAsync<T, TResult>(
            this Components.BatchingChannelConsumer<T> consumer,
            ChannelReader<T> reader,
            Components.BatchingChannelConsumer<T>.BatchProcessorAsync<TResult> processor,
            CancellationToken cancellationToken = default)
        {
            await consumer.ConsumeAsync(reader, processor, cancellationToken);
        }

        /// <summary>
        /// Configure batch consumer with logging
        /// </summary>
        /// <example>
        /// consumer
        ///     .WithLogging(async msg => await File.AppendAllTextAsync("log.txt", msg + "\n"))
        ///     .WithSuccessCallback(async result => await metrics.IncrementAsync("batches.success"))
        ///     .WithFailureCallback(async (batch, ex) => await deadLetter.SendAsync(batch));
        /// </example>
        public static Components.BatchingChannelConsumer<T> WithLogging<T>(
            this Components.BatchingChannelConsumer<T> consumer,
            Func<string, Task>? logger)
        {
            consumer.Logger = logger;
            return consumer;
        }

        /// <summary>
        /// Configure success callback
        /// </summary>
        public static Components.BatchingChannelConsumer<T> WithSuccessCallback<T>(
            this Components.BatchingChannelConsumer<T> consumer,
            Func<Components.IBatchResult, Task>? callback)
        {
            consumer.OnSuccess = callback;
            return consumer;
        }

        /// <summary>
        /// Configure failure callback
        /// </summary>
        public static Components.BatchingChannelConsumer<T> WithFailureCallback<T>(
            this Components.BatchingChannelConsumer<T> consumer,
            Func<IEnumerable<T>, Exception, Task>? callback)
        {
            consumer.OnFailure = callback;
            return consumer;
        }

        /// <summary>
        /// Fluent configuration builder for batch consumer
        /// </summary>
        /// <example>
        /// var consumer = new BatchingChannelConsumer&lt;Order&gt;()
        ///     .WithLogging(logger)
        ///     .WithSuccessCallback(OnBatchSuccess)
        ///     .WithFailureCallback(OnBatchFailure);
        /// </example>
        public static Components.BatchingChannelConsumer<T> Configure<T>(
            this Components.BatchingChannelConsumer<T> consumer,
            Action<Components.BatchingChannelConsumer<T>>? config)
        {
            config?.Invoke(consumer);
            return consumer;
        }
    }
}
