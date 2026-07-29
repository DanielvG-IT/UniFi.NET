using System.Text.Json.Serialization;

namespace UniFi.SiteManager.Client.Models;

/// <summary>Summary of an SD-WAN configuration, as returned by the list endpoint.</summary>
public sealed class SdWanConfig
{
    /// <summary>Unique identifier of the SD-WAN config.</summary>
    [JsonPropertyName("id")]
    public required string Id { get; init; }

    /// <summary>Name of the SD-WAN config.</summary>
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    /// <summary>Type of SD-WAN config. Currently only "sdwan-hbsp" is supported.</summary>
    [JsonPropertyName("type")]
    public string? Type { get; init; }
}

/// <summary>Full detail for a single SD-WAN configuration.</summary>
public sealed class SdWanConfigDetails
{
    [JsonPropertyName("id")]
    public required string Id { get; init; }

    [JsonPropertyName("name")]
    public string? Name { get; init; }

    [JsonPropertyName("type")]
    public string? Type { get; init; }

    /// <summary>Topology variant: "distributed", "failover", or "single".</summary>
    [JsonPropertyName("variant")]
    public string? Variant { get; init; }

    [JsonPropertyName("settings")]
    public SdWanConfigSettings? Settings { get; init; }

    [JsonPropertyName("hubs")]
    public IReadOnlyList<SdWanHub> Hubs { get; init; } = [];

    [JsonPropertyName("spokes")]
    public IReadOnlyList<SdWanSpoke> Spokes { get; init; } = [];
}

public sealed class SdWanConfigSettings
{
    [JsonPropertyName("hubsInterconnect")]
    public bool? HubsInterconnect { get; init; }

    /// <summary>"maxResiliency", "redundant", or "scalable".</summary>
    [JsonPropertyName("spokeToHubTunnelsMode")]
    public string? SpokeToHubTunnelsMode { get; init; }

    /// <summary>Auto-assigns subnet and routes; otherwise users enter them manually.</summary>
    [JsonPropertyName("spokesAutoScaleAndNatEnabled")]
    public bool SpokesAutoScaleAndNatEnabled { get; init; }

    /// <summary>Subnet in CIDR format, e.g. "172.16.0.0/12".</summary>
    [JsonPropertyName("spokesAutoScaleAndNatRange")]
    public string? SpokesAutoScaleAndNatRange { get; init; }

    /// <summary>When true, spokes can reach hubs but not other spokes.</summary>
    [JsonPropertyName("spokesIsolate")]
    public bool SpokesIsolate { get; init; }

    [JsonPropertyName("spokeStandardSettingsEnabled")]
    public bool SpokeStandardSettingsEnabled { get; init; }

    [JsonPropertyName("spokeStandardSettingsValues")]
    public SdWanSpokeStandardSettings? SpokeStandardSettingsValues { get; init; }

    /// <summary>"custom" or "geo".</summary>
    [JsonPropertyName("spokeToHubRouting")]
    public string? SpokeToHubRouting { get; init; }
}

public sealed class SdWanSpokeStandardSettings
{
    /// <summary>Example: "WAN".</summary>
    [JsonPropertyName("primaryWan")]
    public string? PrimaryWan { get; init; }

    /// <summary>Use failover WAN.</summary>
    [JsonPropertyName("wanFailover")]
    public bool? WanFailover { get; init; }
}

public sealed class SdWanHub
{
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    [JsonPropertyName("hostId")]
    public string? HostId { get; init; }

    [JsonPropertyName("siteId")]
    public string? SiteId { get; init; }

    /// <summary>Ids of networks belonging to the hub.</summary>
    [JsonPropertyName("networkIds")]
    public IReadOnlyList<string> NetworkIds { get; init; } = [];

    /// <summary>Subnets in CIDR format, e.g. "10.0.0.0/24".</summary>
    [JsonPropertyName("routes")]
    public IReadOnlyList<string> Routes { get; init; } = [];

    /// <summary>Example: "WAN".</summary>
    [JsonPropertyName("primaryWan")]
    public string? PrimaryWan { get; init; }

    [JsonPropertyName("wanFailover")]
    public bool WanFailover { get; init; }
}

public sealed class SdWanSpoke
{
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    [JsonPropertyName("hostId")]
    public string? HostId { get; init; }

    [JsonPropertyName("siteId")]
    public string? SiteId { get; init; }

    /// <summary>Ids of networks belonging to the spoke.</summary>
    [JsonPropertyName("networkIds")]
    public IReadOnlyList<string> NetworkIds { get; init; } = [];

    /// <summary>Subnets in CIDR format, e.g. "10.0.0.0/24".</summary>
    [JsonPropertyName("routes")]
    public IReadOnlyList<string> Routes { get; init; } = [];

    /// <summary>Example: "WAN".</summary>
    [JsonPropertyName("primaryWan")]
    public string? PrimaryWan { get; init; }

    [JsonPropertyName("wanFailover")]
    public bool WanFailover { get; init; }

    /// <summary>Non-null for distributed topology with custom spoke-to-hub routing.</summary>
    [JsonPropertyName("hubsPriority")]
    public IReadOnlyList<string> HubsPriority { get; init; } = [];
}
