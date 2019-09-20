using System.ServiceModel;

namespace FullStockPriceTickerServer
{
    [ServiceContract(SessionMode = SessionMode.Required, CallbackContract = typeof(IFullStockTickerCallback))]
    public interface IFullStockTickerService
    {
        [OperationContract(IsOneWay = true)]
        void Subscribe();

        [OperationContract(IsOneWay = true)]
        void AddSymbol(string symbol);

        [OperationContract(IsOneWay = true)]
        void RemoveSymbol(string symbol);
    }
}