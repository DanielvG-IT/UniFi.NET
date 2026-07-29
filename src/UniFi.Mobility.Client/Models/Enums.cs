using System.Text.Json.Serialization;

namespace UniFi.Mobility.Client.Models;

/// <summary>Status of a workspace or admin membership.</summary>
[JsonConverter(typeof(JsonStringEnumConverter<MobilityStatus>))]
public enum MobilityStatus
{
    [JsonStringEnumMemberName("ACTIVE")] Active,
    [JsonStringEnumMemberName("PENDING")] Pending,
    [JsonStringEnumMemberName("INACTIVE")] Inactive,
    [JsonStringEnumMemberName("DECLINED")] Declined,
}

/// <summary>Mobile Routing (umr) permission level for a workspace admin.</summary>
[JsonConverter(typeof(JsonStringEnumConverter<MobileRoutingPermission>))]
public enum MobileRoutingPermission
{
    /// <summary>Can view and configure devices (admin).</summary>
    [JsonStringEnumMemberName("ALL")] All,

    /// <summary>Read-only access (viewer).</summary>
    [JsonStringEnumMemberName("VIEW_ONLY")] ViewOnly,

    /// <summary>No access.</summary>
    [JsonStringEnumMemberName("NONE")] None,
}

/// <summary>Hardware model of a mobile routing device.</summary>
[JsonConverter(typeof(JsonStringEnumConverter<DeviceModel>))]
public enum DeviceModel
{
    [JsonStringEnumMemberName("UMR")] Umr,
    [JsonStringEnumMemberName("UMR Industrial")] UmrIndustrial,
    [JsonStringEnumMemberName("UMR Ultra")] UmrUltra,
}

/// <summary>Current device lifecycle/connection state.</summary>
[JsonConverter(typeof(JsonStringEnumConverter<DeviceState>))]
public enum DeviceState
{
    [JsonStringEnumMemberName("CONNECTED")] Connected,
    [JsonStringEnumMemberName("DISCONNECTED")] Disconnected,
    [JsonStringEnumMemberName("ADOPTING")] Adopting,
    [JsonStringEnumMemberName("ADOPTING_TIMEOUT")] AdoptingTimeout,
    [JsonStringEnumMemberName("DOWNLOADING")] Downloading,
    [JsonStringEnumMemberName("UPGRADING")] Upgrading,
    [JsonStringEnumMemberName("RESTARTING")] Restarting,
    [JsonStringEnumMemberName("FACTORY_RESET")] FactoryReset,
    [JsonStringEnumMemberName("GETTING_READY")] GettingReady,
    [JsonStringEnumMemberName("RESTORING")] Restoring,

    /// <summary>Record exists but adoption has not started.</summary>
    [JsonStringEnumMemberName("NULL")] Null,
    [JsonStringEnumMemberName("DELETING")] Deleting,
}

/// <summary>Router operating mode.</summary>
[JsonConverter(typeof(JsonStringEnumConverter<DeviceMode>))]
public enum DeviceMode
{
    [JsonStringEnumMemberName("ROUTER")] Router,
    [JsonStringEnumMemberName("WANBRIDGE")] WanBridge,
    [JsonStringEnumMemberName("LTEPASS")] LtePass,
}

/// <summary>Subscription status (derived priority: FAILED > PENDING > ACTIVE > INACTIVE).</summary>
[JsonConverter(typeof(JsonStringEnumConverter<SubscriptionStatus>))]
public enum SubscriptionStatus
{
    [JsonStringEnumMemberName("ACTIVE")] Active,
    [JsonStringEnumMemberName("INACTIVE")] Inactive,
    [JsonStringEnumMemberName("PENDING")] Pending,
    [JsonStringEnumMemberName("FAILED")] Failed,
}

/// <summary>Whether a client connects over wire or WiFi.</summary>
[JsonConverter(typeof(JsonStringEnumConverter<ClientType>))]
public enum ClientType
{
    [JsonStringEnumMemberName("WIRED")] Wired,
    [JsonStringEnumMemberName("WIRELESS")] Wireless,
}

/// <summary>Client connection status.</summary>
[JsonConverter(typeof(JsonStringEnumConverter<ClientConnectionStatus>))]
public enum ClientConnectionStatus
{
    [JsonStringEnumMemberName("ONLINE")] Online,
    [JsonStringEnumMemberName("OFFLINE")] Offline,
    [JsonStringEnumMemberName("BLOCKED")] Blocked,
}

/// <summary>DHCP server mode used when updating device network settings.</summary>
[JsonConverter(typeof(JsonStringEnumConverter<DhcpMode>))]
public enum DhcpMode
{
    /// <summary>DHCP server enabled.</summary>
    [JsonStringEnumMemberName("dhcp")] Dhcp,

    /// <summary>DHCP server disabled.</summary>
    [JsonStringEnumMemberName("none")] None,
}
