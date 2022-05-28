using IOF.XML.V3;
using MB.EventResults.Blazor.Shared;
using MB.OResults.Core;

namespace MB.EventResults.Blazor.Server;

public class FileResultService : IProcessedResultService {
  private readonly ILogger<FileResultService> _Logger;
  private readonly IXmlSerializerService _XmlSerializerService;
  private readonly IFileService _FileService;
  private readonly IAnalyzerService _AnalyzerService;
  private readonly AppConfiguration _AppConfiguration;

  private DateTime _LastUpdate = DateTime.MinValue;
  private EventResult _Processed = null;

  public FileResultService(ILogger<FileResultService> logger, IXmlSerializerService xmlSerializerService, IFileService fileService, IAnalyzerService analyzerService, AppConfiguration appConfiguration) {
    _Logger = logger;
    _XmlSerializerService = xmlSerializerService;
    _FileService = fileService;
    _AnalyzerService = analyzerService;
    _AppConfiguration = appConfiguration;
  }

  public async Task<EventResult> Get() {
    if (!_FileService.Exists(_AppConfiguration.SourceFile)) {
      return null;
    }

    DateTime fileUpdateTime = _FileService.LastUpdated(_AppConfiguration.SourceFile);

    if (fileUpdateTime > _LastUpdate || _Processed is null) {
      _LastUpdate = fileUpdateTime;

      ResultList resultList = null;

      int retryIndex = 0;

      while (resultList is null && retryIndex < 5) {
        try {
          resultList = await _XmlSerializerService.Deserialize<ResultList>(Path.Combine(_AppConfiguration.UploadFolder, _AppConfiguration.SourceFile));
        } catch (Exception ex) {
          _Logger.LogError(ex, $"Failed to load file {_AppConfiguration.SourceFile}");
        }
        retryIndex++;
      }

      if (resultList is null) {
        return null;
      }

      var grades = resultList.ClassResult.Select(p => _AnalyzerService.ConvertToGradeResult(p)).ToList();

      _Processed = new EventResult {
        Grades = grades,
        EventDate = _LastUpdate,
        EventName = resultList?.Event?.Name
      };
    }

    return _Processed;
  }
}