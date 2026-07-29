using UniFi.Protect.Client.Http;
using UniFi.Protect.Client.Models;

namespace UniFi.Protect.Client.Resources;

/// <summary>Device asset files (e.g. doorbell LCD animations).</summary>
public sealed class FilesResource
{
    private readonly ApiConnection _connection;
    internal FilesResource(ApiConnection connection) => _connection = connection;

    /// <summary>List device asset files of the given type (e.g. "animations").</summary>
    public Task<IReadOnlyList<ProtectFile>> ListAsync(string fileType, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(fileType);
        return _connection.GetAsync<IReadOnlyList<ProtectFile>>($"v1/files/{Uri.EscapeDataString(fileType)}", cancellationToken: cancellationToken);
    }

    /// <summary>Upload a device asset file of the given type.</summary>
    /// <param name="fileType">Asset file type, e.g. "animations".</param>
    /// <param name="content">File content stream.</param>
    /// <param name="fileName">File name to send.</param>
    /// <param name="contentType">MIME type of the file, e.g. "image/png".</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    public Task<ProtectFile> UploadAsync(
        string fileType,
        Stream content,
        string fileName,
        string contentType,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(fileType);
        ArgumentNullException.ThrowIfNull(content);
        ArgumentException.ThrowIfNullOrWhiteSpace(fileName);
        ArgumentException.ThrowIfNullOrWhiteSpace(contentType);
        return _connection.PostFileAsync<ProtectFile>($"v1/files/{Uri.EscapeDataString(fileType)}", content, fileName, contentType, cancellationToken);
    }
}

/// <summary>Alarm manager integration (webhook triggers).</summary>
public sealed class AlarmManagerResource
{
    private readonly ApiConnection _connection;
    internal AlarmManagerResource(ApiConnection connection) => _connection = connection;

    /// <summary>
    /// Send a webhook to the alarm manager. The <paramref name="triggerId"/> matches the id
    /// configured on the alarm to be triggered.
    /// </summary>
    public Task SendWebhookAsync(string triggerId, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(triggerId);
        return _connection.PostAsync($"v1/alarm-manager/webhook/{Uri.EscapeDataString(triggerId)}", body: null, cancellationToken);
    }
}
