using FinSharkMarket.Dtos.comments;

namespace FinSharkMarket.Dtos.stocks;

public class ResponseStockDto
{
    public Guid Id { get; set; }
    public String Symbol { get; set; } = String.Empty;
    public String CompanyName { get; set; } = String.Empty;
    public decimal Price { get; set; }
    public decimal LastDiv { get; set; }
    public String Industry { get; set; } = String.Empty;
    public long MarketCap { get; set; }
    public List<ResponseCommentDto> Comments { get; set; } = new List<ResponseCommentDto>();
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}