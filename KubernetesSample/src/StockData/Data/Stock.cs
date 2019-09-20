using System.Threading;

namespace StockData.Data
{
    public class Stock
    {
        private static int _nextId = 1;
        
        public Stock(string symbol, string name)
        {
            Id = Interlocked.Increment(ref _nextId);
            Symbol = symbol;
            Name = name;
        }

        public int Id { get; }
        public string Symbol { get; }
        public string Name { get; }
    }
}