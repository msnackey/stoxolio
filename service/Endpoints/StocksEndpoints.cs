using Stoxolio.Service.BuildingBlocks.CQRS;
using Stoxolio.Service.Data;
using Stoxolio.Service.Features.Stocks;

namespace Stoxolio.Service.Endpoints;

public static class StocksEndpoints
{
    public static void MapStocksEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/stocks")
            .WithName("Stocks")
            .RequireAuthorization();

        group.MapGet("/", async (
                IQueryHandler<GetStocksQuery, GetStocksResponse> handler,
                StoxolioDbContext context,
                CancellationToken cancellationToken) =>
            {
                var result = await handler.Handle(new GetStocksQuery(), cancellationToken);

                return Results.Ok(result);

                // TODO: Implement Results class
                // return result.IsSuccess
                //     ? Results.Ok(result.Value)
                //     : ApiResults.Problem(result);
            })
            .WithName("GetStocks")
            .WithDescription("Get all stocks.")
            .Produces<GetStocksResponse>(200)
            .ProducesProblem(400)
            .ProducesProblem(500)
            .ProducesProblem(502);

        group.MapPost("/", async (
                CreateStockRequest request,
                ICommandHandler<CreateStockCommand, CreateStockResponse> handler,
                StoxolioDbContext context,
                CancellationToken cancellationToken) =>
            {
                var result = await handler.Handle(new CreateStockCommand(request), cancellationToken);

                return Results.Ok(result);

                // TODO: Implement Results class
                // return result.IsSuccess
                //     ? Results.Ok(result.Value)
                //     : ApiResults.Problem(result);
            })
            .WithName("CreateStock")
            .WithDescription("Create a new stock.")
            .Produces<CreateStockResponse>(200)
            .ProducesProblem(400)
            .ProducesProblem(500)
            .ProducesProblem(502);

        group.MapPost("/delete/", async (
                DeleteStockRequest request,
                ICommandHandler<DeleteStockCommand, DeleteStockResponse> handler,
                StoxolioDbContext context,
                CancellationToken cancellationToken) =>
            {
                var result = await handler.Handle(new DeleteStockCommand(request), cancellationToken);

                return Results.Ok(result);

                // TODO: Implement Results class
                // return result.IsSuccess
                //     ? Results.Ok(result.Value)
                //     : ApiResults.Problem(result);
            })
            .WithName("DeleteStock")
            .WithDescription("Delete a stock.")
            .Produces<DeleteStockResponse>(200)
            .ProducesProblem(400)
            .ProducesProblem(500)
            .ProducesProblem(502);

        group.MapPut("/", async (
                UpdateStockRequest request,
                ICommandHandler<UpdateStockCommand, UpdateStockResponse> handler,
                StoxolioDbContext context,
                CancellationToken cancellationToken) =>
            {
                var result = await handler.Handle(new UpdateStockCommand(request), cancellationToken);

                return Results.Ok(result);

                // TODO: Implement Results class
                // return result.IsSuccess
                //     ? Results.Ok(result.Value)
                //     : ApiResults.Problem(result);
            })
            .WithName("UpdateStock")
            .WithDescription("Update a stock.")
            .Produces<UpdateStockResponse>(200)
            .ProducesProblem(400)
            .ProducesProblem(500)
            .ProducesProblem(502);
    }
}
