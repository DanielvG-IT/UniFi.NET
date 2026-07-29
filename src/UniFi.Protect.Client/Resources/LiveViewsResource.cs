using System.Text.Json.Nodes;
using UniFi.Protect.Client.Http;
using UniFi.Protect.Client.Models;

namespace UniFi.Protect.Client.Resources;

/// <summary>Live views: saved camera grid layouts.</summary>
public sealed class LiveViewsResource
{
    private readonly ApiConnection _connection;
    internal LiveViewsResource(ApiConnection connection) => _connection = connection;

    /// <summary>List all live views.</summary>
    public Task<IReadOnlyList<LiveView>> ListAsync(CancellationToken cancellationToken = default)
        => _connection.GetAsync<IReadOnlyList<LiveView>>("v1/liveviews", cancellationToken: cancellationToken);

    /// <summary>Get a single live view by id.</summary>
    public Task<LiveView> GetAsync(string liveViewId, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(liveViewId);
        return _connection.GetAsync<LiveView>($"v1/liveviews/{Uri.EscapeDataString(liveViewId)}", cancellationToken: cancellationToken);
    }

    /// <summary>Create a live view. The body follows the live view schema (name, layout, slots, ...).</summary>
    public Task<LiveView> CreateAsync(JsonObject liveView, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(liveView);
        return _connection.PostAsync<LiveView>("v1/liveviews", liveView, cancellationToken);
    }

    /// <summary>Patch a live view's configuration with a partial <see cref="JsonObject"/>.</summary>
    public Task<LiveView> UpdateAsync(string liveViewId, JsonObject settings, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(liveViewId);
        ArgumentNullException.ThrowIfNull(settings);
        return _connection.PatchAsync<LiveView>($"v1/liveviews/{Uri.EscapeDataString(liveViewId)}", settings, cancellationToken);
    }
}
