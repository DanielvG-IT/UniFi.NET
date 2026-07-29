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
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions;

    /// <summary>Base address requests resolve against; ends in a trailing slash.</summary>
    public Uri BaseAddress { get; }

    /// <summary>API key sent as the X-API-KEY header.</summary>
    public string ApiKey { get; }

    /// <summary>Whether TLS certificate validation is skipped (used by WebSocket subscriptions too).</summary>
    public bool AllowUntrustedCertificate { get; }

    public ApiConnection(ProtectClientOptions options)
    {
        BaseAddress = options.BaseAddress;
        ApiKey = options.ApiKey;
        AllowUntrustedCertificate = options.AllowUntrustedCertificate;
        _httpClient = BuildHttpClient(options);
        _jsonOptions = CreateJsonOptions();
    }

    /// <summary>Test/advanced seam: bring your own configured HttpClient (e.g. with a mocked handler).</summary>
    public ApiConnection(HttpClient httpClient, string apiKey = "test")
    {
        _httpClient = httpClient;
        BaseAddress = httpClient.BaseAddress ?? new Uri("https://localhost/");
        ApiKey = apiKey;
        AllowUntrustedCertificate = false;
        _jsonOptions = CreateJsonOptions();
    }

    private static JsonSerializerOptions CreateJsonOptions() => new()
    {
        PropertyNameCaseInsensitive = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    };

    private static HttpClient BuildHttpClient(ProtectClientOptions options)
    {
        var handler = new HttpClientHandler();
        if (options.AllowUntrustedCertificate)
        {
            handler.ServerCertificateCustomValidationCallback = (_, _, _, _) => true;
        }

        var httpClient = new HttpClient(handler)
        {
            BaseAddress = options.BaseAddress,
        };
        httpClient.DefaultRequestHeaders.Add("X-API-KEY", options.ApiKey);
        httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        return httpClient;
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

        throw new UniFiProtectException(response.StatusCode, message, body, name);
    }

    public void Dispose() => _httpClient.Dispose();
}
