using System.Text.Json.Serialization;

namespace UniFi.Mobility.Client.Models;

/// <summary>A client associated with a mobile routing device.</summary>
public sealed class DeviceClient
{
    /// <summary>MAC address (upper-case, colon-separated).</summary>
    [JsonPropertyName("mac")]
    public required string Mac { get; init; }

    [JsonPropertyName("name")]
    public required string Name { get; init; }

    [JsonPropertyName("type")]
    public ClientType Type { get; init; }

    [JsonPropertyName("connection_status")]
    public ClientConnectionStatus ConnectionStatus { get; init; }

    /// <summary>IP address. Empty when unknown.</summary>
    [JsonPropertyName("ip_address")]
    public string? IpAddress { get; init; }

    [JsonPropertyName("is_blocked")]
    public bool IsBlocked { get; init; }

    /// <summary>WiFi experience score (0-100). Null/omitted for wired clients.</summary>
    [JsonPropertyName("wifi_experience")]
    public int? WifiExperience { get; init; }
}
