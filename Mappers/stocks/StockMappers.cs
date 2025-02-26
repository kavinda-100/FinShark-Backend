using FinSharkMarket.Dtos.stocks;
using FinSharkMarket.Mappers.comments;
using FinSharkMarket.models;

namespace FinSharkMarket.Mappers.stocks;

public static class StockMappers
{
    // Map Stocks to StockDto (Response body type)
    public static ResponseStockDto ToStockDto(this Stocks stocks)
    {
        return new ResponseStockDto
        {
            Id = stocks.Id,
            Symbol = stocks.Symbol,
            CompanyName = stocks.CompanyName,
            Price = stocks.Price,
            LastDiv = stocks.LastDiv,
            Industry = stocks.Industry,
            MarketCap = stocks.MarketCap,
            Comments = stocks.Comments.Select(c => c.ToResponseCommentDto()).ToList(),
            CreatedAt = stocks.CreatedAt,
            UpdatedAt = stocks.UpdatedAt
        };
    }
    
    // Map StockDto (Request body type) to Stocks
    public static Stocks ToStock(this RequestStockDto stockDto)
    {
        return new Stocks
        {
            Symbol = stockDto.Symbol,
            CompanyName = stockDto.CompanyName,
            Price = stockDto.Price,
            LastDiv = stockDto.LastDiv,
            Industry = stockDto.Industry,
            MarketCap = stockDto.MarketCap,
        };
    }
}