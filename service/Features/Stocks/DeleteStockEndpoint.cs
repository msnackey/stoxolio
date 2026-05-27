using Stoxolio.Service.BuildingBlocks.Common;
using Stoxolio.Service.BuildingBlocks.CQRS;
using Stoxolio.Service.Data;
using Stoxolio.Service.DTOs;
using Stoxolio.Service.Mappings;

namespace Stoxolio.Service.Features.Stocks;

public sealed record DeleteStockCommand(DeleteStockRequest Request) : ICommand<DeleteStockResponse>;

public sealed record DeleteStockRequest(int Id);

public sealed record DeleteStockResponse(StockDto Stock);

public class DeleteStockHandler(StoxolioDbContext context) : ICommandHandler<DeleteStockCommand, DeleteStockResponse>
{
    public async Task<Result<DeleteStockResponse>> Handle(DeleteStockCommand command,
        CancellationToken cancellationToken)
    {
        try
        {
            var stock = await context.Stocks.FindAsync([command.Request.Id], cancellationToken);

            if (stock == null)
                return Result.Failure<DeleteStockResponse>(
                    new Error(
                        "DeleteStock.NotFound",
                        "Stock not found.",
                        ErrorType.NotFound));

            context.Stocks.Remove(stock);
            await context.SaveChangesAsync(cancellationToken);

            return Result.Success(new DeleteStockResponse(stock.ToDto()));
        }
        catch
        {
            return Result.Failure<DeleteStockResponse>(
                new Error(
                    "DeleteStock.Failed",
                    "Failed to delete stock.",
                    ErrorType.Problem));
        }
    }
}