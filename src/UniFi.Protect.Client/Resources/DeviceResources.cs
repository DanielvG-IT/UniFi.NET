using System.Text.Json.Nodes;
using UniFi.Protect.Client.Http;
using UniFi.Protect.Client.Models;

namespace UniFi.Protect.Client.Resources;

/// <summary>Lights: listing and settings.</summary>
public sealed class LightsResource
{
    private readonly ApiConnection _connection;
    internal LightsResource(ApiConnection connection) => _connection = connection;

    /// <summary>List all lights.</summary>
    public Task<IReadOnlyList<Light>> ListAsync(CancellationToken cancellationToken = default)
        => _connection.GetAsync<IReadOnlyList<Light>>("v1/lights", cancellationToken: cancellationToken);

    /// <summary>Get a single light by id.</summary>
    public Task<Light> GetAsync(string lightId, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(lightId);
        return _connection.GetAsync<Light>($"v1/lights/{Uri.EscapeDataString(lightId)}", cancellationToken: cancellationToken);
    }

    /// <summary>Patch light settings with a partial <see cref="JsonObject"/>.</summary>
    public Task<Light> UpdateAsync(string lightId, JsonObject settings, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(lightId);
        ArgumentNullException.ThrowIfNull(settings);
        return _connection.PatchAsync<Light>($"v1/lights/{Uri.EscapeDataString(lightId)}", settings, cancellationToken);
    }
}

/// <summary>Sensors: listing and settings.</summary>
public sealed class SensorsResource
{
    private readonly ApiConnection _connection;
    internal SensorsResource(ApiConnection connection) => _connection = connection;

    /// <summary>List all sensors.</summary>
    public Task<IReadOnlyList<Sensor>> ListAsync(CancellationToken cancellationToken = default)
        => _connection.GetAsync<IReadOnlyList<Sensor>>("v1/sensors", cancellationToken: cancellationToken);

    /// <summary>Get a single sensor by id.</summary>
    public Task<Sensor> GetAsync(string sensorId, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(sensorId);
        return _connection.GetAsync<Sensor>($"v1/sensors/{Uri.EscapeDataString(sensorId)}", cancellationToken: cancellationToken);
    }

    /// <summary>Patch sensor settings with a partial <see cref="JsonObject"/>.</summary>
    public Task<Sensor> UpdateAsync(string sensorId, JsonObject settings, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(sensorId);
        ArgumentNullException.ThrowIfNull(settings);
        return _connection.PatchAsync<Sensor>($"v1/sensors/{Uri.EscapeDataString(sensorId)}", settings, cancellationToken);
    }
}

/// <summary>Chimes: listing and settings.</summary>
public sealed class ChimesResource
{
    private readonly ApiConnection _connection;
    internal ChimesResource(ApiConnection connection) => _connection = connection;

    /// <summary>List all chimes.</summary>
    public Task<IReadOnlyList<Chime>> ListAsync(CancellationToken cancellationToken = default)
        => _connection.GetAsync<IReadOnlyList<Chime>>("v1/chimes", cancellationToken: cancellationToken);

    /// <summary>Get a single chime by id.</summary>
    public Task<Chime> GetAsync(string chimeId, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(chimeId);
        return _connection.GetAsync<Chime>($"v1/chimes/{Uri.EscapeDataString(chimeId)}", cancellationToken: cancellationToken);
    }

    /// <summary>Patch chime settings with a partial <see cref="JsonObject"/>.</summary>
    public Task<Chime> UpdateAsync(string chimeId, JsonObject settings, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(chimeId);
        ArgumentNullException.ThrowIfNull(settings);
        return _connection.PatchAsync<Chime>($"v1/chimes/{Uri.EscapeDataString(chimeId)}", settings, cancellationToken);
    }
}

/// <summary>Viewers (Protect Viewports): listing and settings.</summary>
public sealed class ViewersResource
{
    private readonly ApiConnection _connection;
    internal ViewersResource(ApiConnection connection) => _connection = connection;

