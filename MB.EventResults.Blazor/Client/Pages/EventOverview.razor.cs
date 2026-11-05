namespace MB.EventResults.Blazor.Client.Pages;

[Route("/event")]
public partial class EventOverview {
  private readonly List<string> _Options = [GraphType.EventSummary, GraphType.ClubStandings];
  private string _GraphType = GraphType.EventSummary;
  private EventResult _Result;
  private bool _Loading = false;

  [Inject]
  public IDataClient DataClient { get; set; }

  [Inject]
  public ITextService TextService { get; set; }

  protected async override Task OnInitializedAsync() {
    _Loading = true;
    _Result = await DataClient.Get();
    _Loading = false;
  }
}
