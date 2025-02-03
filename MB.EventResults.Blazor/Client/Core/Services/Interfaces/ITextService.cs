namespace MB.EventResults.Blazor.Client;

public interface ITextService {
  public MarkupString ShowSplits { get; }
  public MarkupString ShowTotals { get; }
  public MarkupString ShowRelativeLegTimes { get; }
  public MarkupString ShowRelativeTotalTimes { get; }
  public MarkupString ShowActualTime { get; }
  public MarkupString ShowPerformanceIndex { get; }
  public MarkupString ShowNormalizedPerformanceIndex { get; }
  public MarkupString ShowPredictedLegTime { get; }
  public MarkupString ShowPredictedLegLoss { get; }
  public MarkupString ShowPack { get; }
  public MarkupString ShowDistance { get; }
  public MarkupString ShowMinPerKmRate { get; }
  public MarkupString ShowAll { get; }
  public MarkupString Result { get; }
  public MarkupString Start { get; }
  public MarkupString Finish { get; }
  public MarkupString EstimatedTimeLoss { get; }
  public MarkupString MinPerKm { get; }
  public MarkupString LegWins { get; }
  public MarkupString SocialControls { get; }
  public MarkupString LeastNumberOfMistakes { get; }
  public MarkupString MostNumberOfMistakes { get; }
  public MarkupString LeastMistakesTimewise { get; }
  public MarkupString MostMistakesTimewise { get; }
  public MarkupString MostConsistant { get; }
  public MarkupString LeastConsistant { get; }
  public MarkupString HallOfFame { get; }
  public MarkupString Reference { get; }
  public MarkupString Fastest { get; }
  public MarkupString Superman { get; }
  public MarkupString Class { get; }
  public MarkupString ReportType { get; }
  public MarkupString AsOf { get; }
}