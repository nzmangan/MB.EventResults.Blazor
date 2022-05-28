namespace MB.EventResults.Blazor.Shared;

public class EventResultRecord {
  public string EventGroupName { get; set; }
  public string EventName { get; set; }
  public DateTime EventDate { get; set; }
  public Guid EventId { get; set; }
  //public EventFile EventFile { get; set; }
  public string WinsplitsId { get; set; }
}