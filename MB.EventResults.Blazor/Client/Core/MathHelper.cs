namespace MB.EventResults.Blazor.Client;

public static class MathHelper {
  public static double StandardDeviation(this IEnumerable<double?> sequence) {
    var sequenceWithOutNull = sequence.Where(p => p is not null).Select(p => p.Value);

    if (sequenceWithOutNull is null || !sequenceWithOutNull.Any()) {
      return 0;
    }

    double average = sequenceWithOutNull.Average();
    double sum = sequenceWithOutNull.Sum(d => Math.Pow(d - average, 2));
    return Math.Sqrt((sum) / sequence.Count());
  }
}