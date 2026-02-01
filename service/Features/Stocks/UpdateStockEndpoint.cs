using Stoxolio.Service.BuildingBlocks.CQRS;
using Stoxolio.Service.Data;
using Stoxolio.Service.DTOs;

namespace Stoxolio.Service.Features.Stocks;

public sealed record UpdateStockCommand(int Id, StockDto StockDto) : ICommand<bool>;

public class UpdateStockEndpoint
{
    private static async Task<IResult> HandleUpdateStock(
        int id,
        StockDto stockDto,
        StoxolioDbContext context,
        CancellationToken cancellationToken)
    {
        var handler = new UpdateStockHandler(context);
        var success = await handler.Handle(new UpdateStockCommand(id, stockDto), cancellationToken);
        return success ? Results.NoContent() : Results.NotFound();
    }

    public static void MapEndpoint(RouteGroupBuilder group)
    {
        group.MapPut("/{id}", HandleUpdateStock)
            .WithName("UpdateStock");
    }

    private class UpdateStockHandler : ICommandHandler<UpdateStockCommand, bool>
    {
        private readonly StoxolioDbContext _context;

        public UpdateStockHandler(StoxolioDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(UpdateStockCommand request, CancellationToken cancellationToken)
        {
            var stock = await _context.Stocks.FindAsync(new object[] { request.Id }, cancellationToken);

            if (stock == null)
                return false;

            stock.Name = request.StockDto.Name;
            stock.Ticker = request.StockDto.Ticker;
            stock.Exchange = request.StockDto.Exchange;
            stock.Sri = request.StockDto.Sri;
            stock.Shares = request.StockDto.Shares;
            stock.Price = request.StockDto.Price;
            stock.Invest = request.StockDto.Invest;
            stock.CategoryId = request.StockDto.CategoryId;
            stock.PrevPrice = request.StockDto.PrevPrice;

            _context.Stocks.Update(stock);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
