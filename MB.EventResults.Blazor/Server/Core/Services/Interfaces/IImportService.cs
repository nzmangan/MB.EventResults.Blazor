namespace MB.EventResults.Blazor.Server;

public interface IImportService {
  Task<bool> Import(Stream stream);
  void Clear();
  Task Reindex();
}