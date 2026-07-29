using System.Text.Json.Serialization;

namespace UniFi.Protect.Client.Models;

/// <summary>RTSPS stream URLs for a camera, keyed by quality. Null means that quality has no stream.</summary>
public sealed class RtspsStreams
{
    [JsonPropertyName("high")]
    public string? High { get; init; }

    [JsonPropertyName("medium")]
    public string? Medium { get; init; }

    [JsonPropertyName("low")]
    public string? Low { get; init; }

    [JsonPropertyName("package")]
    public string? Package { get; init; }
}

/// <summary>Connection details for a two-way audio (talkback) session to a camera.</summary>
public sealed class TalkbackSession
{
    [JsonPropertyName("url")]
    public required string Url { get; init; }

    [JsonPropertyName("codec")]
    public required string Codec { get; init; }

    [JsonPropertyName("samplingRate")]
    public int SamplingRate { get; init; }

    [JsonPropertyName("bitsPerSample")]
    public int BitsPerSample { get; init; }
}

/// <summary>Metadata for an uploaded device asset file.</summary>
public sealed class ProtectFile
{
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    [JsonPropertyName("originalName")]
    public string? OriginalName { get; init; }

    [JsonPropertyName("path")]
    public string? Path { get; init; }

    [JsonPropertyName("type")]
    public string? Type { get; init; }
}
