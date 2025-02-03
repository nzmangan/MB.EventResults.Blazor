namespace MB.EventResults.Blazor.Client;

public class PerformanceIndexGraphService : IPerformanceIndexGraphService {
  public const string Label = "Performance Index";

  public string ChartType => "line";

  public string OptionType => "Performance";

  public List<DataSerie<double?>> GetDataSerie(GradeResult response) {
    return DatasetHelper.BuildSerie(response, GetValues);
  }

  public List<string> GetLabels(GradeResult response) {
    return ChartDefaults.GetChartLabels(response.Legs.Select(p => p.Id).ToList(), false);
  }

  public List<double?> GetValues(Runner runner) {
    return runner.Splits.Select(p => p.PerformanceIndex.HasValue ? (p.PerformanceIndex * 100).Round(1) : null).ToList();
  }
}