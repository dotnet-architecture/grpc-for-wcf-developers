
namespace TraderSys.PortfolioData;
public interface IPortfolioRepository
{
    Task<Portfolio> GetAsync(Guid traderId, int portfolioId);
    Task<List<Portfolio>> GetAllAsync(Guid traderId);
}