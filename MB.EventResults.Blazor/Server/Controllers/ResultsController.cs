namespace MB.EventResults.Blazor.Server;

[ApiController]
public class ResultsController(IProcessedResultService _ProcessedResultService, ICacheService _CacheService) : Controller {
  [HttpGet(UrlConstants.Get)]
  [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
  public async Task<ActionResult<EventResult>> Get() {
    var result = await GetResults();

    if (result is null) {
      return NotFound("Could not find any results.");
    }

    return result;
  }

  [HttpGet(UrlConstants.GetClass)]
  [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
  public async Task<ActionResult<SingleGradeResult>> GetClass(string id) {
    var result = await GetResults();

    if (result is null) {
      return NotFound("Could not find any results.");
    }

    var grade = result.Grades.FirstOrDefault(p => p.Id == id);

    if (grade is null) {
      return NotFound("Could not find any results.");
    }

    return new SingleGradeResult {
      EventDate = result.EventDate,
      EventGroupName = result.EventGroupName,
      EventName = result.EventName,
      Grade = grade
    };
  }

  [HttpGet(UrlConstants.Grades)]
  [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
  public async Task<ActionResult<List<EventGrade>>> GetGrades() {
    var result = await GetResults();

    if (result is null) {
      return new List<EventGrade>();
    }

    return result.Grades.Select(p => new EventGrade {
      Id = p.Id,
      Name = p.Name
    }).ToList();
  }

  [HttpGet(UrlConstants.Stats)]
  [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
  public async Task<ActionResult<EventStatistic>> Stats() {
    var result = await GetResults();
    return result.EventStats;
  }

  private async Task<EventResult> GetResults() {
    return await _CacheService.Get("results", _ProcessedResultService.Get);
  }
}