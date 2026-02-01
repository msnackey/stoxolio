using Microsoft.EntityFrameworkCore;
using Stoxolio.Service.BuildingBlocks.CQRS;
using Stoxolio.Service.Data;
using Stoxolio.Service.Models;

namespace Stoxolio.Service.Features.Stocks;

public sealed record GetStocksQuery : IQuery<GetStocksResponse>;

public sealed record GetStocksResponse
{
    public required List<Stock> Stocks { get; init; }
}

public class GetStocksHandler(StoxolioDbContext context) : IQueryHandler<GetStocksQuery, GetStocksResponse>
{
    public async Task<GetStocksResponse?> Handle(GetStocksQuery request, CancellationToken cancellationToken)
    {
        var stocks = await context.Stocks.ToListAsync(cancellationToken);

        return new GetStocksResponse
        {
            Stocks = stocks.Select(s => new Stock
            {
                Id = s.Id,
                Name = s.Name,
                Ticker = s.Ticker,
                Exchange = s.Exchange,
                Sri = s.Sri,
                Shares = s.Shares,
                Price = s.Price,
                Invest = s.Invest,
                CategoryId = s.CategoryId,
                PrevPrice = s.PrevPrice
            }).ToList()
        };
    }
}