using Autofac;
using Autofac.Integration.Wcf;
using TraderSys.PortfolioData;

namespace TraderSys
{
    public static class AppStart
    {
        public static void AppInitialize()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<PortfolioService>();
            builder.RegisterType<PortfolioRepository>().As<IPortfolioRepository>();
            AutofacHostFactory.Container = builder.Build();
        }
    }
}