namespace MB.EventResults.Blazor.Client;

public class SpeedAccuracyGraphService : ISpeedAccuracyGraphService {
  public const string Label = "Speed vs Accuracy";

  public string ChartType => "scatter";

  public string OptionType => "SpeedAccuracy";

  public List<DataSerie<ScatterPoint>> GetDataSerie(GradeResult response) {
    return [.. DatasetHelper.BuildSerie(response, GetValues).Where(p => p.Data.Count > 0)];
  }

  public List<string> GetLabels(GradeResult response) {
    return [];
  }

  public List<ScatterPoint> GetValues(Runner runner) {
    var cleanLegs = (runner.Splits ?? []).Where(p => p.PerformanceIndex.HasValue && !p.TimeLoss.HasValue).ToList();

    if (!runner.IsFinisher() || cleanLegs.Count == 0) {
      return [];
    }

    return [new ScatterPoint {
      X = (cleanLegs.Average(p => p.PerformanceIndex.Value) * 100).Round(1),
      Y = (runner.TimeLoss ?? 0).Round(0)
    }];
  }
}
