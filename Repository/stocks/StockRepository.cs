

using FinSharkMarket.data;
using FinSharkMarket.Dtos.stocks;
using FinSharkMarket.interfaces.stocks;
using FinSharkMarket.models;
using FinSharkMarket.QueryParams;
using Microsoft.EntityFrameworkCore;

namespace FinSharkMarket.Repository.stocks;

public class StockRepository : IStockRepository
{
    private readonly ApplicationDbContext _context;
    // constructor to inject ApplicationDbContext
    public StockRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    // get all stocks
    public async Task<List<Stocks>> GetAllStocksAsync(StockQuery query)
    {
        var stock = _context.Stocks.Include(c => c.Comments).AsQueryable();

        // filter by CompanyName
        if (!String.IsNullOrWhiteSpace(query.CompanyName))
        {
            stock = stock.Where(s => s.CompanyName.Contains(query.CompanyName));
        }
        // filter by Symbol
        if(!String.IsNullOrWhiteSpace(query.Symbol))
        {
            stock = stock.Where(s => s.Symbol.Contains(query.Symbol));
        }

        if (!String.IsNullOrWhiteSpace(query.SortBy))
        {
            // sort by Symbol
            if (query.SortBy.Equals("Symbol", StringComparison.OrdinalIgnoreCase))
            {
                stock = query.IsDescending ? stock.OrderByDescending(s => s.Symbol) : stock.OrderBy(s => s.Symbol);
            }
            // sort by CompanyName
            if (query.SortBy.Equals("companyName", StringComparison.OrdinalIgnoreCase))
            {
                stock = query.IsDescending ? stock.OrderByDescending(s => s.CompanyName) : stock.OrderBy(s => s.CompanyName);
            }
            // sort by Price
            if (query.SortBy.Equals("industry", StringComparison.OrdinalIgnoreCase))
            {
                stock = query.IsDescending ? stock.OrderByDescending(s => s.Industry) : stock.OrderBy(s => s.Industry);
            }
        }
        
        // pagination
        var skipNumber = (query.PageNumber - 1) * query.PageSize;
        
        // return all stocks
        return await stock.Skip(skipNumber).Take(query.PageSize).ToListAsync();
    }

    // get stock by id
    public async Task<Stocks?> GetStockByIdAsync(Guid id)
    {
        // get stock by id and include comments
        var stock = await _context.Stocks.Include(c => c.Comments).FirstOrDefaultAsync(s => s.Id == id);
        
        return stock;
    }

    // create stock
    public async Task<Stocks> CreateStockAsync(Stocks stock)
    {
        // add stock to a database
        await _context.Stocks.AddAsync(stock);
        await _context.SaveChangesAsync();
        
        return stock;
    }

    // update stock
    public async Task<Stocks?> UpdateStockAsync(Guid id, UpdateRequestStockDto stockDto)
    {
        // get stock by id
        var stock = await _context.Stocks.FindAsync(id);
        
        if (stock == null)
        {
            return null;
        }
        
        // update stock
        stock.Symbol = stockDto.Symbol;
        stock.CompanyName = stockDto.CompanyName;
        stock.Price = stockDto.Price;
        stock.LastDiv = stockDto.LastDiv;
        stock.Industry = stockDto.Industry;
        stock.MarketCap = stockDto.MarketCap;
        
        // save changes
        _context.Stocks.Update(stock); //* not an async method
        await _context.SaveChangesAsync();
        
        return stock;
    }

    // delete stock
    public async Task<Stocks?> DeleteStockAsync(Guid id)
    {
        // get stock by id
        var stock = await _context.Stocks.FindAsync(id);
        
        if (stock == null)
        {
            return null;
        }
        
        // remove stock
        _context.Stocks.Remove(stock); //* not an async method
        await _context.SaveChangesAsync();
        
        return stock;
    }

    public Task<bool> StockExists(Guid id)
    {
        // check if stock exists
        return _context.Stocks.AnyAsync(s => s.Id == id);
    }

    public async Task<Stocks?> GetStockBySymbolAsync(string symbol)
    {
        return await _context.Stocks.FirstOrDefaultAsync(s => s.Symbol == symbol);
    }
}