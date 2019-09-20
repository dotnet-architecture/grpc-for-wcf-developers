using System.ServiceModel;

namespace SimpleStockPriceTickerServer
{
    [ServiceContract(SessionMode = SessionMode.Required, CallbackContract = typeof(ISimpleStockTickerCallback))]
    public interface ISimpleStockTickerService
    {
        [OperationContract(IsOneWay = true)]
        void Subscribe(string[] symbols);
    }
}