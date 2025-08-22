# ExESDBGrpc.Client

A C# gRPC client library for ExESDB (Event Store Database), providing event sourcing capabilities with an idiomatic .NET API.

## Current Status

✅ **Working**: Core client infrastructure and working gRPC operations  
🚧 **In Progress**: Full EventStore API implementation  
📋 **Planned**: Complete examples and documentation  

## Implemented Features

- **Stream Operations**: Get streams, get stream versions, forward/backward streaming
- **Subscription Management**: Persistent subscriptions, list subscriptions, event acknowledgment  
- **Client Infrastructure**: Connection management, logging, disposal patterns
- **Type Safety**: Generated protobuf message contracts from ExESDB schemas
- **Async Support**: Full async/await with cancellation token support
- **Streaming**: Server-side streaming for event batches

## Stub Implementations (Not Yet Implemented)

- **EventStore Operations**: Write/Read events, stream subscriptions, health checks
- **Store Management**: Multi-store context switching
- **Snapshot Operations**: Create, read, delete snapshots

## Installation

```bash
dotnet add package ExESDBGrpc.Client
```

## Quick Start (Current Working Features)

```csharp
using ExESDBGrpc.Client;
using Reckondb.Client.Messages;

// Configure the client
var options = new ExESDBClientOptions
{
    ServerAddress = "localhost:2113", // Your gRPC server
    ConnectTimeout = TimeSpan.FromSeconds(30),
    RequestTimeout = TimeSpan.FromSeconds(10)
};

// Create client
using var client = new ExESDBClient(options);

// Test connection
bool isConnected = await client.TestConnectionAsync();
Console.WriteLine($"Connected: {isConnected}");

// Get streams (working implementation)
var getStreamsResponse = await client.StreamOperations.GetStreamsAsync(
    storeId: "my-store", 
    maxCount: 50);

foreach (var stream in getStreamsResponse.Streams)
{
    Console.WriteLine($"Stream: {stream.StreamId}, Version: {stream.CurrentVersion}");
}

// Get stream version
var versionResponse = await client.StreamOperations.GetStreamVersionAsync("user-123");
if (versionResponse.Success)
{
    Console.WriteLine($"Stream version: {versionResponse.Version}");
}

// Forward streaming (server-side streaming)
var request = new StreamForwardRequest
{
    StreamId = "user-123",
    StartVersion = 0,
    Count = 100
};

await foreach (var batch in client.StreamOperations.StreamForwardAsync(request))
{
    Console.WriteLine($"Received {batch.Events.Count} events");
    if (batch.IsEndOfStream) break;
}
```

## Working Subscription Operations

```csharp
// List subscriptions (working implementation)
var subscriptionsResponse = await client.Subscriptions.ListSubscriptionsAsync("my-store");
Console.WriteLine($"Found {subscriptionsResponse.Subscriptions.Count} subscriptions");

foreach (var subscription in subscriptionsResponse.Subscriptions)
{
    Console.WriteLine($"Subscription: {subscription.SubscriptionName}, Type: {subscription.Type}");
}

// Create persistent subscription
var createRequest = new CreatePersistentSubscriptionRequest
{
    StoreId = "my-store",
    Type = SubscriptionType.ByStream,
    Selector = "user-events",
    SubscriptionName = "user-processor",
    StartFrom = 0
};

var createResponse = await client.Subscriptions.CreatePersistentSubscriptionAsync(createRequest);
if (createResponse.Success)
{
    Console.WriteLine($"Created subscription: {createResponse.SubscriptionId}");
}
```

## Note on Stub Implementations

The following operations throw `NotImplementedException` and are planned for future implementation:

```csharp
// These will throw NotImplementedException:
// await client.EventStore.WriteEventsAsync(...);
// await client.StoreManagement.ListStoresAsync();
// await client.Snapshots.RecordSnapshotAsync(...);
```

## License

MIT License - see LICENSE file for details.
