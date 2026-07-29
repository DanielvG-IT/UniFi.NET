using UniFi.SiteManager.Client.Http;
using UniFi.SiteManager.Client.Models;

namespace UniFi.SiteManager.Client.Resources;

/// <summary>Hosts (consoles and network servers) registered to your cloud account.</summary>
public sealed class HostsResource
{
    private readonly ApiConnection _connection;

    internal HostsResource(ApiConnection connection) => _connection = connection;

    /// <summary>List a page of hosts registered to your account.</summary>
    /// <param name="pageSize">Maximum number of results to return per page.</param>
    /// <param name="nextToken">Cursor from a previous page's <see cref="SiteManagerPage{T}.NextToken"/>.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    public Task<SiteManagerPage<Host>> ListAsync(
        int? pageSize = null,
        string? nextToken = null,
        CancellationToken cancellationToken = default)
        => _connection.GetPagedAsync<Host>("v1/hosts", pageSize, nextToken, extraQuery: null, cancellationToken);

    /// <summary>Get a single host by id.</summary>
    public Task<Host?> GetAsync(string hostId, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(hostId);
        return _connection.GetAsync<Host>($"v1/hosts/{Uri.EscapeDataString(hostId)}", cancellationToken: cancellationToken);
    }
}
