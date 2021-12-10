using TraderSys.FullStockTickerServer;

var builder = WebApplication.CreateBuilder(args);

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.

// Register the repository class as a scoped service (instance per request)
builder.Services.AddSingleton<IFullStockPriceSubscriberFactory, FullStockPriceSubscriberFactory>();
builder.Services.AddAuthentication(CertificateAuthenticationDefaults.AuthenticationScheme)
            .AddCertificate(options =>
            {
                options.AllowedCertificateTypes = CertificateTypes.Chained;
                options.RevocationMode = X509RevocationMode.NoCheck;

                options.Events = new CertificateAuthenticationEvents
                {
                    OnCertificateValidated = DevelopmentModeCertificateHelper.Validate
                };
            });

builder.Services.AddGrpc();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseRouting();

app.UseCertificateForwarding();
app.UseAuthentication();
app.MapGrpcService<FullStockTickerService>();
app.MapGet("/", async context =>
{
    await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
});

app.Run();
