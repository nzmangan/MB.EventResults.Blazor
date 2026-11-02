using System.Net;
using System.Net.Http.Json;

namespace MB.EventResults.Blazor.Client;

// Reads everything from one pre-processed json file, so the host only has to serve static files.
public class StaticDataClient(ILogger<StaticDataClient> _Logger, HttpClient _Http) : IDataClient {
  private RebuildResponse _Results;
  private string _ETag;

  public async Task<EventResult> Get() {
    var results = await Load();

    return new EventResult {
      Grades = results.Results,
      EventDate = results.Created,
      EventStats = results.Stats
    };
  }

  public async Task<SingleGradeResult> Get(string id) {
    var results = await Load();
    var grade = results.Results.FirstOrDefault(p => p.Id == id);

    if (grade is null) {
      return null;
    }

    return new SingleGradeResult {
      EventDate = results.Created,
      Grade = grade
    };
  }

  public async Task<List<EventGrade>> Grades() {
    var results = await Load();

    return results.Results.Select(p => new EventGrade {
      Id = p.Id,
      Name = p.Name
    }).ToList();
  }

  public async Task<EventStatistic> Stats() {
    var results = await Load();
    return results.Stats;
  }

  private async Task<RebuildResponse> Load() {
    try {
      // The browser revalidates the file, so the body is only downloaded again when it has changed.
      using var response = await _Http.GetAsync(UrlConstants.StaticResults);

      if (response.StatusCode == HttpStatusCode.NotFound) {
        _Results = null;
        _ETag = null;
        return Empty();
      }

      response.EnsureSuccessStatusCode();

      var etag = response.Headers.ETag?.Tag;

      if (_Results is null || etag is null || etag != _ETag) {
        _Results = await response.Content.ReadFromJsonAsync<RebuildResponse>();
        _ETag = etag;
      }
    } catch (HttpRequestException ex) when (_Results is not null) {
      _Logger.LogWarning(ex, "Could not refresh results, showing the last ones loaded.");
    }

    return _Results;
  }

  private static RebuildResponse Empty() {
    return new RebuildResponse { Results = [], Stats = new() };
  }
}
