using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Runnable.Tests
{
    /// <summary>
    /// Advanced complex integration tests combining many Runnable extensions
    /// </summary>
    public class RunnableAdvancedIntegrationTests
    {
        // ==================== Multi-Layer Data Processing Pipeline ====================

        [Fact]
        public async Task AdvancedScenario1_MultiLayerProcessing_WorksCorrectly()
        {
            // Arrange - Simulate complex data processing with multiple stages
            var logs = new List<string>();
            var validationErrors = 0;

            // Layer 1: Parse and validate
            var parseData = RunnableLambda.Create<string, int>(s => {
                if (!int.TryParse(s, out var result))
                    throw new FormatException("Invalid number");
                return result;
            });

            // Layer 2: Transform (parallel operations)
            var square = RunnableLambda.Create<int, int>(x => x * x);
            var double_ = RunnableLambda.Create<int, int>(x => x * 2);
            var addTen = RunnableLambda.Create<int, int>(x => x + 10);

            var parallelTransforms = RunnableMap.Create<int, int>(
                ("squared", square),
                ("doubled", double_),
                ("plusTen", addTen)
            );

            // Layer 3: Aggregate results
            var aggregate = RunnableLambda.Create<Dictionary<string, int>, (int sum, double avg, int max)>(
                dict => {
                    var values = dict.Values.ToArray();
                    return (values.Sum(), values.Average(), values.Max());
                });

            // Layer 4: Apply business rules
            var applyRules = RunnableLambda.Create<(int sum, double avg, int max), string>(
                stats => stats.sum > 1000 ? "HIGH" : stats.sum > 100 ? "MEDIUM" : "LOW");

            // Layer 5: Format response
            var formatResponse = RunnableLambda.Create<string, Dictionary<string, object>>(
                category => new Dictionary<string, object> {
                    ["category"] = category,
                    ["timestamp"] = DateTime.UtcNow.ToString("O"),
                    ["processed"] = true
                });

            // Fallback for validation errors
            var validationFallback = RunnableLambda.Create<string, int>(s => {
                validationErrors++;
                logs.Add($"Validation failed for: {s}");
                return 0;
            });

            // Act - Build multi-layer pipeline
            var pipeline = parseData
                .WithRetry(1)  // Retry parsing
                .WithFallback(validationFallback)  // Use default on error
                .WithCache()  // Cache parsing results
                .Pipe(parallelTransforms)  // Parallel processing
                .WithTimeout(TimeSpan.FromMilliseconds(500))  // Must complete quickly
                .Pipe(aggregate)
                .TapAsync(async stats => {
                    await Task.Delay(1);
                    logs.Add($"Stats: sum={stats.sum}, avg={stats.avg:F2}, max={stats.max}");
                })
                .Pipe(applyRules)
                .Pipe(formatResponse);

            // Test cases
            var result1 = await pipeline.InvokeAsync("25");  // Valid
            var result2 = await pipeline.InvokeAsync("invalid");  // Invalid -> uses fallback
            var result3 = await pipeline.InvokeAsync("50");  // Valid, different value
            var result4 = await pipeline.InvokeAsync("25");  // Cache hit

            // Assert
            Assert.NotNull(result1["category"]);  // Has a category assigned
            Assert.NotNull(result1);
            Assert.Equal("LOW", result2["category"]);  // Fallback: 0
            Assert.NotNull(result3);
            Assert.Equal(result1["category"], result4["category"]);  // Cache hit - same category
            Assert.True((bool)result4["processed"]);  // Cached result is processed

            Assert.Equal(1, validationErrors);
            // TapAsync executes even with cache since it's after the cached step
            Assert.True(logs.Count(l => l.StartsWith("Stats:")) >= 3);
        }

        // ==================== Multi-Stage Authentication & Authorization Flow ====================

        [Fact]
        public async Task AdvancedScenario2_AuthFlowWithMultipleStages_WorksCorrectly()
        {
            // Arrange - Simulate OAuth-style authentication flow
            var auditLog = new List<string>();
            var sessionStore = new Dictionary<string, string>();
            var failedAttempts = new Dictionary<string, int>();

            // Stage 1: Parse credentials
            var parseCredentials = RunnableLambda.Create<string, (string username, string password)>(
                input => {
                    var parts = input.Split(':');
                    if (parts.Length != 2)
                        throw new ArgumentException("Invalid credentials format");
                    return (parts[0], parts[1]);
                });

            // Stage 2: Validate credentials (with retry tracking)
            var userDatabase = new Dictionary<string, string> {
                ["alice"] = "password123",
                ["bob"] = "secret456",
                ["charlie"] = "pass789"
            };

            var validateCredentials = RunnableLambda.Create<(string username, string password), (string username, bool isValid)>(
                creds => {
                    var attempts = failedAttempts.GetValueOrDefault(creds.username, 0);
                    if (attempts >= 3)
                        throw new Exception($"Account locked: {creds.username}");

                    var isValid = userDatabase.TryGetValue(creds.username, out var pwd) && pwd == creds.password;
                    if (!isValid)
                    {
                        failedAttempts[creds.username] = attempts + 1;
                        throw new UnauthorizedAccessException("Invalid credentials");
                    }

                    failedAttempts[creds.username] = 0;  // Reset on success
                    return (creds.username, true);
                });

            // Stage 3: Check user permissions (parallel checks)
            var permissionChecks = RunnableMap.Create<string, bool>(
                ("read", RunnableLambda.Create<string, bool>(u => true)),  // All users can read
                ("write", RunnableLambda.Create<string, bool>(u => u != "bob")),  // Bob can't write
                ("admin", RunnableLambda.Create<string, bool>(u => u == "alice"))  // Only Alice is admin
            );

            // Stage 4: Generate session
            var generateSession = RunnableLambda.Create<(string username, Dictionary<string, bool> permissions), string>(
                data => {
                    var sessionId = Guid.NewGuid().ToString();
                    sessionStore[sessionId] = data.username;
                    return sessionId;
                });

            // Stage 5: Create response
            var createResponse = RunnableLambda.Create<string, Dictionary<string, object>>(
                sessionId => new Dictionary<string, object> {
                    ["sessionId"] = sessionId,
                    ["expiresIn"] = 3600,
                    ["tokenType"] = "Bearer"
                });

            // Fallback for validation failures
            var validationFallback = RunnableLambda.Create<(string username, string password), (string username, bool isValid)>(
                creds => {
                    auditLog.Add($"Login failed: {creds.username}");
                    throw new UnauthorizedAccessException("Authentication failed");  // Re-throw for testing
                });

            // Act - Build authentication flow
            var authFlow = parseCredentials
                .WithRetry(1)  // Retry parsing once
                .Pipe(validateCredentials
                    .WithRetry(2)  // Retry auth twice
                    .WithFallback(validationFallback))  // Log and fail
                .TapAsync(async result => {
                    await Task.Delay(1);
                    auditLog.Add($"User authenticated: {result.username}");
                })
                .MapAsync(async result => (result.username, await permissionChecks.InvokeAsync(result.username)))  // Get permissions
                .TapAsync(async data => {
                    await Task.Delay(1);
                    var perms = string.Join(", ", data.Item2.Where(kvp => kvp.Value).Select(kvp => kvp.Key));
                    auditLog.Add($"Permissions for {data.username}: {perms}");
                })
                .Pipe(generateSession)
                .WithDelay(TimeSpan.FromMilliseconds(10))  // Rate limiting
                .Pipe(createResponse)
                .WithCache();  // Cache sessions

            // Test successful login
            var result1 = await authFlow.InvokeAsync("alice:password123");
            var result2 = await authFlow.InvokeAsync("alice:password123");  // Cache hit

            // Test different user
            var result3 = await authFlow.InvokeAsync("bob:secret456");

            // Assert
            Assert.NotNull(result1["sessionId"]);
            Assert.Equal(result1, result2);  // Cache hit

            Assert.NotNull(result3["sessionId"]);
            Assert.NotEqual(result1["sessionId"], result3["sessionId"]);

            Assert.Equal(2, auditLog.Count(l => l.StartsWith("User authenticated")));  // Not for cache
            Assert.Contains("admin", auditLog.Single(l => l.Contains("alice") && l.Contains("Permissions")));
            Assert.DoesNotContain("admin", auditLog.Single(l => l.Contains("bob") && l.Contains("Permissions")));

            // Test failed login
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => 
                authFlow.InvokeAsync("alice:wrongpassword"));
        }

        // ==================== Event-Driven Architecture Simulation ====================

        [Fact]
        public async Task AdvancedScenario3_EventProcessingPipeline_WorksCorrectly()
        {
            // Arrange - Event-driven system with routing and processing
            var eventLogs = new List<string>();
            var processedEvents = new Dictionary<string, int>();

            // Parse event
            var parseEvent = RunnableLambda.Create<string, (string eventType, string eventId, Dictionary<string, string> data)>(
                eventJson => {
                    var parts = eventJson.Split('|');
                    return (
                        eventType: parts[0],
                        eventId: parts[1],
                        data: new Dictionary<string, string> {
                            ["payload"] = parts.Length > 2 ? parts[2] : ""
                        }
                    );
                });

            // Event handlers
            var orderCreatedHandler = RunnableLambda.Create<(string eventType, string eventId, Dictionary<string, string> data), string>(
                evt => {
                    processedEvents["order_created"] = processedEvents.GetValueOrDefault("order_created", 0) + 1;
                    return $"Order processed: {evt.eventId}";
                });

            var paymentReceivedHandler = RunnableLambda.Create<(string eventType, string eventId, Dictionary<string, string> data), string>(
                evt => {
                    processedEvents["payment_received"] = processedEvents.GetValueOrDefault("payment_received", 0) + 1;
                    return $"Payment confirmed: {evt.eventId}";
                });

            var userRegisteredHandler = RunnableLambda.Create<(string eventType, string eventId, Dictionary<string, string> data), string>(
                evt => {
                    processedEvents["user_registered"] = processedEvents.GetValueOrDefault("user_registered", 0) + 1;
                    return $"Welcome sent: {evt.eventId}";
                });

            var unknownHandler = RunnableLambda.Create<(string eventType, string eventId, Dictionary<string, string> data), string>(
                evt => {
                    processedEvents["unknown"] = processedEvents.GetValueOrDefault("unknown", 0) + 1;
                    return $"Logged: {evt.eventId}";
                });

            // Transform response
            var transformResponse = RunnableLambda.Create<string, Dictionary<string, object>>(
                result => new Dictionary<string, object> {
                    ["status"] = "processed",
                    ["message"] = result,
                    ["timestamp"] = DateTime.UtcNow
                });

            // Act - Build event processing pipeline
            var eventPipeline = parseEvent
                .WithRetry(1)  // Retry parsing
                .TapAsync(async evt => {
                    await Task.Delay(1);
                    eventLogs.Add($"Processing event: {evt.eventType} - {evt.eventId}");
                })
                .Pipe(RunnableBranch.CreateAsync(
                    unknownHandler,
                    (async evt => await Task.FromResult(evt.eventType == "order.created"), orderCreatedHandler),
                    (async evt => await Task.FromResult(evt.eventType == "payment.received"), paymentReceivedHandler),
                    (async evt => await Task.FromResult(evt.eventType == "user.registered"), userRegisteredHandler)))
                .WithDelay(TimeSpan.FromMilliseconds(5))  // Rate limiting
                .Pipe(transformResponse)
                .WithCache();  // Idempotency - cache event results

            // Test events
            var evt1 = await eventPipeline.InvokeAsync("order.created|evt-001|{orderId:123}");
            var evt2 = await eventPipeline.InvokeAsync("payment.received|evt-002|{amount:99.99}");
            var evt3 = await eventPipeline.InvokeAsync("user.registered|evt-003|{userId:456}");
            var evt4 = await eventPipeline.InvokeAsync("unknown.event|evt-004|{data:test}");
            var evt5 = await eventPipeline.InvokeAsync("order.created|evt-001|{orderId:123}");  // Duplicate (cache)

            // Assert
            Assert.Equal("processed", evt1["status"]);
            Assert.Contains("Order processed", evt1["message"].ToString());

            Assert.Equal("processed", evt2["status"]);
            Assert.Contains("Payment confirmed", evt2["message"].ToString());

            Assert.Equal("processed", evt3["status"]);
            Assert.Contains("Welcome sent", evt3["message"].ToString());

            Assert.Equal("processed", evt4["status"]);
            Assert.Contains("Logged", evt4["message"].ToString());

            Assert.Equal(evt1, evt5);  // Cache hit - idempotency

            Assert.Equal(1, processedEvents["order_created"]);  // Not processed again for duplicate
            Assert.Equal(1, processedEvents["payment_received"]);
            Assert.Equal(1, processedEvents["user_registered"]);
            Assert.Equal(1, processedEvents["unknown"]);

            Assert.Equal(4, eventLogs.Count);  // Not logged for cache hit
        }

        // ==================== Distributed Cache with Fallback Layers ====================

        [Fact]
        public async Task AdvancedScenario4_MultiTierCaching_WorksCorrectly()
        {
            // Arrange - Multi-tier caching strategy
            var l1CacheHits = 0;
            var l2CacheHits = 0;
            var dbCalls = 0;

            var l1Cache = new Dictionary<string, string>();  // Fast in-memory cache
            var l2Cache = new Dictionary<string, string>();  // Distributed cache (simulated)
            var database = new Dictionary<string, string> {  // Database
                ["user:1"] = "Alice",
                ["user:2"] = "Bob",
                ["user:3"] = "Charlie"
            };

            // Try L1 cache
            var tryL1Cache = RunnableLambda.Create<string, string>(key => {
                if (l1Cache.TryGetValue(key, out var value))
                {
                    l1CacheHits++;
                    return value;
                }
                throw new Exception("L1 miss");
            });

            // Try L2 cache
            var tryL2Cache = RunnableLambda.Create<string, string>(key => {
                System.Threading.Thread.Sleep(5);  // Slower than L1
                if (l2Cache.TryGetValue(key, out var value))
                {
                    l2CacheHits++;
                    l1Cache[key] = value;  // Populate L1
                    return value;
                }
                throw new Exception("L2 miss");
            });

            // Query database
            var queryDatabase = RunnableLambda.Create<string, string>(key => {
                System.Threading.Thread.Sleep(20);  // Slowest
                dbCalls++;
                if (database.TryGetValue(key, out var value))
                {
                    l2Cache[key] = value;  // Populate L2
                    l1Cache[key] = value;  // Populate L1
                    return value;
                }
                throw new KeyNotFoundException($"Key not found: {key}");
            });

            // Default fallback
            var defaultFallback = RunnableLambda.Create<string, string>(
                key => "NOT_FOUND");

            // Act - Build multi-tier caching pipeline
            var cachePipeline = tryL1Cache
                .WithFallback(tryL2Cache
                    .WithRetry(1)  // Retry L2 once
                    .WithTimeout(TimeSpan.FromMilliseconds(200))
                    .WithFallback(queryDatabase
                        .WithRetry(2)  // Retry database twice
                        .WithTimeout(TimeSpan.FromMilliseconds(500))
                        .WithFallback(defaultFallback)))
                .TapAsync(async value => await Task.Delay(1));  // Log access

            // Test - Cold cache (all miss, hit database)
            var result1 = await cachePipeline.InvokeAsync("user:1");
            Assert.Equal("Alice", result1);
            Assert.Equal(0, l1CacheHits);
            Assert.Equal(0, l2CacheHits);
            Assert.Equal(1, dbCalls);

            // Test - Warm L1 cache
            var result2 = await cachePipeline.InvokeAsync("user:1");
            Assert.Equal("Alice", result2);
            Assert.Equal(1, l1CacheHits);
            Assert.Equal(0, l2CacheHits);
            Assert.Equal(1, dbCalls);  // No additional DB call

            // Test - Clear L1, hit L2
            l1Cache.Clear();
            var result3 = await cachePipeline.InvokeAsync("user:1");
            Assert.Equal("Alice", result3);
            Assert.Equal(1, l1CacheHits);  // No change
            Assert.Equal(1, l2CacheHits);
            Assert.Equal(1, dbCalls);  // Still no DB call

            // Test - Different key (cold)
            var result4 = await cachePipeline.InvokeAsync("user:2");
            Assert.Equal("Bob", result4);
            Assert.Equal(2, dbCalls);

            // Test - Not found
            var result5 = await cachePipeline.InvokeAsync("user:999");
            Assert.Equal("NOT_FOUND", result5);
        }

        // ==================== Workflow Orchestration with Conditional Steps ====================

        [Fact]
        public async Task AdvancedScenario5_WorkflowOrchestration_WorksCorrectly()
        {
            // Arrange - Multi-step workflow with conditional execution
            var workflowLogs = new List<string>();
            var executedSteps = new List<string>();

            // Step 1: Initialize workflow
            var initWorkflow = RunnableLambda.Create<string, (string workflowId, string status, Dictionary<string, object> context)>(
                workflowId => (
                    workflowId,
                    status: "initialized",
                    context: new Dictionary<string, object> { ["started"] = DateTime.UtcNow }
                ));

            // Step 2: Validation step (may skip based on context)
            var validateStep = RunnableLambda.Create<(string workflowId, string status, Dictionary<string, object> context), (string workflowId, string status, Dictionary<string, object> context)>(
                workflow => {
                    executedSteps.Add("validate");
                    workflowLogs.Add($"Validating workflow: {workflow.workflowId}");
                    workflow.context["validated"] = true;
                    return (workflow.workflowId, "validated", workflow.context);
                });

            // Step 3: Processing step (parallel operations)
            var processOps = RunnableMap.Create<(string workflowId, string status, Dictionary<string, object> context), string>(
                ("task1", RunnableLambda.Create<(string workflowId, string status, Dictionary<string, object> context), string>(
                    w => { executedSteps.Add("task1"); return "task1-complete"; })),
                ("task2", RunnableLambda.Create<(string workflowId, string status, Dictionary<string, object> context), string>(
                    w => { executedSteps.Add("task2"); return "task2-complete"; })),
                ("task3", RunnableLambda.Create<(string workflowId, string status, Dictionary<string, object> context), string>(
                    w => { executedSteps.Add("task3"); return "task3-complete"; }))
            );

            // Step 4: Finalization
            var finalizeWorkflow = RunnableLambda.Create<(Dictionary<string, string> results, Dictionary<string, object> context), string>(
                data => {
                    executedSteps.Add("finalize");
                    var allComplete = data.results.Values.All(v => v.EndsWith("complete"));
                    return allComplete ? "SUCCESS" : "PARTIAL_SUCCESS";
                });

            // Conditional routing for post-processing
            var successHandler = RunnableLambda.Create<string, Dictionary<string, object>>(
                status => new Dictionary<string, object> {
                    ["status"] = status,
                    ["completedAt"] = DateTime.UtcNow,
                    ["success"] = true
                });

            var partialSuccessHandler = RunnableLambda.Create<string, Dictionary<string, object>>(
                status => new Dictionary<string, object> {
                    ["status"] = status,
                    ["completedAt"] = DateTime.UtcNow,
                    ["success"] = false,
                    ["requiresReview"] = true
                });

            // Act - Build workflow orchestration
            var workflowPipeline = initWorkflow
                .WithRetry(1)  // Retry initialization
                .Pipe(validateStep
                    .WithTimeout(TimeSpan.FromMilliseconds(500)))
                .TapAsync(async w => {
                    await Task.Delay(1);
                    workflowLogs.Add($"Workflow {w.workflowId} validated");
                })
                .MapAsync(async workflow => {
                    var results = await processOps.InvokeAsync(workflow);
                    return (results, workflow.context);
                })
                .WithTimeout(TimeSpan.FromSeconds(1))  // Overall timeout
                .Pipe(finalizeWorkflow)
                .TapAsync(async status => {
                    await Task.Delay(1);
                    workflowLogs.Add($"Workflow finalized with status: {status}");
                })
                .Pipe(RunnableBranch.CreateAsync(
                    partialSuccessHandler,
                    (async status => await Task.FromResult(status == "SUCCESS"), successHandler),
                    (async status => await Task.FromResult(status == "PARTIAL_SUCCESS"), partialSuccessHandler)))
                .WithCache();  // Cache workflow results

            // Test workflow
            var result1 = await workflowPipeline.InvokeAsync("workflow-001");
            var result2 = await workflowPipeline.InvokeAsync("workflow-001");  // Cache
            var result3 = await workflowPipeline.InvokeAsync("workflow-002");  // Different workflow

            // Assert
            Assert.True((bool)result1["success"]);
            Assert.Equal("SUCCESS", result1["status"]);
            Assert.Equal(result1, result2);  // Cache hit

            Assert.True((bool)result3["success"]);

            // Steps executed correct number of times
            Assert.Equal(2, executedSteps.Count(s => s == "validate"));  // Two workflows (no cache)
            Assert.Equal(2, executedSteps.Count(s => s == "task1"));
            Assert.Equal(2, executedSteps.Count(s => s == "task2"));
            Assert.Equal(2, executedSteps.Count(s => s == "task3"));
            Assert.Equal(2, executedSteps.Count(s => s == "finalize"));

            Assert.True(workflowLogs.Count >= 4);  // Includes validation and finalization logs
        }
    }
}
