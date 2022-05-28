using MB.EventResults.Blazor.Shared;
using Microsoft.JSInterop;

namespace MB.EventResults.Blazor.Client.Pages;

[Route("/")]
public partial class Index {
  private List<string> _Options = new();
  private GradeResult _SelectedEventResult = null;
  private DateTime? _EventDate = null;
  private string _GraphType = GraphType.Table;
  private List<EventGrade> _Grades;
  private bool _Loading = false;

  [Inject]
  public IDataClient DataClient { get; set; }

  [Inject]
  public IJSRuntime JS { get; set; }

  private async Task SelectClass(ChangeEventArgs e) {
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
    _Options = new List<string> {
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
        GraphType.HallOfFame
      };


    _Loading = true;
    var grades = await DataClient.Grades();
    grades.Insert(0, new EventGrade { Id = null, Name = "" });
    _Grades = grades;
    _Loading = false;
  }
}