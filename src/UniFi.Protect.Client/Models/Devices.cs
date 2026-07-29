using System.Text.Json;
using System.Text.Json.Serialization;

namespace UniFi.Protect.Client.Models;

// Protect device models. Common identity/overview fields are strongly typed; deeply nested,
// frequently-extended settings blobs are exposed as System.Text.Json.JsonElement so the client
// stays stable as the API grows. Update these devices with a JsonObject via the PATCH methods.

/// <summary>Battery status shared by battery-powered devices.</summary>
public sealed class BatteryStatus
{
    [JsonPropertyName("isLow")]
    public bool? IsLow { get; init; }

    [JsonPropertyName("percentage")]
    public double? Percentage { get; init; }
}

/// <summary>A UniFi Protect camera.</summary>
public sealed class Camera
{
    [JsonPropertyName("id")]
    public required string Id { get; init; }

    [JsonPropertyName("modelKey")]
    public string? ModelKey { get; init; }

    [JsonPropertyName("name")]
    public string? Name { get; init; }

    [JsonPropertyName("mac")]
    public string? Mac { get; init; }

    [JsonPropertyName("state")]
    public DeviceState? State { get; init; }

    [JsonPropertyName("isMicEnabled")]
    public bool? IsMicEnabled { get; init; }

    [JsonPropertyName("micVolume")]
    public double? MicVolume { get; init; }

    [JsonPropertyName("videoMode")]
    public VideoMode? VideoMode { get; init; }

    [JsonPropertyName("hdrType")]
    public HdrType? HdrType { get; init; }

    [JsonPropertyName("hasPackageCamera")]
    public bool? HasPackageCamera { get; init; }

    [JsonPropertyName("activePatrolSlot")]
    public int? ActivePatrolSlot { get; init; }

    [JsonPropertyName("osdSettings")]
    public JsonElement? OsdSettings { get; init; }

    [JsonPropertyName("ledSettings")]
    public JsonElement? LedSettings { get; init; }

    [JsonPropertyName("lcdMessage")]
    public JsonElement? LcdMessage { get; init; }

    [JsonPropertyName("smartDetectSettings")]
    public JsonElement? SmartDetectSettings { get; init; }

    [JsonPropertyName("featureFlags")]
    public JsonElement? FeatureFlags { get; init; }
}

/// <summary>A UniFi Protect light.</summary>
public sealed class Light
{
    [JsonPropertyName("id")]
    public required string Id { get; init; }

    [JsonPropertyName("modelKey")]
    public string? ModelKey { get; init; }

    [JsonPropertyName("name")]
    public string? Name { get; init; }

    [JsonPropertyName("mac")]
    public string? Mac { get; init; }

    [JsonPropertyName("state")]
    public DeviceState? State { get; init; }

    [JsonPropertyName("isDark")]
    public bool? IsDark { get; init; }

    [JsonPropertyName("isLightOn")]
    public bool? IsLightOn { get; init; }

    [JsonPropertyName("isLightForceEnabled")]
    public bool? IsLightForceEnabled { get; init; }

    [JsonPropertyName("isPirMotionDetected")]
    public bool? IsPirMotionDetected { get; init; }

    [JsonPropertyName("lastMotion")]
    public long? LastMotion { get; init; }

    [JsonPropertyName("camera")]
    public string? CameraId { get; init; }

    [JsonPropertyName("lightModeSettings")]
    public JsonElement? LightModeSettings { get; init; }

    [JsonPropertyName("lightDeviceSettings")]
    public JsonElement? LightDeviceSettings { get; init; }
}

/// <summary>A UniFi Protect sensor (motion/entry/environmental).</summary>
public sealed class Sensor
{
    [JsonPropertyName("id")]
    public required string Id { get; init; }

    [JsonPropertyName("modelKey")]
    public string? ModelKey { get; init; }

    [JsonPropertyName("name")]
    public string? Name { get; init; }

    [JsonPropertyName("mac")]
    public string? Mac { get; init; }

    [JsonPropertyName("state")]
    public DeviceState? State { get; init; }

    [JsonPropertyName("mountType")]
    public string? MountType { get; init; }

    [JsonPropertyName("batteryStatus")]
    public BatteryStatus? BatteryStatus { get; init; }

    [JsonPropertyName("isOpened")]
    public bool? IsOpened { get; init; }

    [JsonPropertyName("openStatusChangedAt")]
    public long? OpenStatusChangedAt { get; init; }

    [JsonPropertyName("isMotionDetected")]
    public bool? IsMotionDetected { get; init; }

    [JsonPropertyName("motionDetectedAt")]
    public long? MotionDetectedAt { get; init; }

