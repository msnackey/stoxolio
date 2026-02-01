using Stoxolio.Service.BuildingBlocks.CQRS;
using Stoxolio.Service.Data;
using Stoxolio.Service.DTOs;

namespace Stoxolio.Service.Features.Stocks;

public sealed record GetStockQuery(int Id) : IQuery<StockDto>;

public class GetStockEndpoint
{
    private static async Task<IResult> HandleGetStock(
        int id,
        StoxolioDbContext context,
        CancellationToken cancellationToken)
    {
        var handler = new GetStockHandler(context);
        var result = await handler.Handle(new GetStockQuery(id), cancellationToken);
        return result is not null ? Results.Ok(result) : Results.NotFound();
    }

    public static void MapEndpoint(RouteGroupBuilder group)
    {
        group.MapGet("/{id}", HandleGetStock)
            .WithName("GetStock");
    }

    private class GetStockHandler : IQueryHandler<GetStockQuery, StockDto>
    {
        private readonly StoxolioDbContext _context;

        public GetStockHandler(StoxolioDbContext context)
        {
            _context = context;
        }

        public async Task<StockDto?> Handle(GetStockQuery request, CancellationToken cancellationToken)
        {
            var stock = await _context.Stocks.FindAsync(new object[] { request.Id }, cancellationToken);

            if (stock == null)
                return null;

            return new StockDto
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
    }
}
