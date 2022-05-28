using MB.OResults.Core;

namespace MB.EventResults.Blazor.Shared;

public class EventResult {
  public string EventGroupName { get; set; }
  public string EventName { get; set; }
  public DateTime EventDate { get; set; }
  public List<GradeResult> Grades { get; set; } = new();
}
