using UniFi.Protect.Client.Http;
using UniFi.Protect.Client.Models;

namespace UniFi.Protect.Client.Resources;

/// <summary>Protect users.</summary>
public sealed class UsersResource
{
    private readonly ApiConnection _connection;
    internal UsersResource(ApiConnection connection) => _connection = connection;

    /// <summary>List all Protect users.</summary>
    public Task<IReadOnlyList<ProtectUser>> ListAsync(CancellationToken cancellationToken = default)
        => _connection.GetAsync<IReadOnlyList<ProtectUser>>("v1/users", cancellationToken: cancellationToken);

    /// <summary>Get a single Protect user by id.</summary>
    public Task<ProtectUser> GetAsync(string userId, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(userId);
        return _connection.GetAsync<ProtectUser>($"v1/users/{Uri.EscapeDataString(userId)}", cancellationToken: cancellationToken);
    }
}

/// <summary>UniFi Identity (ULP) users.</summary>
public sealed class UlpUsersResource
{
    private readonly ApiConnection _connection;
    internal UlpUsersResource(ApiConnection connection) => _connection = connection;

    /// <summary>List all UniFi Identity users.</summary>
    public Task<IReadOnlyList<UlpUser>> ListAsync(CancellationToken cancellationToken = default)
        => _connection.GetAsync<IReadOnlyList<UlpUser>>("v1/ulp-users", cancellationToken: cancellationToken);

    /// <summary>Get a single UniFi Identity user by id.</summary>
    public Task<UlpUser> GetAsync(string ulpUserId, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(ulpUserId);
        return _connection.GetAsync<UlpUser>($"v1/ulp-users/{Uri.EscapeDataString(ulpUserId)}", cancellationToken: cancellationToken);
    }
}
