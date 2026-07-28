using UniFi.Network.Client.Http;
using UniFi.Network.Client.Models;

namespace UniFi.Network.Client.Resources;

public sealed class DeviceTagsResource
{
    private readonly ApiConnection _connection;

    internal DeviceTagsResource(ApiConnection connection) => _connection = connection;

    public Task<PagedResult<DeviceTag>> ListAsync(
        Guid siteId, int? offset = null, int? limit = null, string? filter = null, CancellationToken cancellationToken = default)
        => _connection.GetPagedAsync<DeviceTag>($"v1/sites/{siteId}/device-tags", offset, limit, filter, cancellationToken);
}
