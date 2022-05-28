namespace MB.EventResults.Blazor.Client;

public class PerformanceIndexHistogramGraphService : IPerformanceIndexHistogramGraphService {
  public const string Label = "Performance Index (Histogram)";

  public string ChartType => "line";

  public string OptionType => "PerformanceHistogram";

  public List<DataSerie<int>> GetDataSerie(GradeResult response) {
    return DatasetHelper.BuildSerie(response, GetValues);
  }

  public List<string> GetLabels(GradeResult response) {
    List<string> chartLabels = new();

    for (var j = 0; j <= (200 / ChartDefaults.HistogramStep); j++) {
      chartLabels.Add((j * ChartDefaults.HistogramStep).ToString() + '%');
    }

    return chartLabels;
  }

  public List<int> GetValues(Runner runner) {
    List<int> items = new();

    for (var i = 0; i < (200 / ChartDefaults.HistogramStep); i++) {
      items.Add(0);
    }

    for (var i = 0; i < runner.Splits.Count; i++) {
      if (runner.Splits[i].PerformanceIndex.HasValue) {
        for (var s = 0; s < (200 / ChartDefaults.HistogramStep); s++) {
          var boundry = ((s + 1) * ChartDefaults.HistogramStep) - (ChartDefaults.HistogramStep / 2);

          if ((runner.Splits[i].PerformanceIndex * 100) < boundry) {
            items[s] = items[s] + 1;
            break;
          }
        }
      }
    }

    return items;
  }
}