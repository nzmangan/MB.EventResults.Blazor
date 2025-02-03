namespace MB.EventResults.Blazor.Server;

public interface ICacheService {
  Task<T> Get<T>(string key, Func<Task<T>> builder);
  Task Clear();
}