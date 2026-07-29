using UniFi.Protect.Client.Http;
using UniFi.Protect.Client.Models;

namespace UniFi.Protect.Client.Resources;

/// <summary>Information about the Protect application.</summary>
public sealed class MetaResource
{
    private readonly ApiConnection _connection;

    internal MetaResource(ApiConnection connection) => _connection = connection;

    /// <summary>Get application information, including the Protect application version.</summary>
    public Task<ProtectApplicationInfo> GetInfoAsync(CancellationToken cancellationToken = default)
        => _connection.GetAsync<ProtectApplicationInfo>("v1/meta/info", cancellationToken: cancellationToken);
}
