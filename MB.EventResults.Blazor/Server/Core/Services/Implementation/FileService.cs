namespace MB.EventResults.Blazor.Server;

public class FileService : IFileService {
  private readonly AppConfiguration _AppConfiguration;

  public FileService(AppConfiguration appConfiguration) {
    _AppConfiguration = appConfiguration;
  }

  public bool Exists(string filePath) {
    return File.Exists(GetPath(filePath));
  }

  public DateTime LastUpdated(string filePath) {
    return File.GetLastWriteTime(GetPath(filePath));
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

  public void Delete(string file) {
    try {
      File.Delete(GetPath(file));
    } catch { }
  }
}