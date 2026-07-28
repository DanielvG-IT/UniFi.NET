using UniFi.Network.Client.Http;
using UniFi.Network.Client.Models;

namespace UniFi.Network.Client.Resources;

public sealed class SitesResource
{
    private readonly ApiConnection _connection;

    internal SitesResource(ApiConnection connection) => _connection = connection;

    /// <summary>
    /// List local sites managed by this Network application. A site's Id is required for
    /// every other site-scoped call (devices, clients, networks, etc).
    /// </summary>
    /// <param name="offset">Number of results to skip.</param>
    /// <param name="limit">Maximum number of results to return (default 25, max 200).</param>
    /// <param name="filter">
    /// Optional filter expression, e.g. <c>name eq 'Default'</c>. Filterable properties: id, internalReference, name.
    /// </param>
    /// <param name="cancellationToken">Cancellation token.</param>
    public Task<PagedResult<Site>> ListAsync(
        int? offset = null,
        int? limit = null,
        string? filter = null,
        CancellationToken cancellationToken = default)
        => _connection.GetPagedAsync<Site>("v1/sites", offset, limit, filter, cancellationToken);
}
