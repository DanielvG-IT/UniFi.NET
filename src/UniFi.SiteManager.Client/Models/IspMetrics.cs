using System.Text.Json.Serialization;

namespace UniFi.SiteManager.Client.Models;

/// <summary>ISP performance metrics for a single site's WAN over a series of time periods.</summary>
public sealed class IspMetric
{
    /// <summary>The metric aggregation type, e.g. "5m" or "1h".</summary>
    [JsonPropertyName("metricType")]
    public string? MetricType { get; init; }

    /// <summary>Metric samples over time.</summary>
    [JsonPropertyName("periods")]
    public IReadOnlyList<IspMetricPeriod> Periods { get; init; } = [];

    /// <summary>Host the metrics belong to.</summary>
    [JsonPropertyName("hostId")]
    public string? HostId { get; init; }

    /// <summary>Site the metrics belong to.</summary>
    [JsonPropertyName("siteId")]
    public string? SiteId { get; init; }
}

/// <summary>A single time bucket of ISP metrics.</summary>
public sealed class IspMetricPeriod
{
    [JsonPropertyName("metricTime")]
    public DateTimeOffset? MetricTime { get; init; }

    [JsonPropertyName("version")]
    public string? Version { get; init; }

    [JsonPropertyName("data")]
    public IspMetricPeriodData? Data { get; init; }
}

public sealed class IspMetricPeriodData
{
    [JsonPropertyName("wan")]
    public IspWanMetrics? Wan { get; init; }
}

/// <summary>WAN performance figures for a metric period.</summary>
public sealed class IspWanMetrics
{
    [JsonPropertyName("avgLatency")]
    public int? AvgLatency { get; init; }

    [JsonPropertyName("maxLatency")]
    public int? MaxLatency { get; init; }

    [JsonPropertyName("download_kbps")]
    public int? DownloadKbps { get; init; }

    [JsonPropertyName("upload_kbps")]
    public int? UploadKbps { get; init; }

    [JsonPropertyName("packetLoss")]
    public int? PacketLoss { get; init; }

    [JsonPropertyName("uptime")]
    public int? Uptime { get; init; }

    [JsonPropertyName("downtime")]
    public int? Downtime { get; init; }

    [JsonPropertyName("ispName")]
    public string? IspName { get; init; }

    [JsonPropertyName("ispAsn")]
    public string? IspAsn { get; init; }
}

/// <summary>Result payload of a POST ISP-metrics query.</summary>
public sealed class IspMetricsQueryResult
{
    [JsonPropertyName("metrics")]
    public IReadOnlyList<IspMetric> Metrics { get; init; } = [];

    [JsonPropertyName("message")]
    public string? Message { get; init; }

    [JsonPropertyName("status")]
    public string? Status { get; init; }
}

/// <summary>Request body for POST /v1/isp-metrics/{type}/query.</summary>
public sealed class IspMetricsQuery
{
    [JsonPropertyName("sites")]
    public required IReadOnlyList<IspMetricsQuerySite> Sites { get; init; }
}

/// <summary>A single site to query ISP metrics for, with an optional time window.</summary>
public sealed class IspMetricsQuerySite
{
    [JsonPropertyName("hostId")]
    public required string HostId { get; init; }

    [JsonPropertyName("siteId")]
    public required string SiteId { get; init; }

    [JsonPropertyName("beginTimestamp")]
    public DateTimeOffset? BeginTimestamp { get; init; }

    [JsonPropertyName("endTimestamp")]
    public DateTimeOffset? EndTimestamp { get; init; }
}
