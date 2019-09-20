using System.Collections.Generic;
using System.Linq;
using StockData.Protos;

namespace StockWeb.Models
{
    public class IndexViewModel
    {
        public IndexViewModel(IEnumerable<StockViewModel> stocks)
        {
            Stocks = stocks.ToArray();
        }

        public IList<StockViewModel> Stocks { get; }
    }

    public class StockViewModel
    {
        public StockViewModel(Stock stock, string serverId)
        {
            Id = stock.Id;
            Symbol = stock.Symbol;
            Name = stock.Name;
            ServerId = serverId;
        }

        public int Id { get; }
        public string Symbol { get; }
        public string Name { get; }
        public string ServerId { get; }
    }
}