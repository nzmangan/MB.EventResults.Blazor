using IOF.XML.V3;
using MB.OResults.Core;

namespace MB.EventResults.Blazor.Server;

public class XmlStartListService : IStartListService {
  private readonly IXmlSerializerService _XmlSerializerService;
  private readonly IFileService _FileService;

  public XmlStartListService(IXmlSerializerService xmlSerializerService, IFileService fileService) {
    _XmlSerializerService = xmlSerializerService;
    _FileService = fileService;
  }

  public async Task<StartList> Get() {
    if (!_FileService.Exists(Constants.StartListFileName)) {
      return null;
    }

    return await _XmlSerializerService.Deserialize<StartList>(_FileService.GetPath(Constants.StartListFileName));
  }
}