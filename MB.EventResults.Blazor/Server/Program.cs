using Amazon.S3;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

builder
  .Services
  .AddSingleton<IDistanceCalculator, DistanceCalculator>()
  .AddSingleton<IAuthService, AuthService>()
  .AddSingleton<IImportService, ImportService>()
  .AddSingleton<IAnalyzerService, AnalyzerService>()
  .AddSingleton<IStartListService, XmlStartListService>()
  .AddSingleton<IEntryListService, XmlEntryListService>()
  .AddSingleton<IClassListService, XmlClassListService>()
  .AddSingleton<IResultService, XmlResultService>()
  .AddSingleton<ICourseDataService, XmlCourseDataService>()
  .AddSingleton<IJsonSerializerService, JsonSerializerService>()
  .AddSingleton<IResultBuilderService, ResultBuilderService>()
  .AddSingleton<ICacheService, CacheService>()
  .AddAWSService<IAmazonS3>();

var storageMode = builder.Configuration.GetValue<string>("App:Storage");

if (storageMode == "AWS") {
  builder.Services
    .AddSingleton<IXmlSerializerService, S3XmlSerializerService>()
    .AddSingleton<IFileService, S3FileService>();
} else {
  builder.Services
    .AddSingleton<IXmlSerializerService, XmlSerializerService>()
    .AddSingleton<IFileService, FileService>();
}

var mode = builder.Configuration.GetValue<string>("App:Mode");

if (mode == "File") {
  builder.Services.AddSingleton<IProcessedResultService, FileResultService>();
} else {
  builder.Services.AddSingleton<IProcessedResultService, PreProccessedFileResultService>();
}

var awsHosted = builder.Configuration.GetValue<bool>("AwsHosted");

if (awsHosted) {
  builder
    .Services
    .AddAWSLambdaHosting(LambdaEventSource.HttpApi)
    .AddDataProtection()
    .PersistKeysToAWSSystemsManager("/DataProtection");
}

builder.Services.Configure<ForwardedHeadersOptions>(options => {
  options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto | ForwardedHeaders.XForwardedHost;
  // The proxy in front of the container has an address that can't be known up front.
  options.KnownIPNetworks.Clear();
  options.KnownProxies.Clear();
});

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder
  .WithConfig()
  .Add<AppConfiguration>("App")
  .Add<AnalyzerServiceConfiguration>("Analyser");

var app = builder.Build();

app.UseForwardedHeaders();

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
// Without this a GET to a POST-only api route gets index.html and a 200 from the fallback below.
app.Map("/api/{**path}", context => {
  context.Response.StatusCode = StatusCodes.Status404NotFound;
  return Task.CompletedTask;
});
app.MapFallbackToFile("index.html");

app.Run();
