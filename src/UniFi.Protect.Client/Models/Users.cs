using System.Text.Json.Serialization;

namespace UniFi.Protect.Client.Models;

/// <summary>Application information for the Protect application.</summary>
public sealed class ProtectApplicationInfo
{
    [JsonPropertyName("applicationVersion")]
    public required string ApplicationVersion { get; init; }
}

/// <summary>A Protect user.</summary>
public sealed class ProtectUser
{
    [JsonPropertyName("id")]
    public required string Id { get; init; }

    [JsonPropertyName("modelKey")]
    public string? ModelKey { get; init; }

    [JsonPropertyName("name")]
    public string? Name { get; init; }

    [JsonPropertyName("firstName")]
    public string? FirstName { get; init; }

    [JsonPropertyName("lastName")]
    public string? LastName { get; init; }

    [JsonPropertyName("email")]
    public string? Email { get; init; }

    [JsonPropertyName("ucoreUserId")]
    public string? UcoreUserId { get; init; }
}

/// <summary>A UniFi Identity (ULP) user.</summary>
public sealed class UlpUser
{
    [JsonPropertyName("id")]
    public required string Id { get; init; }

    [JsonPropertyName("modelKey")]
    public string? ModelKey { get; init; }

    [JsonPropertyName("firstName")]
    public string? FirstName { get; init; }

    [JsonPropertyName("lastName")]
    public string? LastName { get; init; }

    [JsonPropertyName("fullName")]
    public string? FullName { get; init; }

    [JsonPropertyName("status")]
    public string? Status { get; init; }
}
