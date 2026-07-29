using System.Globalization;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using UniFi.Network.Client.Models;

namespace UniFi.Network.Client.Http;

/// <summary>
/// Owns the HttpClient for a UniFi Network target and centralizes auth, JSON handling,
/// pagination, and error translation. Resource classes are thin wrappers around this.
/// </summary>
internal sealed class ApiConnection : IDisposable
{
    /// <summary>Upper bound on a buffered response body, to bound memory use from a hostile endpoint.</summary>
    private const int MaxResponseContentBytes = 32 * 1024 * 1024;

    /// <summary>Longest a stored error-response body may be, to avoid unbounded exception payloads.</summary>
    private const int MaxErrorBodyChars = 4096;

    private static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(100);

    private readonly HttpClient _httpClient;
    private readonly bool _ownsHttpClient;
    private readonly JsonSerializerOptions _jsonOptions;

    public ApiConnection(UniFiClientOptions options)
    {
        _httpClient = BuildHttpClient(options);
        _ownsHttpClient = true;
        _jsonOptions = CreateJsonOptions();
    }

    /// <summary>
    /// Use a caller-supplied <see cref="HttpClient"/> (e.g. from IHttpClientFactory). The client is
    /// not disposed by this instance; only its base address and auth header are set if not present.
    /// </summary>
    public ApiConnection(UniFiClientOptions options, HttpClient httpClient)
    {
        ArgumentNullException.ThrowIfNull(httpClient);
        ConfigureClient(httpClient, options);
        _httpClient = httpClient;
        _ownsHttpClient = false;
        _jsonOptions = CreateJsonOptions();
    }

    /// <summary>Test seam: bring your own configured HttpClient (e.g. with a mocked handler).</summary>
    public ApiConnection(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _ownsHttpClient = true;
        _jsonOptions = CreateJsonOptions();
    }

    private static JsonSerializerOptions CreateJsonOptions() => new()
    {
        PropertyNameCaseInsensitive = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    };

    private static HttpClient BuildHttpClient(UniFiClientOptions options)
    {
        var handler = new HttpClientHandler { CheckCertificateRevocationList = true };
        var callback = TlsValidation.CreateHandlerCallback(options.PinnedCertificateSha256, options.AllowUntrustedCertificate);
        if (callback is not null)
        {
            handler.ServerCertificateCustomValidationCallback = callback;
        }

        var httpClient = new HttpClient(handler)
        {
            BaseAddress = options.BaseAddress,
            Timeout = DefaultTimeout,
            MaxResponseContentBufferSize = MaxResponseContentBytes,
        };
        httpClient.DefaultRequestHeaders.Add("X-API-KEY", options.ApiKey);
        httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        return httpClient;
    }

    private static void ConfigureClient(HttpClient httpClient, UniFiClientOptions options)
    {
        httpClient.BaseAddress ??= options.BaseAddress;
        if (!httpClient.DefaultRequestHeaders.Contains("X-API-KEY"))
        {
            httpClient.DefaultRequestHeaders.Add("X-API-KEY", options.ApiKey);
        }
    }

    public Task<T> GetAsync<T>(string relativePath, IReadOnlyDictionary<string, string?>? query = null, CancellationToken cancellationToken = default)
        => SendAsync<T>(HttpMethod.Get, relativePath, query, body: null, cancellationToken);

    public async Task<PagedResult<T>> GetPagedAsync<T>(
        string relativePath,
        int? offset,
        int? limit,
        string? filter,
        CancellationToken cancellationToken = default)
    {
        var query = new Dictionary<string, string?>();
        if (offset is not null) query["offset"] = offset.Value.ToString(CultureInfo.InvariantCulture);
        if (limit is not null) query["limit"] = limit.Value.ToString(CultureInfo.InvariantCulture);
        if (!string.IsNullOrWhiteSpace(filter)) query["filter"] = filter;

        return await GetAsync<PagedResult<T>>(relativePath, query, cancellationToken).ConfigureAwait(false);
    }

    public Task<T> PostAsync<T>(string relativePath, object? body, CancellationToken cancellationToken = default)
        => SendAsync<T>(HttpMethod.Post, relativePath, query: null, body, cancellationToken);

