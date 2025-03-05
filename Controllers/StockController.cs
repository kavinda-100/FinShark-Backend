using FinSharkMarket.Dtos.stocks;
using FinSharkMarket.interfaces.stocks;
using FinSharkMarket.Mappers.stocks;
using FinSharkMarket.QueryParams;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    public async Task<IActionResult> GetAllStocks([FromQuery] StockQuery query)
    {
        // Check if the model/data is valid
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var stocks = await _stockRepository.GetAllStocksAsync(query);
        // Map Stocks to StockDto
        var stockDtos = stocks.Select(stock => stock.ToStockDto()).ToList();
        
        return Ok(stockDtos);
    }
    
    [HttpGet("{id:Guid}")]
    [Authorize]
    public async Task<IActionResult> GetStockById([FromRoute] Guid id)
    {
        // Check if the model/data is valid
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var stock = await _stockRepository.GetStockByIdAsync(id);
        
        if (stock == null)
        {
            return NotFound("Stock not found");
        }
        
        return Ok(stock.ToStockDto());
    }
    
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateStock([FromBody] RequestStockDto resBody)
    {
        // Check if the model/data is valid
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var stock = resBody.ToStock();
        
        await _stockRepository.CreateStockAsync(stock);
        
        return CreatedAtAction(nameof(GetStockById), new { id = stock.Id }, stock.ToStockDto());
    }

    [HttpPut("{id:Guid}")]
    [Authorize]
    public async Task<IActionResult> UpdateStock([FromRoute] Guid id, [FromBody] UpdateRequestStockDto resBody)
    {
        // Check if the model/data is valid
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var stock = await _stockRepository.UpdateStockAsync(id, resBody);
        
        if (stock == null)
        {
            return NotFound("Stock not found");
        }
        
        return Ok(stock.ToStockDto());
    }
    
    [HttpDelete("{id:Guid}")]
    [Authorize]
    public async Task<IActionResult> DeleteStock([FromRoute] Guid id)
    {
        // Check if the model/data is valid
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var stock = await _stockRepository.DeleteStockAsync(id);
        
        if (stock == null)
        {
            return NotFound("Stock not found");
        }
        
        return NoContent();
    }
}