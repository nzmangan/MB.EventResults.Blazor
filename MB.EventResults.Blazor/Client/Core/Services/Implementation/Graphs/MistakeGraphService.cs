namespace MB.EventResults.Blazor.Client;

public class MistakeGraphService : IMistakeGraphService {
  public const string Label = "Mistakes";

  public string ChartType => "bar";

  public string OptionType => "Mistakes";

  public List<DataSerie<double>> GetDataSerie(GradeResult response) {
    return DatasetHelper.BuildSerie(response, GetValues);
  }

  public List<string> GetLabels(GradeResult response) {
    return ChartDefaults.GetChartLabels(response.Codes, true);
  }

  public List<double> GetValues(Runner runner) {
    var items = runner.Splits.Select(p => p.TimeLoss.HasValue ? (p.TimeLoss.Value * -1).Round(0) : 0).ToList();
    items.Insert(0, 0);
    return items;
  }
}