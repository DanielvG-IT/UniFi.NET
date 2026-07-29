namespace UniFi.SiteManager.Client;

/// <summary>
/// Describes how to reach the UniFi Site Manager API. The API is always served from the
/// UniFi cloud at <c>https://api.ui.com</c>; you only need an API key generated at unifi.ui.com.
/// </summary>
public sealed class SiteManagerClientOptions
{
    /// <summary>Default base address for the UniFi Site Manager API.</summary>
    public static readonly Uri DefaultBaseAddress = new("https://api.ui.com/");

    private SiteManagerClientOptions(Uri baseAddress, string apiKey)
    {
        BaseAddress = baseAddress;
        ApiKey = apiKey;
    }

    /// <summary>Base address requests are resolved against; always ends in a trailing slash.</summary>
    public Uri BaseAddress { get; }

    public string ApiKey { get; }

    /// <summary>
    /// Create options for the public Site Manager cloud API.
    /// </summary>
    /// <param name="apiKey">API key generated at unifi.ui.com.</param>
    public static SiteManagerClientOptions Create(string apiKey)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(apiKey);
        return new SiteManagerClientOptions(DefaultBaseAddress, apiKey);
    }

    /// <summary>
    /// Create options against a custom base address, e.g. for testing or a proxy.
    /// </summary>
    /// <param name="baseAddress">Base address; a trailing slash is added if missing.</param>
    /// <param name="apiKey">API key generated at unifi.ui.com.</param>
    public static SiteManagerClientOptions Create(Uri baseAddress, string apiKey)
    {
        ArgumentNullException.ThrowIfNull(baseAddress);
        ArgumentException.ThrowIfNullOrWhiteSpace(apiKey);

        var normalized = baseAddress.AbsoluteUri.EndsWith('/')
            ? baseAddress
            : new Uri(baseAddress.AbsoluteUri + "/");
        return new SiteManagerClientOptions(normalized, apiKey);
    }
}
