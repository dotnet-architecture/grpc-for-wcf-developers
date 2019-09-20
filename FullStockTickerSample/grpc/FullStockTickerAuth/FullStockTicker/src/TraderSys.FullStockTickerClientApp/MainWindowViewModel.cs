using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Grpc.Core;
using TraderSys.FullStockTickerClientApp.Annotations;
using TraderSys.FullStockTickerServer.Protos;

namespace TraderSys.FullStockTickerClientApp
{
    public class MainWindowViewModel : IAsyncDisposable, INotifyPropertyChanged
    {
        private readonly FullStockTicker.FullStockTickerClient _client;
        private readonly AsyncDuplexStreamingCall<ActionMessage, StockTickerUpdate> _duplexStream;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly Task _responseTask;
        private string _addSymbol;

        public MainWindowViewModel(FullStockTicker.FullStockTickerClient client)
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _client = client;
            _duplexStream = _client.Subscribe();
            _responseTask = HandleResponsesAsync(_cancellationTokenSource.Token);
            
            AddCommand = new AsyncCommand(Add, CanAdd);
        }
        
        public IAsyncCommand AddCommand { get; }

        public string AddSymbol
        {
            get => _addSymbol;
            set
            {
                if (value == _addSymbol) return;
                _addSymbol = value;
                AddCommand.RaiseCanExecuteChanged();
                OnPropertyChanged();
            }
        }

        public ObservableCollection<PriceViewModel> Prices { get; } = new ObservableCollection<PriceViewModel>();

        private bool CanAdd() => !string.IsNullOrWhiteSpace(AddSymbol);

        private async Task Add()
        {
            if (CanAdd())
            {
                await _duplexStream.RequestStream.WriteAsync(new ActionMessage {Add = new AddSymbolRequest {Symbol = AddSymbol}});
            }
        }

        public async Task Remove(PriceViewModel priceViewModel)
        {
            await _duplexStream.RequestStream.WriteAsync(new ActionMessage {Remove = new RemoveSymbolRequest {Symbol = priceViewModel.Symbol}});
            Prices.Remove(priceViewModel);
        }

        private async Task HandleResponsesAsync(CancellationToken token)
        {
            var stream = _duplexStream.ResponseStream;
            
            await foreach(var update in stream.ReadAllAsync(token))
            {
                var price = Prices.FirstOrDefault(p => p.Symbol.Equals(update.Symbol));
                if (price == null)
                {
                    price = new PriceViewModel(this) {Symbol = update.Symbol, Price = Convert.ToDecimal(update.Price)};
                    Prices.Add(price);
                }
                else
                {
                    price.Price = Convert.ToDecimal(update.Price);
                }
            }
        }

        public async ValueTask DisposeAsync()
        {
            _cancellationTokenSource.Cancel();
            try
            {
                await _duplexStream.RequestStream.CompleteAsync().ConfigureAwait(false);
                await _responseTask.ConfigureAwait(false);
            }
            finally
            {
                _duplexStream.Dispose();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}