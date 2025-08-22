using ExESDBGrpc.Client;
using Microsoft.Extensions.Logging;
using Reckondb.Client.Messages;

namespace ExESDBGrpc.Examples;

/// <summary>
/// Simple example demonstrating basic usage of the C# gRPC client
/// </summary>
public static class SimpleExample
{
    /// <summary>
    /// Example showing the working stream operations
    /// </summary>
    public static async Task RunStreamExample()
    {
        // Configure client options
        var options = new ExESDBClientOptions
        {
            ServerAddress = "localhost:2113" // Default EventStore gRPC port
        };

        // Create the client
        using var client = new ExESDBClient(options);

        try
        {
            Console.WriteLine("Testing stream operations...");

            // Get streams (this uses the actual working gRPC method)
            var getStreamsResponse = await client.StreamOperations.GetStreamsAsync();
            Console.WriteLine($"Found {getStreamsResponse.Streams.Count} streams");
            
            if (getStreamsResponse.Streams.Count > 0)
            {
                var firstStream = getStreamsResponse.Streams[0];
                Console.WriteLine($"First stream: {firstStream.StreamId}, Version: {firstStream.CurrentVersion}");
                
                // Get stream version
                var versionResponse = await client.StreamOperations.GetStreamVersionAsync(firstStream.StreamId);
                Console.WriteLine($"Stream version details - Success: {versionResponse.Success}, Version: {versionResponse.Version}");
            }

            Console.WriteLine("Stream operations example completed successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    /// <summary>
    /// Example showing subscription operations
    /// </summary>
    public static async Task RunSubscriptionExample()
    {
        var options = new ExESDBClientOptions
        {
            ServerAddress = "localhost:2113"
        };

        using var client = new ExESDBClient(options);

        try
        {
            Console.WriteLine("Testing subscription operations...");

            // List subscriptions
            var subscriptionsResponse = await client.Subscriptions.ListSubscriptionsAsync();
            Console.WriteLine($"Found {subscriptionsResponse.Subscriptions.Count} subscriptions");

            foreach (var subscription in subscriptionsResponse.Subscriptions)
            {
                Console.WriteLine($"Subscription: {subscription.SubscriptionName}, Type: {subscription.Type}");
            }

            Console.WriteLine("Subscription operations example completed!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    /// <summary>
    /// Example showing forward streaming
    /// </summary>
    public static async Task RunStreamingExample()
    {
        var options = new ExESDBClientOptions
        {
            ServerAddress = "localhost:2113"
        };

        using var client = new ExESDBClient(options);

        try
        {
            Console.WriteLine("Testing streaming operations...");

            // Create a streaming request
            var streamRequest = new StreamForwardRequest
            {
                StoreId = "test-store",
                StreamId = "test-stream",
                StartVersion = 0,
                Count = 10
            };

            int eventCount = 0;
            await foreach (var batch in client.StreamOperations.StreamForwardAsync(streamRequest))
            {
                Console.WriteLine($"Received batch with {batch.Events.Count} events");
                eventCount += batch.Events.Count;
                
                if (batch.IsEndOfStream)
                {
                    Console.WriteLine("Reached end of stream");
                    break;
                }
            }

            Console.WriteLine($"Streaming example completed - received {eventCount} events total");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    /// <summary>
    /// Test connection example
    /// </summary>
    public static async Task TestConnection()
    {
        var options = new ExESDBClientOptions
        {
            ServerAddress = "localhost:2113"
        };

        using var client = new ExESDBClient(options);

        try
        {
            Console.WriteLine("Testing connection...");
            bool isConnected = await client.TestConnectionAsync();
            Console.WriteLine($"Connection test: {(isConnected ? "Success" : "Failed")}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Connection failed: {ex.Message}");
        }
    }
}
