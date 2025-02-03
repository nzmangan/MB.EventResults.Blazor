using IOF.XML.V3;

namespace MB.EventResults.Blazor.Client.Pages;

[Route("/statistics")]
public partial class Statistics {
  private List<string> _Grades;
  private bool _Loading = false;
  private EventStatistic _Statistic = null;
  private List<CalculatedStat> _CurrentData = [];
  private List<string> _Headings = ["PerformanceIndexAdjusted", "PerformanceIndex", "KmRate", "LegTime", "TimeLoss"];

  [Inject]
  public IDataClient DataClient { get; set; }

  [Inject]
  public ITextService TextService { get; set; }

  [Inject]
  public IStatsCalculator StatsCalculator { get; set; }

  protected void SelectClass(ChangeEventArgs e) {
    var selectedGrade = e.Value.ToString();
    var filteredLegs = _Statistic.Legs.Where(p => p.Grade == selectedGrade || selectedGrade == "All");

    _CurrentData = filteredLegs.GroupBy(c => c.Leg).Select(p => new CalculatedStat {
      Leg = p.Key,
      KmRate = StatsCalculator.CalculateStat(p.Select(p => p.KmRate)),
      PerformanceIndex = StatsCalculator.CalculateStat(p.Select(p => p.PerformanceIndex)),
      //ActualTime = CalculateStat(p.Select(p => p.ActualTime)),
      LegTime = StatsCalculator.CalculateStat(p.Select(p => p.LegTime)),
      PerformanceIndexAdjusted = StatsCalculator.CalculateStat(p.Select(p => p.PerformanceIndexAdjusted)),
      TimeLoss = StatsCalculator.CalculateStat(p.Select(p => p.TimeLoss)),
    }).OrderBy(p => p.Leg).ToList();
  }

  protected async override Task OnInitializedAsync() {
    _Loading = true;
    _Statistic = await DataClient.Stats();
    var grades = _Statistic.Legs.Select(x => x.Grade).Distinct().OrderBy(p => p).ToList();
    grades.Insert(0, "All");
    _Grades = grades;
    _Loading = false;
  }
}