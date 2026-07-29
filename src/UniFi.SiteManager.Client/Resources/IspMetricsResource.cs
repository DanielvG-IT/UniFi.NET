using System.Globalization;
using UniFi.SiteManager.Client.Http;
using UniFi.SiteManager.Client.Models;

namespace UniFi.SiteManager.Client.Resources;

/// <summary>ISP performance metrics (latency, throughput, uptime) for your sites' WANs.</summary>
public sealed class IspMetricsResource
{
    private readonly ApiConnection _connection;

    internal IspMetricsResource(ApiConnection connection) => _connection = connection;

    /// <summary>
    /// Get ISP metrics for all of your sites over a time window.
    /// </summary>
    /// <param name="type">Metric aggregation type, e.g. "5m" or "1h".</param>
    /// <param name="beginTimestamp">Start of the window (inclusive).</param>
    /// <param name="endTimestamp">End of the window (exclusive).</param>
    /// <param name="duration">
    /// A relative window (e.g. "24h", "7d") used instead of explicit timestamps.
    /// </param>
    /// <param name="cancellationToken">Cancellation token.</param>
    public Task<IReadOnlyList<IspMetric>?> GetAsync(
        string type,
        DateTimeOffset? beginTimestamp = null,
        DateTimeOffset? endTimestamp = null,
        string? duration = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(type);

        var query = new List<KeyValuePair<string, string?>>();
        if (beginTimestamp is not null)
        {
            query.Add(new("beginTimestamp", beginTimestamp.Value.ToUniversalTime().ToString("o", CultureInfo.InvariantCulture)));
        }
        if (endTimestamp is not null)
        {
            query.Add(new("endTimestamp", endTimestamp.Value.ToUniversalTime().ToString("o", CultureInfo.InvariantCulture)));
        }
        if (!string.IsNullOrWhiteSpace(duration))
        {
            query.Add(new("duration", duration));
        }

        return _connection.GetAsync<IReadOnlyList<IspMetric>>(
            $"v1/isp-metrics/{Uri.EscapeDataString(type)}",
            query,
            cancellationToken);
    }

    /// <summary>
    /// Query ISP metrics for a specific set of sites, each with its own optional time window.
    /// </summary>
    /// <param name="type">Metric aggregation type, e.g. "5m" or "1h".</param>
    /// <param name="query">The sites (and per-site time windows) to query.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    public Task<IspMetricsQueryResult?> QueryAsync(
        string type,
        IspMetricsQuery query,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(type);
        ArgumentNullException.ThrowIfNull(query);

        return _connection.PostAsync<IspMetricsQueryResult>(
            $"v1/isp-metrics/{Uri.EscapeDataString(type)}/query",
            query,
            cancellationToken);
    }
}
