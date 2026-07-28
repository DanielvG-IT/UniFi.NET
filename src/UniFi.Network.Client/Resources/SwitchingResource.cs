using System.Text.Json;
using UniFi.Network.Client.Http;
using UniFi.Network.Client.Models;

namespace UniFi.Network.Client.Resources;

/// <summary>
/// LAGs, MC-LAG domains, and switch stacks. Read-only in the current API version. Returned as
/// raw <see cref="JsonElement"/> since each is its own discriminated union (LOCAL / MULTI_CHASSIS
/// / SWITCH_STACK for LAGs) — check the <c>type</c> field and consult the OpenAPI spec.
/// </summary>
public sealed class SwitchingResource
{
    private readonly ApiConnection _connection;

    internal SwitchingResource(ApiConnection connection) => _connection = connection;

    public Task<PagedResult<JsonElement>> ListLagsAsync(
        Guid siteId, int? offset = null, int? limit = null, string? filter = null, CancellationToken cancellationToken = default)
        => _connection.GetPagedAsync<JsonElement>($"v1/sites/{siteId}/switching/lags", offset, limit, filter, cancellationToken);

    public Task<JsonElement> GetLagAsync(Guid siteId, Guid lagId, CancellationToken cancellationToken = default)
        => _connection.GetAsync<JsonElement>($"v1/sites/{siteId}/switching/lags/{lagId}", cancellationToken: cancellationToken);

    public Task<PagedResult<JsonElement>> ListMcLagDomainsAsync(
        Guid siteId, int? offset = null, int? limit = null, string? filter = null, CancellationToken cancellationToken = default)
        => _connection.GetPagedAsync<JsonElement>($"v1/sites/{siteId}/switching/mc-lag-domains", offset, limit, filter, cancellationToken);

    public Task<JsonElement> GetMcLagDomainAsync(Guid siteId, Guid mcLagDomainId, CancellationToken cancellationToken = default)
        => _connection.GetAsync<JsonElement>($"v1/sites/{siteId}/switching/mc-lag-domains/{mcLagDomainId}", cancellationToken: cancellationToken);

    public Task<PagedResult<JsonElement>> ListSwitchStacksAsync(
        Guid siteId, int? offset = null, int? limit = null, string? filter = null, CancellationToken cancellationToken = default)
        => _connection.GetPagedAsync<JsonElement>($"v1/sites/{siteId}/switching/switch-stacks", offset, limit, filter, cancellationToken);

    public Task<JsonElement> GetSwitchStackAsync(Guid siteId, Guid switchStackId, CancellationToken cancellationToken = default)
        => _connection.GetAsync<JsonElement>($"v1/sites/{siteId}/switching/switch-stacks/{switchStackId}", cancellationToken: cancellationToken);
}
