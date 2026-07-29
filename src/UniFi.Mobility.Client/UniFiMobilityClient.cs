using UniFi.Mobility.Client.Http;
using UniFi.Mobility.Client.Resources;

namespace UniFi.Mobility.Client;

/// <summary>
/// Entry point for the UniFi Mobility API (api.ui.com). Construct with an API key that carries
/// the <c>mobility</c> scope, generated at unifi.ui.com.
/// </summary>
public sealed class UniFiMobilityClient : IDisposable
{
    private readonly ApiConnection _connection;

    public UniFiMobilityClient(MobilityClientOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        _connection = new ApiConnection(options);

        Workspaces = new WorkspacesResource(_connection);
        Devices = new DevicesResource(_connection);
    }

    /// <summary>Convenience overload that builds options from just an API key.</summary>
    public UniFiMobilityClient(string apiKey)
        : this(MobilityClientOptions.Create(apiKey))
    {
    }

    /// <summary>Workspaces (mobility cloud sites) and their admins.</summary>
    public WorkspacesResource Workspaces { get; }

    /// <summary>Mobile routing devices, their clients, and configuration.</summary>
    public DevicesResource Devices { get; }

    public void Dispose() => _connection.Dispose();
}
