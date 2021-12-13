
using TraderSys.Portfolios;

var builder = WebApplication.CreateBuilder(args);

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.

// Register the repository class as a scoped service (instance per request)
builder.Services.AddScoped<IPortfolioRepository, PortfolioRepository>();
builder.Services.AddSingleton<IUserLookup, UserLookup>();

builder.Services.AddGrpc();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(JwtBearerDefaults.AuthenticationScheme, policy =>
    {
        policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
        policy.RequireClaim(ClaimTypes.Name);
    });
});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateActor = false,
                ValidateLifetime = true,
                IssuerSigningKey = JwtHelper.SecurityKey
            };
    });

var app = builder.Build();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
// Configure the HTTP request pipeline.
app.MapGrpcService<PortfolioService>();
app.MapGet("/generateJwtToken", async context =>
{
    await context.Response.WriteAsync(JwtHelper.GenerateJwtToken(context.Request.Query["name"]));
});

app.Run();