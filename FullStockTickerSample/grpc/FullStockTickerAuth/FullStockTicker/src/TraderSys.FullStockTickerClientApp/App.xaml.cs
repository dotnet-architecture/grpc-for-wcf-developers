using System;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Windows;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.Extensions.DependencyInjection;

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
            var clientCertificate = new X509Certificate2("client.pfx", "secretsquirrel");
            var handler = new HttpClientHandler();
            handler.ClientCertificates.Add(clientCertificate);

            services.AddHttpClient("grpc").ConfigurePrimaryHttpMessageHandler(() => handler);

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