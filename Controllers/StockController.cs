using FinSharkMarket.data;
using FinSharkMarket.Dtos.stocks;
using FinSharkMarket.interfaces.stocks;
using FinSharkMarket.Mappers.stocks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinSharkMarket.Controllers;

[Route("api/stocks")]
[ApiController]
public class StockController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IStockRepository _stockRepository;
    
    public StockController(ApplicationDbContext context, IStockRepository stockRepository)
    {
        _context = context;
        _stockRepository = stockRepository;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllStocks()
    {
        var stocks = await _stockRepository.GetAllStocksAsync();
        // Map Stocks to StockDto
        var stockDtos = stocks.Select(stock => stock.ToStockDto());
        
        return Ok(stockDtos);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetStockById([FromRoute] Guid id)
    {
        var stock = await _context.Stocks.FindAsync(id);
        
        if (stock == null)
        {
            return NotFound("Stock not found");
        }
        
        return Ok(stock.ToStockDto());
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateStock([FromBody] RequestStockDto resBody)
    {
        var stock = resBody.ToStock();
        
        await _context.Stocks.AddAsync(stock);
        await _context.SaveChangesAsync();
        
        return CreatedAtAction(nameof(GetStockById), new { id = stock.Id }, stock.ToStockDto());
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateStock([FromRoute] Guid id, [FromBody] UpdateRequestStockDto resBody)
    {
        var stock = await _context.Stocks.FindAsync(id);
        
        if (stock == null)
        {
            return NotFound("Stock not found");
        }
        
        stock.Symbol = resBody.Symbol;
        stock.CompanyName = resBody.CompanyName;
        stock.Price = resBody.Price;
        stock.LastDiv = resBody.LastDiv;
        stock.Industry = resBody.Industry;
        stock.MarketCap = resBody.MarketCap;
        
        _context.Stocks.Update(stock); //* not an async method
        await _context.SaveChangesAsync();
        
        return Ok(stock.ToStockDto());
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStock([FromRoute] Guid id)
    {
        var stock = await _context.Stocks.FindAsync(id);
        
        if (stock == null)
        {
            return NotFound("Stock not found");
        }
        
        _context.Stocks.Remove(stock); //* not an async method
        await _context.SaveChangesAsync();
        
        return NoContent();
    }
}