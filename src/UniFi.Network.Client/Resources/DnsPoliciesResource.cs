using System.Text.Json;
using System.Text.Json.Nodes;
using UniFi.Network.Client.Http;
using UniFi.Network.Client.Models;

namespace UniFi.Network.Client.Resources;

/// <summary>
/// DNS policies (A/AAAA/CNAME/MX/SRV/TXT records and forward-domain policies). Raw JSON in/out —
/// see <see cref="FirewallPoliciesResource"/> remarks for why. Build create/update bodies per the
/// OpenAPI "Create or update DNS policy" schema, discriminated on <c>type</c>.
/// </summary>
public sealed class DnsPoliciesResource
{
    private readonly ApiConnection _connection;

    internal DnsPoliciesResource(ApiConnection connection) => _connection = connection;

    public Task<PagedResult<JsonElement>> ListAsync(
        Guid siteId,
        int? offset = null,
        int? limit = null,
        string? filter = null,
        CancellationToken cancellationToken = default)
        => _connection.GetPagedAsync<JsonElement>($"v1/sites/{siteId}/dns/policies", offset, limit, filter, cancellationToken);

    public Task<JsonElement> GetAsync(Guid siteId, Guid dnsPolicyId, CancellationToken cancellationToken = default)
        => _connection.GetAsync<JsonElement>($"v1/sites/{siteId}/dns/policies/{dnsPolicyId}", cancellationToken: cancellationToken);

    public Task<JsonElement> CreateAsync(Guid siteId, JsonObject policy, CancellationToken cancellationToken = default)
        => _connection.PostAsync<JsonElement>($"v1/sites/{siteId}/dns/policies", policy, cancellationToken);

    public Task<JsonElement> UpdateAsync(Guid siteId, Guid dnsPolicyId, JsonObject policy, CancellationToken cancellationToken = default)
        => _connection.PutAsync<JsonElement>($"v1/sites/{siteId}/dns/policies/{dnsPolicyId}", policy, cancellationToken);

    public Task DeleteAsync(Guid siteId, Guid dnsPolicyId, CancellationToken cancellationToken = default)
        => _connection.DeleteAsync($"v1/sites/{siteId}/dns/policies/{dnsPolicyId}", cancellationToken);
}
