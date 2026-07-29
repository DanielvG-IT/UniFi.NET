using UniFi.Mobility.Client.Http;
using UniFi.Mobility.Client.Models;

namespace UniFi.Mobility.Client.Resources;

/// <summary>Workspaces (mobility cloud sites) and their admins.</summary>
public sealed class WorkspacesResource
{
    private readonly ApiConnection _connection;

    internal WorkspacesResource(ApiConnection connection) => _connection = connection;

    /// <summary>List workspaces visible to the authenticated user.</summary>
    /// <param name="limit">Page size (1-200, default 200).</param>
    /// <param name="offset">Number of records to skip.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    public Task<MobilityPage<WorkspaceSummary>> ListAsync(
        int? limit = null,
        int? offset = null,
        CancellationToken cancellationToken = default)
        => _connection.GetPagedAsync<WorkspaceSummary>("v1/mobility/workspaces", limit, offset, cancellationToken);

    /// <summary>List the admins of a workspace (mobility permissions only).</summary>
    public Task<MobilityPage<AdminSummary>> ListAdminsAsync(string workspaceId, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(workspaceId);
        return _connection.GetPagedAsync<AdminSummary>(
            $"v1/mobility/workspaces/{Uri.EscapeDataString(workspaceId)}/admins",
            limit: null,
            offset: null,
            cancellationToken);
    }
}
