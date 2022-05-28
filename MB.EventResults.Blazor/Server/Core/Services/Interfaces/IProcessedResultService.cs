using MB.EventResults.Blazor.Shared;

namespace MB.EventResults.Blazor.Server;

public interface IProcessedResultService {
  Task<EventResult> Get();
}