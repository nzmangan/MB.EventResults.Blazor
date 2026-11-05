namespace MB.EventResults.Blazor.Client.Components.Graphs;

public partial class HardestControls {
  private List<HardestControlRow> _Rows = [];

  [Parameter]
  public GradeResult Result { get; set; }

  [Inject]
  public ITextService TextService { get; set; }

  protected override void OnParametersSet() {
    _Rows = [.. Result.GetCourseLegs()
      .Select((leg, index) => {
        var splits = Result.GetTimedSplits(leg);
        var losses = splits.Where(p => p.Split.TimeLoss.HasValue).Select(p => p.Split.TimeLoss.Value).ToList();

        return new HardestControlRow {
          Number = index + 1,
          Name = leg.LegName(),
          Runners = splits.Count,
          Mistakes = losses.Count,
          TotalTimeLost = losses.Sum(),
          AverageTimeLost = losses.Count > 0 ? losses.Average() : null
        };
      })
      .Where(p => p.Runners > 0)
      .OrderByDescending(p => p.TotalTimeLost)
      .ThenByDescending(p => p.Mistakes)];
  }

  private class HardestControlRow {
    public int Number { get; set; }
    public string Name { get; set; }
    public int Runners { get; set; }
    public int Mistakes { get; set; }
    public double? TotalTimeLost { get; set; }
    public double? AverageTimeLost { get; set; }
  }
}
