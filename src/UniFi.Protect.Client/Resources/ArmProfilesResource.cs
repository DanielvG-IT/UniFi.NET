using System.Text.Json.Nodes;
using UniFi.Protect.Client.Http;
using UniFi.Protect.Client.Models;

namespace UniFi.Protect.Client.Resources;

/// <summary>Arm profiles and the global armed/disarmed state.</summary>
public sealed class ArmProfilesResource
{
    private readonly ApiConnection _connection;
    internal ArmProfilesResource(ApiConnection connection) => _connection = connection;

    /// <summary>List all arm profiles.</summary>
    public Task<IReadOnlyList<ArmProfile>> ListAsync(CancellationToken cancellationToken = default)
        => _connection.GetAsync<IReadOnlyList<ArmProfile>>("v1/arm-profiles", cancellationToken: cancellationToken);

    /// <summary>
    /// Create an arm profile. The body requires name, automations, schedules, recordEverything,
    /// and activationDelay per the arm profile schema.
    /// </summary>
    public Task<ArmProfile> CreateAsync(JsonObject armProfile, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(armProfile);
        return _connection.PostAsync<ArmProfile>("v1/arm-profiles", armProfile, cancellationToken);
    }

    /// <summary>Update an existing arm profile with a partial <see cref="JsonObject"/>.</summary>
    public Task<ArmProfile> UpdateAsync(string armProfileId, JsonObject settings, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(armProfileId);
        ArgumentNullException.ThrowIfNull(settings);
        return _connection.PatchAsync<ArmProfile>($"v1/arm-profiles/{Uri.EscapeDataString(armProfileId)}", settings, cancellationToken);
    }

    /// <summary>Delete an arm profile.</summary>
    public Task DeleteAsync(string armProfileId, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(armProfileId);
        return _connection.DeleteAsync($"v1/arm-profiles/{Uri.EscapeDataString(armProfileId)}", cancellationToken: cancellationToken);
    }

    /// <summary>Set which arm profile is currently active.</summary>
    public Task SetCurrentAsync(SetArmProfileRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        return _connection.PatchAsync("v1/arm-profiles/settings", request, cancellationToken);
    }

    /// <summary>Enable the arm alarm (arm the system).</summary>
    public Task EnableAsync(CancellationToken cancellationToken = default)
        => _connection.PostAsync("v1/arm-profiles/enable", body: null, cancellationToken);

    /// <summary>Disable the arm alarm (disarm the system).</summary>
    public Task DisableAsync(CancellationToken cancellationToken = default)
        => _connection.PostAsync("v1/arm-profiles/disable", body: null, cancellationToken);
}
