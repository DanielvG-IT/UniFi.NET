using System.Globalization;
using UniFi.SiteManager.Client.Http;
using UniFi.SiteManager.Client.Models;

namespace UniFi.SiteManager.Client.Resources;

/// <summary>Devices managed across your hosts, grouped by host.</summary>
public sealed class DevicesResource
{
    private readonly ApiConnection _connection;

    internal DevicesResource(ApiConnection connection) => _connection = connection;

    /// <summary>
    /// List devices across your hosts, grouped by host. Each returned <see cref="HostDevices"/>
    /// holds one host's devices.
    /// </summary>
    /// <param name="hostIds">Restrict the result to these host ids. When null, all hosts are returned.</param>
    /// <param name="time">Only return hosts whose device list changed at or after this time.</param>
    /// <param name="pageSize">Maximum number of results to return per page.</param>
    /// <param name="nextToken">Cursor from a previous page's <see cref="SiteManagerPage{T}.NextToken"/>.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    public Task<SiteManagerPage<HostDevices>> ListAsync(
        IEnumerable<string>? hostIds = null,
        DateTimeOffset? time = null,
        int? pageSize = null,
        string? nextToken = null,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string?>>();
        if (hostIds is not null)
        {
            foreach (var hostId in hostIds)
            {
                query.Add(new("hostIds[]", hostId));
            }
        }
        if (time is not null)
        {
            query.Add(new("time", time.Value.ToUniversalTime().ToString("o", CultureInfo.InvariantCulture)));
        }

        return _connection.GetPagedAsync<HostDevices>("v1/devices", pageSize, nextToken, query, cancellationToken);
    }
}
