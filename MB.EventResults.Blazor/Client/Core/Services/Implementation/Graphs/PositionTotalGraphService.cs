namespace MB.EventResults.Blazor.Client;

public class PositionTotalGraphService : IPositionTotalGraphService {
  public const string Label = "Position (Total)";

  public string ChartType => "line";

  public string OptionType => "Position";

  public List<DataSerie<int?>> GetDataSerie(GradeResult response) {
    return DatasetHelper.BuildSerie(response, GetValues);
  }

  public List<string> GetLabels(GradeResult response) {
    return ChartDefaults.GetChartLabels(response.Codes, false);
  }

  public List<int?> GetValues(Runner runner) {
    return runner.Splits.Select(p => p.TotalPosition).ToList();
  }
}