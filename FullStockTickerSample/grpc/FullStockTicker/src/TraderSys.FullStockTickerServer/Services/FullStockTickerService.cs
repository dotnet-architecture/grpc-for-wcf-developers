using System;
using System.Threading;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using TraderSys.FullStockTickerServer.Protos;
using TraderSys.StockMarket;

namespace TraderSys.FullStockTickerServer.Services
{
    public class FullStockTickerService : Protos.FullStockTicker.FullStockTickerBase
    {
        private readonly IFullStockPriceSubscriberFactory _subscriberFactory;
        private readonly ILogger<FullStockTickerService> _logger;
        public FullStockTickerService(IFullStockPriceSubscriberFactory subscriberFactory, ILogger<FullStockTickerService> logger)
        {
            _subscriberFactory = subscriberFactory;
            _logger = logger;
        }

        public override async Task Subscribe(IAsyncStreamReader<ActionMessage> requestStream, IServerStreamWriter<StockTickerUpdate> responseStream, ServerCallContext context)
        {
            using var subscriber = _subscriberFactory.GetSubscriber();

            subscriber.Update += async (sender, args) =>
                await WriteUpdateAsync(responseStream, args.Symbol, args.Price);

            var actionsTask = HandleActions(requestStream, subscriber, context.CancellationToken);
            
            _logger.LogInformation("Subscription started.");
            await AwaitCancellation(context.CancellationToken);

            try { await actionsTask; } catch { /* Ignored */ }

            _logger.LogInformation("Subscription finished.");
        }

        private async Task HandleActions(IAsyncStreamReader<ActionMessage> requestStream, IFullStockPriceSubscriber subscriber, CancellationToken token)
        {
            await foreach(var action in requestStream.ReadAllAsync(token))
            {
                switch (action.ActionCase)
                {
                    case ActionMessage.ActionOneofCase.None:
                        _logger.LogWarning("No Action specified.");
                        break;
                    case ActionMessage.ActionOneofCase.Add:
                        subscriber.Add(action.Add.Symbol);
                        break;
                    case ActionMessage.ActionOneofCase.Remove:
                        subscriber.Remove(action.Remove.Symbol);
                        break;
                    default:
                        _logger.LogWarning($"Unknown Action '{action.ActionCase}'.");
                        break;
                }
            }
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
