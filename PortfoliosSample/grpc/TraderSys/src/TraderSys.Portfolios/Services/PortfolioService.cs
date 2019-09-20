using System;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using TraderSys.PortfolioData;
using TraderSys.Portfolios.Protos;

namespace TraderSys.Portfolios.Services
{
    public class PortfolioService : Protos.Portfolios.PortfoliosBase
    {
        private readonly IPortfolioRepository _repository;

        public PortfolioService(IPortfolioRepository repository)
        {
            _repository = repository;
        }

        [Authorize]
        public override async Task<GetResponse> Get(GetRequest request, ServerCallContext context)
        {
            if (!Guid.TryParse(request.TraderId, out var traderId))
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "traderId must be a UUID"));
            }

            var portfolio = await _repository.GetAsync(traderId, request.PortfolioId);
            
            return new GetResponse
            {
                Portfolio = Portfolio.FromRepositoryModel(portfolio)
            };
        }

        public override async Task<GetAllResponse> GetAll(GetAllRequest request, ServerCallContext context)
        {
            if (!Guid.TryParse(request.TraderId, out var traderId))
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "traderId must be a UUID"));
            }

            var portfolios = await _repository.GetAllAsync(traderId);
            
            var response = new GetAllResponse();
            response.Portfolios.AddRange(portfolios.Select(Portfolio.FromRepositoryModel));

            return response;
        }
    }
}
