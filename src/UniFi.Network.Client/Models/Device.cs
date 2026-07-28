using System.Text.Json.Serialization;

namespace UniFi.Network.Client.Models;

[JsonConverter(typeof(JsonStringEnumConverter<DeviceFeatureType>))]
public enum DeviceFeatureType
{
    [JsonStringEnumMemberName("switching")]
    Switching,

    [JsonStringEnumMemberName("accessPoint")]
    AccessPoint,

    [JsonStringEnumMemberName("gateway")]
    Gateway,
}

[JsonConverter(typeof(JsonStringEnumConverter<DeviceInterfaceType>))]
public enum DeviceInterfaceType
{
    [JsonStringEnumMemberName("ports")]
    Ports,

    [JsonStringEnumMemberName("radios")]
    Radios,
}

/// <summary>Summary of an adopted device, as returned by the devices list endpoint.</summary>
public sealed class AdoptedDeviceOverview
{
    [JsonPropertyName("id")]
    public required Guid Id { get; init; }

    [JsonPropertyName("name")]
    public required string Name { get; init; }

    [JsonPropertyName("model")]
    public required string Model { get; init; }

    [JsonPropertyName("macAddress")]
    public required string MacAddress { get; init; }

    [JsonPropertyName("ipAddress")]
    public required string IpAddress { get; init; }

    [JsonPropertyName("state")]
    public required DeviceState State { get; init; }

    [JsonPropertyName("supported")]
    public required bool Supported { get; init; }

    [JsonPropertyName("firmwareUpdatable")]
    public required bool FirmwareUpdatable { get; init; }

    [JsonPropertyName("firmwareVersion")]
    public string? FirmwareVersion { get; init; }

    [JsonPropertyName("features")]
    public required IReadOnlyList<DeviceFeatureType> Features { get; init; }

    [JsonPropertyName("interfaces")]
    public required IReadOnlyList<DeviceInterfaceType> Interfaces { get; init; }
}

/// <summary>Full detail for a single adopted device.</summary>
public sealed class AdoptedDeviceDetails
{
    [JsonPropertyName("id")]
    public required Guid Id { get; init; }

    [JsonPropertyName("configurationId")]
    public required string ConfigurationId { get; init; }

    [JsonPropertyName("name")]
    public string? Name { get; init; }

    [JsonPropertyName("model")]
    public string? Model { get; init; }

    [JsonPropertyName("macAddress")]
    public string? MacAddress { get; init; }

    [JsonPropertyName("ipAddress")]
    public string? IpAddress { get; init; }

    [JsonPropertyName("state")]
    public DeviceState? State { get; init; }

    [JsonPropertyName("supported")]
    public required bool Supported { get; init; }

    [JsonPropertyName("firmwareUpdatable")]
    public required bool FirmwareUpdatable { get; init; }

    [JsonPropertyName("firmwareVersion")]
    public string? FirmwareVersion { get; init; }

    [JsonPropertyName("adoptedAt")]
    public DateTimeOffset? AdoptedAt { get; init; }

    [JsonPropertyName("provisionedAt")]
    public DateTimeOffset? ProvisionedAt { get; init; }

    [JsonPropertyName("uplink")]
    public DeviceUplinkInterfaceOverview? Uplink { get; init; }

    [JsonPropertyName("interfaces")]
    public DevicePhysicalInterfaces? Interfaces { get; init; }
}

/// <summary>A device seen on the network but not yet adopted into a site.</summary>
public sealed class DevicePendingAdoption
{
    [JsonPropertyName("macAddress")]
    public required string MacAddress { get; init; }

    [JsonPropertyName("model")]
    public required string Model { get; init; }

    [JsonPropertyName("ipAddress")]
    public required string IpAddress { get; init; }

    [JsonPropertyName("state")]
    public required DeviceState State { get; init; }

    [JsonPropertyName("supported")]
    public required bool Supported { get; init; }

    [JsonPropertyName("firmwareUpdatable")]
    public required bool FirmwareUpdatable { get; init; }

    [JsonPropertyName("firmwareVersion")]
    public string? FirmwareVersion { get; init; }

    [JsonPropertyName("features")]
    public required IReadOnlyList<DeviceFeatureType> Features { get; init; }

    /// <summary>Site ids this device can be adopted into.</summary>
    [JsonPropertyName("adoptionTargetSiteIds")]
    public required IReadOnlyList<Guid> AdoptionTargetSiteIds { get; init; }
}

public sealed class DeviceUplinkInterfaceOverview
{
    [JsonPropertyName("deviceId")]
    public required Guid DeviceId { get; init; }
}

public sealed class DevicePhysicalInterfaces
{
    [JsonPropertyName("ports")]
    public IReadOnlyList<PortOverview>? Ports { get; init; }

    [JsonPropertyName("radios")]
    public IReadOnlyList<WirelessRadioOverview>? Radios { get; init; }
}

[JsonConverter(typeof(JsonStringEnumConverter<PortConnector>))]
public enum PortConnector
{
    RJ45,
    SFP,
    SFPPLUS,
    SFP28,
    QSFP28,
}

