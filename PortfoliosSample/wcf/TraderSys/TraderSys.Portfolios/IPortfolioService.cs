using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;
using TraderSys.PortfolioData.Models;

namespace TraderSys
{
    [ServiceContract]
    public interface IPortfolioService
    {
        [OperationContract]
        Task<Portfolio> Get(Guid traderId, int portfolioId);

        [OperationContract]
        Task<List<Portfolio>> GetAll(Guid traderId);
    }

}
