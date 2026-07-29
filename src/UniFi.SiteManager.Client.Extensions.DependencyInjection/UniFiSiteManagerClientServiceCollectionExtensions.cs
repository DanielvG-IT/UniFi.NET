using UniFi.SiteManager.Client;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Registers <see cref="UniFiSiteManagerClient"/> with the dependency-injection container, backed by
/// <c>IHttpClientFactory</c> for pooled, long-lived HTTP connections.
/// </summary>
public static class UniFiSiteManagerClientServiceCollectionExtensions
{
    private const string HttpClientName = "UniFi.SiteManager.Client";

    /// <summary>Register <see cref="UniFiSiteManagerClient"/> as a typed <c>HttpClient</c> using the given options.</summary>
    public static IHttpClientBuilder AddUniFiSiteManagerClient(this IServiceCollection services, SiteManagerClientOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        return services.AddUniFiSiteManagerClient(_ => options);
    }

    /// <summary>Register <see cref="UniFiSiteManagerClient"/> from an API key (Site Manager is cloud-only).</summary>
    public static IHttpClientBuilder AddUniFiSiteManagerClient(this IServiceCollection services, string apiKey)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(apiKey);
        return services.AddUniFiSiteManagerClient(SiteManagerClientOptions.Create(apiKey));
    }

    /// <summary>
    /// Register <see cref="UniFiSiteManagerClient"/> as a typed <c>HttpClient</c>, resolving its
    /// options from the service provider (e.g. from configuration).
    /// </summary>
    public static IHttpClientBuilder AddUniFiSiteManagerClient(
        this IServiceCollection services,
        Func<IServiceProvider, SiteManagerClientOptions> optionsFactory)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(optionsFactory);

        return services
            .AddHttpClient(HttpClientName)
            .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler { CheckCertificateRevocationList = true })
            .AddTypedClient<UniFiSiteManagerClient>((httpClient, sp) => new UniFiSiteManagerClient(optionsFactory(sp), httpClient));
    }
}
