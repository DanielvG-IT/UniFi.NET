namespace UniFi.Mobility.Client;

/// <summary>
/// Describes how to reach the UniFi Mobility API. The API is served from the UniFi cloud at
/// <c>https://api.ui.com</c>; you only need an API key (with <c>mobility</c> scope) from unifi.ui.com.
/// </summary>
public sealed class MobilityClientOptions
{
    /// <summary>Default base address for the UniFi Mobility API.</summary>
    public static readonly Uri DefaultBaseAddress = new("https://api.ui.com/");

    private MobilityClientOptions(Uri baseAddress, string apiKey)
    {
        BaseAddress = baseAddress;
        ApiKey = apiKey;
    }

    /// <summary>Base address requests are resolved against; always ends in a trailing slash.</summary>
    public Uri BaseAddress { get; }

    public string ApiKey { get; }

    /// <summary>
    /// Create options for the public Mobility cloud API.
    /// </summary>
    /// <param name="apiKey">API key with <c>mobility</c> scope, generated at unifi.ui.com.</param>
    public static MobilityClientOptions Create(string apiKey)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(apiKey);
        return new MobilityClientOptions(DefaultBaseAddress, apiKey);
    }

    /// <summary>
    /// Create options against a custom base address, e.g. for testing or a proxy.
    /// </summary>
    /// <param name="baseAddress">Base address; a trailing slash is added if missing.</param>
    /// <param name="apiKey">API key with <c>mobility</c> scope.</param>
    public static MobilityClientOptions Create(Uri baseAddress, string apiKey)
    {
        ArgumentNullException.ThrowIfNull(baseAddress);
        ArgumentException.ThrowIfNullOrWhiteSpace(apiKey);

        var normalized = baseAddress.AbsoluteUri.EndsWith('/')
            ? baseAddress
            : new Uri(baseAddress.AbsoluteUri + "/");
        return new MobilityClientOptions(normalized, apiKey);
    }
}
