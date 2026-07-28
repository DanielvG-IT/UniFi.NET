using System.Text.Json;
using System.Text.Json.Nodes;
using UniFi.Network.Client.Http;
using UniFi.Network.Client.Models;

namespace UniFi.Network.Client.Resources;

/// <summary>
/// ACL rules (IPV4 or MAC). Raw JSON in/out — see <see cref="FirewallPoliciesResource"/> remarks
/// for why. Build create/update bodies per the OpenAPI "ACL rule" schema (discriminated on
/// <c>type</c>: IPV4 uses <c>IntegrationIpAclRuleCreateUpdateDto</c>, MAC uses
/// <c>IntegrationMacAclRuleCreateUpdateDto</c>).
/// </summary>
public sealed class AclRulesResource
{
    private readonly ApiConnection _connection;

    internal AclRulesResource(ApiConnection connection) => _connection = connection;

    public Task<PagedResult<JsonElement>> ListAsync(
        Guid siteId,
        int? offset = null,
        int? limit = null,
        string? filter = null,
        CancellationToken cancellationToken = default)
        => _connection.GetPagedAsync<JsonElement>($"v1/sites/{siteId}/acl-rules", offset, limit, filter, cancellationToken);

    public Task<JsonElement> GetAsync(Guid siteId, Guid aclRuleId, CancellationToken cancellationToken = default)
        => _connection.GetAsync<JsonElement>($"v1/sites/{siteId}/acl-rules/{aclRuleId}", cancellationToken: cancellationToken);

    public Task<JsonElement> CreateAsync(Guid siteId, JsonObject rule, CancellationToken cancellationToken = default)
        => _connection.PostAsync<JsonElement>($"v1/sites/{siteId}/acl-rules", rule, cancellationToken);

    public Task<JsonElement> UpdateAsync(Guid siteId, Guid aclRuleId, JsonObject rule, CancellationToken cancellationToken = default)
        => _connection.PutAsync<JsonElement>($"v1/sites/{siteId}/acl-rules/{aclRuleId}", rule, cancellationToken);

    public Task DeleteAsync(Guid siteId, Guid aclRuleId, CancellationToken cancellationToken = default)
        => _connection.DeleteAsync($"v1/sites/{siteId}/acl-rules/{aclRuleId}", cancellationToken);

    public Task<AclRuleOrdering> GetOrderingAsync(Guid siteId, CancellationToken cancellationToken = default)
        => _connection.GetAsync<AclRuleOrdering>($"v1/sites/{siteId}/acl-rules/ordering", cancellationToken: cancellationToken);

    public Task<AclRuleOrdering> SetOrderingAsync(Guid siteId, AclRuleOrdering ordering, CancellationToken cancellationToken = default)
        => _connection.PutAsync<AclRuleOrdering>($"v1/sites/{siteId}/acl-rules/ordering", ordering, cancellationToken);
}
