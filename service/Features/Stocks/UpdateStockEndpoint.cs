using Stoxolio.Service.BuildingBlocks.CQRS;
using Stoxolio.Service.Data;
using Stoxolio.Service.Models;

namespace Stoxolio.Service.Features.Stocks;

public sealed record UpdateStockCommand(UpdateStockRequest Request) : ICommand<UpdateStockResponse>;

public sealed record UpdateStockRequest
{
    public required int Id { get; init; }
    public string? Name { get; init; }
    public string? Ticker { get; init; }
    public string? Exchange { get; init; }
    public bool? Sri { get; init; }
    public int? Shares { get; init; }
    public decimal? Price { get; init; }
    public bool? Invest { get; init; }
    public int? CategoryId { get; init; }
    public decimal? PrevPrice { get; init; }
}

public sealed record UpdateStockResponse
{
    public required Stock Stock { get; init; }
}

public class UpdateStockHandler(StoxolioDbContext context) : ICommandHandler<UpdateStockCommand, UpdateStockResponse>
{

    public async Task<UpdateStockResponse> Handle(UpdateStockCommand command, CancellationToken cancellationToken)
    {
        var stock = await context.Stocks.FindAsync(command.Request.Id, cancellationToken);

        if (stock == null)
            throw new InvalidOperationException("Stock not found"); // TODO: Implement result pattern

        stock.Name = command.Request.Name ?? stock.Name;
        stock.Ticker = command.Request.Ticker ?? stock.Ticker;
        stock.Exchange = command.Request.Exchange ?? stock.Exchange;
        stock.Sri = command.Request.Sri ?? stock.Sri;
        stock.Shares = command.Request.Shares ?? stock.Shares;
        stock.Price = command.Request.Price ?? stock.Price;
        stock.Invest = command.Request.Invest ?? stock.Invest;
        stock.CategoryId = command.Request.CategoryId ?? stock.CategoryId;
        stock.PrevPrice = command.Request.PrevPrice ?? stock.PrevPrice;
        
        context.Stocks.Update(stock);
        await context.SaveChangesAsync(cancellationToken);

        return new UpdateStockResponse { Stock = stock };
    }
}