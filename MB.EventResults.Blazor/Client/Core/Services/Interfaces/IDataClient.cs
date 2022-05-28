using MB.EventResults.Blazor.Shared;

namespace MB.EventResults.Blazor.Client;

public interface IDataClient {
  Task<EventResult> Get();
  Task<SingleGradeResult> Get(string id);
  Task<List<EventGrade>> Grades();
}