using System.Text.Json.Serialization;

namespace UniFi.Network.Client.Models;

public sealed class Country
{
    /// <summary>ISO 3166-1 alpha-2 country code.</summary>
    [JsonPropertyName("code")]
    public required string Code { get; init; }

    [JsonPropertyName("name")]
    public required string Name { get; init; }
}

public sealed class DpiApplication
{
    [JsonPropertyName("id")]
    public required int Id { get; init; }

    [JsonPropertyName("name")]
    public required string Name { get; init; }
}

public sealed class DpiCategory
{
    [JsonPropertyName("id")]
    public required int Id { get; init; }

    [JsonPropertyName("name")]
    public required string Name { get; init; }
}

public sealed class DeviceTag
{
    [JsonPropertyName("id")]
    public required Guid Id { get; init; }

    [JsonPropertyName("name")]
    public required string Name { get; init; }

    [JsonPropertyName("deviceIds")]
    public required IReadOnlyList<Guid> DeviceIds { get; init; }

    [JsonPropertyName("metadata")]
    public required EntityMetadata Metadata { get; init; }
}

public sealed class RadiusProfileOverview
{
    [JsonPropertyName("id")]
    public required Guid Id { get; init; }

    [JsonPropertyName("name")]
    public required string Name { get; init; }

    [JsonPropertyName("metadata")]
    public required EntityMetadata Metadata { get; init; }
}

public sealed class WanOverview
{
    [JsonPropertyName("id")]
    public required Guid Id { get; init; }

    [JsonPropertyName("name")]
    public required string Name { get; init; }
}
