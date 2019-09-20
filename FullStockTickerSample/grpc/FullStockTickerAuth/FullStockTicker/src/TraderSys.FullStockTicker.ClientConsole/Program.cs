using System;
using System.Threading;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Net.Client;
using TraderSys.FullStockTickerServer.Protos;

namespace TraderSys.FullStockTicker.ClientConsole
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new FullStockTickerServer.Protos.FullStockTicker.FullStockTickerClient(channel);

            using var stream = client.Subscribe();

            var tokenSource = new CancellationTokenSource();
            var displayTask = DisplayAsync(stream.ResponseStream, tokenSource.Token);
            
            await CommandLoopAsync(stream.RequestStream, tokenSource.Token);

            tokenSource.Cancel();
            await displayTask;
        }

        static async Task CommandLoopAsync(IAsyncStreamWriter<ActionMessage> stream, CancellationToken token)
        {
            Console.WriteLine("Use +XXXX to add a symbol, -XXXX to remove it.");
            await Task.Yield();
            
            while (!token.IsCancellationRequested)
            {
                Console.Write("> ");
                var command = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(command)) break;
                ActionMessage message;
                switch (command[0])
                {
                    case '+':
                        message = new ActionMessage {Add = new AddSymbolRequest {Symbol = command.Substring(1)}};
                        break;
                    case '-':
                        message = new ActionMessage {Remove = new RemoveSymbolRequest {Symbol = command.Substring(1)}};
                        break;
                    default:
                        Console.WriteLine($"Unknown command: {command}");
                        continue;
                }

                try
                {
                    await stream.WriteAsync(message);
                }
                catch (RpcException ex)
                {
                    Console.WriteLine($"Error sending command: {ex.Message}");
                }
                catch (OperationCanceledException)
                {
                    // Ignored
                }
            }
            
        }

        static async Task DisplayAsync(IAsyncStreamReader<StockTickerUpdate> stream, CancellationToken token)
        {
            try
            {
                while (await stream.MoveNext(token))
                {
                    var update = stream.Current;
                    Console.WriteLine($"{update.Symbol}: {update.Price}");
                }
            }
            catch (RpcException e)
            {
                if (e.StatusCode == StatusCode.Cancelled)
                {
                    return;
                }
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Finished.");
            }
        }

        static void WaitForExitKey()
        {
            Console.WriteLine("Press E to exit...");

            char ch = ' ';

            while (ch != 'e')
            {
                ch = char.ToLowerInvariant(Console.ReadKey().KeyChar);
            }
        }
    }
}
