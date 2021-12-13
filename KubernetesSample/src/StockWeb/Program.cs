
var builder = WebApplication.CreateBuilder(args);

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.

// Register the repository class as a scoped service (instance per request)

builder.Services.AddControllersWithViews();
builder.Services.Configure<StockDataSettings>(builder.Configuration.GetSection("StockData"));

builder.Services.AddGrpcClient<Stocks.StocksClient>((provider, options) =>
{
    var settings = provider.GetRequiredService<IOptionsMonitor<StockDataSettings>>();
    options.Address = new Uri(settings.CurrentValue.Address);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});

app.Run();