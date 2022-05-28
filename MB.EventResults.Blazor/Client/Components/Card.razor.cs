using Microsoft.AspNetCore.Components;

namespace MB.EventResults.Blazor.Client.Components;

public partial class Card {
  private string cssClass = "";

  [Parameter]
  public RenderFragment ChildContent { get; set; }

  [Parameter]
  public bool Shadow { get; set; }

  [Parameter]
  public string Title { get; set; }

  protected override void OnParametersSet() {
    cssClass = "card card-custom" + (Shadow ? " shadow" : "");
  }
}