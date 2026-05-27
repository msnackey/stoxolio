using Stoxolio.Service.BuildingBlocks.Common;
using Stoxolio.Service.BuildingBlocks.CQRS;
using Stoxolio.Service.Data;
using Stoxolio.Service.DTOs;
using Stoxolio.Service.Mappings;
using Stoxolio.Service.Models;

namespace Stoxolio.Service.Features.Stocks;

public sealed record CreateStockCommand(CreateStockRequest Request) : ICommand<CreateStockResponse>;

public sealed record CreateStockRequest(StockDto Stock);

public sealed record CreateStockResponse(StockDto Stock);

public class CreateStockHandler(StoxolioDbContext context) : ICommandHandler<CreateStockCommand, CreateStockResponse>
{
    public async Task<Result<CreateStockResponse>> Handle(CreateStockCommand command,
        CancellationToken cancellationToken)
    {
        try
        {
            var stock = new Stock
            {
                Name = command.Request.Stock.Name,
                Ticker = command.Request.Stock.Ticker,
                Exchange = command.Request.Stock.Exchange,
                Sri = command.Request.Stock.Sri,
                Shares = command.Request.Stock.Shares,
                Price = command.Request.Stock.Price,
                Invest = command.Request.Stock.Invest,
                CategoryId = command.Request.Stock.CategoryId,
                PrevPrice = command.Request.Stock.Price
            };

            context.Stocks.Add(stock);
            await context.SaveChangesAsync(cancellationToken);

            return Result.Success(new CreateStockResponse(stock.ToDto()));
        }
        catch
        {
            return Result.Failure<CreateStockResponse>(
                new Error(
                    "CreateStock.Failed",
                    "Failed to create stock.",
                    ErrorType.Problem));
        }
    }
}
