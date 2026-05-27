using Stoxolio.Service.DTOs;
using Stoxolio.Service.Models;

namespace Stoxolio.Service.Mappings;

public static class StockMappings
{
    public static StockDto ToDto(this Stock stock) => new()
    {
        Id = stock.Id,
        Name = stock.Name,
        Ticker = stock.Ticker,
        Exchange = stock.Exchange,
        Sri = stock.Sri,
        Shares = stock.Shares,
        Price = stock.Price,
        Invest = stock.Invest,
        CategoryId = stock.CategoryId,
        PrevPrice = stock.PrevPrice,
        Value = stock.Value,
        PriceChange = stock.PriceChange,
        ValueChange = stock.ValueChange
    };
}
