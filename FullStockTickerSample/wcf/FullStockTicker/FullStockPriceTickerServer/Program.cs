using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace FullStockPriceTickerServer
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = new ServiceHost(typeof(FullStockTickerService));
            var binding = new NetTcpBinding(SecurityMode.None);
            host.AddServiceEndpoint(typeof(IFullStockTickerService), binding,
                "net.tcp://localhost:12385/fullstockticker");

            host.Open();

            Console.WriteLine("Press E to exit...");

            char ch = ' ';

            while (ch != 'e')
            {
                ch = char.ToLowerInvariant((char)Console.Read());
            }

            host.Close();
        }
    }
}
