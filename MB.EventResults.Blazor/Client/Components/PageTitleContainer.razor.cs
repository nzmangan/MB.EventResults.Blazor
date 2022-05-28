using Microsoft.AspNetCore.Components;

namespace MB.EventResults.Blazor.Client.Components;

public partial class PageTitleContainer {
  [Parameter]
  public string Title { get; set; }

  [Parameter]
  public RenderFragment ChildContent { get; set; }
}