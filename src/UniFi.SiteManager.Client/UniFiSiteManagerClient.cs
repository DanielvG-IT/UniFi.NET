using UniFi.SiteManager.Client.Http;
using UniFi.SiteManager.Client.Resources;

namespace UniFi.SiteManager.Client;

/// <summary>
/// Entry point for the UniFi Site Manager API (api.ui.com). Construct with an API key
/// generated at unifi.ui.com via <see cref="SiteManagerClientOptions.Create(string)"/>.
/// </summary>
public sealed class UniFiSiteManagerClient : IDisposable
{
    private readonly ApiConnection _connection;

    public UniFiSiteManagerClient(SiteManagerClientOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        _connection = new ApiConnection(options);

        Hosts = new HostsResource(_connection);
        Sites = new SitesResource(_connection);
        Devices = new DevicesResource(_connection);
        IspMetrics = new IspMetricsResource(_connection);
        SdWanConfigs = new SdWanConfigsResource(_connection);
    }

    /// <summary>
    /// Use a caller-supplied <see cref="HttpClient"/> (e.g. registered via IHttpClientFactory).
    /// The client is not disposed by this instance.
    /// </summary>
    public UniFiSiteManagerClient(SiteManagerClientOptions options, HttpClient httpClient)
    {
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(httpClient);
        _connection = new ApiConnection(options, httpClient);

        Hosts = new HostsResource(_connection);
        Sites = new SitesResource(_connection);
        Devices = new DevicesResource(_connection);
        IspMetrics = new IspMetricsResource(_connection);
        SdWanConfigs = new SdWanConfigsResource(_connection);
    }

    /// <summary>Convenience overload that builds options from just an API key.</summary>
    public UniFiSiteManagerClient(string apiKey)
        : this(SiteManagerClientOptions.Create(apiKey))
    {
    }

    /// <summary>Hosts (consoles and network servers) registered to your account.</summary>
    public HostsResource Hosts { get; }

    /// <summary>Sites managed across all of your hosts.</summary>
    public SitesResource Sites { get; }

    /// <summary>Devices managed across your hosts, grouped by host.</summary>
    public DevicesResource Devices { get; }

    /// <summary>ISP performance metrics for your sites' WANs.</summary>
    public IspMetricsResource IspMetrics { get; }

    /// <summary>SD-WAN configurations and their live status.</summary>
    public SdWanConfigsResource SdWanConfigs { get; }

    public void Dispose() => _connection.Dispose();
}
