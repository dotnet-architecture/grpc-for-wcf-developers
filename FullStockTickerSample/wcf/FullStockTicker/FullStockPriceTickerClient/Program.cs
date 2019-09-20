using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using FullStockPriceTickerServer;

namespace FullStockPriceTickerClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var binding = new NetTcpBinding(SecurityMode.None);
            var address = new EndpointAddress("net.tcp://localhost:12385/fullstockticker");
            var factory =
                new DuplexChannelFactory<IFullStockTickerService>(typeof(FullStockTickerCallback), binding,
                    address);
            var context = new InstanceContext(new FullStockTickerCallback());
            var server = factory.CreateChannel(context);

            server.Subscribe();
            server.AddSymbol("MSFT");
            server.AddSymbol("GOOG");

            Console.WriteLine("Press E to exit...");

            char ch = ' ';

            while (ch != 'e')
            {
                ch = char.ToLowerInvariant((char)Console.Read());
            }
        }
    }

    class FullStockTickerCallback : IFullStockTickerCallback
    {
        public void Update(string symbol, decimal price)
        {
            Console.WriteLine($"{symbol}: {price}");
        }
    }
}
