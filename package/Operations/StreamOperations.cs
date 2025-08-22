using Grpc.Core;
using Microsoft.Extensions.Logging;
using Reckondb.Client.Messages;

namespace ExESDBGrpc.Client;

/// <summary>
/// Implementation of stream operations
/// </summary>
internal class StreamOperations : IStreamOperations
{
    private readonly global::Reckondb.Client.Messages.StreamOperations.StreamOperationsClient _client;
    private readonly ExESDBClientOptions _options;
    private readonly ILogger? _logger;

    public StreamOperations(global::Reckondb.Client.Messages.StreamOperations.StreamOperationsClient client, ExESDBClientOptions options, ILogger? logger)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
        _options = options ?? throw new ArgumentNullException(nameof(options));
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<GetStreamsResponse> GetStreamsAsync(string? storeId = null, int? maxCount = null, 
        string? continuationToken = null, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger?.LogDebug("Getting streams from store {StoreId}", storeId ?? "default");
            
            var request = new GetStreamsRequest();

            if (!string.IsNullOrEmpty(storeId))
            {
                request.StoreId = storeId;
            }
            if (maxCount.HasValue)
            {
                request.MaxCount = maxCount.Value;
            }
            if (!string.IsNullOrEmpty(continuationToken))
            {
                request.ContinuationToken = continuationToken;
            }

            return await _client.GetStreamsAsync(request, cancellationToken: cancellationToken);
        }
        catch (RpcException ex)
        {
            _logger?.LogError(ex, "Failed to get streams");
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<GetStreamVersionResponse> GetStreamVersionAsync(string streamId, 
        string? storeId = null, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger?.LogDebug("Getting stream version for {StreamId} from store {StoreId}", streamId, storeId ?? "default");
            
            var request = new GetStreamVersionRequest 
            { 
                StreamId = streamId
            };

            if (!string.IsNullOrEmpty(storeId))
            {
                request.StoreId = storeId;
            }

            return await _client.GetStreamVersionAsync(request, cancellationToken: cancellationToken);
        }
        catch (RpcException ex)
        {
            _logger?.LogError(ex, "Failed to get stream version for {StreamId}", streamId);
            throw;
        }
    }

    /// <inheritdoc />
    public async IAsyncEnumerable<StreamEventBatch> StreamForwardAsync(StreamForwardRequest request, 
        [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        _logger?.LogDebug("Starting forward stream for {StreamId} from store {StoreId}", 
            request.StreamId, request.StoreId ?? "default");

        using var call = _client.StreamForward(request, cancellationToken: cancellationToken);
        
        await foreach (var batch in call.ResponseStream.ReadAllAsync(cancellationToken).ConfigureAwait(false))
        {
            yield return batch;
        }
    }

    /// <inheritdoc />
    public async IAsyncEnumerable<StreamEventBatch> StreamBackwardAsync(StreamBackwardRequest request, 
        [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        _logger?.LogDebug("Starting backward stream for {StreamId} from store {StoreId}", 
            request.StreamId, request.StoreId ?? "default");

        using var call = _client.StreamBackward(request, cancellationToken: cancellationToken);
        
        await foreach (var batch in call.ResponseStream.ReadAllAsync(cancellationToken).ConfigureAwait(false))
        {
            yield return batch;
        }
    }
}
