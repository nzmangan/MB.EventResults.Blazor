using MB.EventResults.Blazor.Client;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services
  .AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) })
  .AddScoped<IMenuService, MenuService>()
        .AddScoped<IMistakeGraphService, MistakeGraphService>()
        .AddScoped<IMistakeTotalGraphService, MistakeTotalGraphService>()
        .AddScoped<IPackGraphService, PackGraphService>()
        .AddScoped<IPerformanceIndexGraphService, PerformanceIndexGraphService>()
        .AddScoped<IPerformanceIndexHistogramGraphService, PerformanceIndexHistogramGraphService>()
        .AddScoped<IPerformanceIndexHistogramNormalizedGraphService, PerformanceIndexHistogramNormalizedGraphService>()
        .AddScoped<IPerformanceIndexNormalizedGraphService, PerformanceIndexNormalizedGraphService>()
        .AddScoped<IPositionLegGraphService, PositionLegGraphService>()
        .AddScoped<IPositionTotalGraphService, PositionTotalGraphService>()
        .AddScoped<ITimeGraphService, TimeGraphService>()
        .AddScoped<IDataClient, DataClient>()
        .AddScoped<IDataService, DataService>()
        .AddScoped<IGraphRenderingService, GraphRenderingService>()
        .AddScoped<IGraphOptionsService, GraphOptionsService>();

await builder.Build().RunAsync();
