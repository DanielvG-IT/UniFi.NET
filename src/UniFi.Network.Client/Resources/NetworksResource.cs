using System.Text.Json.Nodes;
using UniFi.Network.Client.Http;
using UniFi.Network.Client.Models;

namespace UniFi.Network.Client.Resources;

/// <summary>
/// Networks. Create/update bodies are shaped as raw <see cref="JsonObject"/> because their
/// content is management-type specific (GATEWAY/SWITCH/UNMANAGED) and goes several levels deep
/// (DHCP, IPv4/IPv6 configuration). Build the object per the OpenAPI "Create or update Network"
/// schema, e.g.:
/// <code>
/// new JsonObject
/// {
///     ["management"] = "UNMANAGED",
///     ["name"] = "Guest",
///     ["enabled"] = true,
///     ["vlanId"] = 20,
/// }
/// </code>
/// </summary>
public sealed class NetworksResource
{
    private readonly ApiConnection _connection;

    internal NetworksResource(ApiConnection connection) => _connection = connection;

    public Task<PagedResult<NetworkOverview>> ListAsync(
        Guid siteId,
        int? offset = null,
        int? limit = null,
        string? filter = null,
        CancellationToken cancellationToken = default)
        => _connection.GetPagedAsync<NetworkOverview>($"v1/sites/{siteId}/networks", offset, limit, filter, cancellationToken);

    public Task<NetworkDetails> GetAsync(Guid siteId, Guid networkId, CancellationToken cancellationToken = default)
        => _connection.GetAsync<NetworkDetails>($"v1/sites/{siteId}/networks/{networkId}", cancellationToken: cancellationToken);

    public Task<NetworkDetails> CreateAsync(Guid siteId, JsonObject network, CancellationToken cancellationToken = default)
        => _connection.PostAsync<NetworkDetails>($"v1/sites/{siteId}/networks", network, cancellationToken);

    public Task<NetworkDetails> UpdateAsync(Guid siteId, Guid networkId, JsonObject network, CancellationToken cancellationToken = default)
        => _connection.PutAsync<NetworkDetails>($"v1/sites/{siteId}/networks/{networkId}", network, cancellationToken);

    public Task DeleteAsync(Guid siteId, Guid networkId, CancellationToken cancellationToken = default)
        => _connection.DeleteAsync($"v1/sites/{siteId}/networks/{networkId}", cancellationToken);

    /// <summary>Other resources (clients, devices, routes, ...) that reference this network, by type.</summary>
    public Task<NetworkReferences> GetReferencesAsync(Guid siteId, Guid networkId, CancellationToken cancellationToken = default)
        => _connection.GetAsync<NetworkReferences>($"v1/sites/{siteId}/networks/{networkId}/references", cancellationToken: cancellationToken);
}
