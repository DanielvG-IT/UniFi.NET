namespace UniFi.Network.Client;

/// <summary>
/// Describes how to reach a UniFi Network integration API: directly on a local console,
/// or remotely through the Site Manager cloud connector (api.ui.com).
/// </summary>
public sealed class UniFiClientOptions
{
    private UniFiClientOptions(Uri baseAddress, string apiKey, bool allowUntrustedCertificate, string? pinnedCertificateSha256)
    {
        BaseAddress = baseAddress;
        ApiKey = apiKey;
        AllowUntrustedCertificate = allowUntrustedCertificate;
        PinnedCertificateSha256 = pinnedCertificateSha256;
    }

    /// <summary>Base address requests are resolved against; always ends in a trailing slash.</summary>
    public Uri BaseAddress { get; }

    public string ApiKey { get; }

    /// <summary>
    /// When true, TLS certificate validation is skipped entirely. UniFi consoles serve a self-signed
    /// certificate by default, so this is on by default for local console targets. Prefer
    /// <see cref="PinnedCertificateSha256"/> instead: it also works with self-signed certs but,
    /// unlike this switch, still protects against man-in-the-middle attacks on the local network.
    /// </summary>
    public bool AllowUntrustedCertificate { get; }

    /// <summary>
    /// SHA-256 thumbprint (hex, colons optional) of the console's TLS certificate to pin. When set,
    /// only a certificate whose thumbprint matches is accepted — even if <see cref="AllowUntrustedCertificate"/>
    /// is true — which is the recommended way to secure a self-signed local console against MITM.
    /// </summary>
    public string? PinnedCertificateSha256 { get; }

    /// <summary>
    /// Talk directly to a console on the local network, e.g. a UDM or Cloud Gateway.
    /// </summary>
    /// <param name="consoleHost">Console hostname or IP address, without scheme or path.</param>
    /// <param name="apiKey">API key generated on the console or at unifi.ui.com.</param>
    /// <param name="allowUntrustedCertificate">
    /// Skip TLS certificate validation. Defaults to true because local consoles use a
    /// self-signed certificate; set to false if you've installed a trusted certificate.
    /// </param>
    /// <param name="pinnedCertificateSha256">
    /// Optional SHA-256 thumbprint of the console's certificate to pin. Recommended for local
    /// consoles: it secures the self-signed certificate against MITM without disabling validation.
    /// </param>
    public static UniFiClientOptions ForLocalConsole(
        string consoleHost,
        string apiKey,
        bool allowUntrustedCertificate = true,
        string? pinnedCertificateSha256 = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(consoleHost);
        ArgumentException.ThrowIfNullOrWhiteSpace(apiKey);

        var baseAddress = new Uri($"https://{consoleHost}/proxy/network/integration/");
        return new UniFiClientOptions(baseAddress, apiKey, allowUntrustedCertificate, pinnedCertificateSha256);
    }

    /// <summary>
    /// Reach a console remotely through the Site Manager cloud connector, without VPN.
    /// </summary>
    /// <param name="consoleId">The target console's id, as shown in Site Manager.</param>
    /// <param name="apiKey">API key generated at unifi.ui.com.</param>
    public static UniFiClientOptions ForCloudConnector(string consoleId, string apiKey)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(consoleId);
        ArgumentException.ThrowIfNullOrWhiteSpace(apiKey);

        var baseAddress = new Uri($"https://api.ui.com/v1/connector/consoles/{consoleId}/proxy/network/integration/");
        return new UniFiClientOptions(baseAddress, apiKey, allowUntrustedCertificate: false, pinnedCertificateSha256: null);
    }
}
