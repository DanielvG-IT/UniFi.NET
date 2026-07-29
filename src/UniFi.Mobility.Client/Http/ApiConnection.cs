using System.Globalization;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using UniFi.Mobility.Client.Models;

namespace UniFi.Mobility.Client.Http;

/// <summary>
/// Owns the HttpClient for the Mobility API and centralizes auth, JSON handling, envelope
/// unwrapping, pagination, and error translation. Resource classes are thin wrappers.
/// </summary>
internal sealed class ApiConnection : IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions;

    public ApiConnection(MobilityClientOptions options)
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

    private static HttpClient BuildHttpClient(MobilityClientOptions options)
    {
        var httpClient = new HttpClient { BaseAddress = options.BaseAddress };
        httpClient.DefaultRequestHeaders.Add("X-API-Key", options.ApiKey);
        httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        return httpClient;
    }

    /// <summary>GET a single-item endpoint, returning the unwrapped payload.</summary>
    public async Task<T?> GetAsync<T>(string relativePath, CancellationToken cancellationToken = default)
    {
        var envelope = await SendAsync<MobilitySingleEnvelope<T>>(HttpMethod.Get, relativePath, body: null, cancellationToken)
            .ConfigureAwait(false);
        return envelope.Data;
    }

    /// <summary>GET a paginated collection endpoint.</summary>
    public async Task<MobilityPage<T>> GetPagedAsync<T>(
        string relativePath,
        int? limit,
        int? offset,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string>>();
        if (limit is not null) query.Add(new("limit", limit.Value.ToString(CultureInfo.InvariantCulture)));
        if (offset is not null) query.Add(new("offset", offset.Value.ToString(CultureInfo.InvariantCulture)));

        var envelope = await SendAsync<MobilityCollectionEnvelope<T>>(HttpMethod.Get, BuildUri(relativePath, query), body: null, cancellationToken)
            .ConfigureAwait(false);
        return new MobilityPage<T>(envelope.Data, envelope.Total, envelope.Offset, envelope.Limit);
    }

    /// <summary>PUT a body to an endpoint that returns 204 No Content.</summary>
    public Task PutAsync(string relativePath, object body, CancellationToken cancellationToken = default)
        => SendAsync(HttpMethod.Put, relativePath, body, cancellationToken);

    private async Task<T> SendAsync<T>(HttpMethod method, string relativePath, object? body, CancellationToken cancellationToken)
    {
        using var request = CreateRequest(method, relativePath, body);
        using var response = await _httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
        {
            await ThrowApiExceptionAsync(response, cancellationToken).ConfigureAwait(false);
        }

        var result = await response.Content.ReadFromJsonAsync<T>(_jsonOptions, cancellationToken).ConfigureAwait(false);
        return result ?? throw new UniFiMobilityException(response.StatusCode, "Response body was empty or null.", string.Empty);
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
            request.Content = JsonContent.Create(body, body.GetType(), options: _jsonOptions);
        }
        return request;
    }

    private static string BuildUri(string relativePath, IReadOnlyList<KeyValuePair<string, string>> query)
    {
        if (query.Count == 0)
        {
            return relativePath;
        }

        var builder = new StringBuilder(relativePath).Append('?');
        for (var i = 0; i < query.Count; i++)
        {
            if (i > 0) builder.Append('&');
            builder.Append(Uri.EscapeDataString(query[i].Key)).Append('=').Append(Uri.EscapeDataString(query[i].Value));
        }
        return builder.ToString();
    }

    private async Task ThrowApiExceptionAsync(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        var body = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

        string? code = null;
        string? traceId = null;
        var message = $"UniFi Mobility API request failed with status {(int)response.StatusCode} ({response.StatusCode}).";

        try
        {
            using var document = JsonDocument.Parse(body);
            var root = document.RootElement;
            if (root.TryGetProperty("message", out var messageElement) && messageElement.GetString() is { Length: > 0 } m) message = m;
            if (root.TryGetProperty("code", out var codeElement)) code = codeElement.GetString();
            if (root.TryGetProperty("traceId", out var traceIdElement)) traceId = traceIdElement.GetString();
        }
        catch (JsonException)
        {
            // Body wasn't the documented error shape — fall back to the raw body.
        }

        throw new UniFiMobilityException(response.StatusCode, message, body, code, traceId);
    }

    public void Dispose() => _httpClient.Dispose();
}
