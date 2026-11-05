namespace MB.EventResults.Blazor.Client.Components.Graphs;

public partial class PackEffect {
  private List<PackEffectRow> _Rows = [];

  [Parameter]
  public GradeResult Result { get; set; }

  [Inject]
  public ITextService TextService { get; set; }

  protected override void OnParametersSet() {
    _Rows = [.. Result.Runners
      .Select(runner => {
        var rated = (runner.Splits ?? []).Where(p => p.PerformanceIndexAdjusted.HasValue).ToList();
        var nearOthers = rated.Where(p => p.Pack is not null && p.Pack.Count > 0).ToList();
        var alone = rated.Except(nearOthers).ToList();

        return new PackEffectRow {
          Name = runner.Name,
          Club = runner.Club,
          LegsNearOthers = nearOthers.Count,
          LegsAlone = alone.Count,
          PaceNearOthers = nearOthers.Count > 0 ? nearOthers.Average(p => p.PerformanceIndexAdjusted) : null,
          PaceAlone = alone.Count > 0 ? alone.Average(p => p.PerformanceIndexAdjusted) : null
        };
      })
      .Where(p => p.LegsNearOthers > 0 && p.LegsAlone > 0)
      .OrderByDescending(p => p.PaceNearOthers - p.PaceAlone)];
  }

  private static string FormatDifference(double? difference) {
    return difference > 0 ? $"+{difference.Round("%")}" : difference.Round("%");
  }

  private class PackEffectRow {
    public string Name { get; set; }
    public string Club { get; set; }
    public int LegsNearOthers { get; set; }
    public int LegsAlone { get; set; }
    public double? PaceNearOthers { get; set; }
    public double? PaceAlone { get; set; }
  }
}
