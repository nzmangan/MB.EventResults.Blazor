﻿using System.Text;
using IOF.XML.V3;
using MB.OResults.Core;

namespace MB.EventResults.Blazor.Server;

public class ImportService : IImportService {
  private readonly ILogger<ImportService> _Logger;
  private readonly IJsonSerializerService _JsonSerializerService;
  private readonly IFileService _FileService;
  private readonly IResultBuilderService _ResultBuilderService;
  private readonly IXmlSerializerService _XmlSerializerService;

  public ImportService(ILogger<ImportService> logger, IJsonSerializerService jsonSerializerService, IFileService fileService, IResultBuilderService resultBuilderService, IXmlSerializerService xmlSerializerService) {
    _Logger = logger;
    _JsonSerializerService = jsonSerializerService;
    _FileService = fileService;
    _ResultBuilderService = resultBuilderService;
    _XmlSerializerService = xmlSerializerService;
  }

  public void Clear() {
    var filesToClear = new List<string> {
      Constants.ResultListFileName,
      Constants.StartListFileName,
      Constants.EntryListFileName,
      Constants.ClassListFileName,
      Constants.ProcessedResultFileName
    };

    foreach (var file in filesToClear) {
      _FileService.Delete(file);
    }
  }

  public async Task<bool> Import(Stream stream) {
    bool rebuild = false;
    string content = GetString(stream);

    ResultList resultList = await GetXmlContent<ResultList>(content, "Failed to open file as results.");

    if (resultList != null) {
      await _FileService.Save(Constants.ResultListFileName, content);
      rebuild = true;
    }

    StartList startList = await GetXmlContent<StartList>(content, "Failed to open file as starts.");

    if (startList != null) {
      await _FileService.Save(Constants.StartListFileName, content);
      rebuild = true;
    }

    EntryList entryList = await GetXmlContent<EntryList>(content, "Failed to open file as entries.");

    if (entryList != null) {
      await _FileService.Save(Constants.EntryListFileName, content);
      rebuild = true;
    }

    ClassList classList = await GetXmlContent<ClassList>(content, "Failed to open file as class list.");

    if (classList != null) {
      await _FileService.Save(Constants.ClassListFileName, content);
      rebuild = true;
    }

    if (rebuild) {
      await Reindex();
    }

    return rebuild;
  }

  public async Task Reindex() {
    var response = await _ResultBuilderService.Build();
    await _FileService.Save(Constants.ProcessedResultFileName, _JsonSerializerService.Serialize(response.Results));
  }

  private string GetString(Stream stream) {
    using (StreamReader reader = new StreamReader(stream)) {
      return reader.ReadToEnd();
    }
  }

  private async Task<T> GetXmlContent<T>(string content, string message) where T : class {
    try {
      using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(content))) {
        return await _XmlSerializerService.Deserialize<T>(() => ms);
      }
    } catch (Exception ex) {
      _Logger.LogError(ex, message);
      return null;
    }
  }
}