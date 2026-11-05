namespace MB.EventResults.Blazor.Client;

public static class ReportHelper {
  public const string StatusDidNotStart = "DNS";
  public const string StatusDidNotFinish = "DNF";
  public const string StatusMissingPunch = "MP";

  public static bool IsFinisher(this Runner runner) {
    return runner.Status == Constants.StatusOK && runner.TimeInSeconds.HasValue;
  }

  // Places are worked out from the times, because not every result file includes them.
  public static int GetPlace(this Runner runner, IEnumerable<Runner> runners) {
    return runners.Count(p => p.IsFinisher() && p.TimeInSeconds < runner.TimeInSeconds) + 1;
  }

  // The legs in course order, taken from the runner with the most splits.
  public static List<Split> GetCourseLegs(this GradeResult result) {
    var splits = result.Runners.Select(p => p.Splits ?? []).OrderByDescending(p => p.Count).FirstOrDefault() ?? [];
    return [.. splits.DistinctBy(p => (p.PreviousCode, p.Code))];
  }

  public static List<(Runner Runner, Split Split)> GetTimedSplits(this GradeResult result, Split leg) {
    return [.. result.Runners.SelectMany(runner => (runner.Splits ?? [])
      .Where(p => p.PreviousCode == leg.PreviousCode && p.Code == leg.Code && p.Leg.HasValue)
      .Select(p => (runner, p)))];
  }

  public static string LegName(this Split leg) {
    return $"{leg.PreviousCode} - {leg.Code}";
  }

  public static string Percent(int part, int total) {
    return total == 0 ? "" : $"{Math.Round(100d * part / total)}%";
  }

  public static string FormatWholeSeconds(this double? time) {
    return time.Round(0).FormatTime();
  }
}
