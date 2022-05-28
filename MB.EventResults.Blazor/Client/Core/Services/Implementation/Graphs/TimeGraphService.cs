namespace MB.EventResults.Blazor.Client;

public class TimeGraphService : ITimeGraphService {
  public const string Label = "Time";

  private readonly IGraphOptionsService _GraphOptionsService;

  public TimeGraphService(IGraphOptionsService graphOptionsService) {
    _GraphOptionsService = graphOptionsService;
  }

  public string ChartType => "line";

  public string OptionType => "Time";

  public List<DataSerie<double?>> GetDataSerie(GradeResult response) {
    return DatasetHelper.BuildSerie(response, GetValues);
  }

  public List<string> GetLabels(GradeResult response) {
    return ChartDefaults.GetChartLabels(response.Codes, true);
  }

  public List<double?> GetValues(Runner runner) {
    List<double?> items = new();

    if (_GraphOptionsService.Reference?.Name == "superman") {
      double timeBehind = 0;
      for (var j = 0; j < runner.Splits.Count; j++) {
        var split = runner.Splits[j];

        if (split.LegTimeBehind.HasValue) {
          timeBehind += split.LegTimeBehind.Value;
          items.Add(timeBehind * -1);
        } else {
          items.Add(null);
        }
      }

      items.Insert(0, 0);
      return items;
    }

    var referenceRunner = _GraphOptionsService.Reference;

    if (referenceRunner is not null) {
      for (var j = 0; j < runner.Splits.Count; j++) {
        var split = runner.Splits[j];
        var referenceRunnerSplit = referenceRunner.Splits.FirstOrDefault(p => p.PreviousCode == split.PreviousCode && p.Code == split.Code);

        if (referenceRunnerSplit != null && split.TotalBehind.HasValue && referenceRunnerSplit.TotalBehind.HasValue) {
          items.Add((split.TotalBehind.Value - referenceRunnerSplit.TotalBehind.Value) * -1);
        } else {
          items.Add(null);
        }
      }
    } else {
      items = runner.Splits.Select(p => p.TotalBehind.HasValue ? p.TotalBehind.Value * -1 : (double?)null).ToList();
    }

    items.Insert(0, 0);

    return items;
  }
}