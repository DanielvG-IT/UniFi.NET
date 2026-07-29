using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using UniFi.Network.Client;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Registers <see cref="UniFiNetworkClient"/> with the dependency-injection container, backed by
/// <c>IHttpClientFactory</c> for pooled, long-lived HTTP connections.
/// </summary>
public static class UniFiNetworkClientServiceCollectionExtensions
{
    private const string HttpClientName = "UniFi.Network.Client";

    /// <summary>
    /// Register <see cref="UniFiNetworkClient"/> as a typed <c>HttpClient</c> using the given options.
    /// TLS certificate pinning / trust settings from the options are applied to the primary handler.
    /// </summary>
    public static IHttpClientBuilder AddUniFiNetworkClient(this IServiceCollection services, UniFiClientOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        return services.AddUniFiNetworkClient(_ => options);
    }

    /// <summary>
    /// Register <see cref="UniFiNetworkClient"/> as a typed <c>HttpClient</c>, resolving its options
    /// from the service provider (e.g. from configuration or another registered service).
    /// </summary>
    public static IHttpClientBuilder AddUniFiNetworkClient(
        this IServiceCollection services,
        Func<IServiceProvider, UniFiClientOptions> optionsFactory)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(optionsFactory);

        return services
            .AddHttpClient(HttpClientName)
            .ConfigurePrimaryHttpMessageHandler(sp =>
            {
                var options = optionsFactory(sp);
                var handler = new HttpClientHandler { CheckCertificateRevocationList = true };
                var callback = CreateCertificateValidationCallback(options.PinnedCertificateSha256, options.AllowUntrustedCertificate);
                if (callback is not null)
                {
                    handler.ServerCertificateCustomValidationCallback = callback;
                }
                return handler;
            })
            .AddTypedClient<UniFiNetworkClient>((httpClient, sp) => new UniFiNetworkClient(optionsFactory(sp), httpClient));
    }

    // Mirrors the client's own TLS handling using only the public options, so this package works
    // against the already-published core assembly without relying on its internals.
    private static Func<HttpRequestMessage, X509Certificate2?, X509Chain?, SslPolicyErrors, bool>? CreateCertificateValidationCallback(
        string? pinnedCertificateSha256,
        bool allowUntrustedCertificate)
    {
        if (pinnedCertificateSha256 is null && !allowUntrustedCertificate)
        {
            return null; // platform default validation (most secure)
        }

        var pin = pinnedCertificateSha256?.Replace(":", "").Replace(" ", "").Trim().ToUpperInvariant();
        return (_, cert, _, errors) =>
        {
            if (pin is not null)
            {
                if (cert is null)
                {
                    return false;
                }

                var actual = cert.GetCertHashString(HashAlgorithmName.SHA256);
                return CryptographicOperations.FixedTimeEquals(
                    System.Text.Encoding.ASCII.GetBytes(actual),
                    System.Text.Encoding.ASCII.GetBytes(pin));
            }

            return errors == SslPolicyErrors.None || allowUntrustedCertificate;
        };
    }
}
