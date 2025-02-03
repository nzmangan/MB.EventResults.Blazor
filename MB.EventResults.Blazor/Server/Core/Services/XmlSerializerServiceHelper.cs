namespace MB.EventResults.Blazor.Server;

public static class XmlSerializerServiceHelper {
  public static async Task<T> Get<T>(IXmlSerializerService _XmlSerializerService, IFileService _FileService, string path) where T : class {
    if (!await _FileService.Exists(path)) {
      return default;
    }

    return await _XmlSerializerService.Deserialize<T>(_FileService.GetPath(path));
  }
}