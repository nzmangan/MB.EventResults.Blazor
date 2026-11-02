namespace MB.EventResults.Blazor.Client.Components.Graphs;

public partial class CleanRace {
  private List<CleanRaceRow> _Rows = [];

  [Parameter]
  public GradeResult Result { get; set; }

  [Inject]
  public ITextService TextService { get; set; }

  protected override void OnParametersSet() {
    Processor();
  }

  private void Processor() {
    var rows = Result
      .Runners
      .Where(p => p.Status == Constants.StatusOK && p.TimeInSeconds.HasValue)
      .Select(p => {
        var timeLoss = (p.TimeLoss ?? 0).Round(0);

        return new CleanRaceRow {
          Name = p.Name,
          Club = p.Club,
          ActualTime = p.TimeInSeconds.Value,
          TimeLoss = timeLoss,
          CleanTime = p.TimeInSeconds.Value - timeLoss
        };
      })
      .ToList();

    // Places are worked out from the times, because not every result file includes them.
    foreach (var row in rows) {
      row.ActualPlace = rows.Count(p => p.ActualTime < row.ActualTime) + 1;
      row.CleanPlace = rows.Count(p => p.CleanTime < row.CleanTime) + 1;
    }

    _Rows = [.. rows.OrderBy(p => p.CleanTime).ThenBy(p => p.ActualTime)];
  }

  private static string FormatPlaceChange(int change) {
    return change switch {
      > 0 => $"+{change}",
      < 0 => change.ToString(),
      _ => "="
    };
  }

  private class CleanRaceRow {
    public string Name { get; set; }
    public string Club { get; set; }
    public double ActualTime { get; set; }
    public double TimeLoss { get; set; }
    public double CleanTime { get; set; }
    public int ActualPlace { get; set; }
    public int CleanPlace { get; set; }
  }
}
