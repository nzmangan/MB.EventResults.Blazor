namespace MB.EventResults.Blazor.Server;

public class XmlClassListService(IXmlSerializerService _XmlSerializerService, IFileService _FileService) : IClassListService {
  public async Task<ClassList> Get() {
    return await XmlSerializerServiceHelper.Get<ClassList>(_XmlSerializerService, _FileService, Constants.ClassListFileName);
  }
}