using System.Text.Json;
using UniFi.SiteManager.Client;
using UniFi.SiteManager.Client.Http;

// Configure via environment variable:
//   UNIFI_API_KEY=...   (generate at unifi.ui.com)

var apiKey = Environment.GetEnvironmentVariable("UNIFI_API_KEY");
if (string.IsNullOrWhiteSpace(apiKey))
{
    Console.Error.WriteLine("Set UNIFI_API_KEY (generate an API key at unifi.ui.com).");
    return 1;
}

using var client = new UniFiSiteManagerClient(apiKey);

try
{
    var hosts = await client.Hosts.ListAsync(pageSize: 10);
    Console.WriteLine($"Found {hosts.Data.Count} host(s) on this page:");
    foreach (var host in hosts.Data)
    {
        Console.WriteLine($"  - {host.Id} [{host.Type}] {host.IpAddress} (owner: {host.Owner})");
    }

    var sites = await client.Sites.ListAsync(pageSize: 10);
    Console.WriteLine($"Found {sites.Data.Count} site(s) on this page:");
    foreach (var site in sites.Data)
    {
        var name = site.Meta is { } meta && meta.TryGetProperty("name", out var nameEl)
            ? nameEl.GetString()
            : "(unnamed)";
        Console.WriteLine($"  - {name} (siteId: {site.SiteId}, permission: {site.Permission})");
    }

    var deviceGroups = await client.Devices.ListAsync(pageSize: 10);
    foreach (var group in deviceGroups.Data)
    {
        Console.WriteLine($"Host {group.HostName ?? group.HostId} manages {group.Devices.Count} device(s):");
        foreach (var device in group.Devices)
        {
            Console.WriteLine($"  - {device.Name} [{device.Model}] {device.Status} ({device.Ip})");
        }
    }

    var sdWanConfigs = await client.SdWanConfigs.ListAsync();
    Console.WriteLine($"Found {sdWanConfigs?.Count ?? 0} SD-WAN config(s).");

    return 0;
}
catch (UniFiSiteManagerException ex)
{
    Console.Error.WriteLine($"API request failed: {ex.StatusCode} {ex.Code} - {ex.Message} (traceId: {ex.TraceId})");
    return 1;
}
