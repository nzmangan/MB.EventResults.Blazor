using MB.OResults.Core;

namespace MB.EventResults.Blazor.Client;

public interface IGraphTypeService<T> {
  string ChartType { get; }
  string OptionType { get; }
  List<string> GetLabels(GradeResult gradeResult);
  List<T> GetValues(Runner runner);
  List<DataSerie<T>> GetDataSerie(GradeResult gradeResult);
}