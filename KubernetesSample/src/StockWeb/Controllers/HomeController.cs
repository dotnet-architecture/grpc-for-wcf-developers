using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StockData.Protos;
using StockWeb.Internal;
using StockWeb.Models;

namespace StockWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly Stocks.StocksClient _stocksClient;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, Stocks.StocksClient stocksClient)
        {
            _stocksClient = stocksClient;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var tasks = Enumerable.Range(1, 100).Select(GetAsync);
            var stockViewModels = await Task.WhenAll(tasks);
            return View(new IndexViewModel(stockViewModels.ExcludeNulls().OrderBy(m => m.Symbol)));
        }

        private async Task<StockViewModel> GetAsync(int id)
        {
            try
            {
                var response = _stocksClient.GetAsync(new GetRequest {Id = id + 1});
                var getResponse = await response.ResponseAsync;
                var headers = await response.ResponseHeadersAsync;
                return new StockViewModel(getResponse.Stock, headers.GetString("server-id"));
            }
            catch (RpcException e) when (e.StatusCode == Grpc.Core.StatusCode.NotFound)
            {
                _logger.LogWarning("Stock {id} not found", id);
                return null;
            }
            catch (RpcException e)
            {
                _logger.LogError(e, e.Message);
                return null;
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}