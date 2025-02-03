namespace MB.EventResults.Blazor.Client.Components;

public partial class NameAndValueList<T> {
  [Parameter]
  public List<NameAndValue<T>> Runners { get; set; }

  [Parameter]
  public MarkupString Title { get; set; }

  [Parameter]
  public string Unit { get; set; }

  [Parameter]
  public Func<T, string> Formatter { get; set; }

  [Parameter]
  public bool ShowPlacing { get; set; }
}