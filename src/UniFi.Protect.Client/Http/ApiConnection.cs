using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace UniFi.Protect.Client.Http;

/// <summary>
/// Owns the HttpClient for a UniFi Protect target and centralizes auth, JSON handling,
/// and error translation. Resource classes are thin wrappers around this.
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

    /// <summary>Base address requests resolve against; ends in a trailing slash.</summary>
    public Uri BaseAddress { get; }

    /// <summary>API key sent as the X-API-KEY header.</summary>
    public string ApiKey { get; }

    /// <summary>Whether TLS certificate validation is skipped (used by WebSocket subscriptions too).</summary>
    public bool AllowUntrustedCertificate { get; }

    /// <summary>Pinned certificate thumbprint, if any (used by WebSocket subscriptions too).</summary>
    public string? PinnedCertificateSha256 { get; }

    public ApiConnection(ProtectClientOptions options)
    {
        BaseAddress = options.BaseAddress;
        ApiKey = options.ApiKey;
        AllowUntrustedCertificate = options.AllowUntrustedCertificate;
        PinnedCertificateSha256 = options.PinnedCertificateSha256;
        _httpClient = BuildHttpClient(options);
        _ownsHttpClient = true;
        _jsonOptions = CreateJsonOptions();
    }

    /// <summary>
    /// Use a caller-supplied <see cref="HttpClient"/> (e.g. from IHttpClientFactory). The client is
    /// not disposed by this instance; only its base address and auth header are set if not present.
    /// Note: TLS pinning/validation is the caller's responsibility on an injected client.
    /// </summary>
    public ApiConnection(ProtectClientOptions options, HttpClient httpClient)
    {
        ArgumentNullException.ThrowIfNull(httpClient);
        BaseAddress = options.BaseAddress;
        ApiKey = options.ApiKey;
        AllowUntrustedCertificate = options.AllowUntrustedCertificate;
        PinnedCertificateSha256 = options.PinnedCertificateSha256;
        ConfigureClient(httpClient, options);
        _httpClient = httpClient;
        _ownsHttpClient = false;
        _jsonOptions = CreateJsonOptions();
    }

    /// <summary>Test seam: bring your own configured HttpClient (e.g. with a mocked handler).</summary>
    public ApiConnection(HttpClient httpClient, string apiKey = "test")
    {
        _httpClient = httpClient;
        _ownsHttpClient = true;
        BaseAddress = httpClient.BaseAddress ?? new Uri("https://localhost/");
        ApiKey = apiKey;
        AllowUntrustedCertificate = false;
        PinnedCertificateSha256 = null;
        _jsonOptions = CreateJsonOptions();
    }

    private static JsonSerializerOptions CreateJsonOptions() => new()
    {
        PropertyNameCaseInsensitive = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    };

    private static HttpClient BuildHttpClient(ProtectClientOptions options)
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

    private static void ConfigureClient(HttpClient httpClient, ProtectClientOptions options)
    {
        httpClient.BaseAddress ??= options.BaseAddress;
        if (!httpClient.DefaultRequestHeaders.Contains("X-API-KEY"))
        {
            httpClient.DefaultRequestHeaders.Add("X-API-KEY", options.ApiKey);
        }
    }

    public Task<T> GetAsync<T>(string relativePath, IReadOnlyDictionary<string, string?>? query = null, CancellationToken cancellationToken = default)
        => SendAsync<T>(HttpMethod.Get, relativePath, query, body: null, cancellationToken);

    public Task<T> PostAsync<T>(string relativePath, object? body = null, CancellationToken cancellationToken = default)
        => SendAsync<T>(HttpMethod.Post, relativePath, query: null, body, cancellationToken);

    public Task PostAsync(string relativePath, object? body = null, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Post, relativePath, query: null, body, cancellationToken);

    public Task<T> PatchAsync<T>(string relativePath, object? body, CancellationToken cancellationToken = default)
        => SendAsync<T>(HttpMethod.Patch, relativePath, query: null, body, cancellationToken);

    public Task PatchAsync(string relativePath, object? body, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Patch, relativePath, query: null, body, cancellationToken);

    public Task DeleteAsync(string relativePath, IReadOnlyDictionary<string, string?>? query = null, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Delete, relativePath, query, body: null, cancellationToken);

    /// <summary>GET a binary payload (e.g. a camera snapshot).</summary>
    public async Task<byte[]> GetBytesAsync(string relativePath, IReadOnlyDictionary<string, string?>? query = null, CancellationToken cancellationToken = default)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, BuildRelativeUri(relativePath, query));
        using var response = await _httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);
        if (!response.IsSuccessStatusCode)
        {
            await ThrowApiExceptionAsync(response, cancellationToken).ConfigureAwait(false);
        }
        return await response.Content.ReadAsByteArrayAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <summary>POST a multipart/form-data file upload.</summary>
    public async Task<T> PostFileAsync<T>(
        string relativePath,
        Stream content,
        string fileName,
        string contentType,
        CancellationToken cancellationToken = default)
    {
        using var form = new MultipartFormDataContent();
        var fileContent = new StreamContent(content);
        fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(contentType);
        form.Add(fileContent, "file", fileName);

        using var request = new HttpRequestMessage(HttpMethod.Post, relativePath) { Content = form };
        using var response = await _httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);
        if (!response.IsSuccessStatusCode)
        {
            await ThrowApiExceptionAsync(response, cancellationToken).ConfigureAwait(false);
        }
        var result = await response.Content.ReadFromJsonAsync<T>(_jsonOptions, cancellationToken).ConfigureAwait(false);
        return result ?? throw new UniFiProtectException(response.StatusCode, "Response body was empty or null.", string.Empty);
    }

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
        return result ?? throw new UniFiProtectException(response.StatusCode, "Response body was empty or null.", string.Empty);
    }

    private async Task SendAsync(
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
    }

    private HttpRequestMessage CreateRequest(HttpMethod method, string relativePath, object? body)
    {
        var request = new HttpRequestMessage(method, relativePath);
        if (body is not null)
        {
            request.Content = JsonContent.Create(body, body.GetType(), options: _jsonOptions);
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

    private async Task ThrowApiExceptionAsync(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        var body = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

        string? name = null;
        var message = $"UniFi Protect API request failed with status {(int)response.StatusCode} ({response.StatusCode}).";

        try
        {
            using var document = JsonDocument.Parse(body);
            var root = document.RootElement;
            if (root.TryGetProperty("error", out var errorElement) && errorElement.GetString() is { Length: > 0 } e) message = e;
            if (root.TryGetProperty("name", out var nameElement)) name = nameElement.GetString();
        }
        catch (JsonException)
        {
            // Body wasn't the documented error shape — fall back to the raw body.
        }

        var storedBody = body.Length > MaxErrorBodyChars
            ? string.Concat(body.AsSpan(0, MaxErrorBodyChars), "…[truncated]")
            : body;
        throw new UniFiProtectException(response.StatusCode, message, storedBody, name);
    }

    public void Dispose()
    {
        if (_ownsHttpClient)
        {
            _httpClient.Dispose();
        }
    }
}
