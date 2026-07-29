using System.Text.Json.Nodes;
using UniFi.Protect.Client.Http;
using UniFi.Protect.Client.Models;

namespace UniFi.Protect.Client.Resources;

/// <summary>Speakers: listing, settings, and test sound.</summary>
public sealed class SpeakersResource
{
    private readonly ApiConnection _connection;
    internal SpeakersResource(ApiConnection connection) => _connection = connection;

    /// <summary>List all speakers.</summary>
    public Task<IReadOnlyList<Speaker>> ListAsync(CancellationToken cancellationToken = default)
        => _connection.GetAsync<IReadOnlyList<Speaker>>("v1/speakers", cancellationToken: cancellationToken);

    /// <summary>Get a single speaker by id.</summary>
    public Task<Speaker> GetAsync(string speakerId, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(speakerId);
        return _connection.GetAsync<Speaker>($"v1/speakers/{Uri.EscapeDataString(speakerId)}", cancellationToken: cancellationToken);
    }

    /// <summary>Patch speaker settings with a partial <see cref="JsonObject"/>.</summary>
    public Task<Speaker> UpdateAsync(string speakerId, JsonObject settings, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(speakerId);
        ArgumentNullException.ThrowIfNull(settings);
        return _connection.PatchAsync<Speaker>($"v1/speakers/{Uri.EscapeDataString(speakerId)}", settings, cancellationToken);
    }

    /// <summary>Play a test sound on the speaker.</summary>
    public Task TestSoundAsync(string speakerId, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(speakerId);
        return _connection.PostAsync($"v1/speakers/{Uri.EscapeDataString(speakerId)}/test-sound", body: null, cancellationToken);
    }
}

/// <summary>Sirens: listing, settings, play/stop/test.</summary>
public sealed class SirensResource
{
    private readonly ApiConnection _connection;
    internal SirensResource(ApiConnection connection) => _connection = connection;

    /// <summary>List all sirens.</summary>
    public Task<IReadOnlyList<Siren>> ListAsync(CancellationToken cancellationToken = default)
        => _connection.GetAsync<IReadOnlyList<Siren>>("v1/sirens", cancellationToken: cancellationToken);

    /// <summary>Get a single siren by id.</summary>
    public Task<Siren> GetAsync(string sirenId, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(sirenId);
        return _connection.GetAsync<Siren>($"v1/sirens/{Uri.EscapeDataString(sirenId)}", cancellationToken: cancellationToken);
    }

    /// <summary>Patch siren settings with a partial <see cref="JsonObject"/>.</summary>
    public Task<Siren> UpdateAsync(string sirenId, JsonObject settings, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(sirenId);
        ArgumentNullException.ThrowIfNull(settings);
        return _connection.PatchAsync<Siren>($"v1/sirens/{Uri.EscapeDataString(sirenId)}", settings, cancellationToken);
    }

    /// <summary>Play the siren. Duration defaults to 5 seconds when the request is omitted.</summary>
    public Task PlayAsync(string sirenId, SirenPlayRequest? request = null, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(sirenId);
        return _connection.PostAsync($"v1/sirens/{Uri.EscapeDataString(sirenId)}/play", request, cancellationToken);
    }

    /// <summary>Stop the siren.</summary>
    public Task StopAsync(string sirenId, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(sirenId);
        return _connection.PostAsync($"v1/sirens/{Uri.EscapeDataString(sirenId)}/stop", body: null, cancellationToken);
    }

    /// <summary>Play a test sound on the siren.</summary>
    public Task TestSoundAsync(string sirenId, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(sirenId);
        return _connection.PostAsync($"v1/sirens/{Uri.EscapeDataString(sirenId)}/test-sound", body: null, cancellationToken);
    }
}

/// <summary>Relays: listing, settings, and output activation.</summary>
public sealed class RelaysResource
{
    private readonly ApiConnection _connection;
    internal RelaysResource(ApiConnection connection) => _connection = connection;

    /// <summary>List all relays.</summary>
    public Task<IReadOnlyList<Relay>> ListAsync(CancellationToken cancellationToken = default)
        => _connection.GetAsync<IReadOnlyList<Relay>>("v1/relays", cancellationToken: cancellationToken);

