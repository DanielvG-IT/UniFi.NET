using System.Text.Json.Serialization;

namespace UniFi.Network.Client.Models;

public sealed class VoucherCreationRequest
{
    /// <summary>Voucher note, duplicated across all generated vouchers.</summary>
    [JsonPropertyName("name")]
    public required string Name { get; init; }

    /// <summary>How long, in minutes, the voucher grants access for once the first guest authorizes.</summary>
    [JsonPropertyName("timeLimitMinutes")]
    public required long TimeLimitMinutes { get; init; }

    /// <summary>Number of vouchers to generate (1..1000). Defaults to 1.</summary>
    [JsonPropertyName("count")]
    public int? Count { get; init; }

    /// <summary>Optional limit for how many different guests can share this voucher.</summary>
    [JsonPropertyName("authorizedGuestLimit")]
    public long? AuthorizedGuestLimit { get; init; }

    /// <summary>Optional data usage limit in megabytes (1..1048576).</summary>
    [JsonPropertyName("dataUsageLimitMBytes")]
    public long? DataUsageLimitMBytes { get; init; }

    /// <summary>Optional download rate limit in kilobits per second (2..100000).</summary>
    [JsonPropertyName("rxRateLimitKbps")]
    public long? RxRateLimitKbps { get; init; }

    /// <summary>Optional upload rate limit in kilobits per second (2..100000).</summary>
    [JsonPropertyName("txRateLimitKbps")]
    public long? TxRateLimitKbps { get; init; }
}

public sealed class VoucherUsageDetails
{
    [JsonPropertyName("bytes")]
    public required long Bytes { get; init; }

    [JsonPropertyName("rxBytes")]
    public required long RxBytes { get; init; }

    [JsonPropertyName("txBytes")]
    public required long TxBytes { get; init; }

    [JsonPropertyName("durationSec")]
    public required long DurationSec { get; init; }
}

public sealed class VoucherDetails
{
    [JsonPropertyName("id")]
    public required Guid Id { get; init; }

    [JsonPropertyName("name")]
    public string? Name { get; init; }

    /// <summary>Secret code used to activate the voucher via the Hotspot portal.</summary>
    [JsonPropertyName("code")]
    public required string Code { get; init; }

    [JsonPropertyName("createdAt")]
    public required DateTimeOffset CreatedAt { get; init; }

    [JsonPropertyName("activatedAt")]
    public DateTimeOffset? ActivatedAt { get; init; }

    [JsonPropertyName("expiresAt")]
    public DateTimeOffset? ExpiresAt { get; init; }

    [JsonPropertyName("expired")]
    public required bool Expired { get; init; }

    [JsonPropertyName("authorizedGuestCount")]
    public required long AuthorizedGuestCount { get; init; }

    [JsonPropertyName("authorizedGuestLimit")]
    public long? AuthorizedGuestLimit { get; init; }

    [JsonPropertyName("dataUsageLimitMBytes")]
    public long? DataUsageLimitMBytes { get; init; }

    [JsonPropertyName("rxRateLimitKbps")]
    public long? RxRateLimitKbps { get; init; }

    [JsonPropertyName("txRateLimitKbps")]
    public long? TxRateLimitKbps { get; init; }

    [JsonPropertyName("usage")]
    public VoucherUsageDetails? Usage { get; init; }
}

public sealed class VoucherCreationResult
{
    [JsonPropertyName("vouchers")]
    public IReadOnlyList<VoucherDetails> Vouchers { get; init; } = [];
}

public sealed class VoucherDeletionResult
{
    [JsonPropertyName("vouchersDeleted")]
    public long VouchersDeleted { get; init; }
}
