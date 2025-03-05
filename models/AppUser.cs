using Microsoft.AspNetCore.Identity;

namespace FinSharkMarket.models;

public class AppUser: IdentityUser
{
    public List<PortFolio> PortFolios { get; set; } = new List<PortFolio>();
}