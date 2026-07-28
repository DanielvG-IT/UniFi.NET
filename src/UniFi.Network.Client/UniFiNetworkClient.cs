using UniFi.Network.Client.Http;
using UniFi.Network.Client.Models;
using UniFi.Network.Client.Resources;

namespace UniFi.Network.Client;

/// <summary>
/// Entry point for the UniFi Network integration API. Construct with
/// <see cref="UniFiClientOptions.ForLocalConsole"/> or <see cref="UniFiClientOptions.ForCloudConnector"/>.
/// </summary>
public sealed class UniFiNetworkClient : IDisposable
{
    private readonly ApiConnection _connection;

    public UniFiNetworkClient(UniFiClientOptions options)
    {
        _connection = new ApiConnection(options);

        Sites = new SitesResource(_connection);
        Devices = new DevicesResource(_connection);
        Clients = new ClientsResource(_connection);
        Networks = new NetworksResource(_connection);
        WifiBroadcasts = new WifiBroadcastsResource(_connection);
        Vouchers = new VouchersResource(_connection);
        FirewallPolicies = new FirewallPoliciesResource(_connection);
        FirewallZones = new FirewallZonesResource(_connection);
        AclRules = new AclRulesResource(_connection);
        DnsPolicies = new DnsPoliciesResource(_connection);
        TrafficMatchingLists = new TrafficMatchingListsResource(_connection);
        ReferenceData = new ReferenceDataResource(_connection);
        DeviceTags = new DeviceTagsResource(_connection);
        RadiusProfiles = new RadiusProfilesResource(_connection);
        Wans = new WansResource(_connection);
        Vpn = new VpnResource(_connection);
        Switching = new SwitchingResource(_connection);
    }

    public SitesResource Sites { get; }
    public DevicesResource Devices { get; }
    public ClientsResource Clients { get; }
    public NetworksResource Networks { get; }
    public WifiBroadcastsResource WifiBroadcasts { get; }
    public VouchersResource Vouchers { get; }
    public FirewallPoliciesResource FirewallPolicies { get; }
    public FirewallZonesResource FirewallZones { get; }
    public AclRulesResource AclRules { get; }
    public DnsPoliciesResource DnsPolicies { get; }
    public TrafficMatchingListsResource TrafficMatchingLists { get; }
    public ReferenceDataResource ReferenceData { get; }
    public DeviceTagsResource DeviceTags { get; }
    public RadiusProfilesResource RadiusProfiles { get; }
    public WansResource Wans { get; }
    public VpnResource Vpn { get; }
    public SwitchingResource Switching { get; }

    /// <summary>Basic info about the Network application, including its version.</summary>
    public Task<ApplicationInfo> GetApplicationInfoAsync(CancellationToken cancellationToken = default)
        => _connection.GetAsync<ApplicationInfo>("v1/info", cancellationToken: cancellationToken);

    public void Dispose() => _connection.Dispose();
}