    [JsonPropertyName("scheduleMode")]
    public string? ScheduleMode { get; init; }

    [JsonPropertyName("armProfileIds")]
    public IReadOnlyList<string>? ArmProfileIds { get; init; }

    [JsonPropertyName("hasCustomSensitivityWhenArmed")]
    public bool? HasCustomSensitivityWhenArmed { get; init; }

    [JsonPropertyName("alarmTriggeredAt")]
    public long? AlarmTriggeredAt { get; init; }

    [JsonPropertyName("leakDetectedAt")]
    public long? LeakDetectedAt { get; init; }

    [JsonPropertyName("externalLeakDetectedAt")]
    public long? ExternalLeakDetectedAt { get; init; }

    [JsonPropertyName("tamperingDetectedAt")]
    public long? TamperingDetectedAt { get; init; }

    [JsonPropertyName("stats")]
    public JsonElement? Stats { get; init; }

    [JsonPropertyName("lightSettings")]
    public JsonElement? LightSettings { get; init; }

    [JsonPropertyName("humiditySettings")]
    public JsonElement? HumiditySettings { get; init; }

    [JsonPropertyName("temperatureSettings")]
    public JsonElement? TemperatureSettings { get; init; }

    [JsonPropertyName("motionSettings")]
    public JsonElement? MotionSettings { get; init; }

    [JsonPropertyName("glassBreakSettings")]
    public JsonElement? GlassBreakSettings { get; init; }

    [JsonPropertyName("alarmSettings")]
    public JsonElement? AlarmSettings { get; init; }

    [JsonPropertyName("leakSettings")]
    public JsonElement? LeakSettings { get; init; }

    [JsonPropertyName("wirelessConnectionState")]
    public JsonElement? WirelessConnectionState { get; init; }
}

/// <summary>A UniFi Protect NVR/console.</summary>
public sealed class Nvr
{
    [JsonPropertyName("id")]
    public required string Id { get; init; }

    [JsonPropertyName("modelKey")]
    public string? ModelKey { get; init; }

    [JsonPropertyName("name")]
    public string? Name { get; init; }

    [JsonPropertyName("armMode")]
    public JsonElement? ArmMode { get; init; }

    [JsonPropertyName("doorbellSettings")]
    public JsonElement? DoorbellSettings { get; init; }
}

/// <summary>A UniFi Protect chime.</summary>
public sealed class Chime
{
    [JsonPropertyName("id")]
    public required string Id { get; init; }

    [JsonPropertyName("modelKey")]
    public string? ModelKey { get; init; }

    [JsonPropertyName("name")]
    public string? Name { get; init; }

    [JsonPropertyName("mac")]
    public string? Mac { get; init; }

    [JsonPropertyName("state")]
    public DeviceState? State { get; init; }

    [JsonPropertyName("cameraIds")]
    public IReadOnlyList<string> CameraIds { get; init; } = [];

    [JsonPropertyName("ringSettings")]
    public IReadOnlyList<JsonElement> RingSettings { get; init; } = [];
}

/// <summary>A UniFi Protect viewer (Protect Viewport).</summary>
public sealed class Viewer
{
    [JsonPropertyName("id")]
    public required string Id { get; init; }

    [JsonPropertyName("modelKey")]
    public string? ModelKey { get; init; }

    [JsonPropertyName("name")]
    public string? Name { get; init; }

    [JsonPropertyName("mac")]
    public string? Mac { get; init; }

    [JsonPropertyName("state")]
    public DeviceState? State { get; init; }

    [JsonPropertyName("liveview")]
    public string? LiveviewId { get; init; }

    [JsonPropertyName("streamLimit")]
    public double? StreamLimit { get; init; }
}

/// <summary>A UniFi Protect speaker.</summary>
public sealed class Speaker
{
    [JsonPropertyName("id")]
    public required string Id { get; init; }

    [JsonPropertyName("modelKey")]
    public string? ModelKey { get; init; }

    [JsonPropertyName("name")]
    public string? Name { get; init; }

    [JsonPropertyName("mac")]
    public string? Mac { get; init; }

    [JsonPropertyName("state")]
    public DeviceState? State { get; init; }

    [JsonPropertyName("volume")]
    public int? Volume { get; init; }

    [JsonPropertyName("micVolume")]
    public int? MicVolume { get; init; }

    [JsonPropertyName("isMicEnabled")]
    public bool? IsMicEnabled { get; init; }

    [JsonPropertyName("speakerState")]
    public JsonElement? SpeakerState { get; init; }

    [JsonPropertyName("featureFlags")]
    public JsonElement? FeatureFlags { get; init; }
}

