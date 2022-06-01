namespace MB.EventResults.Blazor.Client;

public interface IDataService {
  Task<T> Get<T>(string url, Func<T> errorResponse = null);
  Task<T> Post<T>(string url, Func<T> errorResponse = null);
}