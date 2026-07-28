using System.Text.Json;
using System.Text.Json.Nodes;
using UniFi.Network.Client.Http;
using UniFi.Network.Client.Models;

namespace UniFi.Network.Client.Resources;

/// <summary>
/// Firewall policies. This resource's config surface (source/destination filters, protocol
/// scopes, schedules, ...) is a deep discriminated union — list/get return raw
/// <see cref="JsonElement"/> and create/update take a raw <see cref="JsonObject"/> you build per
/// the OpenAPI "Firewall policy" / "Create or update firewall policy" schemas, rather than a
/// narrow typed model that would need constant upkeep as filter types are added.
/// </summary>
public sealed class FirewallPoliciesResource
{
    private readonly ApiConnection _connection;

    internal FirewallPoliciesResource(ApiConnection connection) => _connection = connection;

    public Task<PagedResult<JsonElement>> ListAsync(
        Guid siteId,
        int? offset = null,
        int? limit = null,
        string? filter = null,
        CancellationToken cancellationToken = default)
        => _connection.GetPagedAsync<JsonElement>($"v1/sites/{siteId}/firewall/policies", offset, limit, filter, cancellationToken);

    public Task<JsonElement> GetAsync(Guid siteId, Guid firewallPolicyId, CancellationToken cancellationToken = default)
        => _connection.GetAsync<JsonElement>($"v1/sites/{siteId}/firewall/policies/{firewallPolicyId}", cancellationToken: cancellationToken);

    public Task<JsonElement> CreateAsync(Guid siteId, JsonObject policy, CancellationToken cancellationToken = default)
        => _connection.PostAsync<JsonElement>($"v1/sites/{siteId}/firewall/policies", policy, cancellationToken);

    public Task<JsonElement> UpdateAsync(Guid siteId, Guid firewallPolicyId, JsonObject policy, CancellationToken cancellationToken = default)
        => _connection.PutAsync<JsonElement>($"v1/sites/{siteId}/firewall/policies/{firewallPolicyId}", policy, cancellationToken);

    public Task<JsonElement> PatchAsync(Guid siteId, Guid firewallPolicyId, JsonObject partialPolicy, CancellationToken cancellationToken = default)
        => _connection.PatchAsync<JsonElement>($"v1/sites/{siteId}/firewall/policies/{firewallPolicyId}", partialPolicy, cancellationToken);

    public Task DeleteAsync(Guid siteId, Guid firewallPolicyId, CancellationToken cancellationToken = default)
        => _connection.DeleteAsync($"v1/sites/{siteId}/firewall/policies/{firewallPolicyId}", cancellationToken);

    public Task<FirewallPolicyOrdering> GetOrderingAsync(Guid siteId, CancellationToken cancellationToken = default)
        => _connection.GetAsync<FirewallPolicyOrdering>($"v1/sites/{siteId}/firewall/policies/ordering", cancellationToken: cancellationToken);

    public Task<FirewallPolicyOrdering> SetOrderingAsync(Guid siteId, FirewallPolicyOrdering ordering, CancellationToken cancellationToken = default)
        => _connection.PutAsync<FirewallPolicyOrdering>($"v1/sites/{siteId}/firewall/policies/ordering", ordering, cancellationToken);
}
