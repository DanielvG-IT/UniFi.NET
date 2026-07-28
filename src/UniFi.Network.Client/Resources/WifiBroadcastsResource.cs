using System.Text.Json.Nodes;
using UniFi.Network.Client.Http;
using UniFi.Network.Client.Models;

namespace UniFi.Network.Client.Resources;

/// <summary>
/// WiFi broadcasts (SSIDs). Create/update bodies are raw <see cref="JsonObject"/> because
/// security configuration and network binding are deep discriminated unions — build the object
/// per the OpenAPI "Wifi broadcast create or update" schema, e.g.:
/// <code>
/// new JsonObject
/// {
///     ["type"] = "STANDARD",
///     ["name"] = "Guest WiFi",
///     ["enabled"] = true,
///     ["network"] = new JsonObject { ["type"] = "NATIVE" },
///     ["securityConfiguration"] = new JsonObject
///     {
///         ["type"] = "WPA2_PERSONAL",
///         ["passphrase"] = "changeme123",
///     },
///     ["broadcastingFrequenciesGHz"] = new JsonArray(2.4, 5),
/// }
/// </code>
/// </summary>
public sealed class WifiBroadcastsResource
{
    private readonly ApiConnection _connection;

    internal WifiBroadcastsResource(ApiConnection connection) => _connection = connection;

    public Task<PagedResult<WifiBroadcastOverview>> ListAsync(
        Guid siteId,
        int? offset = null,
        int? limit = null,
        string? filter = null,
        CancellationToken cancellationToken = default)
        => _connection.GetPagedAsync<WifiBroadcastOverview>($"v1/sites/{siteId}/wifi/broadcasts", offset, limit, filter, cancellationToken);

    public Task<WifiBroadcastDetails> GetAsync(Guid siteId, Guid wifiBroadcastId, CancellationToken cancellationToken = default)
        => _connection.GetAsync<WifiBroadcastDetails>($"v1/sites/{siteId}/wifi/broadcasts/{wifiBroadcastId}", cancellationToken: cancellationToken);

    public Task<WifiBroadcastDetails> CreateAsync(Guid siteId, JsonObject broadcast, CancellationToken cancellationToken = default)
        => _connection.PostAsync<WifiBroadcastDetails>($"v1/sites/{siteId}/wifi/broadcasts", broadcast, cancellationToken);

    public Task<WifiBroadcastDetails> UpdateAsync(Guid siteId, Guid wifiBroadcastId, JsonObject broadcast, CancellationToken cancellationToken = default)
        => _connection.PutAsync<WifiBroadcastDetails>($"v1/sites/{siteId}/wifi/broadcasts/{wifiBroadcastId}", broadcast, cancellationToken);

    public Task DeleteAsync(Guid siteId, Guid wifiBroadcastId, CancellationToken cancellationToken = default)
        => _connection.DeleteAsync($"v1/sites/{siteId}/wifi/broadcasts/{wifiBroadcastId}", cancellationToken);
}
