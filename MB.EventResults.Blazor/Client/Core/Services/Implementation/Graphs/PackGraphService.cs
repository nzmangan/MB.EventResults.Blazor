namespace MB.EventResults.Blazor.Client;

public class PackGraphService : IPackGraphService {
  public const string Label = "Pack";

  public string ChartType => "line";

  public string OptionType => "Pack";

  public List<DataSerie<double?>> GetDataSerie(GradeResult response) {
    return DatasetHelper.BuildSerie(response, GetValues);
  }

  public List<string> GetLabels(GradeResult response) {
    return ChartDefaults.GetChartLabels(response.Codes, true);
  }

  public List<double?> GetValues(Runner runner) {
    if (!runner.StartTime.HasValue) {
      return null;
    }

    return runner.Splits.Select(p => {
      if (!p.ActualTime.HasValue) {
        return (double?)null;
      }

      return (p.ActualTime.Value - p.ActualTime.Value.Date).TotalSeconds;
    }).ToList();
  }
}