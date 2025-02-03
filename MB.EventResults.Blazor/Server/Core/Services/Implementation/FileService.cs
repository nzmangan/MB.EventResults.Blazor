namespace MB.EventResults.Blazor.Server;

public class FileService(AppConfiguration _AppConfiguration) : IFileService {
  public Task<bool> Exists(string filePath) {
    return Task.FromResult(File.Exists(GetPath(filePath)));
  }

  public Task<DateTime> LastUpdated(string filePath) {
    return Task.FromResult(File.GetLastWriteTime(GetPath(filePath)));
  }

  public Task<string> Load(string filePath) {
    return File.ReadAllTextAsync(GetPath(filePath));
  }

  public async Task Save(string filePath, string content) {
    await File.WriteAllTextAsync(GetPath(filePath), content);
  }

  public string GetPath(string filePath) {
    if (!String.IsNullOrWhiteSpace(_AppConfiguration.UploadFolder) && !Directory.Exists(_AppConfiguration.UploadFolder)) {
      Directory.CreateDirectory(_AppConfiguration.UploadFolder);
    }

    return Path.Combine(_AppConfiguration.UploadFolder, filePath);
  }

  public Task Delete(string file) {
    try {
      File.Delete(GetPath(file));
    } catch { }

    return Task.CompletedTask;
  }
}