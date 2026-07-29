using System.Text.Json.Serialization;

namespace UniFi.Mobility.Client.Models;

/// <summary>A workspace (mobility cloud site) visible to the authenticated user.</summary>
public sealed class WorkspaceSummary
{
    [JsonPropertyName("workspace_id")]
    public required string WorkspaceId { get; init; }

    [JsonPropertyName("workspace_name")]
    public required string WorkspaceName { get; init; }

    [JsonPropertyName("is_owner")]
    public bool IsOwner { get; init; }

    [JsonPropertyName("status")]
    public MobilityStatus Status { get; init; }
}

/// <summary>An admin of a workspace, with only mobility permissions exposed.</summary>
public sealed class AdminSummary
{
    [JsonPropertyName("name")]
    public required string Name { get; init; }

    [JsonPropertyName("email")]
    public required string Email { get; init; }

    [JsonPropertyName("status")]
    public MobilityStatus Status { get; init; }

    [JsonPropertyName("is_owner")]
    public bool IsOwner { get; init; }

    /// <summary>Mobility permissions. Null when the admin has no role bindings (e.g. a pending invite).</summary>
    [JsonPropertyName("permissions")]
    public AdminPermission? Permissions { get; init; }
}

/// <summary>Mobility permission levels for an admin.</summary>
public sealed class AdminPermission
{
    /// <summary>Mobile Routing permission level. Omitted when the admin has no role binding.</summary>
    [JsonPropertyName("umr")]
    public MobileRoutingPermission? MobileRouting { get; init; }
}
