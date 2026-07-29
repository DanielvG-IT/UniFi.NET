using System.Text.Json;
using System.Text.Json.Serialization;

namespace UniFi.SiteManager.Client.Models;

/// <summary>Live status of an SD-WAN configuration and its hubs and spokes.</summary>
public sealed class SdWanConfigStatus
{
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>A unique identifier representing the current state of the configuration.</summary>
    [JsonPropertyName("fingerprint")]
    public string? Fingerprint { get; init; }

    /// <summary>Unix timestamp (ms) of the last update to the configuration.</summary>
    [JsonPropertyName("updatedAt")]
    public long? UpdatedAt { get; init; }

    [JsonPropertyName("hubs")]
    public IReadOnlyList<SdWanHubStatus> Hubs { get; init; } = [];

    [JsonPropertyName("spokes")]
    public IReadOnlyList<SdWanSpokeStatus> Spokes { get; init; } = [];

    /// <summary>Unix timestamp (ms) of the last generation of the configuration.</summary>
    [JsonPropertyName("lastGeneratedAt")]
    public long? LastGeneratedAt { get; init; }

    /// <summary>Generation status: "OK", "GENERATING", or "GENERATE_FAILED".</summary>
    [JsonPropertyName("generateStatus")]
    public string? GenerateStatus { get; init; }

    [JsonPropertyName("errors")]
    public IReadOnlyList<JsonElement> Errors { get; init; } = [];

    [JsonPropertyName("warnings")]
    public IReadOnlyList<JsonElement> Warnings { get; init; } = [];
}

public sealed class SdWanHubStatus
{
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    [JsonPropertyName("hostId")]
    public string? HostId { get; init; }

    [JsonPropertyName("siteId")]
    public string? SiteId { get; init; }

    [JsonPropertyName("name")]
    public string? Name { get; init; }

    [JsonPropertyName("primaryWanStatus")]
    public SdWanWanStatus? PrimaryWanStatus { get; init; }

    [JsonPropertyName("secondaryWanStatus")]
    public SdWanWanStatus? SecondaryWanStatus { get; init; }

    [JsonPropertyName("errors")]
    public IReadOnlyList<JsonElement> Errors { get; init; } = [];

    [JsonPropertyName("warnings")]
    public IReadOnlyList<JsonElement> Warnings { get; init; } = [];

    [JsonPropertyName("numberOfTunnelsUsedByOtherFeatures")]
    public int? NumberOfTunnelsUsedByOtherFeatures { get; init; }

    [JsonPropertyName("networks")]
    public IReadOnlyList<SdWanNetworkStatus> Networks { get; init; } = [];

    [JsonPropertyName("routes")]
    public IReadOnlyList<SdWanRouteStatus> Routes { get; init; } = [];

    /// <summary>"ok", "creating", "updating", "removing", "createFailed", "updateFailed", or "removeFailed".</summary>
    [JsonPropertyName("applyStatus")]
    public string? ApplyStatus { get; init; }
}

public sealed class SdWanSpokeStatus
{
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    [JsonPropertyName("hostId")]
    public string? HostId { get; init; }

    [JsonPropertyName("siteId")]
    public string? SiteId { get; init; }

    [JsonPropertyName("name")]
    public string? Name { get; init; }

    [JsonPropertyName("primaryWanStatus")]
    public SdWanWanStatus? PrimaryWanStatus { get; init; }

    [JsonPropertyName("secondaryWanStatus")]
    public SdWanWanStatus? SecondaryWanStatus { get; init; }

    [JsonPropertyName("errors")]
    public IReadOnlyList<JsonElement> Errors { get; init; } = [];

    [JsonPropertyName("warnings")]
    public IReadOnlyList<JsonElement> Warnings { get; init; } = [];

    [JsonPropertyName("numberOfTunnelsUsedByOtherFeatures")]
    public int? NumberOfTunnelsUsedByOtherFeatures { get; init; }

    [JsonPropertyName("networks")]
    public IReadOnlyList<SdWanNetworkStatus> Networks { get; init; } = [];

    [JsonPropertyName("routes")]
    public IReadOnlyList<SdWanRouteStatus> Routes { get; init; } = [];

    [JsonPropertyName("connections")]
    public IReadOnlyList<SdWanConnection> Connections { get; init; } = [];

    /// <summary>"ok", "creating", "updating", "removing", "createFailed", "updateFailed", or "removeFailed".</summary>
    [JsonPropertyName("applyStatus")]
    public string? ApplyStatus { get; init; }
}

public sealed class SdWanWanStatus
{
    /// <summary>IP format: 10.0.0.1.</summary>
    [JsonPropertyName("ip")]
    public string? Ip { get; init; }

    [JsonPropertyName("latency")]
    public double? Latency { get; init; }

    /// <summary>WAN internet issues, if any (raw JSON objects).</summary>
    [JsonPropertyName("internetIssues")]
    public IReadOnlyList<JsonElement> InternetIssues { get; init; } = [];

    [JsonPropertyName("wanId")]
    public string? WanId { get; init; }
}

public sealed class SdWanNetworkStatus
{
    [JsonPropertyName("networkId")]
    public string? NetworkId { get; init; }

    [JsonPropertyName("name")]
    public string? Name { get; init; }

    [JsonPropertyName("errors")]
    public IReadOnlyList<JsonElement> Errors { get; init; } = [];

    [JsonPropertyName("warnings")]
    public IReadOnlyList<JsonElement> Warnings { get; init; } = [];
}

public sealed class SdWanRouteStatus
{
    /// <summary>Subnet in CIDR format: 10.0.0.0/24.</summary>
    [JsonPropertyName("routeValue")]
    public string? RouteValue { get; init; }

    [JsonPropertyName("errors")]
    public IReadOnlyList<JsonElement> Errors { get; init; } = [];

    [JsonPropertyName("warnings")]
    public IReadOnlyList<JsonElement> Warnings { get; init; } = [];
}

public sealed class SdWanConnection
{
    [JsonPropertyName("hubId")]
    public string? HubId { get; init; }

    [JsonPropertyName("tunnels")]
    public IReadOnlyList<SdWanTunnel> Tunnels { get; init; } = [];
}

public sealed class SdWanTunnel
{
    [JsonPropertyName("spokeWanId")]
    public string? SpokeWanId { get; init; }

    [JsonPropertyName("hubWanId")]
    public string? HubWanId { get; init; }

    /// <summary>"connected", "disconnected", or "pending".</summary>
    [JsonPropertyName("status")]
    public string? Status { get; init; }
}
