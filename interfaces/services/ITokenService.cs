using FinSharkMarket.models;

namespace FinSharkMarket.interfaces.services;

public interface ITokenService
{
    String CreateToken(AppUser user);
}