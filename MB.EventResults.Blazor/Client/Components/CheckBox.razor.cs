namespace MB.EventResults.Blazor.Client.Components {
  public partial class CheckBox {
    [Parameter]
    public string Text { get; set; }

    [Parameter]
    public bool Value {
      get => _Value;
      set {
        if (_Value == value) {
          return;
        };
        _Value = value;
        ValueChanged.InvokeAsync(value);
      }
    }

    [Parameter]
    public EventCallback<bool> ValueChanged { get; set; }

    private string _Id;

    private bool _Value;

    protected override void OnParametersSet() {
      base.OnParametersSet();
      _Id = Guid.NewGuid().ToString();
    }
  }
}