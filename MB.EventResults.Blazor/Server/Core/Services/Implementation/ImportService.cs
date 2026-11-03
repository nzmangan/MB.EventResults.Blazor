using System.Xml;
using System.Xml.Linq;

namespace MB.EventResults.Blazor.Server;

public class ImportService(ILogger<ImportService> _Logger, IJsonSerializerService _JsonSerializerService, IFileService _FileService, IResultBuilderService _ResultBuilderService, IXmlSerializerService _XmlSerializerService) : IImportService {
  public async Task Clear() {
    var filesToClear = new List<string> {
      Constants.ResultListFileName,
      Constants.StartListFileName,
      Constants.EntryListFileName,
      Constants.ClassListFileName,
      Constants.CourseDataFileName,
      Constants.ProcessedResultFileName
    };

    foreach (var file in filesToClear) {
      await _FileService.Delete(file);
    }
  }

  public async Task<bool> Import(Stream stream) {
    bool rebuild = false;
    string content = GetNormalizedXml(stream);

    if (content is null) {
      return false;
    }

    ResultList resultList = await GetXmlContent<ResultList>(content);

    if (resultList != null) {
      _Logger.LogInformation($"Saving {Constants.ResultListFileName}...");
      await _FileService.Save(Constants.ResultListFileName, content);
      rebuild = true;
    }

    StartList startList = await GetXmlContent<StartList>(content);

    if (startList != null) {
      _Logger.LogInformation($"Saving {Constants.StartListFileName}...");
      await _FileService.Save(Constants.StartListFileName, content);
      rebuild = true;
    }

    EntryList entryList = await GetXmlContent<EntryList>(content);

    if (entryList != null) {
      _Logger.LogInformation($"Saving {Constants.EntryListFileName}...");
      await _FileService.Save(Constants.EntryListFileName, content);
      rebuild = true;
    }

    ClassList classList = await GetXmlContent<ClassList>(content);

    if (classList != null) {
      _Logger.LogInformation($"Saving {Constants.ClassListFileName}...");
      await _FileService.Save(Constants.ClassListFileName, content);
      rebuild = true;
    }

    CourseData courseData = await GetXmlContent<CourseData>(content);

    if (courseData != null) {
      _Logger.LogInformation($"Saving {Constants.CourseDataFileName}...");
      await _FileService.Save(Constants.CourseDataFileName, content);
      rebuild = true;
    }

    return rebuild;
  }

  public async Task Reindex() {
    var response = await _ResultBuilderService.Build();
    await _FileService.Save(Constants.ProcessedResultFileName, _JsonSerializerService.Serialize(response));
  }

  // Decodes using the encoding the file declares and declares UTF-8 instead, as that is how the content gets stored.
  private string GetNormalizedXml(Stream stream) {
    try {
      using XmlReader reader = XmlReader.Create(stream, new XmlReaderSettings { DtdProcessing = DtdProcessing.Prohibit, XmlResolver = null });
      XDocument document = XDocument.Load(reader, LoadOptions.PreserveWhitespace);
      document.Declaration = new XDeclaration("1.0", "utf-8", null);
      return document.Declaration + document.ToString(SaveOptions.DisableFormatting);
    } catch (XmlException ex) {
      _Logger.LogWarning(ex, "Ignoring upload that is not valid xml.");
      return null;
    }
  }

  private async Task<T> GetXmlContent<T>(string content) where T : class {
    try {
      using MemoryStream ms = new(Encoding.UTF8.GetBytes(content));
      return await _XmlSerializerService.Deserialize<T>(() => ms);
    } catch {
      return null;
    }
  }
}