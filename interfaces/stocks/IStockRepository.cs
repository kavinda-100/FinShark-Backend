using FinSharkMarket.models;

namespace FinSharkMarket.interfaces.stocks;

public interface IStockRepository
{
    Task<List<Stocks>> GetAllStocksAsync();
}