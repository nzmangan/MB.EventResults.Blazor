namespace MB.EventResults.Blazor.Client;

public class TextService : ITextService {
  private readonly Dictionary<string, string> Current = Translations.English;

  private MarkupString Get(string key) {
    return (MarkupString)(Current.TryGetValue(key, out var s) ? s : "");
  }

  public MarkupString ReportDescription(string reportType) => Get($"Description.{reportType}");

  public MarkupString StatisticsDescription => Get("Description.Statistics");

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

  public MarkupString CleanRace => Get("CleanRace");

  public MarkupString CleanPlace => Get("CleanPlace");

  public MarkupString CleanTime => Get("CleanTime");

  public MarkupString ActualTime => Get("ActualTime");

  public MarkupString ActualPlace => Get("ActualPlace");

  public MarkupString PlaceChange => Get("PlaceChange");

  public MarkupString Runner => Get("Runner");

  public MarkupString Club => Get("Club");

  public MarkupString NoValidResults => Get("NoValidResults");

  public MarkupString LegWinners => Get("LegWinners");

  public MarkupString HardestControls => Get("HardestControls");

  public MarkupString RunnerCard => Get("RunnerCard");

  public MarkupString PackEffect => Get("PackEffect");

  public MarkupString EventSummary => Get("EventSummary");

  public MarkupString ClubStandings => Get("ClubStandings");

  public MarkupString EventOverview => Get("EventOverview");

  public MarkupString BackToResults => Get("BackToResults");

  public MarkupString LegNumber => Get("LegNumber");

  public MarkupString Leg => Get("Leg");

  public MarkupString RunnerCount => Get("RunnerCount");

  public MarkupString Mistakes => Get("Mistakes");

  public MarkupString MistakeRate => Get("MistakeRate");

  public MarkupString TotalTimeLost => Get("TotalTimeLost");

  public MarkupString AverageTimeLost => Get("AverageTimeLost");

  public MarkupString Winner => Get("Winner");

  public MarkupString Time => Get("Time");

  public MarkupString SecondPlace => Get("SecondPlace");

  public MarkupString Gap => Get("Gap");

  public MarkupString Place => Get("Place");

  public MarkupString PerformanceIndex => Get("PerformanceIndex");

  public MarkupString TimeLost => Get("TimeLost");

  public MarkupString BestLeg => Get("BestLeg");

  public MarkupString WorstLeg => Get("WorstLeg");

  public MarkupString RanNear => Get("RanNear");

  public MarkupString LegsNearOthers => Get("LegsNearOthers");

  public MarkupString LegsAlone => Get("LegsAlone");

  public MarkupString PaceNearOthers => Get("PaceNearOthers");

  public MarkupString PaceAlone => Get("PaceAlone");

  public MarkupString Difference => Get("Difference");

  public MarkupString Entries => Get("Entries");

  public MarkupString DidNotStart => Get("DidNotStart");

  public MarkupString Started => Get("Started");

  public MarkupString Finished => Get("Finished");

  public MarkupString DidNotFinish => Get("DidNotFinish");

  public MarkupString MissingPunch => Get("MissingPunch");

  public MarkupString OtherStatus => Get("OtherStatus");

  public MarkupString FinishRate => Get("FinishRate");

  public MarkupString WinningTime => Get("WinningTime");

  public MarkupString Total => Get("Total");

  public MarkupString Wins => Get("Wins");

  public MarkupString TopThree => Get("TopThree");

  public MarkupString NoSplitTimes => Get("NoSplitTimes");

  public MarkupString NoRunners => Get("NoRunners");

  public MarkupString NoPackData => Get("NoPackData");

  public MarkupString NoLegLengths => Get("NoLegLengths");

  public MarkupString NoClubs => Get("NoClubs");

  public MarkupString NoDays => Get("NoDays");
}