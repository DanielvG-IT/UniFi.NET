using UniFi.Network.Client.Http;
using UniFi.Network.Client.Models;

namespace UniFi.Network.Client.Resources;

public sealed class ClientsResource
{
    private readonly ApiConnection _connection;

    internal ClientsResource(ApiConnection connection) => _connection = connection;

    /// <summary>List clients currently connected to the site.</summary>
    public Task<PagedResult<ClientOverview>> ListAsync(
        Guid siteId,
        int? offset = null,
        int? limit = null,
        string? filter = null,
        CancellationToken cancellationToken = default)
        => _connection.GetPagedAsync<ClientOverview>($"v1/sites/{siteId}/clients", offset, limit, filter, cancellationToken);

    public Task<ClientDetails> GetAsync(Guid siteId, Guid clientId, CancellationToken cancellationToken = default)
        => _connection.GetAsync<ClientDetails>($"v1/sites/{siteId}/clients/{clientId}", cancellationToken: cancellationToken);

    /// <summary>
    /// Authorize network access for a guest client, cancelling any existing active authorization
    /// and resetting traffic counters.
    /// </summary>
    public Task<GuestAccessAuthorizationResponse> AuthorizeGuestAccessAsync(
        Guid siteId,
        Guid clientId,
        GuestAccessAuthorizationRequest? request = null,
        CancellationToken cancellationToken = default)
        => _connection.PostAsync<GuestAccessAuthorizationResponse>(
            $"v1/sites/{siteId}/clients/{clientId}/actions", request ?? new GuestAccessAuthorizationRequest(), cancellationToken);

    /// <summary>Revoke network access and disconnect a guest client.</summary>
    public Task<GuestAccessUnauthorizationResponse> UnauthorizeGuestAccessAsync(
        Guid siteId,
        Guid clientId,
        CancellationToken cancellationToken = default)
        => _connection.PostAsync<GuestAccessUnauthorizationResponse>(
            $"v1/sites/{siteId}/clients/{clientId}/actions", new GuestAccessUnauthorizationRequest(), cancellationToken);
}
