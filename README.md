# UniFi.NET

A .NET client for the [UniFi Network API](https://developer.ui.com/network/) (v10.4.57), targeting
either a local console directly or the Site Manager cloud connector — no VPN required.

This repo is meant to grow into a family of UniFi .NET clients (`UniFi.Network.Client` today,
`UniFi.Protect.Client` / `UniFi.Access.Client` potentially later), so each product's client lives
under its own namespace and project.

## Install

```bash
dotnet add package UniFi.Network.Client
```

(Not published yet — reference the project directly for now: `dotnet add reference src/UniFi.Network.Client/UniFi.Network.Client.csproj`.)

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

## Project layout

- `src/UniFi.Network.Client` — the library
  - `UniFiClientOptions` — connection targets (local console / cloud connector)
  - `UniFiNetworkClient` — entry point, aggregates resource clients
  - `Http/` — HTTP plumbing (`ApiConnection`, `UniFiApiException`)
  - `Models/` — request/response types
  - `Resources/` — one class per API resource group (Sites, Devices, Clients, Networks, ...)
- `samples/UniFi.Network.Client.Sample` — console app demonstrating both connection targets

## Sample app

```bash
export UNIFI_API_KEY=...
export UNIFI_CONSOLE_HOST=192.168.1.1   # or UNIFI_CONSOLE_ID=... for the cloud connector
dotnet run --project samples/UniFi.Network.Client.Sample
```

## Status

Covers all 44 documented endpoints across the Network API's 25 resource categories. Built against
API version 10.4.57 — generated request/response shapes may drift from newer console firmware;
the OpenAPI spec URL above is the source of truth.
