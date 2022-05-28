namespace MB.EventResults.Blazor.Client;

public class MistakeTotalGraphService : IMistakeTotalGraphService {
  public const string Label = "Mistakes (Total)";

  public string ChartType => "bar";

  public string OptionType => "Mistakes";
  public List<DataSerie<double>> GetDataSerie(GradeResult response) {
    return DatasetHelper.BuildSerie(response, GetValues);
  }

  public List<string> GetLabels(GradeResult response) {
    return new() { "Total" };
  }

  public List<double> GetValues(Runner runner) {
    return new() {
      runner.Splits.Sum(p => p.TimeLoss ?? 0)
    };
  }
}