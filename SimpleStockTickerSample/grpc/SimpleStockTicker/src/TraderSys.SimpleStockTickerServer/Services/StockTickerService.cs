using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using TraderSys.SimpleStockTickerServer.Protos;
using TraderSys.StockMarket;

namespace TraderSys.SimpleStockTickerServer.Services
{
    public class StockTickerService : Protos.SimpleStockTicker.SimpleStockTickerBase
    {
        private readonly IStockPriceSubscriberFactory _subscriberFactory;
        private readonly ILogger<StockTickerService> _logger;

        public StockTickerService(IStockPriceSubscriberFactory subscriberFactory, ILogger<StockTickerService> logger)
        {
            _subscriberFactory = subscriberFactory;
            _logger = logger;
        }

        public override async Task Subscribe(SubscribeRequest request,
            IServerStreamWriter<StockTickerUpdate> responseStream, ServerCallContext context)
        {
            var subscriber = _subscriberFactory.GetSubscriber(request.Symbols.ToArray());

            subscriber.Update += async (sender, args) =>
                await WriteUpdateAsync(responseStream, args.Symbol, args.Price);

            _logger.LogInformation("Subscription started.");

            await AwaitCancellation(context.CancellationToken);

            subscriber.Dispose();

            _logger.LogInformation("Subscription finished.");
        }

        private async Task WriteUpdateAsync(IServerStreamWriter<StockTickerUpdate> stream, string symbol, decimal price)
        {
            try
            {
                await stream.WriteAsync(new StockTickerUpdate
                {
                    Symbol = symbol,
                    Price = Convert.ToDouble(price),
                    Time = Timestamp.FromDateTimeOffset(DateTimeOffset.UtcNow)
                });
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to write message: {e.Message}");
            }
        }

        private static Task AwaitCancellation(CancellationToken token)
        {
            var completion = new TaskCompletionSource<object>();
            token.Register(() => completion.SetResult(null));
            return completion.Task;
        }
    }
}