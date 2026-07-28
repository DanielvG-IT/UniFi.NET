using UniFi.Network.Client.Http;
using UniFi.Network.Client.Models;

namespace UniFi.Network.Client.Resources;

public sealed class DevicesResource
{
    private readonly ApiConnection _connection;

    internal DevicesResource(ApiConnection connection) => _connection = connection;

    /// <summary>List devices seen on the network but not yet adopted into any site.</summary>
    public Task<PagedResult<DevicePendingAdoption>> ListPendingAdoptionAsync(
        int? offset = null,
        int? limit = null,
        string? filter = null,
        CancellationToken cancellationToken = default)
        => _connection.GetPagedAsync<DevicePendingAdoption>("v1/pending-devices", offset, limit, filter, cancellationToken);

    /// <summary>List devices already adopted into the given site.</summary>
    public Task<PagedResult<AdoptedDeviceOverview>> ListAsync(
        Guid siteId,
        int? offset = null,
        int? limit = null,
        string? filter = null,
        CancellationToken cancellationToken = default)
        => _connection.GetPagedAsync<AdoptedDeviceOverview>($"v1/sites/{siteId}/devices", offset, limit, filter, cancellationToken);

    public Task<AdoptedDeviceDetails> GetAsync(Guid siteId, Guid deviceId, CancellationToken cancellationToken = default)
        => _connection.GetAsync<AdoptedDeviceDetails>($"v1/sites/{siteId}/devices/{deviceId}", cancellationToken: cancellationToken);

    /// <summary>Adopt a device (identified by MAC address) that is pending adoption into this site.</summary>
    public Task AdoptAsync(Guid siteId, DeviceAdoptionRequest request, CancellationToken cancellationToken = default)
        => _connection.PostAsync($"v1/sites/{siteId}/devices", request, cancellationToken);

    /// <summary>Unadopt a device from the site. If the device is online it is reset to factory defaults.</summary>
    public Task RemoveAsync(Guid siteId, Guid deviceId, CancellationToken cancellationToken = default)
        => _connection.DeleteAsync($"v1/sites/{siteId}/devices/{deviceId}", cancellationToken);

    public Task RestartAsync(Guid siteId, Guid deviceId, CancellationToken cancellationToken = default)
        => _connection.PostAsync($"v1/sites/{siteId}/devices/{deviceId}/actions", DeviceActionRequest.Restart(), cancellationToken);

    public Task PowerCyclePortAsync(Guid siteId, Guid deviceId, int portIdx, CancellationToken cancellationToken = default)
        => _connection.PostAsync($"v1/sites/{siteId}/devices/{deviceId}/interfaces/ports/{portIdx}/actions", PortActionRequest.PowerCycle(), cancellationToken);

    public Task<DeviceStatistics> GetLatestStatisticsAsync(Guid siteId, Guid deviceId, CancellationToken cancellationToken = default)
        => _connection.GetAsync<DeviceStatistics>($"v1/sites/{siteId}/devices/{deviceId}/statistics/latest", cancellationToken: cancellationToken);
}
