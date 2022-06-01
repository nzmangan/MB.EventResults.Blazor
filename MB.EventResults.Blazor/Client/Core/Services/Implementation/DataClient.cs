using MB.EventResults.Blazor.Shared;

namespace MB.EventResults.Blazor.Client;

public class DataClient : IDataClient {
  private readonly IDataService _DataService;

  public DataClient(IDataService dataService) {
    _DataService = dataService;
  }

  public async Task<EventResult> Get() {
    return await _DataService.Post<EventResult>(UrlConstants.Get);
  }

  public async Task<SingleGradeResult> Get(string id) {
    return await _DataService.Post<SingleGradeResult>(UrlConstants.GetClass.Replace("{id}", id));
  }

  public async Task<List<EventGrade>> Grades() {
    return await _DataService.Post<List<EventGrade>>(UrlConstants.Grades);
  }
}