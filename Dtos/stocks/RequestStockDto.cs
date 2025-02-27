using System.ComponentModel.DataAnnotations;

namespace FinSharkMarket.Dtos.stocks;

public class RequestStockDto
{
    [Required]
    [MaxLength(10, ErrorMessage = "Symbol must be less than 10 characters")]
    public String Symbol { get; set; } = String.Empty;
    
    [Required]
    [MinLength(2, ErrorMessage = "Company Name must be more than 2 characters")]
    [MaxLength(255, ErrorMessage = "Company Name must be less than 255 characters")]
    public String CompanyName { get; set; } = String.Empty;
    
    [Required]
    [Range(1, 1000000000000)]
    public decimal Price { get; set; }
    
    [Required]
    [Range(0.001, 100)]
    public decimal LastDiv { get; set; }
    
    [Required]
    [MinLength(2, ErrorMessage = "Industry must be more than 2 characters")]
    [MaxLength(255, ErrorMessage = "Industry must be less than 255 characters")]
    public String Industry { get; set; } = String.Empty;
    
    [Required]
    [Range(1, 1000000000000)]
    public long MarketCap { get; set; }
}