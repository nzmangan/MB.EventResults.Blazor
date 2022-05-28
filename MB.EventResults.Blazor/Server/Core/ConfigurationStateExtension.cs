namespace MB.EventResults.Blazor.Server;

public static class ConfigurationStateExtension {
  public static ConfigurationState WithConfig(this WebApplicationBuilder builder) {
    return new ConfigurationState { Services = builder.Services, Configuration = builder.Configuration };
  }

  public static ConfigurationState Add<T>(this ConfigurationState state, string sectionName) where T : class, new() {
    var config = new T();
    state.Configuration.Bind(sectionName, config);
    state.Services.AddSingleton(config);
    return state;
  }
}