using System.Net;
using System.Net.Http.Json;

namespace MB.EventResults.Blazor.Client;

public class DataService : IDataService {
  private readonly ILogger<DataService> _Logger;
  private readonly HttpClient _Http;

  public DataService(ILogger<DataService> logger, HttpClient http) {
    _Logger = logger;
    _Http = http;
  }

  public async Task<T> Get<T>(string url, Func<T> errorResponse = null) {
    using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, url)) {
      return await Send<T>(requestMessage, errorResponse);
    }
  }

  public async Task<T> Post<T>(string url, Func<T> errorResponse = null) {
    using (var requestMessage = new HttpRequestMessage(HttpMethod.Post, url)) {
      return await Send<T>(requestMessage, errorResponse);
    }
  }

  private async Task<T> Send<T>(HttpRequestMessage requestMessage, Func<T> errorResponse = null) {
    var response = await _Http.SendAsync(requestMessage);

    // auto logout on 401 response
    if (response.StatusCode == HttpStatusCode.Unauthorized) {
      _Logger.LogError($"Call to {requestMessage.RequestUri} returned 401");
      return default;
    }

    if (!response.IsSuccessStatusCode) {
      if (errorResponse == null) {
        var error = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
        throw new Exception(error["message"]);
      } else {
        return errorResponse();
      }
    }

    try {
      return await response.Content.ReadFromJsonAsync<T>();
    } catch (Exception ex) {
      _Logger.LogError(ex, $"Could not read and deseriliaze json.");
      return default;
    }
  }
}