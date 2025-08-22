using Grpc.Core;
using Microsoft.Extensions.Logging;
using Reckondb.Client.Messages;

namespace ExESDBGrpc.Client;

/// <summary>
/// Implementation of subscription operations
/// </summary>
internal class SubscriptionOperations : ISubscriptionOperations
{
    private readonly global::Reckondb.Client.Messages.SubscriptionManagement.SubscriptionManagementClient _client;
    private readonly ExESDBClientOptions _options;
    private readonly ILogger? _logger;

    public SubscriptionOperations(global::Reckondb.Client.Messages.SubscriptionManagement.SubscriptionManagementClient client, ExESDBClientOptions options, ILogger? logger)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
        _options = options ?? throw new ArgumentNullException(nameof(options));
        _logger = logger;
    }

    // Stub implementation using actual protobuf types that exist
    public async Task<CreatePersistentSubscriptionResponse> CreatePersistentSubscriptionAsync(CreatePersistentSubscriptionRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger?.LogDebug("Creating persistent subscription {SubscriptionName}", request.SubscriptionName);
            return await _client.CreatePersistentSubscriptionAsync(request, cancellationToken: cancellationToken);
        }
        catch (RpcException ex)
        {
            _logger?.LogError(ex, "Failed to create persistent subscription {SubscriptionName}", request.SubscriptionName);
            throw;
        }
    }

    public async Task<RemovePersistentSubscriptionResponse> RemovePersistentSubscriptionAsync(RemovePersistentSubscriptionRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger?.LogDebug("Removing persistent subscription {SubscriptionName}", request.SubscriptionName);
            return await _client.RemovePersistentSubscriptionAsync(request, cancellationToken: cancellationToken);
        }
        catch (RpcException ex)
        {
            _logger?.LogError(ex, "Failed to remove persistent subscription {SubscriptionName}", request.SubscriptionName);
            throw;
        }
    }

    public async Task<ListSubscriptionsResponse> ListSubscriptionsAsync(string? storeId = null, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger?.LogDebug("Listing subscriptions from store {StoreId}", storeId ?? "default");
            var request = new ListSubscriptionsRequest();
            if (!string.IsNullOrEmpty(storeId))
            {
                request.StoreId = storeId;
            }
            return await _client.ListSubscriptionsAsync(request, cancellationToken: cancellationToken);
        }
        catch (RpcException ex)
        {
            _logger?.LogError(ex, "Failed to list subscriptions");
            throw;
        }
    }

    public async Task<AckEventResponse> AckEventAsync(AckEventRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger?.LogDebug("Acknowledging event for subscription {SubscriptionName}", request.SubscriptionName);
            return await _client.AckEventAsync(request, cancellationToken: cancellationToken);
        }
        catch (RpcException ex)
        {
            _logger?.LogError(ex, "Failed to acknowledge event for subscription {SubscriptionName}", request.SubscriptionName);
            throw;
        }
    }
}
