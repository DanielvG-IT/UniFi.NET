using System.Text.Json.Serialization;

namespace UniFi.Protect.Client.Models;

/// <summary>Request body to create RTSPS streams for a camera.</summary>
public sealed class CreateRtspsStreamsRequest
{
    /// <summary>Quality levels to create streams for. At least one is required.</summary>
    [JsonPropertyName("qualities")]
    public required IReadOnlyList<ChannelQuality> Qualities { get; init; }
}

/// <summary>Request body to set the current arm profile.</summary>
public sealed class SetArmProfileRequest
{
    [JsonPropertyName("armProfileId")]
    public required string ArmProfileId { get; init; }
}

/// <summary>Request body to activate a relay output. Omitting <see cref="State"/> toggles the output.</summary>
public sealed class RelayActivateRequest
{
    /// <summary>Desired output state, "on" or "off". If null, the current state is toggled.</summary>
    [JsonPropertyName("state")]
    public string? State { get; init; }

    /// <summary>Auto-off duration in milliseconds (only when turning on). 0 or null means no auto-off.</summary>
    [JsonPropertyName("pulseDuration")]
    public int? PulseDuration { get; init; }
}

/// <summary>Request body to trigger an alarm hub output. Omitting <see cref="Enable"/> toggles the output.</summary>
public sealed class AlarmHubTriggerRequest
{
    /// <summary>True to turn on, false to turn off. If null, the current state is toggled.</summary>
    [JsonPropertyName("enable")]
    public bool? Enable { get; init; }

    /// <summary>Delay in milliseconds before the output activates.</summary>
    [JsonPropertyName("delay")]
    public int? Delay { get; init; }

    /// <summary>Duration in milliseconds to keep the output active. 0 means indefinite.</summary>
    [JsonPropertyName("duration")]
    public int? Duration { get; init; }
}

/// <summary>Request body to play a siren. Duration defaults to 5 seconds when omitted.</summary>
public sealed class SirenPlayRequest
{
    /// <summary>Duration of the siren activation in seconds.</summary>
    [JsonPropertyName("duration")]
    public int? Duration { get; init; }
}
