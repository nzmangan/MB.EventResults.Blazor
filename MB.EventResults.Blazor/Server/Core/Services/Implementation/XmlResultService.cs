namespace MB.EventResults.Blazor.Server;

public class XmlResultService(IXmlSerializerService _XmlSerializerService, IFileService _FileService) : IResultService {
  public async Task<ResultList> Get() {
    return await XmlSerializerServiceHelper.Get<ResultList>(_XmlSerializerService, _FileService, Constants.ResultListFileName);
  }
}