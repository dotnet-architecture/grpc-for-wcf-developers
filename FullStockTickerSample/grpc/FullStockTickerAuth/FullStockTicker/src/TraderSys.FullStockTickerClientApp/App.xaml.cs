
namespace TraderSys.FullStockTickerClientApp;
/// <summary>
/// Interaction logic for App_OnStartup
/// </summary>
public partial class App : Application
{
    private readonly ServiceProvider _serviceProvider;

    public App()
    {
        var services = new ServiceCollection();
        ConfigureServices(services);
        _serviceProvider = services.BuildServiceProvider();
    }

    private void ConfigureServices(ServiceCollection services)
    {
        var clientCertificate = new X509Certificate2("client.pfx", "secretsquirrel");

        services.AddGrpcClient<FullStockTickerServer.Protos.FullStockTicker.FullStockTickerClient>("grpc", options =>
            {
                options.Address = new Uri("https://localhost:5001");
            })
            .ConfigurePrimaryHttpMessageHandler(() =>
            {
                var handler = new HttpClientHandler();
                handler.ClientCertificates.Add(clientCertificate);
                return handler;
            });
        services.AddSingleton<MainWindow>();
        services.AddSingleton<MainWindowViewModel>();
    }

    private void OnStartup(object sender, StartupEventArgs e)
    {
        var window = _serviceProvider.GetService<MainWindow>();
        window.Show();
    }
}