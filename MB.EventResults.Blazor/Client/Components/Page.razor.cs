using Microsoft.AspNetCore.Components;

namespace MB.EventResults.Blazor.Client.Components;

public partial class Page {
  [Parameter]
  public RenderFragment ChildContent { get; set; }
}