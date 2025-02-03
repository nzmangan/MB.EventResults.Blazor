namespace MB.EventResults.Blazor.Server;

public class XmlEntryListService(IXmlSerializerService _XmlSerializerService, IFileService _FileService) : IEntryListService {
  public async Task<EntryList> Get() {
    return await XmlSerializerServiceHelper.Get<EntryList>(_XmlSerializerService, _FileService, Constants.EntryListFileName);
  }
}