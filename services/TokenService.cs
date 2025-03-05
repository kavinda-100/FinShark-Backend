using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FinSharkMarket.interfaces.services;
using FinSharkMarket.models;
using Microsoft.IdentityModel.Tokens;

namespace FinSharkMarket.services;

public class TokenService: ITokenService
{
    // The IConfiguration interface is used to access configuration settings in the appsettings.json file.
    private readonly IConfiguration _config;
    private readonly SymmetricSecurityKey _key;
    
    public TokenService(IConfiguration config)
    {
        _config = config;
        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:SignInKey"]!));
    }
    
    // Create a token for the user
    public string CreateToken(AppUser user)
    {
        // Claims are the statements about an entity (typically, the user) and additional data.
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Email, user.Email!),
            new Claim(JwtRegisteredClaimNames.Sub, user.Id)
        };
        // Signing credentials are used to sign the token.
        var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256);
        // Token descriptor is used to create the token.
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = creds,
            Issuer = _config["Jwt:Issuer"],
            Audience = _config["Jwt:Audience"]
        };
        // Create the token handler
        var tokenHandler = new JwtSecurityTokenHandler();
        // Create the token
        var token = tokenHandler.CreateToken(tokenDescriptor);
        // Write the token
        return tokenHandler.WriteToken(token);
    }
}