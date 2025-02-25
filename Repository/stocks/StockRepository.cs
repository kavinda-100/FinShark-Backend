

using FinSharkMarket.data;
using FinSharkMarket.interfaces.stocks;
using FinSharkMarket.models;
using Microsoft.EntityFrameworkCore;

namespace FinSharkMarket.Repository.stocks;

public class StockRepository : IStockRepository
{
    private readonly ApplicationDbContext _context;
    
    public StockRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<Stocks>> GetAllStocksAsync()
    {
        return await _context.Stocks.ToListAsync();
    }
}