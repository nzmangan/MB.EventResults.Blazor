namespace MB.EventResults.Blazor.Client;

public class TextService : ITextService {
  private readonly Dictionary<string, string> Current = Translations.English;

  private MarkupString Get(string key) {
    return (MarkupString)(Current.TryGetValue(key, out var s) ? s : "");
  }

  public MarkupString ShowSplits => Get("ShowSplits");

  public MarkupString ShowTotals => Get("ShowTotals");

  public MarkupString ShowRelativeLegTimes => Get("ShowRelativeLegTimes");

  public MarkupString ShowRelativeTotalTimes => Get("ShowRelativeTotalTimes");

  public MarkupString ShowActualTime => Get("ShowActualTime");

  public MarkupString ShowPerformanceIndex => Get("ShowPerformanceIndex");

  public MarkupString ShowNormalizedPerformanceIndex => Get("ShowNormalizedPerformanceIndex");

  public MarkupString ShowPredictedLegTime => Get("ShowPredictedLegTime");

  public MarkupString ShowPredictedLegLoss => Get("ShowPredictedLegLoss");

  public MarkupString ShowPack => Get("ShowPack");

  public MarkupString ShowDistance => Get("ShowDistance");

  public MarkupString ShowMinPerKmRate => Get("ShowMinPerKmRate");

  public MarkupString ShowAll => Get("ShowAll");

  public MarkupString Result => Get("Result");

  public MarkupString Start => Get("Start");

  public MarkupString Finish => Get("Finish");

  public MarkupString EstimatedTimeLoss => Get("EstimatedTimeLoss");

  public MarkupString MinPerKm => Get("MinPerKm");

  public MarkupString LegWins => Get("LegWins");

  public MarkupString SocialControls => Get("SocialControls");

  public MarkupString LeastNumberOfMistakes => Get("LeastNumberOfMistakes");

  public MarkupString MostNumberOfMistakes => Get("MostNumberOfMistakes");

  public MarkupString LeastMistakesTimewise => Get("LeastMistakesTimewise");

  public MarkupString MostMistakesTimewise => Get("MostMistakesTimewise");

  public MarkupString MostConsistant => Get("MostConsistant");

  public MarkupString LeastConsistant => Get("LeastConsistant");

  public MarkupString HallOfFame => Get("HallOfFame");

  public MarkupString Reference => Get("Reference");

  public MarkupString Fastest => Get("Fastest");

  public MarkupString Superman => Get("Superman");

  public MarkupString Class => Get("Class");

  public MarkupString ReportType => Get("ReportType");

  public MarkupString AsOf => Get("AsOf");
}