using MB.OResults.Core;

namespace MB.EventResults.Blazor.Client;

public static class DatasetHelper {
  public static List<DataSerie<T>> BuildSerie<T>(GradeResult response, Func<Runner, List<T>> func) {
    return Enumerable.Range(0, response.Runners.Count).Select(i =>
      new DataSerie<T> {
        Label = response.Runners[i].Name,
        Data = func(response.Runners[i]),
        Color = ChartDefaults.Colors[i % ChartDefaults.Colors.Count],
      }).ToList();
  }
}