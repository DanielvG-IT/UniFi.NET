using System.Text.Json.Serialization;

namespace UniFi.Protect.Client.Models;

/// <summary>Connection state shared by all Protect devices.</summary>
[JsonConverter(typeof(JsonStringEnumConverter<DeviceState>))]
public enum DeviceState
{
    [JsonStringEnumMemberName("CONNECTED")]
    Connected,

    [JsonStringEnumMemberName("CONNECTING")]
    Connecting,

    [JsonStringEnumMemberName("DISCONNECTED")]
    Disconnected,
}

/// <summary>Camera video mode.</summary>
[JsonConverter(typeof(JsonStringEnumConverter<VideoMode>))]
public enum VideoMode
{
    [JsonStringEnumMemberName("default")]
    Default,

    [JsonStringEnumMemberName("highFps")]
    HighFps,

    [JsonStringEnumMemberName("sport")]
    Sport,

    [JsonStringEnumMemberName("slowShutter")]
    SlowShutter,

    [JsonStringEnumMemberName("lprReflex")]
    LprReflex,

    [JsonStringEnumMemberName("lprNoneReflex")]
    LprNoneReflex,
}

/// <summary>Camera High Dynamic Range mode.</summary>
[JsonConverter(typeof(JsonStringEnumConverter<HdrType>))]
public enum HdrType
{
    [JsonStringEnumMemberName("auto")]
    Auto,

    [JsonStringEnumMemberName("on")]
    On,

    [JsonStringEnumMemberName("off")]
    Off,
}

/// <summary>RTSPS stream quality level.</summary>
[JsonConverter(typeof(JsonStringEnumConverter<ChannelQuality>))]
public enum ChannelQuality
{
    [JsonStringEnumMemberName("high")]
    High,

    [JsonStringEnumMemberName("medium")]
    Medium,

    [JsonStringEnumMemberName("low")]
    Low,

    [JsonStringEnumMemberName("package")]
    Package,
}
