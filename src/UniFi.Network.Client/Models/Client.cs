using System.Text.Json;
using System.Text.Json.Serialization;

namespace UniFi.Network.Client.Models;

/// <summary>
/// The type of network access and/or authorization a client currently has. Wired and wireless
/// clients can be DEFAULT or GUEST; VPN and Teleport clients are always DEFAULT today.
/// </summary>
[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(DefaultClientAccessOverview), "DEFAULT")]
[JsonDerivedType(typeof(GuestClientAccessOverview), "GUEST")]
public abstract class ClientAccessOverview
{
}

public sealed class DefaultClientAccessOverview : ClientAccessOverview
{
}

public sealed class GuestClientAccessOverview : ClientAccessOverview
{
    [JsonPropertyName("authorized")]
    public required bool Authorized { get; init; }
}

/// <summary>
/// A client connected to the network: wired, wireless, VPN, or Teleport. Use pattern matching
/// (<c>is WiredClientOverview</c>, <c>is WirelessClientOverview</c>, ...) to access type-specific fields.
/// </summary>
[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(WiredClientOverview), "WIRED")]
[JsonDerivedType(typeof(WirelessClientOverview), "WIRELESS")]
[JsonDerivedType(typeof(VpnClientOverview), "VPN")]
[JsonDerivedType(typeof(TeleportClientOverview), "TELEPORT")]
public abstract class ClientOverview
{
    [JsonPropertyName("id")]
    public required Guid Id { get; init; }

    [JsonPropertyName("name")]
    public required string Name { get; init; }

    [JsonPropertyName("ipAddress")]
    public string? IpAddress { get; init; }

    [JsonPropertyName("connectedAt")]
    public DateTimeOffset? ConnectedAt { get; init; }

    [JsonPropertyName("access")]
    public required ClientAccessOverview Access { get; init; }
}

public sealed class WiredClientOverview : ClientOverview
{
    [JsonPropertyName("macAddress")]
    public required string MacAddress { get; init; }

    [JsonPropertyName("uplinkDeviceId")]
    public required Guid UplinkDeviceId { get; init; }
}

public sealed class WirelessClientOverview : ClientOverview
{
    [JsonPropertyName("macAddress")]
    public required string MacAddress { get; init; }

    [JsonPropertyName("uplinkDeviceId")]
    public required Guid UplinkDeviceId { get; init; }
}

public sealed class VpnClientOverview : ClientOverview
{
}

public sealed class TeleportClientOverview : ClientOverview
{
}

/// <summary>
/// Detailed client info. The API's "access" shape varies by client type beyond what's
/// broadly documented, so it's exposed as raw JSON here rather than a narrow typed model.
/// </summary>
public sealed class ClientDetails
{
    [JsonPropertyName("id")]
    public required Guid Id { get; init; }

    [JsonPropertyName("name")]
    public required string Name { get; init; }

    [JsonPropertyName("type")]
    public required string Type { get; init; }

    [JsonPropertyName("ipAddress")]
    public string? IpAddress { get; init; }

    [JsonPropertyName("connectedAt")]
    public DateTimeOffset? ConnectedAt { get; init; }

    [JsonPropertyName("access")]
    public JsonElement Access { get; init; }

    [JsonExtensionData]
    public Dictionary<string, JsonElement>? AdditionalProperties { get; init; }
}

/// <summary>Authorizes network access for a guest client, replacing any existing active authorization.</summary>
public sealed class GuestAccessAuthorizationRequest
{
    [JsonPropertyName("action")]
    public string Action { get; init; } = "AUTHORIZE_GUEST_ACCESS";

    /// <summary>Optional data usage limit in megabytes (1..1048576).</summary>
    [JsonPropertyName("dataUsageLimitMBytes")]
    public long? DataUsageLimitMBytes { get; init; }

    /// <summary>Optional download rate limit in kilobits per second (2..100000).</summary>
    [JsonPropertyName("rxRateLimitKbps")]
    public long? RxRateLimitKbps { get; init; }

    /// <summary>Optional upload rate limit in kilobits per second (2..100000).</summary>
    [JsonPropertyName("txRateLimitKbps")]
    public long? TxRateLimitKbps { get; init; }

    /// <summary>Optional authorization duration in minutes; falls back to the site's default limit.</summary>
    [JsonPropertyName("timeLimitMinutes")]
    public long? TimeLimitMinutes { get; init; }
}

/// <summary>Revokes network access and disconnects a guest client.</summary>
public sealed class GuestAccessUnauthorizationRequest
{
    [JsonPropertyName("action")]
    public string Action { get; init; } = "UNAUTHORIZE_GUEST_ACCESS";
}

[JsonConverter(typeof(JsonStringEnumConverter<GuestAuthorizationMethod>))]
public enum GuestAuthorizationMethod
{
    [JsonStringEnumMemberName("VOUCHER")]
    Voucher,

    [JsonStringEnumMemberName("API")]
    Api,

    [JsonStringEnumMemberName("OTHER")]
    Other,
}

public sealed class GuestAuthorizationDetails
{
    [JsonPropertyName("authorizationMethod")]
    public required GuestAuthorizationMethod AuthorizationMethod { get; init; }

    [JsonPropertyName("authorizedAt")]
    public required DateTimeOffset AuthorizedAt { get; init; }

    [JsonPropertyName("expiresAt")]
    public required DateTimeOffset ExpiresAt { get; init; }

    [JsonPropertyName("dataUsageLimitMBytes")]
    public long? DataUsageLimitMBytes { get; init; }

    [JsonPropertyName("rxRateLimitKbps")]
    public long? RxRateLimitKbps { get; init; }

    [JsonPropertyName("txRateLimitKbps")]
    public long? TxRateLimitKbps { get; init; }

    [JsonPropertyName("usage")]
    public JsonElement Usage { get; init; }
}

public sealed class GuestAccessAuthorizationResponse
{
    [JsonPropertyName("action")]
    public required string Action { get; init; }

    [JsonPropertyName("grantedAuthorization")]
    public required GuestAuthorizationDetails GrantedAuthorization { get; init; }

    [JsonPropertyName("revokedAuthorization")]
    public GuestAuthorizationDetails? RevokedAuthorization { get; init; }
}

public sealed class GuestAccessUnauthorizationResponse
{
    [JsonPropertyName("action")]
    public required string Action { get; init; }

    [JsonPropertyName("revokedAuthorization")]
    public required GuestAuthorizationDetails RevokedAuthorization { get; init; }
}
