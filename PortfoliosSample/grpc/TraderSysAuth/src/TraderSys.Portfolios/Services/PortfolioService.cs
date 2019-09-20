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
        private readonly IUserLookup _userLookup;

        public PortfolioService(IPortfolioRepository repository, IUserLookup userLookup)
        {
            _repository = repository;
            _userLookup = userLookup;
        }

        [Authorize]
        public override async Task<GetResponse> Get(GetRequest request, ServerCallContext context)
        {
            var traderId = ValidateUser(request.TraderId, context);

            var portfolio = await _repository.GetAsync(traderId, request.PortfolioId);
            
            return new GetResponse
            {
                Portfolio = Portfolio.FromRepositoryModel(portfolio)
            };
        }

        public override async Task<GetAllResponse> GetAll(GetAllRequest request, ServerCallContext context)
        {
            var traderId = ValidateUser(request.TraderId, context);

            var portfolios = await _repository.GetAllAsync(traderId);
            
            var response = new GetAllResponse();
            response.Portfolios.AddRange(portfolios.Select(Portfolio.FromRepositoryModel));

            return response;
        }

        private Guid ValidateUser(string suppliedId, ServerCallContext context)
        {
            if (!Guid.TryParse(suppliedId, out var traderId))
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "traderId must be a UUID"));
            }

            var user = context.GetHttpContext().User;
            
            if (!_userLookup.TryGetId(user.Identity.Name, out var knownGuid) || knownGuid != traderId)
            {
                throw new RpcException(new Status(StatusCode.PermissionDenied, "Permission denied."));
            }

            return traderId;
        }
    }
}
