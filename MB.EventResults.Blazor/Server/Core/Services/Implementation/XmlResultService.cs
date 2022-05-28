using IOF.XML.V3;
using MB.OResults.Core;

namespace MB.EventResults.Blazor.Server;

public class XmlResultService : IResultService {
  private readonly IXmlSerializerService _XmlSerializerService;
  private readonly IFileService _FileService;

  public XmlResultService(IXmlSerializerService xmlSerializerService, IFileService fileService) {
    _XmlSerializerService = xmlSerializerService;
    _FileService = fileService;
  }

  public async Task<ResultList> Get() {
    if (!_FileService.Exists(Constants.ResultListFileName)) {
      return null;
    }

    return await _XmlSerializerService.Deserialize<ResultList>(_FileService.GetPath(Constants.ResultListFileName));
  }
}