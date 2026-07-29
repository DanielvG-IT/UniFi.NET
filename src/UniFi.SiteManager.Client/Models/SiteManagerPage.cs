namespace UniFi.SiteManager.Client.Models;

/// <summary>
/// A single page of results from a Site Manager list endpoint. The API uses cursor-based
/// pagination: pass <see cref="NextToken"/> back to the same call to fetch the next page.
/// </summary>
public sealed class SiteManagerPage<T>
{
    public SiteManagerPage(IReadOnlyList<T> data, string? nextToken)
    {
        Data = data;
        NextToken = nextToken;
    }

    /// <summary>The items on this page.</summary>
    public IReadOnlyList<T> Data { get; }

    /// <summary>Cursor to pass as <c>nextToken</c> to fetch the next page, or null on the last page.</summary>
    public string? NextToken { get; }

    /// <summary>True when another page is available.</summary>
    public bool HasMore => !string.IsNullOrEmpty(NextToken);
}
