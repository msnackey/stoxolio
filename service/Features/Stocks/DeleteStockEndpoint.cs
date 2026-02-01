using Stoxolio.Service.BuildingBlocks.CQRS;
using Stoxolio.Service.Data;

namespace Stoxolio.Service.Features.Stocks;

public sealed record DeleteStockCommand(int Id) : ICommand<bool>;

public class DeleteStockEndpoint
{
    private static async Task<IResult> HandleDeleteStock(
        int id,
        StoxolioDbContext context,
        CancellationToken cancellationToken)
    {
        var handler = new DeleteStockHandler(context);
        var success = await handler.Handle(new DeleteStockCommand(id), cancellationToken);
        return success ? Results.NoContent() : Results.NotFound();
    }

    public static void MapEndpoint(RouteGroupBuilder group)
    {
        group.MapDelete("/{id}", HandleDeleteStock)
            .WithName("DeleteStock");
    }

    private class DeleteStockHandler : ICommandHandler<DeleteStockCommand, bool>
    {
        private readonly StoxolioDbContext _context;

        public DeleteStockHandler(StoxolioDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(DeleteStockCommand request, CancellationToken cancellationToken)
        {
            var stock = await _context.Stocks.FindAsync(new object[] { request.Id }, cancellationToken);

            if (stock == null)
                return false;

            _context.Stocks.Remove(stock);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
