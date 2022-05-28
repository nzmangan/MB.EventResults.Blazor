namespace MB.EventResults.Blazor.Client;

public interface IMenuService {
  public event EventHandler<bool> ToggleMenu;
  void Show();
  void Hide();
}