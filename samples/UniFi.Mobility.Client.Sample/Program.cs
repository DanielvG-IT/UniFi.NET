using UniFi.Mobility.Client;
using UniFi.Mobility.Client.Http;

// Configure via environment variable:
//   UNIFI_API_KEY=...   (generate at unifi.ui.com, with the `mobility` scope)

var apiKey = Environment.GetEnvironmentVariable("UNIFI_API_KEY");
if (string.IsNullOrWhiteSpace(apiKey))
{
    Console.Error.WriteLine("Set UNIFI_API_KEY (generate an API key with the 'mobility' scope at unifi.ui.com).");
    return 1;
}

using var client = new UniFiMobilityClient(apiKey);

try
{
    var workspaces = await client.Workspaces.ListAsync();
    Console.WriteLine($"Found {workspaces.Total} workspace(s):");

    foreach (var workspace in workspaces.Data)
    {
        Console.WriteLine($"  - {workspace.WorkspaceName} ({workspace.WorkspaceId}) [{workspace.Status}]");

        var devices = await client.Devices.ListAsync(workspace.WorkspaceId, limit: 10);
        Console.WriteLine($"    {devices.Total} device(s), showing up to 10:");
        foreach (var device in devices.Data)
        {
            Console.WriteLine($"      - {device.Name} [{device.Model}] {device.State} (fw {device.FirmwareVersion})");

            var detail = await client.Devices.GetAsync(workspace.WorkspaceId, device.Id);
            if (detail is not null)
            {
                Console.WriteLine($"          WAN {detail.WanSource} via {detail.Isp}, {detail.ClientCount} client(s)");
            }
        }
    }

    return 0;
}
catch (UniFiMobilityException ex)
{
    Console.Error.WriteLine($"API request failed: {ex.StatusCode} {ex.Code} - {ex.Message} (traceId: {ex.TraceId})");
    return 1;
}
