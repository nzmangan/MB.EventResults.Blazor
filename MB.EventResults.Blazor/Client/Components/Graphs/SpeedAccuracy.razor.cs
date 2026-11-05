namespace MB.EventResults.Blazor.Client.Components.Graphs;

public partial class SpeedAccuracy {
  [Parameter]
  public GradeResult Result { get; set; }

  [Inject]
  public ITextService TextService { get; set; }

  [Inject]
  public ISpeedAccuracyGraphService GraphService { get; set; }

  [Inject]
  public IGraphRenderingService GraphRenderingService { get; set; }

  protected override async Task OnAfterRenderAsync(bool firstRender) {
    if (firstRender && GraphService.GetDataSerie(Result).Count > 0) {
      await GraphRenderingService.Render(GraphService, Result);
    }
  }
}
