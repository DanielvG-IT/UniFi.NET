using System.Text.Json.Serialization;

namespace UniFi.SiteManager.Client.Models;

/// <summary>
/// Envelope every Site Manager API response is wrapped in. Resource methods unwrap
/// <see cref="Data"/> for you, so this type is mostly internal plumbing.
/// </summary>
internal sealed class SiteManagerEnvelope<TData>
{
    [JsonPropertyName("data")]
    public TData? Data { get; init; }

    [JsonPropertyName("httpStatusCode")]
    public int HttpStatusCode { get; init; }

    [JsonPropertyName("traceId")]
    public string? TraceId { get; init; }

    [JsonPropertyName("nextToken")]
    public string? NextToken { get; init; }

    [JsonPropertyName("code")]
    public string? Code { get; init; }
}
