using System.Text.Json.Serialization;

namespace UniFi.Network.Client.Models;

/// <summary>A local site managed by this Network application. Its Id is required by every other site-scoped call.</summary>
public sealed class Site
{
    [JsonPropertyName("id")]
    public required Guid Id { get; init; }

    [JsonPropertyName("internalReference")]
    public string? InternalReference { get; init; }

    [JsonPropertyName("name")]
    public required string Name { get; init; }
}
