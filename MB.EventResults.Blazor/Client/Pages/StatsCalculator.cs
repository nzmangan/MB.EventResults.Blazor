namespace MB.EventResults.Blazor.Client;

public class StatsCalculator : IStatsCalculator {

  public Stat CalculateStat(IEnumerable<double?> enumerable) {
    // Filter out any null values from the collection
    var validValues = enumerable.Where(x => x.HasValue).Select(x => x.Value).ToList();

    // Create a Stat object to hold the calculated statistics
    Stat stat = new() {
      Records = validValues.Count
    };

    if (stat.Records > 0) {
      // Avarage (mean) is the sum of values divided by count
      stat.Avarage = validValues.Average();

      // Median is the middle value of the sorted collection
      stat.Median = CalculateMedian(validValues);

      // Min and Max values
      stat.Min = validValues.Min();
      stat.Max = validValues.Max();

      // Standard Deviation: sqrt(sum((x - mean)^2) / count)
      stat.Variance = validValues.Average(v => Math.Pow(v - stat.Median.Value, 2));
      stat.StandardDeviation = Math.Sqrt(stat.Variance);
    }

    return stat;
  }

  private static double? CalculateMedian(List<double> sortedValues) {
    int count = sortedValues.Count;

    if (count == 0) return null;

    // Sort the values
    var sortedList = sortedValues.OrderBy(x => x).ToList();

    if (count % 2 == 1) {
      // Odd number of elements, return the middle element
      return sortedList[count / 2];
    } else {
      // Even number of elements, return the average of the two middle elements
      double middle1 = sortedList[(count / 2) - 1];
      double middle2 = sortedList[count / 2];
      return (middle1 + middle2) / 2.0;
    }
  }
}