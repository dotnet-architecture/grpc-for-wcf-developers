using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace SimpleStockPriceTickerServer
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = new ServiceHost(typeof(SimpleStockTickerService));
            var binding = new NetTcpBinding(SecurityMode.None);
            host.AddServiceEndpoint(typeof(ISimpleStockTickerService), binding,
                "net.tcp://localhost:12384/simplestockticker");

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
