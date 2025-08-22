using Reckondb.Client.Messages;

namespace ExESDBGrpc.Client;

/// <summary>
/// Interface for event store operations
/// </summary>
public interface IEventStoreOperations
{
    /// <summary>
    /// Write events to a stream
    /// </summary>
    Task<WriteEventsCompleted> WriteEventsAsync(WriteEvents request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Read a single event from a stream
    /// </summary>
    Task<ReadEventCompleted> ReadEventAsync(ReadEvent request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Read multiple events from a stream
    /// </summary>
    Task<ReadStreamEventsCompleted> ReadStreamEventsAsync(ReadStreamEvents request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Read all events from the global stream
    /// </summary>
    Task<ReadAllEventsCompleted> ReadAllEventsAsync(ReadAllEvents request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Subscribe to stream events
    /// </summary>
    IAsyncEnumerable<StreamEventAppeared> SubscribeToStreamAsync(SubscribeToStream request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Delete a stream
    /// </summary>
    Task<DeleteStreamCompleted> DeleteStreamAsync(DeleteStream request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get stream information
    /// </summary>
    Task<GetStreamInfoResponse> GetStreamInfoAsync(string streamId, string? storeId = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Test server health
    /// </summary>
    Task<HealthCheckResponse> HealthCheckAsync(CancellationToken cancellationToken = default);
}

/// <summary>
/// Interface for store management operations
/// </summary>
public interface IStoreManagement
{
    /// <summary>
    /// List all available stores
    /// </summary>
    Task<ListStoresResponse> ListStoresAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Set the store context for operations
    /// </summary>
    Task<SetStoreContextResponse> SetStoreContextAsync(string storeId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get the current store context
    /// </summary>
    Task<GetStoreContextResponse> GetStoreContextAsync(CancellationToken cancellationToken = default);
}

/// <summary>
/// Interface for snapshot operations
/// </summary>
/// <remarks>
/// Note: This interface uses simplified method signatures since the snapshot protobuf messages
/// may differ from the expected signatures. Actual implementation will use the generated types.
/// </remarks>
public interface ISnapshots
{
    /// <summary>
    /// Record a snapshot with simplified parameters
    /// </summary>
    Task<RecordSnapshotResponse> RecordSnapshotAsync(string streamName, byte[] data, string? storeId = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Read a snapshot with simplified parameters
    /// </summary>
    Task<ReadSnapshotResponse> ReadSnapshotAsync(string streamName, string? storeId = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Delete a snapshot
    /// </summary>
    Task<DeleteSnapshotResponse> DeleteSnapshotAsync(string streamName, string? storeId = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// List snapshots
    /// </summary>
    Task<ListSnapshotsResponse> ListSnapshotsAsync(string? storeId = null, CancellationToken cancellationToken = default);
}

/// <summary>
/// Interface for stream operations
/// </summary>
public interface IStreamOperations
{
    /// <summary>
    /// Get all streams in a store
    /// </summary>
    Task<GetStreamsResponse> GetStreamsAsync(string? storeId = null, int? maxCount = null, string? continuationToken = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get stream version
    /// </summary>
    Task<GetStreamVersionResponse> GetStreamVersionAsync(string streamId, string? storeId = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Stream events forward
    /// </summary>
    IAsyncEnumerable<StreamEventBatch> StreamForwardAsync(StreamForwardRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Stream events backward
    /// </summary>
    IAsyncEnumerable<StreamEventBatch> StreamBackwardAsync(StreamBackwardRequest request, CancellationToken cancellationToken = default);
}

/// <summary>
/// Interface for subscription operations
/// </summary>
public interface ISubscriptionOperations
{
    /// <summary>
    /// Create a persistent subscription
    /// </summary>
    Task<CreatePersistentSubscriptionResponse> CreatePersistentSubscriptionAsync(CreatePersistentSubscriptionRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Remove a persistent subscription
    /// </summary>
    Task<RemovePersistentSubscriptionResponse> RemovePersistentSubscriptionAsync(RemovePersistentSubscriptionRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// List subscriptions for a store
    /// </summary>
    Task<ListSubscriptionsResponse> ListSubscriptionsAsync(string? storeId = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Acknowledge an event in a persistent subscription
    /// </summary>
    Task<AckEventResponse> AckEventAsync(AckEventRequest request, CancellationToken cancellationToken = default);
}
