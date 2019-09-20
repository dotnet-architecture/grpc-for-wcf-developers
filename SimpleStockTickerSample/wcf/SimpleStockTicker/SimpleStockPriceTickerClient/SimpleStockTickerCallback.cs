using System;
using SimpleStockPriceTickerServer;

namespace SimpleStockPriceTickerClient
{
    class SimpleStockTickerCallback : ISimpleStockTickerCallback
    {
        public void Update(string symbol, decimal price)
        {
            Console.WriteLine($"{symbol}: {price}");
        }
    }
}