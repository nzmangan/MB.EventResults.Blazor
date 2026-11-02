using Renci.SshNet;
using Renci.SshNet.Common;

namespace MB.EventResults.Uploader;

public record TargetFile(string Name, byte[] Content);

public interface IResultTarget {
  string Description { get; }
  Task Upload(IReadOnlyList<TargetFile> files, CancellationToken cancellationToken);
}

public class FolderTarget(UploaderConfiguration _Configuration) : IResultTarget {
  public string Description => _Configuration.Target.Path;

  public async Task Upload(IReadOnlyList<TargetFile> files, CancellationToken cancellationToken) {
    Directory.CreateDirectory(_Configuration.Target.Path);

    foreach (var file in files) {
      var path = Path.Combine(_Configuration.Target.Path, file.Name);
      var temporaryPath = $"{path}.tmp";
      await File.WriteAllBytesAsync(temporaryPath, file.Content, cancellationToken);
      File.Move(temporaryPath, path, true);
    }
  }
}

public class SftpTarget(ILogger<SftpTarget> _Logger, UploaderConfiguration _Configuration) : IResultTarget {
  private readonly TargetConfiguration _Target = _Configuration.Target;

  public string Description => $"{_Target.Host}:{_Target.Path}";

  public Task Upload(IReadOnlyList<TargetFile> files, CancellationToken cancellationToken) {
    return Task.Run(() => {
      using var client = new SftpClient(_Target.Host, _Target.Port, _Target.Username, _Target.Password);
      client.HostKeyReceived += CheckHostKey;
      client.Connect();

      EnsureFolder(client, _Target.Path);

      foreach (var file in files) {
        cancellationToken.ThrowIfCancellationRequested();
        Upload(client, file);
      }

      client.Disconnect();
    }, cancellationToken);
  }

  private void CheckHostKey(object sender, HostKeyEventArgs e) {
    if (String.IsNullOrWhiteSpace(_Target.HostKeySha256)) {
      _Logger.LogInformation($"Host key not pinned. Set Uploader:Target:HostKeySha256 to {e.FingerPrintSHA256} to verify it.");
      return;
    }

    e.CanTrust = _Target.HostKeySha256 == e.FingerPrintSHA256;
  }

  // Uploads to a temporary name first so that visitors never download a half written file.
  private void Upload(SftpClient client, TargetFile file) {
    var path = $"{_Target.Path.TrimEnd('/')}/{file.Name}";
    var temporaryPath = $"{path}.tmp";

    using (var stream = new MemoryStream(file.Content)) {
      client.UploadFile(stream, temporaryPath, true);
    }

    try {
      client.RenameFile(temporaryPath, path, true);
    } catch (Exception ex) when (ex is NotSupportedException or SshException) {
      _Logger.LogDebug(ex, "Server does not support replacing a file by rename.");

      if (client.Exists(path)) {
        client.DeleteFile(path);
      }

      client.RenameFile(temporaryPath, path);
    }
  }

  private static void EnsureFolder(SftpClient client, string folder) {
    var current = folder.StartsWith('/') ? "" : ".";

    foreach (var part in folder.Split('/', StringSplitOptions.RemoveEmptyEntries)) {
      current = $"{current}/{part}";

      if (!client.Exists(current)) {
        client.CreateDirectory(current);
      }
    }
  }
}
