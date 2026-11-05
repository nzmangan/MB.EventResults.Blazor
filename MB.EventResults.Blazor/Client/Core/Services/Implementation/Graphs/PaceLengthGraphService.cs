using System.Globalization;

namespace MB.EventResults.Blazor.Client;

public class PaceLengthGraphService : IPaceLengthGraphService {
  public const string Label = "Pace vs Leg Length";

  private Dictionary<(string PreviousCode, string Code), double> _Distances = [];

  public string ChartType => "scatter";

  public string OptionType => "PaceLength";

  public bool HasData(GradeResult response) {
    return GetDataSerie(response).Count > 0;
  }

  public List<DataSerie<ScatterPoint>> GetDataSerie(GradeResult response) {
    _Distances = GetDistances(response);
    return [.. DatasetHelper.BuildSerie(response, GetValues).Where(p => p.Data.Count > 0)];
  }

  public List<string> GetLabels(GradeResult response) {
    return [];
  }

  public List<ScatterPoint> GetValues(Runner runner) {
    return [.. (runner.Splits ?? [])
      .Where(p => p.KmRate.HasValue && _Distances.ContainsKey((p.PreviousCode, p.Code)))
      .Select(p => new ScatterPoint {
        X = _Distances[(p.PreviousCode, p.Code)],
        Y = p.KmRate.Value.Round(2)
      })];
  }

  // Distances are only there when a course file with control positions has been published.
  private static Dictionary<(string PreviousCode, string Code), double> GetDistances(GradeResult response) {
    var distances = new Dictionary<(string PreviousCode, string Code), double>();
    var legs = response.Legs ?? [];

    for (var i = 0; i < legs.Count; i++) {
      var previousCode = i == 0 ? Constants.StartCode : legs[i - 1].Id;

      if (Double.TryParse(legs[i].Distance?.TrimEnd('m'), NumberStyles.Number, CultureInfo.InvariantCulture, out var distance)) {
        distances.TryAdd((previousCode, legs[i].Id), distance);
      }
    }

    return distances;
  }
}
