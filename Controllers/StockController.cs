using FinSharkMarket.data;
using FinSharkMarket.Mappers.stocks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinSharkMarket.Controllers;

[Route("api/stocks")]
[ApiController]
public class StockController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    
    public StockController(ApplicationDbContext context)
    {
        _context = context;
    }
    
    [HttpGet]
    public async Task<ActionResult> GetAllStocks()
    {
        var stocks = await _context.Stocks.ToListAsync();
        // Map Stocks to StockDto
        var stockDtos = stocks.Select(stock => stock.ToStockDto());
        
        return Ok(stockDtos);
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult> GetStockById([FromRoute] Guid id)
    {
        var stock = await _context.Stocks.FindAsync(id);
        
        if (stock == null)
        {
            return NotFound("Stock not found");
        }
        
        return Ok(stock.ToStockDto());
    }
}