using System.Text.Json.Serialization;

namespace UniFi.Mobility.Client.Models;

/// <summary>Lightweight device representation returned in the device list.</summary>
public class DeviceSummary
{
    [JsonPropertyName("id")]
    public required string Id { get; init; }

    [JsonPropertyName("name")]
    public required string Name { get; init; }

    [JsonPropertyName("model")]
    public DeviceModel Model { get; init; }

    [JsonPropertyName("state")]
    public DeviceState State { get; init; }

    [JsonPropertyName("firmware_version")]
    public string? FirmwareVersion { get; init; }

    /// <summary>Primary MAC address (upper-case, colon-separated). Empty until initialised.</summary>
    [JsonPropertyName("mac_address")]
    public string? MacAddress { get; init; }
}

/// <summary>GPS location. Omitted (the whole object) when no GPS fix is available.</summary>
public sealed class DeviceLocation
{
    [JsonPropertyName("latitude")]
    public double Latitude { get; init; }

    [JsonPropertyName("longitude")]
    public double Longitude { get; init; }

    /// <summary>Unix timestamp (ms) of the last GPS fix.</summary>
    [JsonPropertyName("last_updated")]
    public long LastUpdated { get; init; }
}

/// <summary>
/// Full device detail including WAN, cellular, WiFi, VPN, subscription, and GPS.
/// Several string fields use an empty string as a "not set / not reported" sentinel.
/// </summary>
public sealed class DeviceDetail : DeviceSummary
{
    /// <summary>Active WAN interface: "LTE", "WAN", "WIFIWAN", or "" when none connected.</summary>
    [JsonPropertyName("wan_source")]
    public string? WanSource { get; init; }

    /// <summary>Public WAN IP. Empty when not connected.</summary>
    [JsonPropertyName("wan_ip")]
    public string? WanIp { get; init; }

    /// <summary>Enabled WAN interfaces sorted by priority (index 0 = highest).</summary>
    [JsonPropertyName("enabled_wans")]
    public IReadOnlyList<string> EnabledWans { get; init; } = [];

    [JsonPropertyName("isp")]
    public string? Isp { get; init; }

    /// <summary>LTE signal quality: "NO_SIGNAL", "POOR", "FAIR", "STRONG", or "" when not reported.</summary>
    [JsonPropertyName("lte_signal_level")]
    public string? LteSignalLevel { get; init; }

    /// <summary>Data consumed in the current billing cycle, in bytes.</summary>
    [JsonPropertyName("cellular_data_usage_bytes")]
    public long? CellularDataUsageBytes { get; init; }

    /// <summary>Data cap in bytes for the current billing cycle. -1 means unlimited.</summary>
    [JsonPropertyName("cellular_data_limit_bytes")]
    public long? CellularDataLimitBytes { get; init; }

    [JsonPropertyName("memory_usage_percent")]
    public int? MemoryUsagePercent { get; init; }

    /// <summary>Seconds since last boot. 0 when state is not CONNECTED.</summary>
    [JsonPropertyName("uptime_seconds")]
    public long? UptimeSeconds { get; init; }

    [JsonPropertyName("client_count")]
    public int? ClientCount { get; init; }

    /// <summary>LAN gateway IP. Contains the WAN IP in WANBRIDGE mode.</summary>
    [JsonPropertyName("host_address")]
    public string? HostAddress { get; init; }

    [JsonPropertyName("poe_passthrough")]
    public bool? PoePassthrough { get; init; }

    [JsonPropertyName("device_mode")]
    public DeviceMode? DeviceMode { get; init; }

    [JsonPropertyName("wifi_enabled")]
    public bool? WifiEnabled { get; init; }

    [JsonPropertyName("wifi_ssid")]
    public string? WifiSsid { get; init; }

    /// <summary>TX power: "HIGH", "MEDIUM", "LOW", or "" when the wireless record is not initialised.</summary>
    [JsonPropertyName("tx_power_level")]
    public string? TxPowerLevel { get; init; }

    /// <summary>VPN profile name. Empty when no VPN is configured.</summary>
    [JsonPropertyName("vpn_profile_name")]
    public string? VpnProfileName { get; init; }

    /// <summary>VPN status: "CONNECTING", "CONNECTED", "DISCONNECTED", "FAILED", or "" when no session.</summary>
    [JsonPropertyName("vpn_status")]
    public string? VpnStatus { get; init; }

    [JsonPropertyName("firewall_rule_names")]
    public IReadOnlyList<string> FirewallRuleNames { get; init; } = [];

    [JsonPropertyName("routing_rule_names")]
    public IReadOnlyList<string> RoutingRuleNames { get; init; } = [];

    [JsonPropertyName("ddns_profile_names")]
    public IReadOnlyList<string> DdnsProfileNames { get; init; } = [];

    /// <summary>Active data plan, e.g. "FREE_TRIAL", "1GB", "5GB", "CLOUD", or "" when none.</summary>
    [JsonPropertyName("subscription_plan")]
    public string? SubscriptionPlan { get; init; }

    [JsonPropertyName("subscription_status")]
    public SubscriptionStatus? SubscriptionStatus { get; init; }

    /// <summary>GPS location. Null when no GPS fix is available.</summary>
    [JsonPropertyName("location")]
    public DeviceLocation? Location { get; init; }
}
