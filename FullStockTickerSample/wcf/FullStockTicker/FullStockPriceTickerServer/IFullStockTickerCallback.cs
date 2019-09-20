using System.ServiceModel;

namespace FullStockPriceTickerServer
{
    [ServiceContract]
    public interface IFullStockTickerCallback
    {
        [OperationContract(IsOneWay = true)]
        void Update(string symbol, decimal price);
    }
}