using MB.EventResults.Blazor.Server;
using MB.OResults.Core;

var builder = WebApplication.CreateBuilder(args);

builder
  .Services
  .AddSingleton<IAuthService, AuthService>()
  .AddSingleton<IImportService, ImportService>()
  .AddSingleton<IAnalyzerService, AnalyzerService>()
  .AddSingleton<IFileService, FileService>()
  .AddSingleton<IStartListService, XmlStartListService>()
  .AddSingleton<IEntryListService, XmlEntryListService>()
  .AddSingleton<IClassListService, XmlClassListService>()
  .AddSingleton<IResultService, XmlResultService>()
  .AddSingleton<IAnalyzerService, AnalyzerService>()
  .AddSingleton<IXmlSerializerService, XmlSerializerService>()
  .AddSingleton<IJsonSerializerService, JsonSerializerService>()
  .AddSingleton<IResultBuilderService, ResultBuilderService>();

var mode = builder.Configuration.GetValue<string>("App:Mode");

if (mode == "File") {
  builder.Services.AddSingleton<IProcessedResultService, FileResultService>();
} else {
  builder.Services.AddSingleton<IProcessedResultService, PreProccessedFileResultService>();
}

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder
  .WithConfig()
  .Add<AppConfiguration>("App")
  .Add<AnalyzerServiceConfiguration>("Analyser");

var app = builder.Build();

if (app.Environment.IsDevelopment()) {
  app.UseWebAssemblyDebugging();
} else {
  app.UseExceptionHandler("/Error");
}

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();


app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
