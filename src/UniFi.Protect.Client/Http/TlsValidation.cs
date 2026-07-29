using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace UniFi.Protect.Client.Http;

/// <summary>
/// Builds TLS server-certificate validation callbacks. Prefers OS/default validation, supports
/// certificate pinning (SHA-256 thumbprint), and only falls back to trusting an untrusted
/// certificate when the caller has explicitly opted in.
/// </summary>
internal static class TlsValidation
{
    /// <summary>
    /// Returns a validation callback, or <c>null</c> when the platform's default validation should
    /// be used (the most secure option: pin not set and untrusted certs not allowed).
    /// </summary>
    public static Func<HttpRequestMessage, X509Certificate2?, X509Chain?, SslPolicyErrors, bool>? CreateHandlerCallback(
        string? pinnedCertificateSha256,
        bool allowUntrustedCertificate)
    {
        if (pinnedCertificateSha256 is null && !allowUntrustedCertificate)
        {
            return null;
        }

        var pin = Normalize(pinnedCertificateSha256);
        return (_, cert, _, errors) => IsAcceptable(cert, errors, pin, allowUntrustedCertificate);
    }

    /// <summary>Validation callback for a <see cref="System.Net.WebSockets.ClientWebSocket"/>.</summary>
    public static RemoteCertificateValidationCallback? CreateWebSocketCallback(
        string? pinnedCertificateSha256,
        bool allowUntrustedCertificate)
    {
        if (pinnedCertificateSha256 is null && !allowUntrustedCertificate)
        {
            return null;
        }

        var pin = Normalize(pinnedCertificateSha256);
        return (_, cert, _, errors) => IsAcceptable(cert, errors, pin, allowUntrustedCertificate);
    }

    private static bool IsAcceptable(X509Certificate? cert, SslPolicyErrors errors, string? normalizedPin, bool allowUntrusted)
    {
        if (normalizedPin is not null)
        {
            return MatchesPin(cert, normalizedPin);
        }

        return errors == SslPolicyErrors.None || allowUntrusted;
    }

    private static bool MatchesPin(X509Certificate? cert, string normalizedPin)
    {
        if (cert is null)
        {
            return false;
        }

        var actual = Normalize(cert.GetCertHashString(HashAlgorithmName.SHA256));
        return actual is not null
            && CryptographicOperations.FixedTimeEquals(
                System.Text.Encoding.ASCII.GetBytes(actual),
                System.Text.Encoding.ASCII.GetBytes(normalizedPin));
    }

    private static string? Normalize(string? thumbprint)
        => thumbprint?.Replace(":", "").Replace(" ", "").Trim().ToUpperInvariant();
}
