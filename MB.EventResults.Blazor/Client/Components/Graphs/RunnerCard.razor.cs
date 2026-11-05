namespace MB.EventResults.Blazor.Client.Components.Graphs;

public partial class RunnerCard {
  private Runner _Runner;
  private List<Split> _TimedLegs = [];
  private Split _BestLeg;
  private Split _WorstLeg;
  private string _RanNear;

  [Parameter]
  public GradeResult Result { get; set; }

  [Inject]
  public ITextService TextService { get; set; }

  protected override void OnParametersSet() {
    Show(Result.Runners.Contains(_Runner) ? _Runner : Result.Runners.FirstOrDefault());
  }

  private void SelectRunner(ChangeEventArgs e) {
    Show(Result.Runners.FirstOrDefault(p => p.Name == e.Value.ToString()));
  }

  private void Show(Runner runner) {
    _Runner = runner;

    if (runner is null) {
      return;
    }

    _TimedLegs = [.. (runner.Splits ?? []).Where(p => p.Leg.HasValue)];

    var rated = _TimedLegs.Where(p => p.PerformanceIndex.HasValue).OrderByDescending(p => p.PerformanceIndex).ToList();
    _BestLeg = rated.FirstOrDefault();
    _WorstLeg = rated.LastOrDefault();

    _RanNear = String.Join(", ", _TimedLegs
      .SelectMany(p => p.Pack ?? [])
      .GroupBy(p => p.Name)
      .OrderByDescending(p => p.Count())
      .Take(3)
      .Select(p => $"{p.Key} ({p.Count()})"));
  }

  private static string FormatLeg(Split leg) {
    return leg is null ? "" : $"{leg.LegName()} ({leg.PerformanceIndex.Round("%")})";
  }
}
