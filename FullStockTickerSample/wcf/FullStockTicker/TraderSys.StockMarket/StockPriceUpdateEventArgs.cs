using System;
using System.Collections.Generic;
using System.Text;

namespace TraderSys.StockMarket
{
    public class StockPriceUpdateEventArgs : EventArgs
    {
        public StockPriceUpdateEventArgs(string symbol, decimal price)
        {
            Symbol = symbol;
            Price = price;
        }

        public string Symbol { get; }
        public decimal Price { get; }
    }
}