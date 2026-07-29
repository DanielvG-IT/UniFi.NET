using System.Text.Json;
using System.Text.Json.Serialization;

namespace UniFi.SiteManager.Client.Models;

/// <summary>
/// A site managed by one of your hosts, as returned by the sites endpoint.
/// </summary>
public sealed class Site
{
    /// <summary>Unique identifier of the site.</summary>
    [JsonPropertyName("siteId")]
    public required string SiteId { get; init; }

    /// <summary>Unique identifier of the host device managing this site.</summary>
    [JsonPropertyName("hostId")]
    public string? HostId { get; init; }

    /// <summary>
    /// Site metadata (name, description, timezone, gateway MAC, etc.). Shape varies by
    /// UniFi Network version, so it is exposed as raw JSON.
    /// </summary>
    [JsonPropertyName("meta")]
    public JsonElement? Meta { get; init; }

    /// <summary>
    /// Site statistics (device/client counts, performance metrics, etc.). Shape varies by
    /// UniFi Network version, so it is exposed as raw JSON.
    /// </summary>
    [JsonPropertyName("statistics")]
    public JsonElement? Statistics { get; init; }

    /// <summary>Permission level of the current user for this site, e.g. "admin" or "readonly".</summary>
    [JsonPropertyName("permission")]
    public string? Permission { get; init; }

    /// <summary>Whether the current user owns this site.</summary>
    [JsonPropertyName("isOwner")]
    public bool IsOwner { get; init; }
}
