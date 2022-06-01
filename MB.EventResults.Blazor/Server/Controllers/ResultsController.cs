using MB.EventResults.Blazor.Server;
using Microsoft.AspNetCore.Mvc;

namespace MB.EventResults.Blazor.Shared;

[ApiController]
public class ResultsController : Controller {
  private readonly IProcessedResultService _ResultService;

  public ResultsController(IProcessedResultService resultService) {
    _ResultService = resultService;
  }

  [HttpGet(UrlConstants.Get)]
  [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
  public async Task<ActionResult<EventResult>> Get() {
    var result = await _ResultService.Get();

    if (result is null) {
      return NotFound("Could not find any results.");
    }

    return result;
  }

  [HttpGet(UrlConstants.GetClass)]
  [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
  public async Task<ActionResult<SingleGradeResult>> GetClass(string id) {
    var result = await _ResultService.Get();

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
  public async Task<ActionResult<List<EventGrade>>> GetGrades(string id) {
    var result = await _ResultService.Get();

    if (result is null) {
      return new List<EventGrade>();
    }

    return result.Grades.Select(p => new EventGrade {
      Id = p.Id,
      Name = p.Name
    }).ToList();
  }
}
