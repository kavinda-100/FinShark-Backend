

using FinSharkMarket.data;
using FinSharkMarket.Dtos.stocks;
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
    
    // get all stocks
    public async Task<List<Stocks>> GetAllStocksAsync()
    {
        return await _context.Stocks.Include(c => c.Comments).ToListAsync();
    }

    // get stock by id
    public async Task<Stocks?> GetStockByIdAsync(Guid id)
    {
        var stock = await _context.Stocks.Include(c => c.Comments).FirstOrDefaultAsync(s => s.Id == id);
        
        return stock;
    }

    // create stock
    public async Task<Stocks> CreateStockAsync(Stocks stock)
    {
        await _context.Stocks.AddAsync(stock);
        await _context.SaveChangesAsync();
        
        return stock;
    }

    // update stock
    public async Task<Stocks?> UpdateStockAsync(Guid id, UpdateRequestStockDto stockDto)
    {
        var stock = await _context.Stocks.FindAsync(id);
        
        if (stock == null)
        {
            return null;
        }
        
        stock.Symbol = stockDto.Symbol;
        stock.CompanyName = stockDto.CompanyName;
        stock.Price = stockDto.Price;
        stock.LastDiv = stockDto.LastDiv;
        stock.Industry = stockDto.Industry;
        stock.MarketCap = stockDto.MarketCap;
        
        _context.Stocks.Update(stock); //* not an async method
        await _context.SaveChangesAsync();
        
        return stock;
    }

    // delete stock
    public async Task<Stocks?> DeleteStockAsync(Guid id)
    {
        var stock = await _context.Stocks.FindAsync(id);
        
        if (stock == null)
        {
            return null;
        }
        
        _context.Stocks.Remove(stock); //* not an async method
        await _context.SaveChangesAsync();
        
        return stock;
    }
}