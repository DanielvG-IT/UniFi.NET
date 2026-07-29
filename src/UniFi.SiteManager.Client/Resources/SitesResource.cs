using UniFi.SiteManager.Client.Http;
using UniFi.SiteManager.Client.Models;

namespace UniFi.SiteManager.Client.Resources;

/// <summary>Sites managed across all of your hosts.</summary>
public sealed class SitesResource
{
    private readonly ApiConnection _connection;

    internal SitesResource(ApiConnection connection) => _connection = connection;

    /// <summary>List a page of sites across all hosts on your account.</summary>
    /// <param name="pageSize">Maximum number of results to return per page.</param>
    /// <param name="nextToken">Cursor from a previous page's <see cref="SiteManagerPage{T}.NextToken"/>.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    public Task<SiteManagerPage<Site>> ListAsync(
        int? pageSize = null,
        string? nextToken = null,
        CancellationToken cancellationToken = default)
        => _connection.GetPagedAsync<Site>("v1/sites", pageSize, nextToken, extraQuery: null, cancellationToken);
}