/// <summary>A UniFi Protect siren.</summary>
public sealed class Siren
{
    [JsonPropertyName("id")]
    public required string Id { get; init; }

    [JsonPropertyName("modelKey")]
    public string? ModelKey { get; init; }

    [JsonPropertyName("name")]
    public string? Name { get; init; }

    [JsonPropertyName("mac")]
    public string? Mac { get; init; }

    [JsonPropertyName("state")]
    public DeviceState? State { get; init; }

    [JsonPropertyName("volume")]
    public int? Volume { get; init; }

    [JsonPropertyName("connectionType")]
    public string? ConnectionType { get; init; }

    [JsonPropertyName("sirenStatus")]
    public SirenStatus? SirenStatus { get; init; }

    [JsonPropertyName("ledSettings")]
    public JsonElement? LedSettings { get; init; }

    [JsonPropertyName("wirelessConnectionState")]
    public JsonElement? WirelessConnectionState { get; init; }
}

public sealed class SirenStatus
{
    [JsonPropertyName("isActive")]
    public bool? IsActive { get; init; }

    [JsonPropertyName("activatedAt")]
    public long? ActivatedAt { get; init; }

    [JsonPropertyName("duration")]
    public long? Duration { get; init; }
}

/// <summary>A UniFi Protect bridge.</summary>
public sealed class Bridge
{
    [JsonPropertyName("id")]
    public required string Id { get; init; }

    [JsonPropertyName("modelKey")]
    public string? ModelKey { get; init; }

    [JsonPropertyName("name")]
    public string? Name { get; init; }

    [JsonPropertyName("mac")]
    public string? Mac { get; init; }

    [JsonPropertyName("state")]
    public DeviceState? State { get; init; }

    [JsonPropertyName("platform")]
    public string? Platform { get; init; }

    [JsonPropertyName("maxClients")]
    public double? MaxClients { get; init; }

    [JsonPropertyName("clients")]
    public JsonElement? Clients { get; init; }
}

/// <summary>A UniFi Protect relay (e.g. door lock relay).</summary>
public sealed class Relay
{
    [JsonPropertyName("id")]
    public required string Id { get; init; }

    [JsonPropertyName("modelKey")]
    public string? ModelKey { get; init; }

    [JsonPropertyName("name")]
    public string? Name { get; init; }

    [JsonPropertyName("mac")]
    public string? Mac { get; init; }

    [JsonPropertyName("state")]
    public DeviceState? State { get; init; }

    [JsonPropertyName("outputs")]
    public JsonElement? Outputs { get; init; }

    [JsonPropertyName("inputs")]
    public JsonElement? Inputs { get; init; }

    [JsonPropertyName("ledSettings")]
    public JsonElement? LedSettings { get; init; }

    [JsonPropertyName("wirelessConnectionState")]
    public JsonElement? WirelessConnectionState { get; init; }
}

/// <summary>A UniFi Protect key fob.</summary>
public sealed class Fob
{
    [JsonPropertyName("id")]
    public required string Id { get; init; }

    [JsonPropertyName("modelKey")]
    public string? ModelKey { get; init; }

    [JsonPropertyName("name")]
    public string? Name { get; init; }

    [JsonPropertyName("mac")]
    public string? Mac { get; init; }

    [JsonPropertyName("state")]
    public DeviceState? State { get; init; }

    [JsonPropertyName("awayState")]
    public string? AwayState { get; init; }

    [JsonPropertyName("buttonLabels")]
    public JsonElement? ButtonLabels { get; init; }

    [JsonPropertyName("featureFlags")]
    public JsonElement? FeatureFlags { get; init; }

    [JsonPropertyName("wirelessConnectionState")]
    public JsonElement? WirelessConnectionState { get; init; }
}

/// <summary>A UniFi Protect link station / alarm hub. Also returned by the alarm-hubs endpoints.</summary>
public sealed class LinkStation
{
    [JsonPropertyName("id")]
    public required string Id { get; init; }

    [JsonPropertyName("modelKey")]
    public string? ModelKey { get; init; }

    [JsonPropertyName("name")]
    public string? Name { get; init; }

    [JsonPropertyName("mac")]
    public string? Mac { get; init; }

    [JsonPropertyName("state")]
    public DeviceState? State { get; init; }

    [JsonPropertyName("isAlarmHub")]
    public bool? IsAlarmHub { get; init; }

    [JsonPropertyName("ledSettings")]
    public JsonElement? LedSettings { get; init; }

    [JsonPropertyName("lastEvent")]
    public JsonElement? LastEvent { get; init; }

    [JsonPropertyName("alarmHub")]
    public JsonElement? AlarmHub { get; init; }
}
