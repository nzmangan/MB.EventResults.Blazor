namespace MB.EventResults.Blazor.Client.Components.Graphs;

public partial class EventSummary {
  private List<EventSummaryRow> _Rows = [];

  [Parameter]
  public List<GradeResult> Grades { get; set; }

  [Inject]
  public ITextService TextService { get; set; }

  protected override void OnParametersSet() {
    _Rows = [.. Grades.Select(grade => {
      var runners = grade.Runners ?? [];
      var finished = runners.Count(p => p.IsFinisher());
      var didNotStart = runners.Count(p => p.Status == ReportHelper.StatusDidNotStart);
      var didNotFinish = runners.Count(p => p.Status == ReportHelper.StatusDidNotFinish);
      var missingPunch = runners.Count(p => p.Status == ReportHelper.StatusMissingPunch);

      return new EventSummaryRow {
        Name = grade.Name,
        Entries = runners.Count,
        DidNotStart = didNotStart,
        Started = runners.Count - didNotStart,
        Finished = finished,
        DidNotFinish = didNotFinish,
        MissingPunch = missingPunch,
        Other = runners.Count - finished - didNotStart - didNotFinish - missingPunch,
        WinningTime = runners.Where(p => p.IsFinisher()).Min(p => p.TimeInSeconds)
      };
    })];
  }

  private class EventSummaryRow {
    public string Name { get; set; }
    public int Entries { get; set; }
    public int DidNotStart { get; set; }
    public int Started { get; set; }
    public int Finished { get; set; }
    public int DidNotFinish { get; set; }
    public int MissingPunch { get; set; }
    public int Other { get; set; }
    public double? WinningTime { get; set; }
  }
}
