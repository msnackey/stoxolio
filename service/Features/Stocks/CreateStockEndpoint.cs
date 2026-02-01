using Stoxolio.Service.BuildingBlocks.CQRS;
using Stoxolio.Service.Data;
using Stoxolio.Service.DTOs;
using Stoxolio.Service.Models;

namespace Stoxolio.Service.Features.Stocks;

public sealed record CreateStockCommand(StockDto StockDto) : ICommand<StockDto>;

public class CreateStockEndpoint
{
    private static async Task<IResult> HandleCreateStock(
        StockDto stockDto,
        StoxolioDbContext context,
        CancellationToken cancellationToken)
    {
        var handler = new CreateStockHandler(context);
        var result = await handler.Handle(new CreateStockCommand(stockDto), cancellationToken);
        return Results.CreatedAtRoute("GetStock", new { id = result.Id }, result);
    }

    public static void MapEndpoint(RouteGroupBuilder group)
    {
        group.MapPost("/", HandleCreateStock)
            .WithName("CreateStock");
    }

    private class CreateStockHandler : ICommandHandler<CreateStockCommand, StockDto>
    {
        private readonly StoxolioDbContext _context;

        public CreateStockHandler(StoxolioDbContext context)
        {
            _context = context;
        }

        public async Task<StockDto> Handle(CreateStockCommand request, CancellationToken cancellationToken)
        {
            var stock = new Stock
            {
                Name = request.StockDto.Name,
                Ticker = request.StockDto.Ticker,
                Exchange = request.StockDto.Exchange,
                Sri = request.StockDto.Sri,
                Shares = request.StockDto.Shares,
                Price = request.StockDto.Price,
                Invest = request.StockDto.Invest,
                CategoryId = request.StockDto.CategoryId,
                PrevPrice = request.StockDto.PrevPrice
            };

            _context.Stocks.Add(stock);
            await _context.SaveChangesAsync(cancellationToken);

            request.StockDto.Id = stock.Id;
            return request.StockDto;
        }
    }
}
