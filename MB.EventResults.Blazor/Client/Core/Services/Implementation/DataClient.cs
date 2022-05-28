using MB.EventResults.Blazor.Shared;

namespace MB.EventResults.Blazor.Client;

public class DataClient : IDataClient {
  private readonly IDataService _DataService;

  public DataClient(IDataService dataService) {
    _DataService = dataService;
  }

  public async Task<EventResult> Get() {
    return await _DataService.Get<EventResult>(UrlConstants.Get);
  }

  public async Task<SingleGradeResult> Get(string id) {
    return await _DataService.Get<SingleGradeResult>(UrlConstants.GetClass.Replace("{id}", id));
  }

  public async Task<List<EventGrade>> Grades() {
    return await _DataService.Get<List<EventGrade>>(UrlConstants.Grades);
  }
}