namespace MB.EventResults.Blazor.Client.Components.Graphs;

public partial class HallOfFame {
  private List<NameAndValue<int>> _LegWins;
  private List<NameAndValue<int>> _Social;
  private List<NameAndValue<int>> _LeastNumberOfMistakes;
  private List<NameAndValue<int>> _MostNumberOfMistakes;
  private List<NameAndValue<double>> _LeastMistakesTimewise;
  private List<NameAndValue<double>> _MostMistakesTimewise;
  private List<NameAndValue<double>> _MostConsistant;
  private List<NameAndValue<double>> _LeastConsistant;

  [Parameter]
  public GradeResult Result { get; set; }

  protected override void OnParametersSet() {
    Processor();
  }

  private void Processor() {
    _LegWins = Result
      .Runners
      .Select(p => new NameAndValue<int> { Name = p.Name, Club = p.Club, Value = p.Splits.Where(p => p.LegPosition == 1).Count() })
      .Where(p => p.Value > 0)
      .OrderByDescending(p => p.Value)
      .ToList();

    _Social = Result
      .Runners
      .Select(p => new NameAndValue<int> { Name = p.Name, Club = p.Club, Value = p.Splits.Where(p => p.Pack is not null && p.Pack.Count > 0).Count() })
      .Where(p => p.Value > 0)
      .OrderByDescending(p => p.Value)
      .ToList();

    _LeastNumberOfMistakes = Result
      .Runners
      .Select(p => new NameAndValue<int> { Name = p.Name, Club = p.Club, Value = p.Splits.Where(p => p.TimeLoss.HasValue).Count() })
      .OrderBy(p => p.Value)
      .ToList();

    _MostNumberOfMistakes = Result
      .Runners
      .Select(p => new NameAndValue<int> { Name = p.Name, Club = p.Club, Value = p.Splits.Where(p => p.TimeLoss.HasValue).Count() })
      .OrderByDescending(p => p.Value)
      .ToList();

    _LeastMistakesTimewise = Result
      .Runners
      .Select(p => new NameAndValue<double> { Name = p.Name, Club = p.Club, Value = p.Splits.Sum(p => p.TimeLoss ?? 0) })
      .OrderBy(p => p.Value)
      .ToList();

    _MostMistakesTimewise = Result
      .Runners
      .Select(p => new NameAndValue<double> { Name = p.Name, Club = p.Club, Value = p.Splits.Sum(p => p.TimeLoss ?? 0) })
      .Where(p => p.Value > 0)
      .OrderByDescending(p => p.Value)
      .ToList();

    _MostConsistant = Result
      .Runners
      .Select(p => new NameAndValue<double> { Name = p.Name, Club = p.Club, Value = p.Splits.Select(p => p.PerformanceIndexAdjusted).StandardDeviation() })
      .OrderBy(p => p.Value)
      .ToList();

    _LeastConsistant = Result
      .Runners
      .Select(p => new NameAndValue<double> { Name = p.Name, Club = p.Club, Value = p.Splits.Select(p => p.PerformanceIndexAdjusted).StandardDeviation() })
      .OrderByDescending(p => p.Value)
      .ToList();
  }
}