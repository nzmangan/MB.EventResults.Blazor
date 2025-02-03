namespace MB.EventResults.Blazor.Server;

public class XmlStartListService(IXmlSerializerService _XmlSerializerService, IFileService _FileService) : IStartListService {
  public async Task<StartList> Get() {
    return await XmlSerializerServiceHelper.Get<StartList>(_XmlSerializerService, _FileService, Constants.StartListFileName);
  }
}