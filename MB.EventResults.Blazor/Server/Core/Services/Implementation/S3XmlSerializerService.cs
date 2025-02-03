using System.Xml;
using System.Xml.Serialization;

namespace MB.EventResults.Blazor.Server;

public class S3XmlSerializerService : IXmlSerializerService {
  private readonly IFileService _FileService;

  public S3XmlSerializerService(IFileService fileService) {
    Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
    _FileService = fileService;
  }

  public async Task Serialize<T>(string path, T instance) {
    using var stream = new MemoryStream();
    Get<T>().Serialize(stream, instance);
    stream.Seek(0, SeekOrigin.Begin);
    using var reader = new StreamReader(stream);
    var content = reader.ReadToEnd();

    await _FileService.Save(path, content);
  }

  public async Task<T> Deserialize<T>(string path) where T : class {
    if (!await _FileService.Exists(path)) {
      return null;
    }

    var content = await _FileService.Load(path);

    if (String.IsNullOrEmpty(content)) {
      return null;
    }

    return Get<T>().Deserialize(XmlReader.Create(new MemoryStream(Encoding.UTF8.GetBytes(content)))) as T;

  }

  public Task<T> Deserialize<T>(Func<Stream> streamGetter) where T : class {
    using var stream = streamGetter();
    return Task.FromResult(Get<T>().Deserialize(stream) as T);
  }

  private XmlSerializer Get<T>() {
    return new XmlSerializer(typeof(T));
  }
}