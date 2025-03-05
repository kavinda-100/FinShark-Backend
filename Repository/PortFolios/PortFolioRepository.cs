using FinSharkMarket.data;
using FinSharkMarket.interfaces.PortFolios;
using FinSharkMarket.models;
using Microsoft.EntityFrameworkCore;

namespace FinSharkMarket.Repository.PortFolios;

public class PortFolioRepository: IPortFolioRepository
{
    private readonly ApplicationDbContext _context;
    
    public PortFolioRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<Stocks>> GetUserPortfolio(AppUser user)
    {
        return await _context.PortFolios.Where(u => u.AppUserId == user.Id)
            .Select(stock => new Stocks
            {
                Id = stock.StockId,
                Symbol = stock.Stock.Symbol,
                CompanyName = stock.Stock.CompanyName,
                Price = stock.Stock.Price,
                Industry = stock.Stock.Industry,
                LastDiv = stock.Stock.LastDiv,
                MarketCap = stock.Stock.MarketCap,

            }).ToListAsync();
    }

    public async Task<PortFolio> CreatePortfolio(PortFolio portFolios)
    {
        await _context.PortFolios.AddAsync(portFolios);
        await _context.SaveChangesAsync();
        return portFolios;
    }

    public async Task<PortFolio?> DeletePortfolio(AppUser user, string symbol)
    {
        var portfolio = await _context.PortFolios.FirstOrDefaultAsync(p => p.AppUserId == user.Id && p.Stock.Symbol.ToLower() == symbol.ToLower());

        if (portfolio == null)
        {
            return null;
        }
        
        _context.PortFolios.Remove(portfolio);
        await _context.SaveChangesAsync();
        
        return portfolio;
    }
}