using UniFi.Network.Client;
using UniFi.Network.Client.Http;

// Configure exactly one of these two targets, via environment variables:
//
// Local console:
//   UNIFI_CONSOLE_HOST=192.168.1.1
//   UNIFI_API_KEY=...
//
// Site Manager cloud connector:
//   UNIFI_CONSOLE_ID=...
//   UNIFI_API_KEY=...

var apiKey = Environment.GetEnvironmentVariable("UNIFI_API_KEY");
if (string.IsNullOrWhiteSpace(apiKey))
{
    Console.Error.WriteLine("Set UNIFI_API_KEY.");
    return 1;
}

var consoleHost = Environment.GetEnvironmentVariable("UNIFI_CONSOLE_HOST");
var consoleId = Environment.GetEnvironmentVariable("UNIFI_CONSOLE_ID");

UniFiClientOptions options;
if (!string.IsNullOrWhiteSpace(consoleHost))
{
    Console.WriteLine($"Connecting to local console at {consoleHost}...");
    options = UniFiClientOptions.ForLocalConsole(consoleHost, apiKey);
}
else if (!string.IsNullOrWhiteSpace(consoleId))
{
    Console.WriteLine($"Connecting via Site Manager cloud connector to console {consoleId}...");
    options = UniFiClientOptions.ForCloudConnector(consoleId, apiKey);
}
else
{
    Console.Error.WriteLine("Set either UNIFI_CONSOLE_HOST (local) or UNIFI_CONSOLE_ID (cloud connector).");
    return 1;
}

using var client = new UniFiNetworkClient(options);

try
{
    var info = await client.GetApplicationInfoAsync();
    Console.WriteLine($"Network application version: {info.ApplicationVersion}");

    var sites = await client.Sites.ListAsync();
    Console.WriteLine($"Found {sites.TotalCount} site(s):");

    foreach (var site in sites.Data)
    {
        Console.WriteLine($"  - {site.Name} ({site.Id})");

        var devices = await client.Devices.ListAsync(site.Id, limit: 10);
        Console.WriteLine($"    {devices.TotalCount} adopted device(s), showing up to 10:");
        foreach (var device in devices.Data)
        {
            Console.WriteLine($"      - {device.Name} [{device.Model}] {device.State} ({device.IpAddress})");
        }

        var clients = await client.Clients.ListAsync(site.Id, limit: 10);
        Console.WriteLine($"    {clients.TotalCount} connected client(s), showing up to 10:");
        foreach (var overviewClient in clients.Data)
        {
            Console.WriteLine($"      - {overviewClient.Name} ({overviewClient.GetType().Name}) {overviewClient.IpAddress}");
        }
    }

    return 0;
}
catch (UniFiApiException ex)
{
    Console.Error.WriteLine($"API request failed: {ex.StatusCode} {ex.Code} - {ex.Message}");
    return 1;
}
