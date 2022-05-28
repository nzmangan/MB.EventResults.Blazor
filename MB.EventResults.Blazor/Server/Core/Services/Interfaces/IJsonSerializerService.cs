namespace MB.EventResults.Blazor.Server;

public interface IJsonSerializerService {
  string Serialize<T>(T input);
  T Deserialize<T>(string content);
}
