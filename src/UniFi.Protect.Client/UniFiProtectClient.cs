using UniFi.Protect.Client.Http;
using UniFi.Protect.Client.Resources;

namespace UniFi.Protect.Client;

/// <summary>
/// Entry point for the UniFi Protect integration API. Construct with
/// <see cref="ProtectClientOptions.ForLocalConsole"/> or <see cref="ProtectClientOptions.ForCloudConnector"/>.
/// </summary>
public sealed class UniFiProtectClient : IDisposable
{
    private readonly ApiConnection _connection;

    public UniFiProtectClient(ProtectClientOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        _connection = new ApiConnection(options);

        Meta = new MetaResource(_connection);
        Cameras = new CamerasResource(_connection);
        Lights = new LightsResource(_connection);
        Sensors = new SensorsResource(_connection);
        Chimes = new ChimesResource(_connection);
        Viewers = new ViewersResource(_connection);
        Bridges = new BridgesResource(_connection);
        Fobs = new FobsResource(_connection);
        Nvrs = new NvrsResource(_connection);
        Speakers = new SpeakersResource(_connection);
        Sirens = new SirensResource(_connection);
        Relays = new RelaysResource(_connection);
        LinkStations = new LinkStationsResource(_connection);
        AlarmHubs = new AlarmHubsResource(_connection);
        LiveViews = new LiveViewsResource(_connection);
        ArmProfiles = new ArmProfilesResource(_connection);
        Users = new UsersResource(_connection);
        UlpUsers = new UlpUsersResource(_connection);
        Files = new FilesResource(_connection);
        AlarmManager = new AlarmManagerResource(_connection);
        Subscriptions = new SubscriptionsResource(_connection);
    }

    /// <summary>Application information (version).</summary>
    public MetaResource Meta { get; }
    public CamerasResource Cameras { get; }
    public LightsResource Lights { get; }
    public SensorsResource Sensors { get; }
    public ChimesResource Chimes { get; }
    public ViewersResource Viewers { get; }
    public BridgesResource Bridges { get; }
    public FobsResource Fobs { get; }
    public NvrsResource Nvrs { get; }
    public SpeakersResource Speakers { get; }
    public SirensResource Sirens { get; }
    public RelaysResource Relays { get; }
    public LinkStationsResource LinkStations { get; }
    public AlarmHubsResource AlarmHubs { get; }
    public LiveViewsResource LiveViews { get; }
    public ArmProfilesResource ArmProfiles { get; }
    public UsersResource Users { get; }
    public UlpUsersResource UlpUsers { get; }
    public FilesResource Files { get; }
    public AlarmManagerResource AlarmManager { get; }

    /// <summary>Real-time device and event subscriptions over WebSocket.</summary>
    public SubscriptionsResource Subscriptions { get; }

    public void Dispose() => _connection.Dispose();
}
