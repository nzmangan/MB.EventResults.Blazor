using Microsoft.JSInterop;

namespace MB.EventResults.Blazor.Client.Components.Graphs;

public partial class Data {
  [Parameter]
  public GradeResult Result { get; set; }

  [Inject]
  public IJSRuntime JSRuntime { get; set; }

  [Inject]
  public IJsonSerializerService JsonSerializerService { get; set; }

  private async Task DownloadFile() {
    await JSRuntime.InvokeVoidAsync("window.downloadString", $"{Result.Name}.json", JsonSerializerService.Serialize(Result));
  }
}