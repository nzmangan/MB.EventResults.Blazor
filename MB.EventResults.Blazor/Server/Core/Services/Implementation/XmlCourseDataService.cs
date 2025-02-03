namespace MB.EventResults.Blazor.Server;

public class XmlCourseDataService(IXmlSerializerService _XmlSerializerService, IFileService _FileService) : ICourseDataService {
  public async Task<CourseData> Get() {
    return await XmlSerializerServiceHelper.Get<CourseData>(_XmlSerializerService, _FileService, Constants.CourseDataFileName);
  }
}