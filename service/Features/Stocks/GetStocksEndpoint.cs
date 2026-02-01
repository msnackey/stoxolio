using Microsoft.EntityFrameworkCore;
using Stoxolio.Service.BuildingBlocks.CQRS;
using Stoxolio.Service.Data;
using Stoxolio.Service.DTOs;

namespace Stoxolio.Service.Features.Stocks;

public sealed record GetStocksQuery : IQuery<List<StockDto>>;

public class GetStocksEndpoint
{
    private static async Task<IResult> HandleGetStocks(
        StoxolioDbContext context,
        CancellationToken cancellationToken)
    {
        var handler = new GetStocksHandler(context);
        var result = await handler.Handle(new GetStocksQuery(), cancellationToken);
        return Results.Ok(result);
    }

    public static void MapEndpoint(RouteGroupBuilder group)
    {
        group.MapGet("/", HandleGetStocks)
            .WithName("GetStocks");
    }

    private class GetStocksHandler : IQueryHandler<GetStocksQuery, List<StockDto>>
    {
        private readonly StoxolioDbContext _context;

        public GetStocksHandler(StoxolioDbContext context)
        {
            _context = context;
        }

        public async Task<List<StockDto>?> Handle(GetStocksQuery request, CancellationToken cancellationToken)
        {
            var stocks = await _context.Stocks.ToListAsync(cancellationToken);

            return stocks.Select(s => new StockDto
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
                PrevPrice = s.PrevPrice,
                Value = s.Value,
                PriceChange = s.PriceChange,
                ValueChange = s.ValueChange
            }).ToList();
        }
    }
}
