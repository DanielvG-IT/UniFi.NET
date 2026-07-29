using System.Net;

namespace UniFi.Mobility.Client.Http;

/// <summary>
/// Thrown when the UniFi Mobility API returns a non-success status code. The API reports errors
/// as a <c>{ code, httpStatusCode, message, traceId }</c> object.
/// </summary>
public sealed class UniFiMobilityException : Exception
{
    public HttpStatusCode StatusCode { get; }

    /// <summary>Machine-readable error code, e.g. "unauthorized", "forbidden", "rate_limit", "upstream_error".</summary>
    public string? Code { get; }

    /// <summary>Server-assigned trace identifier (echoes X-Request-ID when supplied).</summary>
    public string? TraceId { get; }

    /// <summary>Raw response body, kept for cases the typed fields above don't cover.</summary>
    public string ResponseBody { get; }

    public UniFiMobilityException(
        HttpStatusCode statusCode,
        string message,
        string responseBody,
        string? code = null,
        string? traceId = null)
        : base(message)
    {
        StatusCode = statusCode;
        ResponseBody = responseBody;
        Code = code;
        TraceId = traceId;
    }
}
