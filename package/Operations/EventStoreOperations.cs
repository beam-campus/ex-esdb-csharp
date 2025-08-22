using Grpc.Core;
using Microsoft.Extensions.Logging;
using Reckondb.Client.Messages;

namespace ExESDBGrpc.Client;

/// <summary>
/// Stub implementation of EventStore operations
/// </summary>
internal class EventStoreOperations : IEventStoreOperations
{
    private readonly global::Reckondb.Client.Messages.StreamOperations.StreamOperationsClient _client;
    private readonly ExESDBClientOptions _options;
    private readonly ILogger? _logger;

    public EventStoreOperations(global::Reckondb.Client.Messages.StreamOperations.StreamOperationsClient client, ExESDBClientOptions options, ILogger? logger)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
        _options = options ?? throw new ArgumentNullException(nameof(options));
        _logger = logger;
    }

    // All methods are stubbed out for now since the protobuf doesn't match
    public Task<WriteEventsCompleted> WriteEventsAsync(WriteEvents request, CancellationToken cancellationToken = default)
        => throw new NotImplementedException("EventStore operations not yet implemented");
    
    public Task<ReadEventCompleted> ReadEventAsync(ReadEvent request, CancellationToken cancellationToken = default)
        => throw new NotImplementedException("EventStore operations not yet implemented");
    
    public Task<ReadStreamEventsCompleted> ReadStreamEventsAsync(ReadStreamEvents request, CancellationToken cancellationToken = default)
        => throw new NotImplementedException("EventStore operations not yet implemented");
    
    public Task<ReadAllEventsCompleted> ReadAllEventsAsync(ReadAllEvents request, CancellationToken cancellationToken = default)
        => throw new NotImplementedException("EventStore operations not yet implemented");
    
    public async IAsyncEnumerable<StreamEventAppeared> SubscribeToStreamAsync(SubscribeToStream request, 
        [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException("EventStore operations not yet implemented");
        #pragma warning disable CS0162 // Unreachable code detected
        yield break;
        #pragma warning restore CS0162 // Unreachable code detected
    }
    
    public Task<DeleteStreamCompleted> DeleteStreamAsync(DeleteStream request, CancellationToken cancellationToken = default)
        => throw new NotImplementedException("EventStore operations not yet implemented");
    
    public Task<GetStreamInfoResponse> GetStreamInfoAsync(string streamId, string? storeId = null, CancellationToken cancellationToken = default)
        => throw new NotImplementedException("EventStore operations not yet implemented");
    
    public Task<HealthCheckResponse> HealthCheckAsync(CancellationToken cancellationToken = default)
        => throw new NotImplementedException("EventStore operations not yet implemented");
}
