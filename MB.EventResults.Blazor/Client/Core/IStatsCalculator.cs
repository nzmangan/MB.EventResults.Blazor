namespace MB.EventResults.Blazor.Client;

public interface IStatsCalculator {
  Stat CalculateStat(IEnumerable<double?> enumerable);
}