using UniFi.Protect.Client;
using UniFi.Protect.Client.Http;

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

ProtectClientOptions options;
if (!string.IsNullOrWhiteSpace(consoleHost))
{
    Console.WriteLine($"Connecting to local Protect console at {consoleHost}...");
    options = ProtectClientOptions.ForLocalConsole(consoleHost, apiKey);
}
else if (!string.IsNullOrWhiteSpace(consoleId))
{
    Console.WriteLine($"Connecting via Site Manager cloud connector to console {consoleId}...");
    options = ProtectClientOptions.ForCloudConnector(consoleId, apiKey);
}
else
{
    Console.Error.WriteLine("Set either UNIFI_CONSOLE_HOST (local) or UNIFI_CONSOLE_ID (cloud connector).");
    return 1;
}

using var client = new UniFiProtectClient(options);

try
{
    var info = await client.Meta.GetInfoAsync();
    Console.WriteLine($"Protect application version: {info.ApplicationVersion}");

    var cameras = await client.Cameras.ListAsync();
    Console.WriteLine($"Found {cameras.Count} camera(s):");
    foreach (var camera in cameras)
    {
        Console.WriteLine($"  - {camera.Name} [{camera.ModelKey}] {camera.State} (mac {camera.Mac})");
    }

    var lights = await client.Lights.ListAsync();
    Console.WriteLine($"Found {lights.Count} light(s), {lights.Count(l => l.IsLightOn == true)} on.");

    var sensors = await client.Sensors.ListAsync();
    Console.WriteLine($"Found {sensors.Count} sensor(s).");

    Console.WriteLine("Listening for device updates for 10 seconds (Ctrl+C to stop)...");
    using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
    try
    {
        await foreach (var message in client.Subscriptions.SubscribeToDevicesAsync(cts.Token))
        {
            Console.WriteLine($"  update: {message.GetRawText()}");
        }
    }
    catch (OperationCanceledException)
    {
        // expected when the 10s window elapses
    }

    return 0;
}
catch (UniFiProtectException ex)
{
    Console.Error.WriteLine($"API request failed: {ex.StatusCode} {ex.Name} - {ex.Message}");
    return 1;
}
