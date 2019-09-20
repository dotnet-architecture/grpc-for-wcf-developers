using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TraderSys.PortfolioData;
using TraderSys.PortfolioData.Models;

namespace TraderSys
{
    public class PortfolioService : IPortfolioService
    {
        private readonly IPortfolioRepository _repository;

        public PortfolioService(IPortfolioRepository repository)
        {
            _repository = repository;
        }

        public async Task<Portfolio> Get(Guid traderId, int portfolioId)
        {
            return await _repository.GetAsync(traderId, portfolioId);
        }

        public async Task<List<Portfolio>> GetAll(Guid traderId)
        {
            return await _repository.GetAllAsync(traderId);
        }
    }
}
