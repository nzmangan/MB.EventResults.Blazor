namespace MB.EventResults.Blazor.Client;

internal class ChartDefaults {
  public static int HistogramStep = 5;

  public static List<string> Colors = new List<string>{
      "#C0392B", "#9B59B6",
      "#2980B9", "#1ABC9C",
      "#2ECC71", "#F39C12",
      "#D35400", "#7F8C8D",
      "#2C3E50", "#E74C3C",
      "#8E44AD", "#3498DB",
      "#27AE60", "#F1C40F",
      "#E67E22", "#95A5A6",
      "#34495E"
    };

  public static List<string> GetChartLabels(List<string> codes, bool addStart) {
    List<string> chartLabels = new();

    if (addStart) {
      chartLabels.Add("S");
    }

    chartLabels.AddRange(Enumerable.Range(1, codes.Count).Select(p => p.ToString()));
    chartLabels.Add("F");

    return chartLabels;
  }
}