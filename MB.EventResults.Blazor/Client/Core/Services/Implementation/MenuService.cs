namespace MB.EventResults.Blazor.Client;

public class MenuService : IMenuService {
  public event EventHandler<bool> ToggleMenu;

  public void Hide() {
    Publish(false);
  }

  public void Show() {
    Publish(true);
  }

  private void Publish(bool expanded) {
    ToggleMenu?.Invoke(this, expanded);
  }
}