    /// <summary>List all viewers.</summary>
    public Task<IReadOnlyList<Viewer>> ListAsync(CancellationToken cancellationToken = default)
        => _connection.GetAsync<IReadOnlyList<Viewer>>("v1/viewers", cancellationToken: cancellationToken);

    /// <summary>Get a single viewer by id.</summary>
    public Task<Viewer> GetAsync(string viewerId, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(viewerId);
        return _connection.GetAsync<Viewer>($"v1/viewers/{Uri.EscapeDataString(viewerId)}", cancellationToken: cancellationToken);
    }

    /// <summary>Patch viewer settings, e.g. <c>new JsonObject { ["liveview"] = liveviewId }</c>.</summary>
    public Task<Viewer> UpdateAsync(string viewerId, JsonObject settings, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(viewerId);
        ArgumentNullException.ThrowIfNull(settings);
        return _connection.PatchAsync<Viewer>($"v1/viewers/{Uri.EscapeDataString(viewerId)}", settings, cancellationToken);
    }
}

/// <summary>Bridges: listing and settings.</summary>
public sealed class BridgesResource
{
    private readonly ApiConnection _connection;
    internal BridgesResource(ApiConnection connection) => _connection = connection;

    /// <summary>List all bridges.</summary>
    public Task<IReadOnlyList<Bridge>> ListAsync(CancellationToken cancellationToken = default)
        => _connection.GetAsync<IReadOnlyList<Bridge>>("v1/bridges", cancellationToken: cancellationToken);

    /// <summary>Get a single bridge by id.</summary>
    public Task<Bridge> GetAsync(string bridgeId, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(bridgeId);
        return _connection.GetAsync<Bridge>($"v1/bridges/{Uri.EscapeDataString(bridgeId)}", cancellationToken: cancellationToken);
    }

    /// <summary>Patch bridge settings with a partial <see cref="JsonObject"/>.</summary>
    public Task<Bridge> UpdateAsync(string bridgeId, JsonObject settings, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(bridgeId);
        ArgumentNullException.ThrowIfNull(settings);
        return _connection.PatchAsync<Bridge>($"v1/bridges/{Uri.EscapeDataString(bridgeId)}", settings, cancellationToken);
    }
}

/// <summary>Key fobs: listing and settings.</summary>
public sealed class FobsResource
{
    private readonly ApiConnection _connection;
    internal FobsResource(ApiConnection connection) => _connection = connection;

    /// <summary>List all fobs.</summary>
    public Task<IReadOnlyList<Fob>> ListAsync(CancellationToken cancellationToken = default)
        => _connection.GetAsync<IReadOnlyList<Fob>>("v1/fobs", cancellationToken: cancellationToken);

    /// <summary>Get a single fob by id.</summary>
    public Task<Fob> GetAsync(string fobId, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(fobId);
        return _connection.GetAsync<Fob>($"v1/fobs/{Uri.EscapeDataString(fobId)}", cancellationToken: cancellationToken);
    }

    /// <summary>Patch fob settings with a partial <see cref="JsonObject"/>.</summary>
    public Task<Fob> UpdateAsync(string fobId, JsonObject settings, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(fobId);
        ArgumentNullException.ThrowIfNull(settings);
        return _connection.PatchAsync<Fob>($"v1/fobs/{Uri.EscapeDataString(fobId)}", settings, cancellationToken);
    }
}

/// <summary>The NVR/console itself.</summary>
public sealed class NvrsResource
{
    private readonly ApiConnection _connection;
    internal NvrsResource(ApiConnection connection) => _connection = connection;

    /// <summary>Get details about the NVR.</summary>
    public Task<Nvr> GetAsync(CancellationToken cancellationToken = default)
        => _connection.GetAsync<Nvr>("v1/nvrs", cancellationToken: cancellationToken);
}
