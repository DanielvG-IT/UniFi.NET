using UniFi.Mobility.Client.Http;
using UniFi.Mobility.Client.Models;

namespace UniFi.Mobility.Client.Resources;

/// <summary>Mobile routing devices (UMR) within a workspace, their clients, and configuration.</summary>
public sealed class DevicesResource
{
    private readonly ApiConnection _connection;

    internal DevicesResource(ApiConnection connection) => _connection = connection;

    /// <summary>List devices in a workspace.</summary>
    /// <param name="workspaceId">Workspace id.</param>
    /// <param name="limit">Page size (1-200, default 200).</param>
    /// <param name="offset">Number of records to skip.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    public Task<MobilityPage<DeviceSummary>> ListAsync(
        string workspaceId,
        int? limit = null,
        int? offset = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(workspaceId);
        return _connection.GetPagedAsync<DeviceSummary>(
            $"v1/mobility/workspaces/{Uri.EscapeDataString(workspaceId)}/devices",
            limit, offset, cancellationToken);
    }

    /// <summary>Get full detail for a single device.</summary>
    public Task<DeviceDetail?> GetAsync(string workspaceId, string deviceId, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(workspaceId);
        ArgumentException.ThrowIfNullOrWhiteSpace(deviceId);
        return _connection.GetAsync<DeviceDetail>(
            $"v1/mobility/workspaces/{Uri.EscapeDataString(workspaceId)}/devices/{Uri.EscapeDataString(deviceId)}",
            cancellationToken);
    }

    /// <summary>List the clients connected to a device.</summary>
    public Task<MobilityPage<DeviceClient>> ListClientsAsync(
        string workspaceId,
        string deviceId,
        int? limit = null,
        int? offset = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(workspaceId);
        ArgumentException.ThrowIfNullOrWhiteSpace(deviceId);
        return _connection.GetPagedAsync<DeviceClient>(
            $"v1/mobility/workspaces/{Uri.EscapeDataString(workspaceId)}/devices/{Uri.EscapeDataString(deviceId)}/clients",
            limit, offset, cancellationToken);
    }

    /// <summary>Rename a device. Requires an API key with write:mobility scope.</summary>
    public Task UpdateNameAsync(string workspaceId, string deviceId, UpdateDeviceNameRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(workspaceId);
        ArgumentException.ThrowIfNullOrWhiteSpace(deviceId);
        ArgumentNullException.ThrowIfNull(request);
        return _connection.PutAsync(
            $"v1/mobility/workspaces/{Uri.EscapeDataString(workspaceId)}/devices/{Uri.EscapeDataString(deviceId)}",
            request, cancellationToken);
    }

    /// <summary>Update a device's LAN / DHCP settings (partial). Requires write:mobility scope.</summary>
    public Task UpdateNetworkAsync(string workspaceId, string deviceId, UpdateNetworkRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(workspaceId);
        ArgumentException.ThrowIfNullOrWhiteSpace(deviceId);
        ArgumentNullException.ThrowIfNull(request);
        return _connection.PutAsync(
            $"v1/mobility/workspaces/{Uri.EscapeDataString(workspaceId)}/devices/{Uri.EscapeDataString(deviceId)}/network",
            request, cancellationToken);
    }

    /// <summary>Update a device's WiFi settings. Requires write:mobility scope.</summary>
    public Task UpdateWirelessAsync(string workspaceId, string deviceId, UpdateWirelessRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(workspaceId);
        ArgumentException.ThrowIfNullOrWhiteSpace(deviceId);
        ArgumentNullException.ThrowIfNull(request);
        return _connection.PutAsync(
            $"v1/mobility/workspaces/{Uri.EscapeDataString(workspaceId)}/devices/{Uri.EscapeDataString(deviceId)}/wireless",
            request, cancellationToken);
    }
}
