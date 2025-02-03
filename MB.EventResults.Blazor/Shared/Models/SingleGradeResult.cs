namespace MB.EventResults.Blazor.Shared;

public class SingleGradeResult {
  public string EventGroupName { get; set; }
  public string EventName { get; set; }
  public DateTime EventDate { get; set; }
  public GradeResult Grade { get; set; }
}