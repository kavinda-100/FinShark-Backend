using System.Security.Claims;

namespace FinSharkMarket.Extensions;

public static class ClaimsExtensions
{
    public static string? GetUserEmail(this ClaimsPrincipal user)
    {
        // return user.Claims.SingleOrDefault(x => x.Type.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname"))?.Value;
        var email = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

        if (string.IsNullOrWhiteSpace(email))
        {
            return null;
        }
        
        return email;
    }
}