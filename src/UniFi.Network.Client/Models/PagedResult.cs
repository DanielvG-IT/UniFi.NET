using System.Text.Json.Serialization;

namespace UniFi.Network.Client.Models;

/// <summary>
/// Envelope returned by every paginated list endpoint in the Network API.
/// </summary>
public sealed class PagedResult<T>
{
    [JsonPropertyName("data")]
    public IReadOnlyList<T> Data { get; init; } = [];

    [JsonPropertyName("count")]
    public int Count { get; init; }

    [JsonPropertyName("limit")]
    public int Limit { get; init; }

    [JsonPropertyName("offset")]
    public long Offset { get; init; }

    [JsonPropertyName("totalCount")]
    public long TotalCount { get; init; }

    /// <summary>True if calling the same endpoint with Offset + Count would return more data.</summary>
    public bool HasMore => Offset + Count < TotalCount;
}
