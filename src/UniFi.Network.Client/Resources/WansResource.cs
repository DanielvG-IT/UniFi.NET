using UniFi.Network.Client.Http;
using UniFi.Network.Client.Models;

namespace UniFi.Network.Client.Resources;

public sealed class WansResource
{
    private readonly ApiConnection _connection;

    internal WansResource(ApiConnection connection) => _connection = connection;

    public Task<PagedResult<WanOverview>> ListAsync(
        Guid siteId, int? offset = null, int? limit = null, string? filter = null, CancellationToken cancellationToken = default)
        => _connection.GetPagedAsync<WanOverview>($"v1/sites/{siteId}/wans", offset, limit, filter, cancellationToken);
}
