using System.Text.Json;

namespace MB.EventResults.Blazor.Server;

public class JsonSerializerService : IJsonSerializerService {
  public T Deserialize<T>(string content) {
    return JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions { WriteIndented = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
  }

  public string Serialize<T>(T input) {
    return JsonSerializer.Serialize(input, new JsonSerializerOptions { WriteIndented = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
  }
}