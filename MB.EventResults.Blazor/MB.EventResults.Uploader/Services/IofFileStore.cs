using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace MB.EventResults.Uploader;

// The newest file of each IOF XML type in the watch folder is the current version of that type.
public class IofFileStore : IResultService, IStartListService, IEntryListService, IClassListService, ICourseDataService {
  private readonly ILogger<IofFileStore> _Logger;
  private Dictionary<string, FileInfo> _Files = [];

  public IofFileStore(ILogger<IofFileStore> logger, UploaderConfiguration configuration) {
    _Logger = logger;
    Folder = Path.GetFullPath(configuration.WatchFolder, Environment.CurrentDirectory);
    Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
    Directory.CreateDirectory(Folder);
  }

  public string Folder { get; }

  public bool HasFiles => _Files.Count > 0;

  // Returns a value that changes whenever one of the selected files changes.
  public string Scan() {
    _Files = new DirectoryInfo(Folder)
      .EnumerateFiles("*.xml")
      .Select(p => (File: p, Type: GetDocumentType(p)))
      .Where(p => p.Type is not null)
      .GroupBy(p => p.Type)
      .ToDictionary(p => p.Key, p => p.OrderByDescending(x => x.File.LastWriteTimeUtc).First().File);

    return String.Join("|", _Files.OrderBy(p => p.Key).Select(p => $"{p.Key}:{p.Value.Name}:{p.Value.LastWriteTimeUtc.Ticks}:{p.Value.Length}"));
  }

  Task<ResultList> IResultService.Get() => Load<ResultList>();

  Task<StartList> IStartListService.Get() => Load<StartList>();

  Task<EntryList> IEntryListService.Get() => Load<EntryList>();

  Task<ClassList> IClassListService.Get() => Load<ClassList>();

  Task<CourseData> ICourseDataService.Get() => Load<CourseData>();

  private Task<T> Load<T>() where T : class {
    if (!_Files.TryGetValue(typeof(T).Name, out var file)) {
      return Task.FromResult<T>(null);
    }

    _Logger.LogInformation($"Reading {file.Name} as {typeof(T).Name}.");

    using var stream = Open(file);
    return Task.FromResult(new XmlSerializer(typeof(T)).Deserialize(stream) as T);
  }

  private string GetDocumentType(FileInfo file) {
    try {
      using var stream = Open(file);
      using var reader = XmlReader.Create(stream, new XmlReaderSettings { DtdProcessing = DtdProcessing.Ignore });
      reader.MoveToContent();

      if (reader.LocalName is nameof(ResultList) or nameof(StartList) or nameof(EntryList) or nameof(ClassList) or nameof(CourseData)) {
        return reader.LocalName;
      }

      _Logger.LogDebug($"Ignoring {file.Name}, root element is {reader.LocalName}.");
    } catch (XmlException) {
      _Logger.LogDebug($"Ignoring {file.Name}, it is not readable XML (yet).");
    }

    return null;
  }

  // The results software may still have the file open while it is being written.
  private static FileStream Open(FileInfo file) {
    return new FileStream(file.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete);
  }
}
