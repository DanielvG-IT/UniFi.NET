using System.Text.Json;
using System.Text.Json.Serialization;

namespace UniFi.Network.Client.Models;

[JsonConverter(typeof(JsonStringEnumConverter<EntityOrigin>))]
public enum EntityOrigin
{
    [JsonStringEnumMemberName("USER_DEFINED")]
    UserDefined,

    [JsonStringEnumMemberName("SYSTEM_DEFINED")]
    SystemDefined,

    [JsonStringEnumMemberName("ORCHESTRATED")]
    Orchestrated,

    [JsonStringEnumMemberName("DERIVED")]
    Derived,
}

public sealed class EntityMetadata
{
    [JsonPropertyName("origin")]
    public required EntityOrigin Origin { get; init; }
}

/// <summary>
/// A network configured on the site. <c>Management</c> tells you which derived type this is:
/// GATEWAY (routed by a gateway device), SWITCH (a switch-managed VLAN), or UNMANAGED.
/// </summary>
[JsonPolymorphic(TypeDiscriminatorPropertyName = "management")]
[JsonDerivedType(typeof(GatewayManagedNetworkOverview), "GATEWAY")]
[JsonDerivedType(typeof(SwitchManagedNetworkOverview), "SWITCH")]
[JsonDerivedType(typeof(UnmanagedNetworkOverview), "UNMANAGED")]
public abstract class NetworkOverview
{
    [JsonPropertyName("id")]
    public required Guid Id { get; init; }

    [JsonPropertyName("name")]
    public required string Name { get; init; }

    [JsonPropertyName("enabled")]
    public required bool Enabled { get; init; }

    [JsonPropertyName("default")]
    public required bool Default { get; init; }

    [JsonPropertyName("vlanId")]
    public required int VlanId { get; init; }

    [JsonPropertyName("metadata")]
    public required EntityMetadata Metadata { get; init; }
}

public sealed class GatewayManagedNetworkOverview : NetworkOverview
{
    [JsonPropertyName("zoneId")]
    public Guid? ZoneId { get; init; }
}

public sealed class SwitchManagedNetworkOverview : NetworkOverview
{
    /// <summary>Id of the switch this network is managed by.</summary>
    [JsonPropertyName("deviceId")]
    public required Guid DeviceId { get; init; }
}

public sealed class UnmanagedNetworkOverview : NetworkOverview
{
}

[JsonConverter(typeof(JsonStringEnumConverter<NetworkReferenceResourceType>))]
public enum NetworkReferenceResourceType
{
    [JsonStringEnumMemberName("CLIENT")]
    Client,

    [JsonStringEnumMemberName("DEVICE")]
    Device,

    [JsonStringEnumMemberName("STATIC_ROUTE")]
    StaticRoute,

    [JsonStringEnumMemberName("OSPF_ROUTE")]
    OspfRoute,

    [JsonStringEnumMemberName("NEXT_AI")]
    NextAi,

    [JsonStringEnumMemberName("WIFI")]
    Wifi,

    [JsonStringEnumMemberName("NAT_RULE")]
    NatRule,

    [JsonStringEnumMemberName("SD_WAN")]
    SdWan,
}

public sealed class NetworkReferenceDetail
{
    [JsonPropertyName("referenceId")]
    public required Guid ReferenceId { get; init; }
}

public sealed class NetworkReferenceResource
{
    [JsonPropertyName("resourceType")]
    public required NetworkReferenceResourceType ResourceType { get; init; }

    [JsonPropertyName("referenceCount")]
    public required int ReferenceCount { get; init; }

    /// <summary>Present only for resource types that have an API model defined.</summary>
    [JsonPropertyName("references")]
    public IReadOnlyList<NetworkReferenceDetail>? References { get; init; }
}

public sealed class NetworkReferences
{
    [JsonPropertyName("referenceResources")]
    public required IReadOnlyList<NetworkReferenceResource> ReferenceResources { get; init; }
}

/// <summary>
/// Full network configuration. GATEWAY/SWITCH managed networks carry deep, management-specific
/// DHCP/IPv4/IPv6 configuration beyond what's typed here — those fields land in
/// <see cref="AdditionalProperties"/> as raw JSON. Inspect <see cref="Management"/> and consult
/// the OpenAPI spec (Gateway/Switch managed network details) for their exact shape.
/// </summary>
public sealed class NetworkDetails
{
    [JsonPropertyName("id")]
    public required Guid Id { get; init; }

    [JsonPropertyName("name")]
    public required string Name { get; init; }

    [JsonPropertyName("management")]
    public required string Management { get; init; }

    [JsonPropertyName("enabled")]
    public required bool Enabled { get; init; }

    [JsonPropertyName("default")]
    public required bool Default { get; init; }

    [JsonPropertyName("vlanId")]
    public required int VlanId { get; init; }

    [JsonPropertyName("metadata")]
    public required EntityMetadata Metadata { get; init; }

    [JsonExtensionData]
    public Dictionary<string, JsonElement>? AdditionalProperties { get; init; }
}
