using System.Text.Json;
using System.Text.Json.Nodes;
using UniFi.Network.Client.Http;
using UniFi.Network.Client.Models;

namespace UniFi.Network.Client.Resources;

/// <summary>
/// Firewall zones. Raw JSON in/out — see <see cref="FirewallPoliciesResource"/> remarks for why.
/// Build create/update bodies per the OpenAPI "Create or update firewall zone" schema, e.g.:
/// <code>
/// new JsonObject
/// {
///     ["name"] = "IoT",
///     ["networkIds"] = new JsonArray(networkId.ToString()),
/// }
/// </code>
/// </summary>
public sealed class FirewallZonesResource
{
    private readonly ApiConnection _connection;

    internal FirewallZonesResource(ApiConnection connection) => _connection = connection;

    public Task<PagedResult<JsonElement>> ListAsync(
        Guid siteId,
        int? offset = null,
        int? limit = null,
        string? filter = null,
        CancellationToken cancellationToken = default)
        => _connection.GetPagedAsync<JsonElement>($"v1/sites/{siteId}/firewall/zones", offset, limit, filter, cancellationToken);

    public Task<JsonElement> GetAsync(Guid siteId, Guid firewallZoneId, CancellationToken cancellationToken = default)
        => _connection.GetAsync<JsonElement>($"v1/sites/{siteId}/firewall/zones/{firewallZoneId}", cancellationToken: cancellationToken);

    public Task<JsonElement> CreateAsync(Guid siteId, JsonObject zone, CancellationToken cancellationToken = default)
        => _connection.PostAsync<JsonElement>($"v1/sites/{siteId}/firewall/zones", zone, cancellationToken);

    public Task<JsonElement> UpdateAsync(Guid siteId, Guid firewallZoneId, JsonObject zone, CancellationToken cancellationToken = default)
        => _connection.PutAsync<JsonElement>($"v1/sites/{siteId}/firewall/zones/{firewallZoneId}", zone, cancellationToken);

    public Task DeleteAsync(Guid siteId, Guid firewallZoneId, CancellationToken cancellationToken = default)
        => _connection.DeleteAsync($"v1/sites/{siteId}/firewall/zones/{firewallZoneId}", cancellationToken);
}
