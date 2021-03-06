using MB.EventResults.Blazor.Shared;
using MB.OResults.Core;

namespace MB.EventResults.Blazor.Server;

public class PreProccessedFileResultService : IProcessedResultService {
  private readonly ILogger<FileResultService> _Logger;
  private readonly IFileService _FileService;
  private readonly IJsonSerializerService _JsonSerializerService;

  private DateTime _LastUpdate = DateTime.MinValue;
  private EventResult _Processed = null;

  public PreProccessedFileResultService(ILogger<FileResultService> logger, IFileService fileService, IJsonSerializerService jsonSerializerService) {
    _Logger = logger;
    _FileService = fileService;
    _JsonSerializerService = jsonSerializerService;
  }

  public async Task<EventResult> Get() {
    if (!_FileService.Exists(Constants.ProcessedResultFileName)) {
      return null;
    }

    DateTime fileUpdateTime = _FileService.LastUpdated(Constants.ProcessedResultFileName);

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
        EventDate = rebuildResponse.Created
      };
    }

    return _Processed;
  }
}