
namespace TraderSys.Portfolios;
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddScoped<IPortfolioRepository, PortfolioRepository>();
        services.AddGrpc();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapGrpcService<PortfolioService>();
        });
    }
}