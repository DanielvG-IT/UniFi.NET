using System.Text.Json.Serialization;

namespace UniFi.Network.Client.Models;

public sealed class ApplicationInfo
{
    [JsonPropertyName("applicationVersion")]
    public required string ApplicationVersion { get; init; }
}
