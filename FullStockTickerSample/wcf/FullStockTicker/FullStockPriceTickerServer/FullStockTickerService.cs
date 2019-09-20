using System.ServiceModel;
using TraderSys.StockMarket;

namespace FullStockPriceTickerServer
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class FullStockTickerService : IFullStockTickerService
    {
        private IFullStockTickerCallback _callback;
        private FullStockPriceSubscriber _subscriber;

        public void Subscribe()
        {
            _callback = OperationContext.Current.GetCallbackChannel<IFullStockTickerCallback>();
            _subscriber = new FullStockPriceSubscriber();
            _subscriber.Update += SubscriberOnUpdate;
        }

        public void AddSymbol(string symbol)
        {
            _subscriber?.Add(symbol);
        }

        public void RemoveSymbol(string symbol)
        {
            _subscriber?.Remove(symbol);
        }

        private void SubscriberOnUpdate(object sender, StockPriceUpdateEventArgs e)
        {
            try
            {
                _callback.Update(e.Symbol, e.Price);
            }
            catch (CommunicationException)
            {
                _subscriber.Dispose();
                _subscriber = null;
            }
        }
    }
}