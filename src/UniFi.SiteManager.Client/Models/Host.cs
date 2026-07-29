using System.Text.Json;
using System.Text.Json.Serialization;

namespace UniFi.SiteManager.Client.Models;

/// <summary>
/// A UniFi console or network server registered to your cloud account, as returned by
/// the hosts endpoints.
/// </summary>
public sealed class Host
{
    /// <summary>Unique identifier of the host device.</summary>
    [JsonPropertyName("id")]
    public required string Id { get; init; }

    /// <summary>Hardware identifier of the device.</summary>
    [JsonPropertyName("hardwareId")]
    public string? HardwareId { get; init; }

    /// <summary>Type of the device, e.g. "console" or "network-server".</summary>
    [JsonPropertyName("type")]
    public string? Type { get; init; }

    /// <summary>Current IP address of the device.</summary>
    [JsonPropertyName("ipAddress")]
    public string? IpAddress { get; init; }

    /// <summary>Whether the current user owns this device.</summary>
    [JsonPropertyName("owner")]
    public bool Owner { get; init; }

    /// <summary>Whether the device is blocked from cloud access.</summary>
    [JsonPropertyName("isBlocked")]
    public bool IsBlocked { get; init; }

    /// <summary>When the device was registered to the cloud.</summary>
    [JsonPropertyName("registrationTime")]
    public DateTimeOffset? RegistrationTime { get; init; }

    /// <summary>When the connection state last changed.</summary>
    [JsonPropertyName("lastConnectionStateChange")]
    public DateTimeOffset? LastConnectionStateChange { get; init; }

    /// <summary>Time of the latest device backup.</summary>
    [JsonPropertyName("latestBackupTime")]
    public DateTimeOffset? LatestBackupTime { get; init; }

    /// <summary>
    /// User-specific data including permissions and role information. Shape varies by
    /// UniFi version, so it is exposed as raw JSON.
    /// </summary>
    [JsonPropertyName("userData")]
    public JsonElement? UserData { get; init; }

    /// <summary>
    /// The device's reported state. Shape varies by UniFi version, so it is exposed as raw JSON.
    /// </summary>
    [JsonPropertyName("reportedState")]
    public JsonElement? ReportedState { get; init; }
}
