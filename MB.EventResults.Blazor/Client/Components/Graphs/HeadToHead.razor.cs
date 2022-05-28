namespace MB.EventResults.Blazor.Client.Components.Graphs;

public partial class HeadToHead {
  [Parameter]
  public GradeResult Result { get; set; }

  protected string ShowComparison(Runner runner1, Runner runner2) {
    if (runner1.Name == runner2.Name) {
      return "";
    }

    var runner1Points = 0;
    var drawPoints = 0;
    var runner2Points = 0;

    foreach (var leg in runner1.Splits) {
      var sameLeg = runner2.Splits.FirstOrDefault(p => p.PreviousCode == leg.PreviousCode && p.Code == leg.Code && p.NextCode == leg.NextCode);

      if (sameLeg is not null && leg.Leg.HasValue && sameLeg.Leg.HasValue) {
        if (leg.Leg < sameLeg.Leg) {
          runner1Points++;
        } else if (leg.Leg > sameLeg.Leg) {
          runner2Points++;
        } else if (leg.Leg == sameLeg.Leg) {
          drawPoints++;
        }
      }
    }

    return $"{runner1Points}/{drawPoints}/{runner2Points}";
  }
}