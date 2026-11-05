namespace MB.EventResults.Blazor.Client.Components.Graphs;

public partial class ClubStandings {
  private List<ClubStandingRow> _Rows = [];

  [Parameter]
  public List<GradeResult> Grades { get; set; }

  [Inject]
  public ITextService TextService { get; set; }

  protected override void OnParametersSet() {
    var runners = Grades.SelectMany(grade => (grade.Runners ?? []).Select(runner => new {
      runner.Club,
      runner.Status,
      Finished = runner.IsFinisher(),
      Place = runner.IsFinisher() ? runner.GetPlace(grade.Runners) : (int?)null
    }));

    var rows = runners
      .Where(p => !String.IsNullOrWhiteSpace(p.Club))
      .GroupBy(p => p.Club)
      .Select(club => new ClubStandingRow {
        Name = club.Key,
        Wins = club.Count(p => p.Place == 1),
        TopThree = club.Count(p => p.Place <= 3),
        Finished = club.Count(p => p.Finished),
        Started = club.Count(p => p.Status != ReportHelper.StatusDidNotStart)
      })
      .OrderByDescending(p => p.Wins)
      .ThenByDescending(p => p.TopThree)
      .ThenByDescending(p => p.Finished)
      .ThenBy(p => p.Name)
      .ToList();

    foreach (var row in rows) {
      row.Place = rows.Count(p => p.Wins > row.Wins
        || (p.Wins == row.Wins && p.TopThree > row.TopThree)
        || (p.Wins == row.Wins && p.TopThree == row.TopThree && p.Finished > row.Finished)) + 1;
    }

    _Rows = rows;
  }

  private class ClubStandingRow {
    public int Place { get; set; }
    public string Name { get; set; }
    public int Wins { get; set; }
    public int TopThree { get; set; }
    public int Finished { get; set; }
    public int Started { get; set; }
  }
}
