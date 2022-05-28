using MB.OResults.Core;
using Microsoft.AspNetCore.Components;

namespace MB.EventResults.Blazor.Client.Components.Graphs {
  public partial class Mistake {
    [Parameter]
    public GradeResult Result { get; set; }

    [Inject]
    public IMistakeGraphService GraphService { get; set; }

    [Inject]
    public IGraphRenderingService GraphRenderingService { get; set; }

    protected override async Task OnInitializedAsync() {
      await GraphRenderingService.Render(GraphService, Result);
    }
  }
}