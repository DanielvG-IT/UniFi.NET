using UniFi.Mobility.Client;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Registers <see cref="UniFiMobilityClient"/> with the dependency-injection container, backed by
/// <c>IHttpClientFactory</c> for pooled, long-lived HTTP connections.
/// </summary>
public static class UniFiMobilityClientServiceCollectionExtensions
{
    private const string HttpClientName = "UniFi.Mobility.Client";

    /// <summary>Register <see cref="UniFiMobilityClient"/> as a typed <c>HttpClient</c> using the given options.</summary>
    public static IHttpClientBuilder AddUniFiMobilityClient(this IServiceCollection services, MobilityClientOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        return services.AddUniFiMobilityClient(_ => options);
    }

    /// <summary>Register <see cref="UniFiMobilityClient"/> from an API key with the <c>mobility</c> scope (cloud-only).</summary>
    public static IHttpClientBuilder AddUniFiMobilityClient(this IServiceCollection services, string apiKey)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(apiKey);
        return services.AddUniFiMobilityClient(MobilityClientOptions.Create(apiKey));
    }

    /// <summary>
    /// Register <see cref="UniFiMobilityClient"/> as a typed <c>HttpClient</c>, resolving its options
    /// from the service provider (e.g. from configuration).
    /// </summary>
    public static IHttpClientBuilder AddUniFiMobilityClient(
        this IServiceCollection services,
        Func<IServiceProvider, MobilityClientOptions> optionsFactory)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(optionsFactory);

        return services
            .AddHttpClient(HttpClientName)
            .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler { CheckCertificateRevocationList = true })
            .AddTypedClient<UniFiMobilityClient>((httpClient, sp) => new UniFiMobilityClient(optionsFactory(sp), httpClient));
    }
}
