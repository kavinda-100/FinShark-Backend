using FinSharkMarket.Dtos.stocks;
using FinSharkMarket.interfaces.stocks;
using FinSharkMarket.Mappers.stocks;
using Microsoft.AspNetCore.Mvc;

namespace FinSharkMarket.Controllers;

[Route("api/stocks")]
[ApiController]
public class StockController : ControllerBase
{
    private readonly IStockRepository _stockRepository;
    
    public StockController(IStockRepository stockRepository)
    {
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
        var stock = await _stockRepository.GetStockByIdAsync(id);
        
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
        
        await _stockRepository.CreateStockAsync(stock);
        
        return CreatedAtAction(nameof(GetStockById), new { id = stock.Id }, stock.ToStockDto());
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateStock([FromRoute] Guid id, [FromBody] UpdateRequestStockDto resBody)
    {
        var stock = await _stockRepository.UpdateStockAsync(id, resBody);
        
        if (stock == null)
        {
            return NotFound("Stock not found");
        }
        
        return Ok(stock.ToStockDto());
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStock([FromRoute] Guid id)
    {
        var stock = await _stockRepository.DeleteStockAsync(id);
        
        if (stock == null)
        {
            return NotFound("Stock not found");
        }
        
        return NoContent();
    }
}