[JsonConverter(typeof(JsonStringEnumConverter<PortLinkState>))]
public enum PortLinkState
{
    [JsonStringEnumMemberName("UP")]
    Up,

    [JsonStringEnumMemberName("DOWN")]
    Down,

    [JsonStringEnumMemberName("UNKNOWN")]
    Unknown,
}

public sealed class PortOverview
{
    [JsonPropertyName("idx")]
    public required int Idx { get; init; }

    [JsonPropertyName("connector")]
    public required PortConnector Connector { get; init; }

    [JsonPropertyName("state")]
    public required PortLinkState State { get; init; }

    [JsonPropertyName("speedMbps")]
    public int? SpeedMbps { get; init; }

    [JsonPropertyName("maxSpeedMbps")]
    public required int MaxSpeedMbps { get; init; }

    [JsonPropertyName("poe")]
    public PortPoeOverview? Poe { get; init; }
}

[JsonConverter(typeof(JsonStringEnumConverter<PortPoeState>))]
public enum PortPoeState
{
    [JsonStringEnumMemberName("UP")]
    Up,

    [JsonStringEnumMemberName("DOWN")]
    Down,

    [JsonStringEnumMemberName("LIMITED")]
    Limited,

    [JsonStringEnumMemberName("UNKNOWN")]
    Unknown,
}

public sealed class PortPoeOverview
{
    [JsonPropertyName("enabled")]
    public required bool Enabled { get; init; }

    /// <summary>PoE standard, e.g. "802.3bt".</summary>
    [JsonPropertyName("standard")]
    public required string Standard { get; init; }

    [JsonPropertyName("state")]
    public required PortPoeState State { get; init; }

    /// <summary>PoE type as a numeric grade (1-4), sent by the API as a string.</summary>
    [JsonPropertyName("type")]
    public required string Type { get; init; }
}

public sealed class WirelessRadioOverview
{
    [JsonPropertyName("frequencyGHz")]
    public required double FrequencyGHz { get; init; }

    [JsonPropertyName("channel")]
    public int? Channel { get; init; }

    [JsonPropertyName("channelWidthMHz")]
    public required int ChannelWidthMHz { get; init; }

    /// <summary>Wi-Fi standard, e.g. "802.11ax".</summary>
    [JsonPropertyName("wlanStandard")]
    public required string WlanStandard { get; init; }
}

public sealed class DeviceStatistics
{
    [JsonPropertyName("cpuUtilizationPct")]
    public double? CpuUtilizationPct { get; init; }

    [JsonPropertyName("memoryUtilizationPct")]
    public double? MemoryUtilizationPct { get; init; }

    [JsonPropertyName("loadAverage1Min")]
    public double? LoadAverage1Min { get; init; }

    [JsonPropertyName("loadAverage5Min")]
    public double? LoadAverage5Min { get; init; }

    [JsonPropertyName("loadAverage15Min")]
    public double? LoadAverage15Min { get; init; }

    [JsonPropertyName("uptimeSec")]
    public long? UptimeSec { get; init; }

    [JsonPropertyName("lastHeartbeatAt")]
    public DateTimeOffset? LastHeartbeatAt { get; init; }

    [JsonPropertyName("nextHeartbeatAt")]
    public DateTimeOffset? NextHeartbeatAt { get; init; }

    [JsonPropertyName("uplink")]
    public DeviceUplinkStatistics? Uplink { get; init; }

    [JsonPropertyName("interfaces")]
    public DeviceInterfaceStatistics? Interfaces { get; init; }
}

public sealed class DeviceUplinkStatistics
{
    [JsonPropertyName("rxRateBps")]
    public long? RxRateBps { get; init; }

    [JsonPropertyName("txRateBps")]
    public long? TxRateBps { get; init; }
}

public sealed class DeviceInterfaceStatistics
{
    [JsonPropertyName("radios")]
    public IReadOnlyList<WirelessRadioStatistics>? Radios { get; init; }
}

public sealed class WirelessRadioStatistics
{
    [JsonPropertyName("frequencyGHz")]
    public required double FrequencyGHz { get; init; }

    [JsonPropertyName("txRetriesPct")]
    public double? TxRetriesPct { get; init; }
}

/// <summary>Request body to adopt a pending device into a site.</summary>
public sealed class DeviceAdoptionRequest
{
    [JsonPropertyName("macAddress")]
    public required string MacAddress { get; init; }

    [JsonPropertyName("ignoreDeviceLimit")]
    public bool IgnoreDeviceLimit { get; init; }
}

/// <summary>Request body for POST devices/{deviceId}/actions. RESTART is currently the only supported action.</summary>
public sealed class DeviceActionRequest
{
    [JsonPropertyName("action")]
    public string Action { get; init; } = "RESTART";

    public static DeviceActionRequest Restart() => new();
}

/// <summary>Request body for POST devices/{deviceId}/interfaces/ports/{portIdx}/actions. POWER_CYCLE is currently the only supported action.</summary>
public sealed class PortActionRequest
{
    [JsonPropertyName("action")]
    public string Action { get; init; } = "POWER_CYCLE";

    public static PortActionRequest PowerCycle() => new();
}
