using MB.EventResults.Uploader;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

// --once processes the watch folder a single time and exits.
var once = args.Contains("--once", StringComparer.OrdinalIgnoreCase);

var builder = Host.CreateApplicationBuilder(new HostApplicationBuilderSettings {
  Args = args.Where(p => !p.Equals("--once", StringComparison.OrdinalIgnoreCase)).ToArray(),
  ContentRootPath = AppContext.BaseDirectory
});

var configuration = builder.Configuration.GetSection("Uploader").Get<UploaderConfiguration>() ?? new();
var analyzerConfiguration = builder.Configuration.GetSection("Analyser").Get<AnalyzerServiceConfiguration>() ?? new();

builder.Services
  .AddSingleton(configuration)
  .AddSingleton(analyzerConfiguration)
  .AddSingleton(new WatchOptions(once))
  .AddSingleton<IofFileStore>()
  .AddSingleton<IResultService>(sp => sp.GetRequiredService<IofFileStore>())
  .AddSingleton<IStartListService>(sp => sp.GetRequiredService<IofFileStore>())
  .AddSingleton<IEntryListService>(sp => sp.GetRequiredService<IofFileStore>())
  .AddSingleton<IClassListService>(sp => sp.GetRequiredService<IofFileStore>())
  .AddSingleton<ICourseDataService>(sp => sp.GetRequiredService<IofFileStore>())
  .AddSingleton<IDistanceCalculator, DistanceCalculator>()
  .AddSingleton<IAnalyzerService, AnalyzerService>()
  .AddSingleton<IResultBuilderService, ResultBuilderService>()
  .AddSingleton<IJsonSerializerService, JsonSerializerService>()
  .AddSingleton<ResultPublisher>()
  .AddHostedService<WatchService>();

if (configuration.Target.Type.Equals("Folder", StringComparison.OrdinalIgnoreCase)) {
  builder.Services.AddSingleton<IResultTarget, FolderTarget>();
} else {
  builder.Services.AddSingleton<IResultTarget, SftpTarget>();
}

await builder.Build().RunAsync();
