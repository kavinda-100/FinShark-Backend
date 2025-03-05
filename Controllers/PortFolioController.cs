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
        if (email == null) return StatusCode(500, "User email is null");
        // Find the user by email
        var user = await _userManager.FindByEmailAsync(email);
        // if user is null return unauthorized
        if (user == null) return Unauthorized();
        
        var portfolio = await _portFolioRepository.GetUserPortfolio(user);
        return Ok(portfolio);
    }
}