using System.Text.Json;
using System.Text.Json.Serialization;

namespace UniFi.Network.Client.Models;

/// <summary>
/// A broadcast WiFi network. <c>Type</c> distinguishes STANDARD from IOT_OPTIMIZED broadcasts.
/// <c>Network</c>, <c>SecurityConfiguration</c> and <c>BroadcastingDeviceFilter</c> are left as raw
/// JSON: each is its own deep discriminated union (security alone spans open/PSK/SAE/RADIUS
/// variants) — inspect <c>type</c> within them and consult the OpenAPI spec for exact shapes.
/// </summary>
public sealed class WifiBroadcastOverview
{
    [JsonPropertyName("id")]
    public required Guid Id { get; init; }

    [JsonPropertyName("type")]
    public required string Type { get; init; }

    [JsonPropertyName("name")]
    public required string Name { get; init; }

    [JsonPropertyName("enabled")]
    public required bool Enabled { get; init; }

    [JsonPropertyName("metadata")]
    public required EntityMetadata Metadata { get; init; }

    [JsonPropertyName("network")]
    public JsonElement Network { get; init; }

    [JsonPropertyName("securityConfiguration")]
    public JsonElement SecurityConfiguration { get; init; }

    [JsonPropertyName("broadcastingDeviceFilter")]
    public JsonElement BroadcastingDeviceFilter { get; init; }

    [JsonExtensionData]
    public Dictionary<string, JsonElement>? AdditionalProperties { get; init; }
}

/// <summary>Full WiFi broadcast configuration. See <see cref="WifiBroadcastOverview"/> remarks on raw-JSON fields.</summary>
public sealed class WifiBroadcastDetails
{
    [JsonPropertyName("id")]
    public required Guid Id { get; init; }

    [JsonPropertyName("type")]
    public required string Type { get; init; }

    [JsonPropertyName("name")]
    public required string Name { get; init; }

    [JsonPropertyName("enabled")]
    public required bool Enabled { get; init; }

    [JsonPropertyName("metadata")]
    public required EntityMetadata Metadata { get; init; }

    [JsonExtensionData]
    public Dictionary<string, JsonElement>? AdditionalProperties { get; init; }
}
