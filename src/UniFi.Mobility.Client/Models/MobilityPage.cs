namespace UniFi.Mobility.Client.Models;

/// <summary>
/// A page of results from a Mobility list endpoint. The API uses offset/limit pagination.
/// </summary>
public sealed class MobilityPage<T>
{
    public MobilityPage(IReadOnlyList<T> data, int total, int offset, int limit)
    {
        Data = data;
        Total = total;
        Offset = offset;
        Limit = limit;
    }

    /// <summary>The items on this page.</summary>
    public IReadOnlyList<T> Data { get; }

    /// <summary>Total number of records matching the query, ignoring pagination.</summary>
    public int Total { get; }

    /// <summary>Number of records skipped (echoes the requested offset).</summary>
    public int Offset { get; }

    /// <summary>Page size applied (echoes the requested limit).</summary>
    public int Limit { get; }

    /// <summary>True if requesting the next page (Offset + Data.Count) would return more records.</summary>
    public bool HasMore => Offset + Data.Count < Total;
}
