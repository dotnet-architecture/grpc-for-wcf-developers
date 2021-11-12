
namespace TraderSys.FullStockTickerClientApp;
public interface IAsyncCommand : ICommand
{
    bool CanExecute();
    Task ExecuteAsync();
    void RaiseCanExecuteChanged();
}