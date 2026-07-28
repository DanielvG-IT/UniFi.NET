using UniFi.Network.Client.Http;
using UniFi.Network.Client.Models;

namespace UniFi.Network.Client.Resources;

public sealed class RadiusProfilesResource
{
    private readonly ApiConnection _connection;

    internal RadiusProfilesResource(ApiConnection connection) => _connection = connection;

    public Task<PagedResult<RadiusProfileOverview>> ListAsync(
        Guid siteId, int? offset = null, int? limit = null, string? filter = null, CancellationToken cancellationToken = default)
        => _connection.GetPagedAsync<RadiusProfileOverview>($"v1/sites/{siteId}/radius/profiles", offset, limit, filter, cancellationToken);
}
