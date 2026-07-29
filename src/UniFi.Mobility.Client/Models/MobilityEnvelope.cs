using System.Text.Json.Serialization;

namespace UniFi.Mobility.Client.Models;

/// <summary>Envelope for a single-item Mobility response. Internal plumbing; resources unwrap Data.</summary>
internal sealed class MobilitySingleEnvelope<T>
{
    [JsonPropertyName("data")]
    public T? Data { get; init; }

    [JsonPropertyName("httpStatusCode")]
    public int HttpStatusCode { get; init; }

    [JsonPropertyName("traceId")]
    public string? TraceId { get; init; }
}

/// <summary>Envelope for a paginated Mobility collection response.</summary>
internal sealed class MobilityCollectionEnvelope<T>
{
    [JsonPropertyName("data")]
    public IReadOnlyList<T> Data { get; init; } = [];

    [JsonPropertyName("total")]
    public int Total { get; init; }

    [JsonPropertyName("offset")]
    public int Offset { get; init; }

    [JsonPropertyName("limit")]
    public int Limit { get; init; }

    [JsonPropertyName("httpStatusCode")]
    public int HttpStatusCode { get; init; }

    [JsonPropertyName("traceId")]
    public string? TraceId { get; init; }
}
