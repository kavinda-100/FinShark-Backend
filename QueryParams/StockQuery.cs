
namespace FinSharkMarket.QueryParams;

public class StockQuery
{
    public String? Symbol { get; set; } = null;
    public String? CompanyName { get; set; } = null;
    public String? SortBy { get; set; } = null;
    public bool IsDescending { get; set; } = false;
}