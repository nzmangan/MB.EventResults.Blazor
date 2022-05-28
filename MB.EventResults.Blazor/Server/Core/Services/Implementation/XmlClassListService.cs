using IOF.XML.V3;
using MB.OResults.Core;

namespace MB.EventResults.Blazor.Server;

public class XmlClassListService : IClassListService {
  private readonly IXmlSerializerService _XmlSerializerService;
  private readonly IFileService _FileService;

  public XmlClassListService(IXmlSerializerService xmlSerializerService, IFileService fileService) {
    _XmlSerializerService = xmlSerializerService;
    _FileService = fileService;
  }

  public async Task<ClassList> Get() {
    if (!_FileService.Exists(Constants.ClassListFileName)) {
      return null;
    }

    return await _XmlSerializerService.Deserialize<ClassList>(_FileService.GetPath(Constants.ClassListFileName));
  }
}
