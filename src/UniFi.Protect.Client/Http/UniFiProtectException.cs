using System.Net;

namespace UniFi.Protect.Client.Http;

/// <summary>
/// Thrown when the UniFi Protect API returns a non-success status code. The API reports
/// errors as a <c>{ error, name, cause }</c> object.
/// </summary>
public sealed class UniFiProtectException : Exception
{
    public HttpStatusCode StatusCode { get; }

    /// <summary>Machine-readable error name, e.g. "API_ERROR".</summary>
    public string? Name { get; }

    /// <summary>Raw response body, kept for cases the typed fields above don't cover.</summary>
    public string ResponseBody { get; }

    public UniFiProtectException(
        HttpStatusCode statusCode,
        string message,
        string responseBody,
        string? name = null)
        : base(message)
    {
        StatusCode = statusCode;
        ResponseBody = responseBody;
        Name = name;
    }
}
