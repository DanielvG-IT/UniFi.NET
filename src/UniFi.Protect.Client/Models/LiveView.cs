using System.Text.Json;
using System.Text.Json.Serialization;

namespace UniFi.Protect.Client.Models;

/// <summary>A Protect live view (a saved grid layout of camera slots).</summary>
public sealed class LiveView
{
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    [JsonPropertyName("modelKey")]
    public string? ModelKey { get; init; }

    [JsonPropertyName("name")]
    public string? Name { get; init; }

    [JsonPropertyName("isDefault")]
    public bool? IsDefault { get; init; }

    [JsonPropertyName("isGlobal")]
    public bool? IsGlobal { get; init; }

    [JsonPropertyName("owner")]
    public string? Owner { get; init; }

    [JsonPropertyName("layout")]
    public double? Layout { get; init; }

    [JsonPropertyName("slots")]
    public IReadOnlyList<JsonElement> Slots { get; init; } = [];
}
