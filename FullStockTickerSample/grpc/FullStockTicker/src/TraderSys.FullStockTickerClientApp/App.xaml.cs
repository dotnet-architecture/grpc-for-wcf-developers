
namespace TraderSys.FullStockTickerClientApp
{
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
            services.AddHttpClient("grpc");

            services.AddGrpcClient<FullStockTickerServer.Protos.FullStockTicker.FullStockTickerClient>(options =>
                {
                    options.Address = new Uri("https://localhost:5001");
                })
                .ConfigureChannel((provider, channel) =>
                {
                    var client = provider.GetRequiredService<IHttpClientFactory>().CreateClient("grpc");
                    channel.HttpClient = client;
                    channel.DisposeHttpClient = true;
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
}