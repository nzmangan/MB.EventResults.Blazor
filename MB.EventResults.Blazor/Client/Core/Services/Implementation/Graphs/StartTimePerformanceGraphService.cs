namespace MB.EventResults.Blazor.Client;

public class StartTimePerformanceGraphService : IStartTimePerformanceGraphService {
  public const string Label = "Start Time vs Performance";

  public string ChartType => "scatter";

  public string OptionType => "StartTimePerformance";

  public List<DataSerie<ScatterPoint>> GetDataSerie(GradeResult response) {
    return [.. DatasetHelper.BuildSerie(response, GetValues).Where(p => p.Data.Count > 0)];
  }

  public List<string> GetLabels(GradeResult response) {
    return [];
  }

  public List<ScatterPoint> GetValues(Runner runner) {
    if (!runner.IsFinisher() || !runner.StartTime.HasValue || !runner.PerformanceIndex.HasValue) {
      return [];
    }

    return [new ScatterPoint {
      X = runner.StartTime.Value.TimeOfDay.TotalSeconds,
      Y = (runner.PerformanceIndex.Value * 100).Round(1)
    }];
  }
}