    public Task PostAsync(string relativePath, object? body, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Post, relativePath, body, cancellationToken);

    public Task<T> PutAsync<T>(string relativePath, object? body, CancellationToken cancellationToken = default)
        => SendAsync<T>(HttpMethod.Put, relativePath, query: null, body, cancellationToken);

    public Task<T> PatchAsync<T>(string relativePath, object? body, CancellationToken cancellationToken = default)
        => SendAsync<T>(HttpMethod.Patch, relativePath, query: null, body, cancellationToken);

    public Task DeleteAsync(string relativePath, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Delete, relativePath, body: null, cancellationToken);

    public Task<T> DeleteAsync<T>(
        string relativePath,
        IReadOnlyDictionary<string, string?>? query = null,
        CancellationToken cancellationToken = default)
        => SendAsync<T>(HttpMethod.Delete, relativePath, query, body: null, cancellationToken);

    private async Task<T> SendAsync<T>(
        HttpMethod method,
        string relativePath,
        IReadOnlyDictionary<string, string?>? query,
        object? body,
        CancellationToken cancellationToken)
    {
        using var request = CreateRequest(method, BuildRelativeUri(relativePath, query), body);
        using var response = await _httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
        {
            await ThrowApiExceptionAsync(response, cancellationToken).ConfigureAwait(false);
        }

        var result = await response.Content.ReadFromJsonAsync<T>(_jsonOptions, cancellationToken).ConfigureAwait(false);
        return result ?? throw new UniFiApiException(response.StatusCode, "Response body was empty or null.", string.Empty);
    }

    private async Task SendAsync(HttpMethod method, string relativePath, object? body, CancellationToken cancellationToken)
    {
        using var request = CreateRequest(method, relativePath, body);
        using var response = await _httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
        {
            await ThrowApiExceptionAsync(response, cancellationToken).ConfigureAwait(false);
        }
    }

    private HttpRequestMessage CreateRequest(HttpMethod method, string relativePath, object? body)
    {
        var request = new HttpRequestMessage(method, relativePath);
        if (body is not null)
        {
            request.Content = JsonContent.Create(body, options: _jsonOptions);
        }
        return request;
    }

    private static string BuildRelativeUri(string relativePath, IReadOnlyDictionary<string, string?>? query)
    {
        if (query is null || query.Count == 0)
        {
            return relativePath;
        }

        var builder = new StringBuilder(relativePath).Append('?');
        var first = true;
        foreach (var (key, value) in query)
        {
            if (value is null) continue;
            if (!first) builder.Append('&');
            builder.Append(Uri.EscapeDataString(key)).Append('=').Append(Uri.EscapeDataString(value));
            first = false;
        }
        return builder.ToString();
    }

    private static async Task ThrowApiExceptionAsync(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        var body = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

        string? code = null;
        Guid? requestId = null;
        string? requestPath = null;
        var message = $"UniFi Network API request failed with status {(int)response.StatusCode} ({response.StatusCode}).";

        try
        {
            using var document = JsonDocument.Parse(body);
            var root = document.RootElement;
            if (root.TryGetProperty("code", out var codeElement)) code = codeElement.GetString();
            if (root.TryGetProperty("message", out var messageElement) && messageElement.GetString() is { Length: > 0 } m) message = m;
            if (root.TryGetProperty("requestId", out var requestIdElement) && requestIdElement.TryGetGuid(out var id)) requestId = id;
            if (root.TryGetProperty("requestPath", out var requestPathElement)) requestPath = requestPathElement.GetString();
        }
        catch (JsonException)
        {
            // Body wasn't the documented "Error Message" shape (e.g. an upstream proxy error) — fall back to the raw body.
        }

        var storedBody = body.Length > MaxErrorBodyChars
            ? string.Concat(body.AsSpan(0, MaxErrorBodyChars), "…[truncated]")
            : body;
        throw new UniFiApiException(response.StatusCode, message, storedBody, code, requestId, requestPath);
    }

    public void Dispose()
    {
        if (_ownsHttpClient)
        {
            _httpClient.Dispose();
        }
    }
}
