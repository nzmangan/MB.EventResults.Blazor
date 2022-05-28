namespace MB.EventResults.Blazor.Client.Components;

public partial class NameAndValueList<T> {
  [Parameter]
  public List<NameAndValue<T>> Runners { get; set; }

  [Parameter]
  public string Title { get; set; }

  [Parameter]
  public string Unit { get; set; }
}