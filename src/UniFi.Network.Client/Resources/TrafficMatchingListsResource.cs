using System.Text.Json;
using System.Text.Json.Nodes;
using UniFi.Network.Client.Http;
using UniFi.Network.Client.Models;

namespace UniFi.Network.Client.Resources;

/// <summary>
/// Traffic matching lists (reusable IPv4/IPv6 address or port sets referenced by firewall
/// policies). Raw JSON in/out — see <see cref="FirewallPoliciesResource"/> remarks for why.
/// Build create/update bodies per the OpenAPI "Create or update traffic matching list" schema,
/// discriminated on <c>type</c> (IPV4_ADDRESSES / IPV6_ADDRESSES / PORTS).
/// </summary>
public sealed class TrafficMatchingListsResource
{
    private readonly ApiConnection _connection;

    internal TrafficMatchingListsResource(ApiConnection connection) => _connection = connection;

    public Task<PagedResult<JsonElement>> ListAsync(
        Guid siteId,
        int? offset = null,
        int? limit = null,
        string? filter = null,
        CancellationToken cancellationToken = default)
        => _connection.GetPagedAsync<JsonElement>($"v1/sites/{siteId}/traffic-matching-lists", offset, limit, filter, cancellationToken);

    public Task<JsonElement> GetAsync(Guid siteId, Guid trafficMatchingListId, CancellationToken cancellationToken = default)
        => _connection.GetAsync<JsonElement>($"v1/sites/{siteId}/traffic-matching-lists/{trafficMatchingListId}", cancellationToken: cancellationToken);

    public Task<JsonElement> CreateAsync(Guid siteId, JsonObject list, CancellationToken cancellationToken = default)
        => _connection.PostAsync<JsonElement>($"v1/sites/{siteId}/traffic-matching-lists", list, cancellationToken);

    public Task<JsonElement> UpdateAsync(Guid siteId, Guid trafficMatchingListId, JsonObject list, CancellationToken cancellationToken = default)
        => _connection.PutAsync<JsonElement>($"v1/sites/{siteId}/traffic-matching-lists/{trafficMatchingListId}", list, cancellationToken);

    public Task DeleteAsync(Guid siteId, Guid trafficMatchingListId, CancellationToken cancellationToken = default)
        => _connection.DeleteAsync($"v1/sites/{siteId}/traffic-matching-lists/{trafficMatchingListId}", cancellationToken);
}
