using UniFi.Network.Client.Http;
using UniFi.Network.Client.Models;

namespace UniFi.Network.Client.Resources;

/// <summary>Global (non-site-scoped) reference data.</summary>
public sealed class ReferenceDataResource
{
    private readonly ApiConnection _connection;

    internal ReferenceDataResource(ApiConnection connection) => _connection = connection;

    public Task<PagedResult<Country>> ListCountriesAsync(
        int? offset = null, int? limit = null, string? filter = null, CancellationToken cancellationToken = default)
        => _connection.GetPagedAsync<Country>("v1/countries", offset, limit, filter, cancellationToken);

    public Task<PagedResult<DpiApplication>> ListDpiApplicationsAsync(
        int? offset = null, int? limit = null, string? filter = null, CancellationToken cancellationToken = default)
        => _connection.GetPagedAsync<DpiApplication>("v1/dpi/applications", offset, limit, filter, cancellationToken);

    public Task<PagedResult<DpiCategory>> ListDpiCategoriesAsync(
        int? offset = null, int? limit = null, string? filter = null, CancellationToken cancellationToken = default)
        => _connection.GetPagedAsync<DpiCategory>("v1/dpi/categories", offset, limit, filter, cancellationToken);
}
