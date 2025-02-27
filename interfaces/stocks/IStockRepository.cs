using FinSharkMarket.Dtos.stocks;
using FinSharkMarket.models;
using FinSharkMarket.QueryParams;

namespace FinSharkMarket.interfaces.stocks;

public interface IStockRepository
{
    Task<List<Stocks>> GetAllStocksAsync(StockQuery query);
    Task<Stocks?> GetStockByIdAsync(Guid id);
    Task<Stocks> CreateStockAsync(Stocks stock);
    Task<Stocks?> UpdateStockAsync(Guid id, UpdateRequestStockDto stockDto);
    Task<Stocks?> DeleteStockAsync(Guid id);
    Task<bool> StockExists(Guid id);
}