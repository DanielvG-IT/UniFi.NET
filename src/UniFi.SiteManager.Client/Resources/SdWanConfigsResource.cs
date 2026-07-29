using UniFi.SiteManager.Client.Http;
using UniFi.SiteManager.Client.Models;

namespace UniFi.SiteManager.Client.Resources;

/// <summary>SD-WAN configurations and their live status.</summary>
public sealed class SdWanConfigsResource
{
    private readonly ApiConnection _connection;

    internal SdWanConfigsResource(ApiConnection connection) => _connection = connection;

    /// <summary>List all SD-WAN configs on your account.</summary>
    public Task<IReadOnlyList<SdWanConfig>?> ListAsync(CancellationToken cancellationToken = default)
        => _connection.GetAsync<IReadOnlyList<SdWanConfig>>("v1/sd-wan-configs", cancellationToken: cancellationToken);

    /// <summary>Get the full definition of a single SD-WAN config.</summary>
    public Task<SdWanConfigDetails?> GetAsync(string configId, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(configId);
        return _connection.GetAsync<SdWanConfigDetails>(
            $"v1/sd-wan-configs/{Uri.EscapeDataString(configId)}",
            cancellationToken: cancellationToken);
    }

    /// <summary>Get the live status of a single SD-WAN config, including hub/spoke tunnel state.</summary>
    public Task<SdWanConfigStatus?> GetStatusAsync(string configId, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(configId);
        return _connection.GetAsync<SdWanConfigStatus>(
            $"v1/sd-wan-configs/{Uri.EscapeDataString(configId)}/status",
            cancellationToken: cancellationToken);
    }
}
