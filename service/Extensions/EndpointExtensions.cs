using Stoxolio.Service.Endpoints;

namespace Stoxolio.Service.Extensions;

public static class EndpointExtensions
{
    public static IApplicationBuilder MapEndpoints(
        this WebApplication app,
        RouteGroupBuilder? routeGroupBuilder = null)
    {
        app.MapAuthEndpoints();
        app.MapStocksEndpoints();
        app.MapCategoriesEndpoints();
        
        return app;
    }
}