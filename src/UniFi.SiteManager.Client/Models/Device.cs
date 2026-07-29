using System.Text.Json;
using System.Text.Json.Serialization;

namespace UniFi.SiteManager.Client.Models;

/// <summary>
/// Devices grouped by the host that manages them, as returned by the devices endpoint.
/// </summary>
public sealed class HostDevices
{
    /// <summary>Unique identifier of the host device.</summary>
    [JsonPropertyName("hostId")]
    public required string HostId { get; init; }

    /// <summary>Name of the host device.</summary>
    [JsonPropertyName("hostName")]
    public string? HostName { get; init; }

    /// <summary>Devices managed by this host.</summary>
    [JsonPropertyName("devices")]
    public IReadOnlyList<Device> Devices { get; init; } = [];

    /// <summary>Last update time for this host's device list.</summary>
    [JsonPropertyName("updatedAt")]
    public DateTimeOffset? UpdatedAt { get; init; }
}

/// <summary>A single UniFi device managed by a host.</summary>
public sealed class Device
{
    /// <summary>Unique identifier of the device.</summary>
    [JsonPropertyName("id")]
    public required string Id { get; init; }

    /// <summary>MAC address of the device.</summary>
    [JsonPropertyName("mac")]
    public string? Mac { get; init; }

    /// <summary>User-defined name of the device.</summary>
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    /// <summary>Model name of the device.</summary>
    [JsonPropertyName("model")]
    public string? Model { get; init; }

    /// <summary>Short identifier of the device model, e.g. "UDMPROSE".</summary>
    [JsonPropertyName("shortname")]
    public string? Shortname { get; init; }

    /// <summary>IP address of the device.</summary>
    [JsonPropertyName("ip")]
    public string? Ip { get; init; }

    /// <summary>Product line of the device, e.g. "network" or "protect".</summary>
    [JsonPropertyName("productLine")]
    public string? ProductLine { get; init; }

    /// <summary>Current connection status of the device, e.g. "online" or "offline".</summary>
    [JsonPropertyName("status")]
    public string? Status { get; init; }

    /// <summary>Current firmware version of the device.</summary>
    [JsonPropertyName("version")]
    public string? Version { get; init; }

    /// <summary>Status of device firmware, e.g. "upToDate" or "updateAvailable".</summary>
    [JsonPropertyName("firmwareStatus")]
    public string? FirmwareStatus { get; init; }

    /// <summary>Version of firmware update available for the device, if any.</summary>
    [JsonPropertyName("updateAvailable")]
    public string? UpdateAvailable { get; init; }

    /// <summary>Whether the device is a console.</summary>
    [JsonPropertyName("isConsole")]
    public bool? IsConsole { get; init; }

    /// <summary>Whether the device is managed by the controller.</summary>
    [JsonPropertyName("isManaged")]
    public bool? IsManaged { get; init; }

    /// <summary>When the device was last started.</summary>
    [JsonPropertyName("startupTime")]
    public DateTimeOffset? StartupTime { get; init; }

    /// <summary>When the device was adopted.</summary>
    [JsonPropertyName("adoptionTime")]
    public DateTimeOffset? AdoptionTime { get; init; }

    /// <summary>User-defined notes for the device.</summary>
    [JsonPropertyName("note")]
    public string? Note { get; init; }

    /// <summary>UI-specific metadata including images and identifiers, exposed as raw JSON.</summary>
    [JsonPropertyName("uidb")]
    public JsonElement? Uidb { get; init; }
}
