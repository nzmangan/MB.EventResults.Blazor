namespace MB.EventResults.Blazor.Server;

public interface IProcessedResultService {
  Task<EventResult> Get();
}