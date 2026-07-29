using System.Text.Json;
using System.Text.Json.Serialization;

namespace UniFi.Protect.Client.Models;

/// <summary>An arm profile: a named set of automations and schedules for arming Protect.</summary>
public sealed class ArmProfile
{
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    [JsonPropertyName("name")]
    public string? Name { get; init; }

    [JsonPropertyName("recordEverything")]
    public bool? RecordEverything { get; init; }

    [JsonPropertyName("activationDelay")]
    public double? ActivationDelay { get; init; }

    [JsonPropertyName("createdAt")]
    public long? CreatedAt { get; init; }

    [JsonPropertyName("updatedAt")]
    public long? UpdatedAt { get; init; }

    [JsonPropertyName("creator")]
    public string? Creator { get; init; }

    [JsonPropertyName("automations")]
    public JsonElement? Automations { get; init; }

    [JsonPropertyName("schedules")]
    public JsonElement? Schedules { get; init; }
}
