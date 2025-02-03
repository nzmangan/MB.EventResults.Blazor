namespace MB.OResults.Core;

public class Results {
  public List<GradeResult> Grades { get; set; } = new();
  public DateTime Created { get; set; }
}