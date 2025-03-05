using FinSharkMarket.models;

namespace FinSharkMarket.interfaces.PortFolios;

public interface IPortFolioRepository
{
    Task<List<Stocks>> GetUserPortfolio(AppUser user);
    Task<PortFolio> CreatePortfolio(PortFolio portFolios);
    Task<PortFolio?> DeletePortfolio(AppUser user, String symbol);
}