    /// <summary>Get a single relay by id.</summary>
    public Task<Relay> GetAsync(string relayId, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(relayId);
        return _connection.GetAsync<Relay>($"v1/relays/{Uri.EscapeDataString(relayId)}", cancellationToken: cancellationToken);
    }

    /// <summary>Patch relay settings with a partial <see cref="JsonObject"/>.</summary>
    public Task<Relay> UpdateAsync(string relayId, JsonObject settings, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(relayId);
        ArgumentNullException.ThrowIfNull(settings);
        return _connection.PatchAsync<Relay>($"v1/relays/{Uri.EscapeDataString(relayId)}", settings, cancellationToken);
    }

    /// <summary>
    /// Activate a relay output channel (0 or 1). Omit <paramref name="request"/> to toggle.
    /// </summary>
    public Task ActivateOutputAsync(string relayId, int outputId, RelayActivateRequest? request = null, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(relayId);
        return _connection.PostAsync($"v1/relays/{Uri.EscapeDataString(relayId)}/outputs/{outputId}/activate", request, cancellationToken);
    }
}

/// <summary>Link stations, and the alarm-hub view of the same devices.</summary>
public sealed class LinkStationsResource
{
    private readonly ApiConnection _connection;
    internal LinkStationsResource(ApiConnection connection) => _connection = connection;

    /// <summary>List all link stations.</summary>
    public Task<IReadOnlyList<LinkStation>> ListAsync(CancellationToken cancellationToken = default)
        => _connection.GetAsync<IReadOnlyList<LinkStation>>("v1/link-stations", cancellationToken: cancellationToken);

    /// <summary>Get a single link station by id.</summary>
    public Task<LinkStation> GetAsync(string linkStationId, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(linkStationId);
        return _connection.GetAsync<LinkStation>($"v1/link-stations/{Uri.EscapeDataString(linkStationId)}", cancellationToken: cancellationToken);
    }

    /// <summary>Patch link station settings with a partial <see cref="JsonObject"/>.</summary>
    public Task<LinkStation> UpdateAsync(string linkStationId, JsonObject settings, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(linkStationId);
        ArgumentNullException.ThrowIfNull(settings);
        return _connection.PatchAsync<LinkStation>($"v1/link-stations/{Uri.EscapeDataString(linkStationId)}", settings, cancellationToken);
    }
}

/// <summary>Alarm hubs: listing, settings, and output triggering.</summary>
public sealed class AlarmHubsResource
{
    private readonly ApiConnection _connection;
    internal AlarmHubsResource(ApiConnection connection) => _connection = connection;

    /// <summary>List all alarm hubs.</summary>
    public Task<IReadOnlyList<LinkStation>> ListAsync(CancellationToken cancellationToken = default)
        => _connection.GetAsync<IReadOnlyList<LinkStation>>("v1/alarm-hubs", cancellationToken: cancellationToken);

    /// <summary>Get a single alarm hub by id.</summary>
    public Task<LinkStation> GetAsync(string alarmHubId, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(alarmHubId);
        return _connection.GetAsync<LinkStation>($"v1/alarm-hubs/{Uri.EscapeDataString(alarmHubId)}", cancellationToken: cancellationToken);
    }

    /// <summary>Patch alarm hub settings with a partial <see cref="JsonObject"/>.</summary>
    public Task<LinkStation> UpdateAsync(string alarmHubId, JsonObject settings, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(alarmHubId);
        ArgumentNullException.ThrowIfNull(settings);
        return _connection.PatchAsync<LinkStation>($"v1/alarm-hubs/{Uri.EscapeDataString(alarmHubId)}", settings, cancellationToken);
    }

    /// <summary>Trigger an alarm hub output channel (0 or 1). Omit <paramref name="request"/> to toggle.</summary>
    public Task TriggerOutputAsync(string alarmHubId, int outputId, AlarmHubTriggerRequest? request = null, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(alarmHubId);
        return _connection.PostAsync($"v1/alarm-hubs/{Uri.EscapeDataString(alarmHubId)}/outputs/{outputId}/trigger", request, cancellationToken);
    }
}
