using System.Net;

namespace UniFi.Network.Client.Http;

/// <summary>
/// Thrown when the UniFi Network API returns a non-success status code.
/// </summary>
public sealed class UniFiApiException : Exception
{
    public HttpStatusCode StatusCode { get; }

    /// <summary>Machine-readable error code, e.g. "api.authentication.missing-credentials".</summary>
    public string? Code { get; }

    /// <summary>Server-assigned request id, useful when reporting 500s.</summary>
    public Guid? RequestId { get; }

    public string? RequestPath { get; }

    /// <summary>Raw response body, kept for cases the typed fields above don't cover.</summary>
    public string ResponseBody { get; }

    public UniFiApiException(
        HttpStatusCode statusCode,
        string message,
        string responseBody,
        string? code = null,
        Guid? requestId = null,
        string? requestPath = null)
        : base(message)
    {
        StatusCode = statusCode;
        ResponseBody = responseBody;
        Code = code;
        RequestId = requestId;
        RequestPath = requestPath;
    }
}
