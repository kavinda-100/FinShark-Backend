using FinSharkMarket.Dtos.stocks;
using FinSharkMarket.models;

namespace FinSharkMarket.Mappers.stocks;

public static class StockMappers
{
    // Map Stocks to StockDto (Response body type)
    public static StockDto ToStockDto(this Stocks stocks)
    {
        return new StockDto
        {
            Symbol = stocks.Symbol,
            CompanyName = stocks.CompanyName,
            Price = stocks.Price,
            LastDiv = stocks.LastDiv,
            Industry = stocks.Industry,
            MarketCap = stocks.MarketCap
        };
    }
}