using Grpc.Net.Client;
using Microsoft.Extensions.Logging;
using Reckondb.Client.Messages;
using System.Text.Json;

namespace ExESDBGrpc.Client;

/// <summary>
/// Configuration options for the ExESDB gRPC client
/// </summary>
public class ExESDBClientOptions
{
    /// <summary>
    /// The gRPC server address (e.g., "localhost:2113")
    /// </summary>
    public string ServerAddress { get; set; } = "localhost:2113";

    /// <summary>
    /// Credentials for authentication (null for insecure connections)
    /// </summary>
    public object? Credentials { get; set; }

    /// <summary>
    /// Connection timeout
    /// </summary>
    public TimeSpan ConnectTimeout { get; set; } = TimeSpan.FromSeconds(30);

    /// <summary>
    /// Request timeout
    /// </summary>
    public TimeSpan RequestTimeout { get; set; } = TimeSpan.FromSeconds(10);

    /// <summary>
    /// Additional gRPC channel options
    /// </summary>
    public GrpcChannelOptions? ChannelOptions { get; set; }
}

/// <summary>
/// Main client for interacting with ExESDB gRPC services
/// </summary>
public class ExESDBClient : IDisposable
{
    private readonly GrpcChannel _channel;
    private readonly global::Reckondb.Client.Messages.StreamOperations.StreamOperationsClient _streamClient;
    private readonly SubscriptionManagement.SubscriptionManagementClient _subscriptionClient;
    private readonly ILogger<ExESDBClient>? _logger;
    private readonly ExESDBClientOptions _options;
    private bool _disposed;

    /// <summary>
    /// Gets the event store operations client
    /// </summary>
    public IEventStoreOperations EventStore { get; }

    /// <summary>
    /// Gets the store management client
    /// </summary>
    public IStoreManagement StoreManagement { get; }

    /// <summary>
    /// Gets the snapshot operations client
    /// </summary>
    public ISnapshots Snapshots { get; }

    /// <summary>
    /// Gets the stream operations client
    /// </summary>
    public IStreamOperations StreamOperations { get; }

    /// <summary>
    /// Gets the subscription management client
    /// </summary>
    public ISubscriptionOperations Subscriptions { get; }

    /// <summary>
    /// Initializes a new instance of the ExESDB client
    /// </summary>
    /// <param name="options">Client configuration options</param>
    /// <param name="logger">Optional logger instance</param>
    public ExESDBClient(ExESDBClientOptions options, ILogger<ExESDBClient>? logger = null)
    {
        _options = options ?? throw new ArgumentNullException(nameof(options));
        _logger = logger;

        if (string.IsNullOrEmpty(options.ServerAddress))
            throw new ArgumentException("Server address cannot be null or empty", nameof(options));
        
        if (options.ConnectTimeout <= TimeSpan.Zero)
            throw new ArgumentException("Connect timeout must be positive", nameof(options));
            
        if (options.RequestTimeout <= TimeSpan.Zero)
            throw new ArgumentException("Request timeout must be positive", nameof(options));

        // Configure gRPC channel
        var channelOptions = options.ChannelOptions ?? new GrpcChannelOptions();
        var serverUrl = options.ServerAddress.StartsWith("http") ? options.ServerAddress : $"http://{options.ServerAddress}";
        
        _channel = GrpcChannel.ForAddress(serverUrl, channelOptions);

        // Initialize only the gRPC clients that actually exist
        _streamClient = new global::Reckondb.Client.Messages.StreamOperations.StreamOperationsClient(_channel);
        _subscriptionClient = new global::Reckondb.Client.Messages.SubscriptionManagement.SubscriptionManagementClient(_channel);

        // Initialize wrapper services with stub implementations for now
        EventStore = new EventStoreOperations(_streamClient, _options, _logger);
        StoreManagement = new StoreManagementOperations(_streamClient, _options, _logger);
        Snapshots = new SnapshotOperations(_streamClient, _options, _logger);
        StreamOperations = new StreamOperations(_streamClient, _options, _logger);
        Subscriptions = new SubscriptionOperations(_subscriptionClient, _options, _logger);

        _logger?.LogDebug("ExESDB client initialized with server address: {ServerAddress}", options.ServerAddress);
    }

    /// <summary>
    /// Creates a client with minimal configuration
    /// </summary>
    /// <param name="serverAddress">The gRPC server address</param>
    /// <returns>A configured ExESDB client</returns>
    public static ExESDBClient Create(string serverAddress)
    {
        return new ExESDBClient(new ExESDBClientOptions
        {
            ServerAddress = serverAddress
        });
    }

    /// <summary>
    /// Tests the connection to the server by making a simple gRPC call
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if the connection is healthy</returns>
    public async Task<bool> TestConnectionAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            // Make a simple call to test connectivity
            var request = new GetStreamsRequest();
            await _streamClient.GetStreamsAsync(request, cancellationToken: cancellationToken);
            return true;
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Connection test failed");
            return false;
        }
    }

    /// <summary>
    /// Disposes the client and releases resources
    /// </summary>
    public void Dispose()
    {
        if (!_disposed)
        {
            _channel?.Dispose();
            _disposed = true;
        }
        GC.SuppressFinalize(this);
    }
}
