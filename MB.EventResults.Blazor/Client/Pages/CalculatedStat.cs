namespace MB.EventResults.Blazor.Client.Pages;

public class CalculatedStat {
  public Stat KmRate { get; set; }
  public Stat PerformanceIndex { get; set; }
  public Stat ActualTime { get; set; }
  public Stat LegTime { get; set; }
  public Stat PerformanceIndexAdjusted { get; set; }
  public Stat TimeLoss { get; set; }
  public int Leg { get; set; }
}