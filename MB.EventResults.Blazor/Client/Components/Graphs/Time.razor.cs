namespace MB.EventResults.Blazor.Client.Components.Graphs;
public partial class Time {
  [Parameter]
  public GradeResult Result { get; set; }

  [Inject]
  public ITimeGraphService GraphService { get; set; }

  [Inject]
  public IGraphRenderingService GraphRenderingService { get; set; }

  [Inject]
  public IGraphOptionsService _GraphOptionsService { get; set; }

  protected override async Task OnInitializedAsync() {
    await RenderGraph();
  }

  private async Task RenderGraph() {
    await GraphRenderingService.Render(GraphService, Result);
  }

  private async Task UpdateReference(ChangeEventArgs e) {
    var selectedValue = e.Value.ToString();
    if (selectedValue != "superman") {
      _GraphOptionsService.Reference = Result?.Runners?.FirstOrDefault(p => p.Name == selectedValue);
    } else {
      _GraphOptionsService.Reference = new Runner { Name = "superman" };
    }
    await RenderGraph();
  }
}