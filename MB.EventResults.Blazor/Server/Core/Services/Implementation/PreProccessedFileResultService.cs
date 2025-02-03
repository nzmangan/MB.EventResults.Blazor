namespace MB.EventResults.Blazor.Server;

public class PreProccessedFileResultService(ILogger<FileResultService> _Logger, IFileService _FileService, IJsonSerializerService _JsonSerializerService) : IProcessedResultService {
  private DateTime _LastUpdate = DateTime.MinValue;
  private EventResult _Processed = null;

  public async Task<EventResult> Get() {
    if (!await _FileService.Exists(Constants.ProcessedResultFileName)) {
      return null;
    }

    DateTime fileUpdateTime = await _FileService.LastUpdated(Constants.ProcessedResultFileName);

    if (fileUpdateTime > _LastUpdate) {
      _LastUpdate = fileUpdateTime;

      RebuildResponse rebuildResponse = null;

      int retryIndex = 0;

      while (rebuildResponse is null && retryIndex < 5) {
        try {
          rebuildResponse = _JsonSerializerService.Deserialize<RebuildResponse>(await _FileService.Load(Constants.ProcessedResultFileName));
        } catch (Exception ex) {
          _Logger.LogError(ex, $"Failed to load file {Constants.ProcessedResultFileName}");
        }
        retryIndex++;
      }

      _Processed = new EventResult {
        Grades = rebuildResponse.Results,
        EventDate = rebuildResponse.Created,
        EventStats = rebuildResponse.Stats
      };
    }

    return _Processed;
  }
}