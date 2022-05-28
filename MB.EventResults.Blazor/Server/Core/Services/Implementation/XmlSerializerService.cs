using System.Text;
using System.Xml.Serialization;
using MB.OResults.Core;

namespace MB.EventResults.Blazor.Server;

public class XmlSerializerService : IXmlSerializerService {
  public XmlSerializerService() {
    Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
  }

  public Task Serialize<T>(string path, T instance) {
    using (FileStream fileStream = new FileStream(path, FileMode.Create)) {
      new XmlSerializer(typeof(T)).Serialize(fileStream, instance);
    }

    return Task.CompletedTask;
  }

  public Task<T> Deserialize<T>(string path) where T : class {
    if (!File.Exists(path)) {
      return null;
    }

    using (FileStream fileStream = new FileStream(path, FileMode.Open)) {
      return Task.FromResult(new XmlSerializer(typeof(T)).Deserialize(fileStream) as T);
    }
  }

  public Task<T> Deserialize<T>(Func<Stream> streamGetter) where T : class {
    using (var stream = streamGetter()) {
      return Task.FromResult(new XmlSerializer(typeof(T)).Deserialize(stream) as T);
    }
  }
}