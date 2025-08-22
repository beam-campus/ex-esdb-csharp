using ExESDBGrpc.Client;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace ExESDBGrpc.Tests;

/// <summary>
/// Unit tests for the ExESDBClient
/// </summary>
public class ExESDBClientTests
{
    private readonly Mock<ILogger<ExESDBClient>> _mockLogger;
    private readonly ExESDBClientOptions _defaultOptions;

    public ExESDBClientTests()
    {
        _mockLogger = new Mock<ILogger<ExESDBClient>>();
        _defaultOptions = new ExESDBClientOptions
        {
            ServerAddress = "localhost:2113",
            ConnectTimeout = TimeSpan.FromSeconds(5),
            RequestTimeout = TimeSpan.FromSeconds(5)
        };
    }

    [Fact]
    public void Constructor_WithValidOptions_ShouldCreateInstance()
    {
        // Act
        var client = new ExESDBClient(_defaultOptions, _mockLogger.Object);

        // Assert
        Assert.NotNull(client);
        Assert.NotNull(client.EventStore);
        Assert.NotNull(client.StoreManagement);
        Assert.NotNull(client.Snapshots);
        Assert.NotNull(client.StreamOperations);
        Assert.NotNull(client.Subscriptions);
    }

    [Fact]
    public void Constructor_WithNullOptions_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new ExESDBClient(null!, _mockLogger.Object));
    }

    [Fact]
    public void Constructor_WithoutLogger_ShouldCreateInstance()
    {
        // Act
        var client = new ExESDBClient(_defaultOptions);

        // Assert
        Assert.NotNull(client);
        Assert.NotNull(client.EventStore);
        Assert.NotNull(client.StoreManagement);
        Assert.NotNull(client.Snapshots);
        Assert.NotNull(client.StreamOperations);
        Assert.NotNull(client.Subscriptions);
    }

    [Fact]
    public void Constructor_WithEmptyServerAddress_ShouldThrowArgumentException()
    {
        // Arrange
        var options = new ExESDBClientOptions
        {
            ServerAddress = "",
            ConnectTimeout = TimeSpan.FromSeconds(5),
            RequestTimeout = TimeSpan.FromSeconds(5)
        };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new ExESDBClient(options, _mockLogger.Object));
    }

    [Fact]
    public void Constructor_WithInvalidTimeout_ShouldThrowArgumentException()
    {
        // Arrange
        var options = new ExESDBClientOptions
        {
            ServerAddress = "localhost:2113",
            ConnectTimeout = TimeSpan.Zero,
            RequestTimeout = TimeSpan.FromSeconds(5)
        };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new ExESDBClient(options, _mockLogger.Object));
    }

    [Fact]
    public void Dispose_ShouldNotThrowException()
    {
        // Arrange
        var client = new ExESDBClient(_defaultOptions, _mockLogger.Object);

        // Act & Assert
        var exception = Record.Exception(() => client.Dispose());
        Assert.Null(exception);
    }

    [Fact]
    public void Dispose_CalledMultipleTimes_ShouldNotThrowException()
    {
        // Arrange
        var client = new ExESDBClient(_defaultOptions, _mockLogger.Object);

        // Act & Assert
        var exception1 = Record.Exception(() => client.Dispose());
        var exception2 = Record.Exception(() => client.Dispose());
        Assert.Null(exception1);
        Assert.Null(exception2);
    }

    // Note: Integration tests that actually connect to a server would go in a separate test project
    // These unit tests focus on the client construction and basic validation
}
