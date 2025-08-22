using Grpc.Core;
using Microsoft.Extensions.Logging;
using Reckondb.Client.Messages;

namespace ExESDBGrpc.Client;

/// <summary>
/// Stub implementation of snapshot operations
/// </summary>
internal class SnapshotOperations : ISnapshots
{
    private readonly global::Reckondb.Client.Messages.StreamOperations.StreamOperationsClient _client;
    private readonly ExESDBClientOptions _options;
    private readonly ILogger? _logger;

    public SnapshotOperations(global::Reckondb.Client.Messages.StreamOperations.StreamOperationsClient client, ExESDBClientOptions options, ILogger? logger)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
        _options = options ?? throw new ArgumentNullException(nameof(options));
        _logger = logger;
    }

    public Task<RecordSnapshotResponse> RecordSnapshotAsync(string streamName, byte[] data, 
        string? storeId = null, CancellationToken cancellationToken = default)
        => throw new NotImplementedException("Snapshot operations not yet implemented");
    
    public Task<ReadSnapshotResponse> ReadSnapshotAsync(string streamName, 
        string? storeId = null, CancellationToken cancellationToken = default)
        => throw new NotImplementedException("Snapshot operations not yet implemented");
    
    public Task<DeleteSnapshotResponse> DeleteSnapshotAsync(string streamName, 
        string? storeId = null, CancellationToken cancellationToken = default)
        => throw new NotImplementedException("Snapshot operations not yet implemented");
    
    public Task<ListSnapshotsResponse> ListSnapshotsAsync(string? storeId = null, CancellationToken cancellationToken = default)
        => throw new NotImplementedException("Snapshot operations not yet implemented");
}
