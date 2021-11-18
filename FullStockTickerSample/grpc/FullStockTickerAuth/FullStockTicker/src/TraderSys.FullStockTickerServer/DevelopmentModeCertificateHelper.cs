
namespace TraderSys.FullStockTickerServer;
internal static class DevelopmentModeCertificateHelper
{
    // Please go through the procedure to create certificate from readme and include the certificate in the project
    public static readonly X509Certificate2 Certificate = new X509Certificate2("certificate.pfx", "secretsquirrel");

    public static Task Validate(CertificateValidatedContext context)
    {
        if (context.ClientCertificate.Issuer == Certificate.Issuer)
        {
            var claims = new[]
            {
                    new Claim(ClaimTypes.NameIdentifier, context.ClientCertificate.Subject, ClaimValueTypes.String,
                        context.Options.ClaimsIssuer),
                    new Claim(ClaimTypes.Name, context.ClientCertificate.Subject, ClaimValueTypes.String, context.Options.ClaimsIssuer),
                };

            context.Principal = new ClaimsPrincipal(new ClaimsIdentity(claims, context.Scheme.Name));
            context.Success();
        }
        else
        {
            context.Fail("Invalid certificate.");
        }

        return Task.CompletedTask;
    }
}