using UniFi.Network.Client.Http;
using UniFi.Network.Client.Models;

namespace UniFi.Network.Client.Resources;

public sealed class VouchersResource
{
    private readonly ApiConnection _connection;

    internal VouchersResource(ApiConnection connection) => _connection = connection;

    public Task<PagedResult<VoucherDetails>> ListAsync(
        Guid siteId,
        int? offset = null,
        int? limit = null,
        string? filter = null,
        CancellationToken cancellationToken = default)
        => _connection.GetPagedAsync<VoucherDetails>($"v1/sites/{siteId}/hotspot/vouchers", offset, limit, filter, cancellationToken);

    public Task<VoucherDetails> GetAsync(Guid siteId, Guid voucherId, CancellationToken cancellationToken = default)
        => _connection.GetAsync<VoucherDetails>($"v1/sites/{siteId}/hotspot/vouchers/{voucherId}", cancellationToken: cancellationToken);

    public Task<VoucherCreationResult> CreateAsync(Guid siteId, VoucherCreationRequest request, CancellationToken cancellationToken = default)
        => _connection.PostAsync<VoucherCreationResult>($"v1/sites/{siteId}/hotspot/vouchers", request, cancellationToken);

    public Task DeleteAsync(Guid siteId, Guid voucherId, CancellationToken cancellationToken = default)
        => _connection.DeleteAsync($"v1/sites/{siteId}/hotspot/vouchers/{voucherId}", cancellationToken);

    /// <summary>Remove every voucher matching the given filter, e.g. <c>expired.eq(true)</c>.</summary>
    public Task<VoucherDeletionResult> DeleteAllAsync(Guid siteId, string filter, CancellationToken cancellationToken = default)
        => _connection.DeleteAsync<VoucherDeletionResult>(
            $"v1/sites/{siteId}/hotspot/vouchers",
            new Dictionary<string, string?> { ["filter"] = filter },
            cancellationToken);
}
