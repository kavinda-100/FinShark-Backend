namespace FinSharkMarket.Dtos.stocks;

public class StockDto
{
    public String Symbol { get; set; } = String.Empty;
    public String CompanyName { get; set; } = String.Empty;
    public decimal Price { get; set; }
    public decimal LastDiv { get; set; }
    public String Industry { get; set; } = String.Empty;
    public long MarketCap { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}