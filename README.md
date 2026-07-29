# UniFi.NET

A .NET client for the [UniFi Network API](https://developer.ui.com/network/) (v10.4.57), targeting
either a local console directly or the Site Manager cloud connector — no VPN required.

It also includes a client for the [UniFi Site Manager API](https://developer.ui.com/site-manager/)
(`api.ui.com`) — the account-wide cloud API for listing hosts, sites, devices, ISP metrics, and
SD-WAN configs across all your consoles. See [Site Manager API](#site-manager-api) below.

It also includes a client for the [UniFi Protect API](https://developer.ui.com/protect/) — cameras,
sensors, lights, and the rest of the Protect device fleet, over both local console and cloud
connector. See [Protect API](#protect-api) below.

This repo is meant to grow into a family of UniFi .NET clients (`UniFi.Network.Client`,
`UniFi.SiteManager.Client`, and `UniFi.Protect.Client` today, `UniFi.Access.Client` potentially
later), so each product's client lives under its own namespace and project.

## Install

```bash
dotnet add package UniFi.Network.Client
dotnet add package UniFi.SiteManager.Client
dotnet add package UniFi.Protect.Client
```

(Not published yet — reference the projects directly for now, e.g. `dotnet add reference src/UniFi.Network.Client/UniFi.Network.Client.csproj`, `src/UniFi.SiteManager.Client/UniFi.SiteManager.Client.csproj`, or `src/UniFi.Protect.Client/UniFi.Protect.Client.csproj`.)

## Usage

Generate an API key at [unifi.ui.com](https://unifi.ui.com) or on your console.

### Local console

Talk directly to a console on your LAN (e.g. a UDM or Cloud Gateway). Local consoles serve a
self-signed certificate, so TLS validation is skipped by default.

```csharp
using UniFi.Network.Client;

var options = UniFiClientOptions.ForLocalConsole("192.168.1.1", apiKey);
using var client = new UniFiNetworkClient(options);

var sites = await client.Sites.ListAsync();
foreach (var site in sites.Data)
{
    var devices = await client.Devices.ListAsync(site.Id);
    foreach (var device in devices.Data)
    {
        Console.WriteLine($"{device.Name} [{device.Model}] {device.State}");
    }
}
```

### Site Manager cloud connector

Reach a console remotely through `api.ui.com`, without VPN. You'll need the console's id, as
shown in Site Manager.

```csharp
var options = UniFiClientOptions.ForCloudConnector(consoleId, apiKey);
using var client = new UniFiNetworkClient(options);
```

### Pagination

List endpoints return a `PagedResult<T>`: `Data`, `Count`, `Limit`, `Offset`, `TotalCount`, and a
`HasMore` helper.

```csharp
var clients = await client.Clients.ListAsync(siteId, offset: 0, limit: 200);
while (true)
{
    foreach (var c in clients.Data) { /* ... */ }
    if (!clients.HasMore) break;
    clients = await client.Clients.ListAsync(siteId, offset: (int)(clients.Offset + clients.Count), limit: 200);
}
```

Most list endpoints also accept a `filter` expression per their documented filterable properties,
e.g. `client.Devices.ListAsync(siteId, filter: "state eq 'ONLINE'")`.

### Polymorphic resources

A handful of resources are typed discriminated unions using `System.Text.Json` polymorphism —
pattern-match to get at type-specific fields:

```csharp
var clients = await client.Clients.ListAsync(siteId);
foreach (var c in clients.Data)
{
    if (c is WiredClientOverview wired)
        Console.WriteLine($"{wired.Name} wired via {wired.UplinkDeviceId}, mac {wired.MacAddress}");
    else if (c is WirelessClientOverview wireless)
        Console.WriteLine($"{wireless.Name} wireless via {wireless.UplinkDeviceId}");
}
```

### Deep/flexible resources

Some resources — firewall policies, firewall zones, ACL rules, DNS policies, traffic matching
lists, WiFi broadcast security config, network DHCP/IPv4/IPv6 config — are themselves deep,
frequently-extended discriminated unions in the underlying API (dozens of nested DTOs). Modeling
every variant 1:1 would mean constant upkeep chasing new filter/protocol types the API adds.
Instead, these resources read as `System.Text.Json.JsonElement` and write as
`System.Text.Json.Nodes.JsonObject`, built directly against the
[OpenAPI spec](https://developer.ui.com/network/v10.4.57/openapi.json):

```csharp
using System.Text.Json.Nodes;

var zone = new JsonObject
{
    ["name"] = "IoT",
    ["networkIds"] = new JsonArray(networkId.ToString()),
};
var created = await client.FirewallZones.CreateAsync(siteId, zone);
```

Everything else — sites, devices, clients, networks (overview), WiFi broadcasts (overview),
vouchers, reference data — is fully typed.

### Errors

Non-2xx responses throw `UniFi.Network.Client.Http.UniFiApiException`, carrying the HTTP status
code plus the API's `code`/`message`/`requestId` fields when present.

```csharp
try
{
    await client.Devices.RestartAsync(siteId, deviceId);
}
catch (UniFiApiException ex)
{
    Console.WriteLine($"{ex.StatusCode} {ex.Code}: {ex.Message}");
}
```

## Site Manager API

`UniFi.SiteManager.Client` targets the account-wide cloud API at `https://api.ui.com`. Unlike the
Network client (which talks to one console), the Site Manager API spans every host on your account.
It only needs an API key generated at [unifi.ui.com](https://unifi.ui.com).

```csharp
using UniFi.SiteManager.Client;

using var client = new UniFiSiteManagerClient(apiKey);

// Hosts (consoles / network servers) on your account
var hosts = await client.Hosts.ListAsync(pageSize: 50);
foreach (var host in hosts.Data)
    Console.WriteLine($"{host.Id} [{host.Type}] {host.IpAddress}");

// Sites across all hosts
var sites = await client.Sites.ListAsync();

// Devices, grouped by host
var deviceGroups = await client.Devices.ListAsync(hostIds: new[] { "host-id" });

// ISP metrics and SD-WAN
var metrics = await client.IspMetrics.GetAsync("5m", duration: "24h");
var sdwan = await client.SdWanConfigs.ListAsync();
```

The available resources are `Hosts`, `Sites`, `Devices`, `IspMetrics`, and `SdWanConfigs`.

### Pagination (cursor-based)

Site Manager list endpoints use cursor pagination, not offset/limit. Each `SiteManagerPage<T>`
carries `Data`, a `NextToken`, and a `HasMore` helper — pass `NextToken` back to fetch the next page:

```csharp
var page = await client.Hosts.ListAsync(pageSize: 50);
while (true)
{
    foreach (var host in page.Data) { /* ... */ }
    if (!page.HasMore) break;
    page = await client.Hosts.ListAsync(pageSize: 50, nextToken: page.NextToken);
}
```

### Flexible fields and errors

Version-dependent blobs (a host's `UserData`/`ReportedState`, a site's `Meta`/`Statistics`, a
device's `Uidb`) are exposed as `System.Text.Json.JsonElement`. Non-2xx responses throw
`UniFi.SiteManager.Client.Http.UniFiSiteManagerException`, carrying the HTTP status plus the API's
`code`/`message`/`traceId` when present.

## Protect API

`UniFi.Protect.Client` targets the UniFi Protect integration API. Like the Network client, it
supports both a **local console** and the **Site Manager cloud connector** — pick a target the
same way:

```csharp
using UniFi.Protect.Client;

var options = ProtectClientOptions.ForLocalConsole("192.168.1.1", apiKey);
// or: ProtectClientOptions.ForCloudConnector(consoleId, apiKey);
using var client = new UniFiProtectClient(options);

var info = await client.Meta.GetInfoAsync();
Console.WriteLine($"Protect {info.ApplicationVersion}");

foreach (var camera in await client.Cameras.ListAsync())
    Console.WriteLine($"{camera.Name} [{camera.ModelKey}] {camera.State}");
```

Resources cover the full device fleet: `Cameras`, `Lights`, `Sensors`, `Chimes`, `Viewers`,
`Bridges`, `Fobs`, `Nvrs`, `Speakers`, `Sirens`, `Relays`, `LinkStations`, `AlarmHubs`,
`LiveViews`, `ArmProfiles`, `Users`, `UlpUsers`, `Files`, `AlarmManager`, and `Subscriptions`,
plus camera actions (RTSPS streams, snapshots, talkback, PTZ) and siren/relay/alarm-hub control.

### Typed reads, JsonObject writes

Protect device responses return plain arrays (no pagination). Identity/overview fields — `Id`,
`Name`, `Mac`, `State`, quality/volume/mode fields, battery status, timestamps — are strongly
typed, while deep, frequently-extended settings blobs (`osdSettings`, `smartDetectSettings`,
`featureFlags`, ...) are exposed as `System.Text.Json.JsonElement`. Update devices by PATCHing a
partial `System.Text.Json.Nodes.JsonObject`:

```csharp
using System.Text.Json.Nodes;

await client.Cameras.UpdateAsync(cameraId, new JsonObject
{
    ["name"] = "Front Door",
    ["osdSettings"] = new JsonObject { ["isNameEnabled"] = true },
});

var snapshot = await client.Cameras.GetSnapshotAsync(cameraId, highQuality: true); // byte[]
```

### Real-time updates

`Subscriptions` streams Protect's WebSocket feeds as `IAsyncEnumerable<JsonElement>` (the event
schemas are numerous and version-dependent, so messages are surfaced as raw JSON):

```csharp
await foreach (var message in client.Subscriptions.SubscribeToEventsAsync(cancellationToken))
    Console.WriteLine(message.GetRawText());
```

### Errors

Non-2xx responses throw `UniFi.Protect.Client.Http.UniFiProtectException`, carrying the HTTP status
plus the API's `error`/`name` fields.

## Project layout

- `src/UniFi.Network.Client` — the Network API library
  - `UniFiClientOptions` — connection targets (local console / cloud connector)
  - `UniFiNetworkClient` — entry point, aggregates resource clients
  - `Http/` — HTTP plumbing (`ApiConnection`, `UniFiApiException`)
  - `Models/` — request/response types
  - `Resources/` — one class per API resource group (Sites, Devices, Clients, Networks, ...)
- `src/UniFi.SiteManager.Client` — the Site Manager API library
  - `SiteManagerClientOptions` — API key / base address
  - `UniFiSiteManagerClient` — entry point, aggregates resource clients
  - `Http/` — HTTP plumbing (`ApiConnection`, `UniFiSiteManagerException`)
  - `Models/` — response types and the `SiteManagerPage<T>` pagination envelope
  - `Resources/` — Hosts, Sites, Devices, IspMetrics, SdWanConfigs
- `src/UniFi.Protect.Client` — the Protect API library
  - `ProtectClientOptions` — connection targets (local console / cloud connector)
  - `UniFiProtectClient` — entry point, aggregates resource clients
  - `Http/` — HTTP plumbing (`ApiConnection`, `UniFiProtectException`)
  - `Models/` — device and response types
  - `Resources/` — one class per device group plus Files, AlarmManager, and Subscriptions
- `samples/UniFi.Network.Client.Sample` — console app demonstrating both connection targets
- `samples/UniFi.SiteManager.Client.Sample` — console app for the Site Manager API
- `samples/UniFi.Protect.Client.Sample` — console app for the Protect API

## Sample app

```bash
export UNIFI_API_KEY=...
export UNIFI_CONSOLE_HOST=192.168.1.1   # or UNIFI_CONSOLE_ID=... for the cloud connector
dotnet run --project samples/UniFi.Network.Client.Sample
```

For the Site Manager API (only an API key is needed):

```bash
export UNIFI_API_KEY=...
dotnet run --project samples/UniFi.SiteManager.Client.Sample
```

For the Protect API (same local/cloud targets as the Network sample):

```bash
export UNIFI_API_KEY=...
export UNIFI_CONSOLE_HOST=192.168.1.1   # or UNIFI_CONSOLE_ID=... for the cloud connector
dotnet run --project samples/UniFi.Protect.Client.Sample
```

## Status

Covers all 44 documented endpoints across the Network API's 25 resource categories. Built against
API version 10.4.57 — generated request/response shapes may drift from newer console firmware;
the OpenAPI spec URL above is the source of truth.
