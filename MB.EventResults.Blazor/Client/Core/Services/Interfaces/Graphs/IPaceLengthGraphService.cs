namespace MB.EventResults.Blazor.Client;

public interface IPaceLengthGraphService : IGraphTypeService<ScatterPoint> {
  bool HasData(GradeResult gradeResult);
}
