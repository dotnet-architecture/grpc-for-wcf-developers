using System.ServiceModel;
using TraderSys.StockMarket;

namespace SimpleStockPriceTickerServer
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class SimpleStockTickerService : ISimpleStockTickerService
    {
        private ISimpleStockTickerCallback _callback;
        private SimpleStockPriceSubscriber _subscriber;

        public void Subscribe(string[] symbols)
        {
            _callback = OperationContext.Current.GetCallbackChannel<ISimpleStockTickerCallback>();
            _subscriber = new SimpleStockPriceSubscriber(symbols);
            _subscriber.Update += SubscriberOnUpdate;
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

        private ISimpleStockTickerCallback Callback =>
            OperationContext.Current.GetCallbackChannel<ISimpleStockTickerCallback>();
    }
}