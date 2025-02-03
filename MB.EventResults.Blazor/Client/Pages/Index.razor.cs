namespace MB.EventResults.Blazor.Client.Pages;

[Route("/")]
public partial class Index {
  private List<string> _Options = [];
  private GradeResult _SelectedEventResult = null;
  private DateTime? _EventDate = null;
  private string _GraphType = GraphType.Table;
  private List<EventGrade> _Grades;
  private bool _Loading = false;

  [Inject]
  public IDataClient DataClient { get; set; }

  [Inject]
  public ITextService TextService { get; set; }

  protected async Task SelectClass(ChangeEventArgs e) {
    var grade = _Grades.FirstOrDefault(p => p.Name == e.Value.ToString());

    if (grade?.Id is not null) {
      _SelectedEventResult = null;
      _EventDate = null;
      _Loading = true;
      await Task.Delay(100);
      var result = await DataClient.Get(grade.Id);
      _Loading = false;
      await Task.Delay(100);
      _SelectedEventResult = result?.Grade;
      _EventDate = result?.EventDate;
      string gt = _GraphType;
      _GraphType = null;
      await Task.Delay(100);
      _GraphType = gt;
    }
  }

  protected async override Task OnInitializedAsync() {
    _Options = [
      GraphType.Table,
      MistakeGraphService.Label,
      MistakeTotalGraphService.Label,
      PackGraphService.Label,
      PerformanceIndexGraphService.Label,
      PerformanceIndexHistogramGraphService.Label,
      PerformanceIndexHistogramNormalizedGraphService.Label,
      PerformanceIndexNormalizedGraphService.Label,
      PositionLegGraphService.Label,
      PositionTotalGraphService.Label,
      TimeGraphService.Label,
      GraphType.HeadToHead,
      GraphType.HallOfFame,
      GraphType.Data
    ];

    _Loading = true;
    var grades = await DataClient.Grades();
    grades.Insert(0, new EventGrade { Id = null, Name = "" });
    _Grades = grades;
    _Loading = false;
  }
}