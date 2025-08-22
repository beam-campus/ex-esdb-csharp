using Grpc.Core;
using Microsoft.Extensions.Logging;
using Reckondb.Client.Messages;

namespace ExESDBGrpc.Client;

/// <summary>
/// Stub implementation of store management operations
/// </summary>
internal class StoreManagementOperations : IStoreManagement
{
    private readonly global::Reckondb.Client.Messages.StreamOperations.StreamOperationsClient _client;
    private readonly ExESDBClientOptions _options;
    private readonly ILogger? _logger;

    public StoreManagementOperations(global::Reckondb.Client.Messages.StreamOperations.StreamOperationsClient client, ExESDBClientOptions options, ILogger? logger)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
        _options = options ?? throw new ArgumentNullException(nameof(options));
        _logger = logger;
    }

    public Task<ListStoresResponse> ListStoresAsync(CancellationToken cancellationToken = default)
        => throw new NotImplementedException("Store management operations not yet implemented");
    
    public Task<SetStoreContextResponse> SetStoreContextAsync(string storeId, CancellationToken cancellationToken = default)
        => throw new NotImplementedException("Store management operations not yet implemented");
    
    public Task<GetStoreContextResponse> GetStoreContextAsync(CancellationToken cancellationToken = default)
        => throw new NotImplementedException("Store management operations not yet implemented");
}
