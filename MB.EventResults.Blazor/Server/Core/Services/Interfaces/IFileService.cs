namespace MB.EventResults.Blazor.Server;

public interface IFileService {
  Task Save(string filePath, string content);
  DateTime LastUpdated(string filePath);
  bool Exists(string filePath);
  Task<string> Load(string filePath);
  string GetPath(string filePath);
  void Delete(string file);
}
