using MB.OResults.Core;

namespace MB.EventResults.Blazor.Client;

public interface IGraphRenderingService {
  Task Render<T>(IGraphTypeService<T> graphService, GradeResult result);
  Task Clear();
}