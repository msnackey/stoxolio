using Stoxolio.Service.Features.Categories;

namespace Stoxolio.Service.Endpoints;

public static class CategoriesEndpoints
{
    public static void MapCategoriesEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/categories")
            .WithName("Categories")
            .RequireAuthorization();

        GetCategoriesEndpoint.MapEndpoint(group);
        GetCategoryEndpoint.MapEndpoint(group);
        CreateCategoryEndpoint.MapEndpoint(group);
        UpdateCategoryEndpoint.MapEndpoint(group);
        DeleteCategoryEndpoint.MapEndpoint(group);
    }
}
