
namespace StockData;
public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
                webBuilder.ConfigureKestrel(kestrel =>
                {
                    kestrel.ConfigureHttpsDefaults(https =>
                    {
                        https.ServerCertificate = new X509Certificate2();
                    });
                });
            });
}