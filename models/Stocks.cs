using System.ComponentModel.DataAnnotations.Schema;
using FinSharkMarket.utils;

namespace FinSharkMarket.models;

public class Stocks
{
    [Column(TypeName = "uuid")]
    public Guid Id { get; set; }
    [Column(TypeName = "varchar(255)")]
    public String Symbol { get; set; } = String.Empty;
    [Column(TypeName = "varchar(255)")]
    public String CompanyName { get; set; } = String.Empty;
    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }
    [Column(TypeName = "decimal(18,2)")]
    public decimal LastDiv { get; set; }
    [Column(TypeName = "varchar(255)")]
    public String Industry { get; set; } = String.Empty;
    public long MarketCap { get; set; }
    // relationships
    public List<Comments> Comments { get; set; } = new List<Comments>();
    // date fields
    public DateTime CreatedAt { get; set; } = DateTimeUtils.ToUtc(DateTime.Now);
    public DateTime UpdatedAt { get; set; } = DateTimeUtils.ToUtc(DateTime.Now);
    
    
}