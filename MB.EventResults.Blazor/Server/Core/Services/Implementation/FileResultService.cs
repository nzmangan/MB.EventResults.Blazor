namespace MB.EventResults.Blazor.Server;

public class FileResultService(ILogger<FileResultService> _Logger, IXmlSerializerService _XmlSerializerService, IFileService _FileService, IAnalyzerService _AnalyzerService, AppConfiguration _AppConfiguration) : IProcessedResultService {
  private DateTime _LastUpdate = DateTime.MinValue;
  private EventResult _Processed = null;

  public async Task<EventResult> Get() {
    string filePath = _AppConfiguration.ResultsSourceFile;

    if (!await _FileService.Exists(filePath)) {
      return null;
    }

    DateTime fileUpdateTime = await _FileService.LastUpdated(filePath);

    if (fileUpdateTime > _LastUpdate || _Processed is null) {
      _LastUpdate = fileUpdateTime;

      ResultList resultList = await LoadFile<ResultList>(filePath);

      if (resultList is null) {
        return null;
      }

      var courseData = await LoadFile<CourseData>(_AppConfiguration.CourseDataSourceFile);

      var courseInfo = _AnalyzerService.ConvertToControlData(courseData);

      var grades = resultList.ClassResult.Select(p => _AnalyzerService.ConvertToGradeResult(p, courseInfo)).ToList();
      var eventStats = _AnalyzerService.AnalysEvent(grades, courseInfo);

      _Processed = new EventResult {
        Grades = grades,
        EventDate = _LastUpdate,
        EventName = resultList?.Event?.Name,
        EventStats = eventStats
      };
    }

    return _Processed;
  }

  private async Task<T> LoadFile<T>(string filePath) where T : class {
    T file = default;

    int retryIndex = 0;

    while (file is null && retryIndex < 5) {
      try {
        file = await _XmlSerializerService.Deserialize<T>(Path.Combine(_AppConfiguration.UploadFolder, filePath));
      } catch (Exception ex) {
        _Logger.LogError(ex, $"Failed to load file {_AppConfiguration.ResultsSourceFile}");
      }
      retryIndex++;
    }

    return file;
  }
}