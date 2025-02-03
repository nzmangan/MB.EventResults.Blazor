namespace MB.EventResults.Blazor.Shared;

public interface IJsonSerializerService {
  string Serialize<T>(T input);
  T Deserialize<T>(string content);
}