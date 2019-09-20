using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using SimpleStockPriceTickerServer;

namespace SimpleStockPriceTickerClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var binding = new NetTcpBinding(SecurityMode.None);
            var address = new EndpointAddress("net.tcp://localhost:12384/simplestockticker");
            var factory =
                new DuplexChannelFactory<ISimpleStockTickerService>(typeof(SimpleStockTickerCallback), binding,
                    address);
            var context = new InstanceContext(new SimpleStockTickerCallback());
            var server = factory.CreateChannel(context);

            server.Subscribe(new []{"MSFT", "AAPL"});

            Console.WriteLine("Press E to exit...");

            char ch = ' ';

            while (ch != 'e')
            {
                ch = char.ToLowerInvariant((char)Console.Read());
            }
        }
    }
}
