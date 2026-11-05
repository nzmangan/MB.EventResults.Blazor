namespace MB.EventResults.Blazor.Client.Components.Graphs;

public partial class LegWinners {
  private List<LegWinnerRow> _Rows = [];

  [Parameter]
  public GradeResult Result { get; set; }

  [Inject]
  public ITextService TextService { get; set; }

  protected override void OnParametersSet() {
    _Rows = [.. Result.GetCourseLegs()
      .Select((leg, index) => {
        var splits = Result.GetTimedSplits(leg);

        if (splits.Count == 0) {
          return null;
        }

        var best = splits.Min(p => p.Split.Leg.Value);
        var winners = splits.Where(p => p.Split.Leg == best).ToList();
        var nextTime = splits.Where(p => p.Split.Leg > best).Select(p => p.Split.Leg).Min();

        return new LegWinnerRow {
          Number = index + 1,
          Name = leg.LegName(),
          Winners = String.Join(", ", winners.Select(p => p.Runner.Name)),
          Clubs = String.Join(", ", winners.Select(p => p.Runner.Club).Distinct()),
          Time = best,
          Second = String.Join(", ", splits.Where(p => nextTime.HasValue && p.Split.Leg == nextTime).Select(p => p.Runner.Name)),
          Gap = nextTime - best
        };
      })
      .Where(p => p is not null)];
  }

  private class LegWinnerRow {
    public int Number { get; set; }
    public string Name { get; set; }
    public string Winners { get; set; }
    public string Clubs { get; set; }
    public double Time { get; set; }
    public string Second { get; set; }
    public double? Gap { get; set; }
  }
}
