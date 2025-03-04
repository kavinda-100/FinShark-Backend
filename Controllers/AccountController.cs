using FinSharkMarket.Dtos.Account;
using FinSharkMarket.models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FinSharkMarket.Controllers;

[Route("api/account")]
[ApiController]
public class AccountController: ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    
    public AccountController(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RequestRegisterDto registerDto)
    {
        // Check if the model is valid
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        // do the registration
        try
        {
            // Create a new user object
            var appUser = new AppUser()
            {
                UserName = registerDto.Email,
                Email = registerDto.Email
            };
            // Create the user
            var createdUser = await _userManager.CreateAsync(appUser, registerDto.Password);
            // Check if the user was created successfully
            if (createdUser.Succeeded)
            {
                // anyone who registers via this endpoint is a user
                var roleResult = await _userManager.AddToRoleAsync(appUser, "User");
                // Check if the role was added successfully
                if (roleResult.Succeeded)
                {
                    return Ok(new
                    {
                        message = "User created successfully"
                    });
                }
                else
                {
                    return StatusCode(500, roleResult.Errors);
                }
            }
            else
            {
                return StatusCode(500, createdUser.Errors);
            }
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
    
}