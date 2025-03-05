using FinSharkMarket.Dtos.Account;
using FinSharkMarket.interfaces.services;
using FinSharkMarket.models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FinSharkMarket.Controllers;

[Route("api/account")]
[ApiController]
public class AccountController: ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly ITokenService _tokenService;
    
    public AccountController(UserManager<AppUser> userManager, ITokenService tokenService, SignInManager<AppUser> signInManager)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _signInManager = signInManager;
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
                    return Ok(new ResponseRegisterDto
                    {
                        Email = appUser.Email!,
                        Token = _tokenService.CreateToken(appUser)
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

    [HttpPost("login")]
    public async Task<IActionResult> LogIn([FromBody] RequestLogInDto requestLogInDto)
    {
        if(!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        // Check if the user exists
        var user = await _userManager.FindByEmailAsync(requestLogInDto.Email!);
        if (user == null)
        {
            return Unauthorized("Invalid Credentials");
        }
        // Check if the password is correct
        var result = await _signInManager.CheckPasswordSignInAsync(user, requestLogInDto.Password!, false);
        // If the password is InCorrect
        if (!result.Succeeded)
        {
            return Unauthorized("Invalid Credentials");
        }
        // If the password is correct
        return Ok(new ResponseRegisterDto
        {
            Email = user.Email!,
            Token = _tokenService.CreateToken(user)
        });
    }
    
}