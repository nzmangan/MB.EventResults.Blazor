using IOF.XML.V3;
using MB.OResults.Core;

namespace MB.EventResults.Blazor.Server;

public class XmlEntryListService : IEntryListService {
  private readonly IXmlSerializerService _XmlSerializerService;
  private readonly IFileService _FileService;

  public XmlEntryListService(IXmlSerializerService xmlSerializerService, IFileService fileService) {
    _XmlSerializerService = xmlSerializerService;
    _FileService = fileService;
  }

  public async Task<EntryList> Get() {
    if (!_FileService.Exists(Constants.EntryListFileName)) {
      return null;
    }

    return await _XmlSerializerService.Deserialize<EntryList>(_FileService.GetPath(Constants.EntryListFileName));
  }
}
