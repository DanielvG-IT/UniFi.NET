using System.Text.Json;
using UniFi.Network.Client.Http;
using UniFi.Network.Client.Models;

namespace UniFi.Network.Client.Resources;

/// <summary>
/// VPN servers (L2TP/OpenVPN/PPTP/WireGuard/UID) and site-to-site tunnels. Read-only in the
/// current API version. Returned as raw <see cref="JsonElement"/> since each server/tunnel type
/// is its own discriminated union — check the <c>type</c> field and consult the OpenAPI spec.
/// </summary>
public sealed class VpnResource
{
    private readonly ApiConnection _connection;

    internal VpnResource(ApiConnection connection) => _connection = connection;

    public Task<PagedResult<JsonElement>> ListServersAsync(
        Guid siteId, int? offset = null, int? limit = null, string? filter = null, CancellationToken cancellationToken = default)
        => _connection.GetPagedAsync<JsonElement>($"v1/sites/{siteId}/vpn/servers", offset, limit, filter, cancellationToken);

    public Task<PagedResult<JsonElement>> ListSiteToSiteTunnelsAsync(
        Guid siteId, int? offset = null, int? limit = null, string? filter = null, CancellationToken cancellationToken = default)
        => _connection.GetPagedAsync<JsonElement>($"v1/sites/{siteId}/vpn/site-to-site-tunnels", offset, limit, filter, cancellationToken);
}
