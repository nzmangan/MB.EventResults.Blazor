namespace MB.EventResults.Blazor.Server;

public interface IFileService {
  Task Save(string filePath, string content);
  Task<DateTime> LastUpdated(string filePath);
  Task<bool> Exists(string filePath);
  Task<string> Load(string filePath);
  string GetPath(string filePath);
  Task Delete(string file);
}
