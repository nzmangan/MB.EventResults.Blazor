namespace MB.EventResults.Blazor.Client.Components.Graphs;

public partial class PaceLength {
  [Parameter]
  public GradeResult Result { get; set; }

  [Inject]
  public ITextService TextService { get; set; }

  [Inject]
  public IPaceLengthGraphService GraphService { get; set; }

  [Inject]
  public IGraphRenderingService GraphRenderingService { get; set; }

  protected override async Task OnAfterRenderAsync(bool firstRender) {
    if (firstRender && GraphService.HasData(Result)) {
      await GraphRenderingService.Render(GraphService, Result);
    }
  }
}
