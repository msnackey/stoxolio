using Stoxolio.Service.Features.Stocks;

namespace Stoxolio.Service.Endpoints;

public static class StocksEndpoints
{
    public static void MapStocksEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/stocks")
            .WithName("Stocks")
            .RequireAuthorization();

        GetStocksEndpoint.MapEndpoint(group);
        GetStockEndpoint.MapEndpoint(group);
        CreateStockEndpoint.MapEndpoint(group);
        UpdateStockEndpoint.MapEndpoint(group);
        DeleteStockEndpoint.MapEndpoint(group);
    }
}
