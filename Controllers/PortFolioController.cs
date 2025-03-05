using FinSharkMarket.Extensions;
using FinSharkMarket.interfaces.PortFolios;
using FinSharkMarket.interfaces.stocks;
using FinSharkMarket.models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FinSharkMarket.Controllers;

[Route("api/portfolio")]
[ApiController]
public class PortFolioController: ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IStockRepository _stockRepository;
    private readonly IPortFolioRepository _portFolioRepository;
    
    public PortFolioController(UserManager<AppUser> userManager, IStockRepository stockRepository, IPortFolioRepository portFolioRepository)
    {
        _userManager = userManager;
        _stockRepository = stockRepository;
        _portFolioRepository = portFolioRepository;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetUserPortfolio()
    {
        // Get the user email
        var email = User.GetUserEmail();
        // if email is null return unauthorized
        if (email == null) return StatusCode(500, new {message = "User email is null"});
        // Find the user by email
        var user = await _userManager.FindByEmailAsync(email);
        // if user is null return unauthorized
        if (user == null) return Unauthorized();
        
        var portfolio = await _portFolioRepository.GetUserPortfolio(user);
        return Ok(portfolio);
    }
    
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddStockToPortfolio(String symbol)
    {
        // Get the user email
        var email = User.GetUserEmail();
        // if email is null return unauthorized
        if (email == null) return StatusCode(500, new {message = "User email is null"});
        // Find the user by email
        var user = await _userManager.FindByEmailAsync(email);
        // if user is null return unauthorized
        if (user == null) return Unauthorized();
        // Find the stock by symbol
        var stock = await _stockRepository.GetStockBySymbolAsync(symbol);
        // if stock is null return not found
        if (stock == null) return NotFound(new {message = "Stock not found"});
        // Check if the stock is already in the user's portfolio
        var portfolio = await _portFolioRepository.GetUserPortfolio(user);
        // if stock is already in portfolio return bad request
        if (portfolio.Any(s => s.Symbol.ToLower() == symbol.ToLower())) return BadRequest(new {message = "Stock already in portfolio"});
        // Add the stock to the user's portfolio
        var profile = new PortFolio
        {
            AppUserId = user.Id,
            StockId = stock.Id
        };
        await _portFolioRepository.CreatePortfolio(profile);

        return Created();
    }
    
    [HttpDelete]
    [Authorize]
    public async Task<IActionResult> RemoveStockFromPortfolio(String symbol)
    {
        // Get the user email
        var email = User.GetUserEmail();
        // if email is null return unauthorized
        if (email == null) return StatusCode(500, new {message = "User email is null"});
        // Find the user by email
        var user = await _userManager.FindByEmailAsync(email);
        // if user is null return unauthorized
        if (user == null) return Unauthorized();
        // Find the stock by symbol
        var stock = await _stockRepository.GetStockBySymbolAsync(symbol);
        // if stock is null return not found
        if (stock == null) return NotFound(new {message = "Stock not found"});
        // Check if the stock is already in the user's portfolio
        var portfolio = await _portFolioRepository.GetUserPortfolio(user);
        // if stock is not in portfolio return bad request
        bool stockInPortfolio = portfolio.Any(s => s.Symbol.ToLower() == symbol.ToLower());
        if (!stockInPortfolio) return BadRequest(new {message = "Stock not in portfolio"});
        // Remove the stock from the user's portfolio
        await _portFolioRepository.DeletePortfolio(user, symbol);

        return Ok(new {message = "Stock removed from portfolio"});
    }
}