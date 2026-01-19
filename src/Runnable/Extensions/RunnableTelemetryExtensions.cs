using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Runnable
{
    /// <summary>
    /// Telemetry and metrics support for Runnable pipelines
    /// </summary>
    public static class RunnableTelemetryExtensions
    {
        /// <summary>
        /// Telemetry data collected during execution
        /// </summary>
        public class TelemetryData
        {
            public string OperationName { get; set; }
            public TimeSpan Duration { get; set; }
            public bool Success { get; set; }
            public Exception Exception { get; set; }
            public DateTime StartTime { get; set; }
            public DateTime EndTime { get; set; }
        }

        // ==================== WithTelemetry (Track execution metrics) ====================

        /// <summary>
        /// Track execution metrics (1 input version)
        /// </summary>
        public static Runnable<TInput, TOutput> WithTelemetry<TInput, TOutput>(
            this IRunnable<TInput, TOutput> runnable,
            Action<TelemetryData> telemetryCallback,
            string operationName = null)
        {
            return new Runnable<TInput, TOutput>(
                input =>
                {
                    var telemetry = new TelemetryData
                    {
                        OperationName = operationName ?? typeof(TOutput).Name,
                        StartTime = DateTime.UtcNow
                    };

                    var sw = Stopwatch.StartNew();
                    try
                    {
                        var result = runnable.Invoke(input);
                        sw.Stop();
                        
                        telemetry.EndTime = DateTime.UtcNow;
                        telemetry.Duration = sw.Elapsed;
                        telemetry.Success = true;
                        telemetryCallback(telemetry);
                        
                        return result;
                    }
                    catch (Exception ex)
                    {
                        sw.Stop();
                        telemetry.EndTime = DateTime.UtcNow;
                        telemetry.Duration = sw.Elapsed;
                        telemetry.Success = false;
                        telemetry.Exception = ex;
                        telemetryCallback(telemetry);
                        throw;
                    }
                },
                async input =>
                {
                    var telemetry = new TelemetryData
                    {
                        OperationName = operationName ?? typeof(TOutput).Name,
                        StartTime = DateTime.UtcNow
                    };

                    var sw = Stopwatch.StartNew();
                    try
                    {
                        var result = await runnable.InvokeAsync(input);
                        sw.Stop();
                        
                        telemetry.EndTime = DateTime.UtcNow;
                        telemetry.Duration = sw.Elapsed;
                        telemetry.Success = true;
                        telemetryCallback(telemetry);
                        
                        return result;
                    }
                    catch (Exception ex)
                    {
                        sw.Stop();
                        telemetry.EndTime = DateTime.UtcNow;
                        telemetry.Duration = sw.Elapsed;
                        telemetry.Success = false;
                        telemetry.Exception = ex;
                        telemetryCallback(telemetry);
                        throw;
                    }
                }
            );
        }

        // ==================== WithDurationTracking ====================

        /// <summary>
        /// Track execution duration (1 input version)
        /// </summary>
        public static Runnable<TInput, TOutput> WithDurationTracking<TInput, TOutput>(
            this IRunnable<TInput, TOutput> runnable,
            Action<TimeSpan> durationCallback)
        {
            return runnable.WithTelemetry(
                telemetry => durationCallback(telemetry.Duration),
                "DurationTracking");
        }

        /// <summary>
        /// Track execution duration with input context (1 input version)
        /// </summary>
        public static Runnable<TInput, TOutput> WithDurationTracking<TInput, TOutput>(
            this IRunnable<TInput, TOutput> runnable,
            Action<TInput, TimeSpan> durationCallback)
        {
            return new Runnable<TInput, TOutput>(
                input =>
                {
                    var sw = Stopwatch.StartNew();
                    try
                    {
                        var result = runnable.Invoke(input);
                        sw.Stop();
                        durationCallback(input, sw.Elapsed);
                        return result;
                    }
                    catch
                    {
                        sw.Stop();
                        durationCallback(input, sw.Elapsed);
                        throw;
                    }
                },
                async input =>
                {
                    var sw = Stopwatch.StartNew();
                    try
                    {
                        var result = await runnable.InvokeAsync(input);
                        sw.Stop();
                        durationCallback(input, sw.Elapsed);
                        return result;
                    }
                    catch
                    {
                        sw.Stop();
                        durationCallback(input, sw.Elapsed);
                        throw;
                    }
                }
            );
        }

        // ==================== WithExceptionTracking ====================

        /// <summary>
        /// Track exceptions without catching them (1 input version)
        /// </summary>
        public static Runnable<TInput, TOutput> WithExceptionTracking<TInput, TOutput>(
            this IRunnable<TInput, TOutput> runnable,
            Action<Exception> exceptionCallback)
        {
            return new Runnable<TInput, TOutput>(
                input =>
                {
                    try
                    {
                        return runnable.Invoke(input);
                    }
                    catch (Exception ex)
                    {
                        exceptionCallback(ex);
                        throw;
                    }
                },
                async input =>
                {
                    try
                    {
                        return await runnable.InvokeAsync(input);
                    }
                    catch (Exception ex)
                    {
                        exceptionCallback(ex);
                        throw;
                    }
                }
            );
        }

        // ==================== WithSuccessRateTracking ====================

        /// <summary>
        /// Track success/failure rate
        /// </summary>
        public static Runnable<TInput, TOutput> WithSuccessRateTracking<TInput, TOutput>(
            this IRunnable<TInput, TOutput> runnable,
            Action<bool> successCallback)
        {
            return new Runnable<TInput, TOutput>(
                input =>
                {
                    try
                    {
                        var result = runnable.Invoke(input);
                        successCallback(true);
                        return result;
                    }
                    catch
                    {
                        successCallback(false);
                        throw;
                    }
                },
                async input =>
                {
                    try
                    {
                        var result = await runnable.InvokeAsync(input);
                        successCallback(true);
                        return result;
                    }
                    catch
                    {
                        successCallback(false);
                        throw;
                    }
                }
            );
        }

        // ==================== WithPerformanceThreshold ====================

        /// <summary>
        /// Log when execution exceeds performance threshold (1 input version)
        /// </summary>
        public static Runnable<TInput, TOutput> WithPerformanceThreshold<TInput, TOutput>(
            this IRunnable<TInput, TOutput> runnable,
            TimeSpan threshold,
            Action<TInput, TimeSpan> thresholdExceededCallback)
        {
            return new Runnable<TInput, TOutput>(
                input =>
                {
                    var sw = Stopwatch.StartNew();
                    try
                    {
                        var result = runnable.Invoke(input);
                        sw.Stop();
                        
                        if (sw.Elapsed > threshold)
                        {
                            thresholdExceededCallback(input, sw.Elapsed);
                        }
                        
                        return result;
                    }
                    catch
                    {
                        sw.Stop();
                        if (sw.Elapsed > threshold)
                        {
                            thresholdExceededCallback(input, sw.Elapsed);
                        }
                        throw;
                    }
                },
                async input =>
                {
                    var sw = Stopwatch.StartNew();
                    try
                    {
                        var result = await runnable.InvokeAsync(input);
                        sw.Stop();
                        
                        if (sw.Elapsed > threshold)
                        {
                            thresholdExceededCallback(input, sw.Elapsed);
                        }
                        
                        return result;
                    }
                    catch
                    {
                        sw.Stop();
                        if (sw.Elapsed > threshold)
                        {
                            thresholdExceededCallback(input, sw.Elapsed);
                        }
                        throw;
                    }
                }
            );
        }

        // ==================== WithMetrics (Comprehensive metrics) ====================

        /// <summary>
        /// Comprehensive metrics tracking
        /// </summary>
        public class MetricsCollector
        {
            public long TotalInvocations { get; private set; }
            public long SuccessfulInvocations { get; private set; }
            public long FailedInvocations { get; private set; }
            public TimeSpan TotalDuration { get; private set; }
            public TimeSpan MinDuration { get; private set; } = TimeSpan.MaxValue;
            public TimeSpan MaxDuration { get; private set; }
            
            public double AverageDuration => TotalInvocations > 0 
                ? TotalDuration.TotalMilliseconds / TotalInvocations 
                : 0;
            
            public double SuccessRate => TotalInvocations > 0 
                ? (double)SuccessfulInvocations / TotalInvocations * 100 
                : 0;

            internal void RecordSuccess(TimeSpan duration)
            {
                lock (this)
                {
                    TotalInvocations++;
                    SuccessfulInvocations++;
                    TotalDuration += duration;
                    if (duration < MinDuration) MinDuration = duration;
                    if (duration > MaxDuration) MaxDuration = duration;
                }
            }

            internal void RecordFailure(TimeSpan duration)
            {
                lock (this)
                {
                    TotalInvocations++;
                    FailedInvocations++;
                    TotalDuration += duration;
                    if (duration < MinDuration) MinDuration = duration;
                    if (duration > MaxDuration) MaxDuration = duration;
                }
            }
        }

        /// <summary>
        /// Collect comprehensive metrics (1 input version)
        /// </summary>
        public static Runnable<TInput, TOutput> WithMetrics<TInput, TOutput>(
            this IRunnable<TInput, TOutput> runnable,
            MetricsCollector metrics)
        {
            return new Runnable<TInput, TOutput>(
                input =>
                {
                    var sw = Stopwatch.StartNew();
                    try
                    {
                        var result = runnable.Invoke(input);
                        sw.Stop();
                        metrics.RecordSuccess(sw.Elapsed);
                        return result;
                    }
                    catch
                    {
                        sw.Stop();
                        metrics.RecordFailure(sw.Elapsed);
                        throw;
                    }
                },
                async input =>
                {
                    var sw = Stopwatch.StartNew();
                    try
                    {
                        var result = await runnable.InvokeAsync(input);
                        sw.Stop();
                        metrics.RecordSuccess(sw.Elapsed);
                        return result;
                    }
                    catch
                    {
                        sw.Stop();
                        metrics.RecordFailure(sw.Elapsed);
                        throw;
                    }
                }
            );
        }
    }
}
