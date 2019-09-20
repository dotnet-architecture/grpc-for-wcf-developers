using System.ServiceModel;

namespace SimpleStockPriceTickerServer
{
    [ServiceContract]
    public interface ISimpleStockTickerCallback
    {
        [OperationContract(IsOneWay = true)]
        void Update(string symbol, decimal price);
    }
}