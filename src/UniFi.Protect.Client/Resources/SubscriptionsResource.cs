using System.Buffers;
using System.Net.WebSockets;
using System.Runtime.CompilerServices;
using System.Text.Json;
using UniFi.Protect.Client.Http;

namespace UniFi.Protect.Client.Resources;

/// <summary>
/// Real-time update streams delivered over WebSocket. Each yielded <see cref="JsonElement"/> is one
/// message from Protect; the event/update schemas are numerous and version-dependent, so messages
/// are surfaced as raw JSON for you to interpret.
/// </summary>
public sealed class SubscriptionsResource
{
    private readonly ApiConnection _connection;

    internal SubscriptionsResource(ApiConnection connection) => _connection = connection;

    /// <summary>Subscribe to device add/update/remove messages.</summary>
    public IAsyncEnumerable<JsonElement> SubscribeToDevicesAsync(CancellationToken cancellationToken = default)
        => StreamAsync("v1/subscribe/devices", cancellationToken);

    /// <summary>Subscribe to Protect event messages (motion, smart detections, alarms, ...).</summary>
    public IAsyncEnumerable<JsonElement> SubscribeToEventsAsync(CancellationToken cancellationToken = default)
        => StreamAsync("v1/subscribe/events", cancellationToken);

    private async IAsyncEnumerable<JsonElement> StreamAsync(
        string relativePath,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        using var socket = new ClientWebSocket();
        socket.Options.SetRequestHeader("X-API-KEY", _connection.ApiKey);
        var certCallback = TlsValidation.CreateWebSocketCallback(_connection.PinnedCertificateSha256, _connection.AllowUntrustedCertificate);
        if (certCallback is not null)
        {
            socket.Options.RemoteCertificateValidationCallback = certCallback;
        }

        var uri = BuildWebSocketUri(relativePath);
        await socket.ConnectAsync(uri, cancellationToken).ConfigureAwait(false);

        var buffer = new byte[8192];
        var message = new ArrayBufferWriter<byte>();

        while (socket.State == WebSocketState.Open && !cancellationToken.IsCancellationRequested)
        {
            message.Clear();
            WebSocketReceiveResult result;
            do
            {
                result = await socket.ReceiveAsync(buffer, cancellationToken).ConfigureAwait(false);
                if (result.MessageType == WebSocketMessageType.Close)
                {
                    yield break;
                }
                message.Write(buffer.AsSpan(0, result.Count));
            }
            while (!result.EndOfMessage);

            if (message.WrittenCount == 0)
            {
                continue;
            }

            using var document = JsonDocument.Parse(message.WrittenMemory);
            yield return document.RootElement.Clone();
        }
    }

    private Uri BuildWebSocketUri(string relativePath)
    {
        var baseUri = _connection.BaseAddress;
        var scheme = baseUri.Scheme == Uri.UriSchemeHttps ? "wss" : "ws";
        var builder = new UriBuilder(baseUri) { Scheme = scheme };
        builder.Path = builder.Path.EndsWith('/') ? builder.Path + relativePath : builder.Path + "/" + relativePath;
        return builder.Uri;
    }
}
