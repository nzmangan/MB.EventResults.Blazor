namespace MB.EventResults.Blazor.Client.Components.Graphs {
  public partial class Matrix {
    public bool _Splits = true;
    public bool _Totals = true;
    public bool _RelativeLegTimes = false;
    public bool _RelativeTotalTimes = false;
    public bool _Actual = false;
    public bool _Performance = false;
    public bool _NormalizedPerformance = false;
    public bool _PredictedTime = false;
    public bool _PredictedLoss = false;
    public bool _Pack = false;
    public bool _KmRate = false;

    [Parameter]
    public GradeResult Result { get; set; }

    public List<Runner> _VisibleRunners = new();
    public List<string> _HiddenRunners = new();

    protected override void OnInitialized() {
      FilterRunners();
    }

    private void HideRunner(Runner runner) {
      _HiddenRunners.Add(runner.Name);
      FilterRunners();
    }

    private void ShowAll() {
      _HiddenRunners = new();
      FilterRunners();
    }

    private void FilterRunners() {
      _VisibleRunners = Result.Runners.Where(p => !_HiddenRunners.Contains(p.Name)).ToList();
    }
  }
}