using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using UniFi.SiteManager.Client.Models;

namespace UniFi.SiteManager.Client.Http;

/// <summary>
/// Owns the HttpClient for the Site Manager API and centralizes auth, JSON handling,
/// envelope unwrapping, pagination, and error translation. Resource classes are thin wrappers.
/// </summary>
internal sealed class ApiConnection : IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions;

    public ApiConnection(SiteManagerClientOptions options)
        : this(BuildHttpClient(options))
    {
    }

    /// <summary>Test/advanced seam: bring your own configured HttpClient (e.g. with a mocked handler).</summary>
    public ApiConnection(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        };
    }

    private static HttpClient BuildHttpClient(SiteManagerClientOptions options)
    {
        var httpClient = new HttpClient
        {
            BaseAddress = options.BaseAddress,
        };
        httpClient.DefaultRequestHeaders.Add("X-API-Key", options.ApiKey);
        httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        return httpClient;
    }

    /// <summary>GET an endpoint whose envelope wraps a single object, returning the unwrapped payload.</summary>
    public async Task<T?> GetAsync<T>(
        string relativePath,
        IEnumerable<KeyValuePair<string, string?>>? query = null,
        CancellationToken cancellationToken = default)
    {
        var envelope = await SendAsync<SiteManagerEnvelope<T>>(HttpMethod.Get, relativePath, query, body: null, cancellationToken)
            .ConfigureAwait(false);
        return envelope.Data;
    }

    /// <summary>GET a list endpoint, returning the page of items and the next-page cursor.</summary>
    public async Task<SiteManagerPage<T>> GetPagedAsync<T>(
        string relativePath,
        int? pageSize,
        string? nextToken,
        IEnumerable<KeyValuePair<string, string?>>? extraQuery = null,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string?>>();
        if (extraQuery is not null) query.AddRange(extraQuery);
        if (pageSize is not null) query.Add(new("pageSize", pageSize.Value.ToString(System.Globalization.CultureInfo.InvariantCulture)));
        if (!string.IsNullOrWhiteSpace(nextToken)) query.Add(new("nextToken", nextToken));

        var envelope = await SendAsync<SiteManagerEnvelope<IReadOnlyList<T>>>(HttpMethod.Get, relativePath, query, body: null, cancellationToken)
            .ConfigureAwait(false);
        return new SiteManagerPage<T>(envelope.Data ?? [], envelope.NextToken);
    }

    /// <summary>POST a body to an endpoint whose envelope wraps a single object, returning the unwrapped payload.</summary>
    public async Task<T?> PostAsync<T>(string relativePath, object? body, CancellationToken cancellationToken = default)
    {
        var envelope = await SendAsync<SiteManagerEnvelope<T>>(HttpMethod.Post, relativePath, query: null, body, cancellationToken)
            .ConfigureAwait(false);
        return envelope.Data;
    }

    private async Task<T> SendAsync<T>(
        HttpMethod method,
        string relativePath,
        IEnumerable<KeyValuePair<string, string?>>? query,
        object? body,
        CancellationToken cancellationToken)
    {
        using var request = new HttpRequestMessage(method, BuildRelativeUri(relativePath, query));
        if (body is not null)
        {
            request.Content = JsonContent.Create(body, options: _jsonOptions);
        }

        using var response = await _httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
        {
            await ThrowApiExceptionAsync(response, cancellationToken).ConfigureAwait(false);
        }

        var result = await response.Content.ReadFromJsonAsync<T>(_jsonOptions, cancellationToken).ConfigureAwait(false);
        return result ?? throw new UniFiSiteManagerException(response.StatusCode, "Response body was empty or null.", string.Empty);
    }

    private static string BuildRelativeUri(string relativePath, IEnumerable<KeyValuePair<string, string?>>? query)
    {
        if (query is null)
        {
            return relativePath;
        }

        var builder = new StringBuilder(relativePath);
        var first = true;
        foreach (var (key, value) in query)
        {
            if (value is null) continue;
            builder.Append(first ? '?' : '&');
            builder.Append(Uri.EscapeDataString(key)).Append('=').Append(Uri.EscapeDataString(value));
            first = false;
        }
        return builder.ToString();
    }

    private async Task ThrowApiExceptionAsync(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        var body = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

        string? code = null;
        string? traceId = null;
        var message = $"UniFi Site Manager API request failed with status {(int)response.StatusCode} ({response.StatusCode}).";

        try
        {
            using var document = JsonDocument.Parse(body);
            var root = document.RootElement;
            if (root.TryGetProperty("message", out var messageElement) && messageElement.GetString() is { Length: > 0 } m) message = m;
            if (root.TryGetProperty("code", out var codeElement)) code = codeElement.ValueKind == JsonValueKind.String ? codeElement.GetString() : codeElement.ToString();
            if (root.TryGetProperty("traceId", out var traceIdElement)) traceId = traceIdElement.GetString();
        }
        catch (JsonException)
        {
            // Body wasn't JSON (e.g. an HTML gateway error) — fall back to the raw body.
        }

        throw new UniFiSiteManagerException(response.StatusCode, message, body, code, traceId);
    }

    public void Dispose() => _httpClient.Dispose();
}
