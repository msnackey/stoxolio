using Stoxolio.Service.BuildingBlocks.CQRS;
using Stoxolio.Service.Data;
using Stoxolio.Service.DTOs;
using Stoxolio.Service.Models;

namespace Stoxolio.Service.Features.Stocks;

public sealed record CreateStockCommand(CreateStockRequest Request) : ICommand<CreateStockResponse>;

public sealed record CreateStockRequest
{
    public required StockDto StockDto { get; init; }
}

public sealed record CreateStockResponse
{
    public required Stock Stock { get; init; }
};

public class CreateStockHandler(StoxolioDbContext context) : ICommandHandler<CreateStockCommand, CreateStockResponse>
{
    public async Task<CreateStockResponse> Handle(CreateStockCommand command, CancellationToken cancellationToken)
    {
        var stock = new Stock
        {
            Name = command.Request.StockDto.Name,
            Ticker = command.Request.StockDto.Ticker,
            Exchange = command.Request.StockDto.Exchange,
            Sri = command.Request.StockDto.Sri,
            Shares = command.Request.StockDto.Shares,
            Price = command.Request.StockDto.Price,
            Invest = command.Request.StockDto.Invest,
            CategoryId = command.Request.StockDto.CategoryId,
            PrevPrice = command.Request.StockDto.PrevPrice // TODO: Remove from DTO and make calculated field in database
        };

        context.Stocks.Add(stock);
        await context.SaveChangesAsync(cancellationToken);

        return new CreateStockResponse { Stock = stock };
    }
}
