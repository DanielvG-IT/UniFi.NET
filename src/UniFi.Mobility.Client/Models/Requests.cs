using System.Text.Json.Serialization;

namespace UniFi.Mobility.Client.Models;

/// <summary>Request body to rename a device.</summary>
public sealed class UpdateDeviceNameRequest
{
    /// <summary>New device name (1-32 characters).</summary>
    [JsonPropertyName("name")]
    public required string Name { get; init; }
}

/// <summary>
/// Request body to update LAN / DHCP settings. Partial update — only provided (non-null) fields
/// are applied. WAN, IPv6, and internet source are not configurable here.
/// </summary>
public sealed class UpdateNetworkRequest
{
    /// <summary>LAN gateway IPv4 address.</summary>
    [JsonPropertyName("host_address")]
    public string? HostAddress { get; init; }

    /// <summary>DHCP server mode.</summary>
    [JsonPropertyName("dhcp_mode")]
    public DhcpMode? DhcpMode { get; init; }

    [JsonPropertyName("dhcp_range_start")]
    public string? DhcpRangeStart { get; init; }

    [JsonPropertyName("dhcp_range_stop")]
    public string? DhcpRangeStop { get; init; }

    /// <summary>DHCP lease time in seconds. 0 = infinite.</summary>
    [JsonPropertyName("dhcp_lease_time")]
    public int? DhcpLeaseTime { get; init; }
}

/// <summary>
/// Request body to update WiFi settings. Both fields are required. Channel, TX power, and
/// security protocol are not configurable here.
/// </summary>
public sealed class UpdateWirelessRequest
{
    /// <summary>SSID (1-32 characters).</summary>
    [JsonPropertyName("ssid")]
    public required string Ssid { get; init; }

    /// <summary>WPA2-PSK password (8-63 characters).</summary>
    [JsonPropertyName("password")]
    public required string Password { get; init; }
}
