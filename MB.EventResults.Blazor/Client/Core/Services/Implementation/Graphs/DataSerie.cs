namespace MB.EventResults.Blazor.Client;

public class DataSerie<T> {
  public List<T> Data { get; set; }
  public string Label { get; set; }
  public string Color { get; set; }
}