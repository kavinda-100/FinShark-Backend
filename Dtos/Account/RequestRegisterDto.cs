using System.ComponentModel.DataAnnotations;

namespace FinSharkMarket.Dtos.Account;

public class RequestRegisterDto
{
    [Required]
    [EmailAddress]
    public String? Email { get; set; }
    
    [Required]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
    [MaxLength(12, ErrorMessage = "Password must be at most 12 characters")]
    public String? Password { get; set; }
}