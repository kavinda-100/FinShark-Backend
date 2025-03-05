using FinSharkMarket.models;

namespace FinSharkMarket.interfaces.PortFolios;

public interface IPortFolioRepository
{
    Task<List<Stocks>> GetUserPortfolio(AppUser user);
}