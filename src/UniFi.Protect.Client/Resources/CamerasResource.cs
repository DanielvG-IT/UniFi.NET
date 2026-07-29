using System.Text.Json.Nodes;
using UniFi.Protect.Client.Http;
using UniFi.Protect.Client.Models;

namespace UniFi.Protect.Client.Resources;

/// <summary>Cameras: listing, settings, RTSPS streams, snapshots, talkback, and PTZ control.</summary>
public sealed class CamerasResource
{
    private readonly ApiConnection _connection;

    internal CamerasResource(ApiConnection connection) => _connection = connection;

    /// <summary>List all cameras.</summary>
    public Task<IReadOnlyList<Camera>> ListAsync(CancellationToken cancellationToken = default)
        => _connection.GetAsync<IReadOnlyList<Camera>>("v1/cameras", cancellationToken: cancellationToken);

    /// <summary>Get a single camera by id.</summary>
    public Task<Camera> GetAsync(string cameraId, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(cameraId);
        return _connection.GetAsync<Camera>($"v1/cameras/{Uri.EscapeDataString(cameraId)}", cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Patch camera settings. Provide only the fields you want to change, e.g.
    /// <c>new JsonObject { ["name"] = "Front Door", ["videoMode"] = "highFps" }</c>.
    /// </summary>
    public Task<Camera> UpdateAsync(string cameraId, JsonObject settings, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(cameraId);
        ArgumentNullException.ThrowIfNull(settings);
        return _connection.PatchAsync<Camera>($"v1/cameras/{Uri.EscapeDataString(cameraId)}", settings, cancellationToken);
    }

    /// <summary>Get the existing RTSPS stream URLs for a camera.</summary>
    public Task<RtspsStreams> GetRtspsStreamsAsync(string cameraId, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(cameraId);
        return _connection.GetAsync<RtspsStreams>($"v1/cameras/{Uri.EscapeDataString(cameraId)}/rtsps-stream", cancellationToken: cancellationToken);
    }

    /// <summary>Create RTSPS streams for a camera at the given quality levels.</summary>
    public Task<RtspsStreams> CreateRtspsStreamsAsync(string cameraId, CreateRtspsStreamsRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(cameraId);
        ArgumentNullException.ThrowIfNull(request);
        return _connection.PostAsync<RtspsStreams>($"v1/cameras/{Uri.EscapeDataString(cameraId)}/rtsps-stream", request, cancellationToken);
    }

    /// <summary>Delete RTSPS streams for a camera at the given quality levels.</summary>
    public Task DeleteRtspsStreamsAsync(string cameraId, IEnumerable<ChannelQuality> qualities, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(cameraId);
        ArgumentNullException.ThrowIfNull(qualities);
        var value = string.Join(",", qualities.Select(ToApiString));
        var query = new Dictionary<string, string?> { ["qualities"] = value };
        return _connection.DeleteAsync($"v1/cameras/{Uri.EscapeDataString(cameraId)}/rtsps-stream", query, cancellationToken);
    }

    /// <summary>Get a JPEG snapshot from a camera.</summary>
    /// <param name="cameraId">Camera id.</param>
    /// <param name="channel">Channel to capture, "main" or "package". Defaults to main.</param>
    /// <param name="highQuality">Force 1080P or higher resolution.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    public Task<byte[]> GetSnapshotAsync(string cameraId, string? channel = null, bool? highQuality = null, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(cameraId);
        var query = new Dictionary<string, string?>();
        if (channel is not null) query["channel"] = channel;
        if (highQuality is not null) query["highQuality"] = highQuality.Value ? "true" : "false";
        return _connection.GetBytesAsync($"v1/cameras/{Uri.EscapeDataString(cameraId)}/snapshot", query, cancellationToken);
    }

    /// <summary>Create a two-way audio (talkback) session to a camera.</summary>
    public Task<TalkbackSession> CreateTalkbackSessionAsync(string cameraId, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(cameraId);
        return _connection.PostAsync<TalkbackSession>($"v1/cameras/{Uri.EscapeDataString(cameraId)}/talkback-session", body: null, cancellationToken);
    }

    /// <summary>Permanently disable a camera's microphone. This cannot be undone via the API.</summary>
    public Task<Camera> DisableMicrophonePermanentlyAsync(string cameraId, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(cameraId);
        return _connection.PostAsync<Camera>($"v1/cameras/{Uri.EscapeDataString(cameraId)}/disable-mic-permanently", body: null, cancellationToken);
    }

    /// <summary>Move a PTZ camera to a preset slot (-1 is the home preset).</summary>
    public Task PtzGoToAsync(string cameraId, int slot, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(cameraId);
        return _connection.PostAsync($"v1/cameras/{Uri.EscapeDataString(cameraId)}/ptz/goto/{slot}", body: null, cancellationToken);
    }

    /// <summary>Start a PTZ patrol at the given slot (0-4).</summary>
    public Task PtzStartPatrolAsync(string cameraId, int slot, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(cameraId);
        return _connection.PostAsync($"v1/cameras/{Uri.EscapeDataString(cameraId)}/ptz/patrol/start/{slot}", body: null, cancellationToken);
    }

    /// <summary>Stop the active PTZ patrol.</summary>
    public Task PtzStopPatrolAsync(string cameraId, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(cameraId);
        return _connection.PostAsync($"v1/cameras/{Uri.EscapeDataString(cameraId)}/ptz/patrol/stop", body: null, cancellationToken);
    }

    internal static string ToApiString(ChannelQuality quality) => quality switch
    {
        ChannelQuality.High => "high",
        ChannelQuality.Medium => "medium",
        ChannelQuality.Low => "low",
        ChannelQuality.Package => "package",
        _ => quality.ToString().ToLowerInvariant(),
    };
}
