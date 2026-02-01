using Stoxolio.Service.BuildingBlocks.CQRS;
using Stoxolio.Service.Data;
using Stoxolio.Service.Models;

namespace Stoxolio.Service.Features.Stocks;

public sealed record DeleteStockCommand(DeleteStockRequest Request) : ICommand<DeleteStockResponse>;

public sealed record DeleteStockRequest
{
    public required int Id { get; init; }
}

public sealed record DeleteStockResponse
{
    public required Stock Stock { get; init; }
}

public class DeleteStockHandler(StoxolioDbContext context) : ICommandHandler<DeleteStockCommand, DeleteStockResponse>
{
    public async Task<DeleteStockResponse> Handle(DeleteStockCommand command, CancellationToken cancellationToken)
    {
        var stock = await context.Stocks.FindAsync(command.Request.Id, cancellationToken);

        if (stock == null)
            throw new InvalidOperationException("Stock not found"); // TODO: Implement result pattern

        context.Stocks.Remove(stock);
        await context.SaveChangesAsync(cancellationToken);

        return new DeleteStockResponse { Stock = stock };
    }
}