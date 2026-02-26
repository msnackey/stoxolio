using Stoxolio.Service.BuildingBlocks.CQRS;
using Stoxolio.Service.Features.Categories;

namespace Stoxolio.Service.Endpoints;

public static class CategoriesEndpoints
{
    public static void MapCategoriesEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/categories")
            .WithName("Categories")
            .RequireAuthorization();

        group.MapGet("/", async (
                IQueryHandler<GetCategoriesQuery, GetCategoriesResponse> handler,
                CancellationToken cancellationToken) =>
            {
                var result = await handler.Handle(new GetCategoriesQuery(), cancellationToken);

                return Results.Ok(result);

                // TODO: Implement Results class
                // return result.IsSuccess
                //     ? Results.Ok(result.Value)
                //     : ApiResults.Problem(result);
            })
            .WithName("GetCategories")
            .WithDescription("Get all categories.")
            .Produces<GetCategoriesResponse>(200)
            .ProducesProblem(400)
            .ProducesProblem(500)
            .ProducesProblem(502);

        group.MapPost("/", async (
                CreateCategoryRequest request,
                ICommandHandler<CreateCategoryCommand, CreateCategoryResponse> handler,
                CancellationToken cancellationToken) =>
            {
                var result = await handler.Handle(new CreateCategoryCommand(request), cancellationToken);

                return Results.Ok(result);

                // TODO: Implement Results class
                // return result.IsSuccess
                //     ? Results.Ok(result.Value)
                //     : ApiResults.Problem(result);
            })
            .WithName("CreateCategory")
            .WithDescription("Create a new category.")
            .Produces<CreateCategoryResponse>(200)
            .ProducesProblem(400)
            .ProducesProblem(500)
            .ProducesProblem(502);

        group.MapPost("/delete/", async (
                DeleteCategoryRequest request,
                ICommandHandler<DeleteCategoryCommand, DeleteCategoryResponse> handler,
                CancellationToken cancellationToken) =>
            {
                var result = await handler.Handle(new DeleteCategoryCommand(request), cancellationToken);

                return Results.Ok(result);

                // TODO: Implement Results class
                // return result.IsSuccess
                //     ? Results.Ok(result.Value)
                //     : ApiResults.Problem(result);
            })
            .WithName("DeleteCategory")
            .WithDescription("Delete a category.")
            .Produces<CreateCategoryResponse>(200)
            .ProducesProblem(400)
            .ProducesProblem(500)
            .ProducesProblem(502);

        group.MapPut("/", async (
                UpdateCategoryRequest request,
                ICommandHandler<UpdateCategoryCommand, UpdateCategoryResponse> handler,
                CancellationToken cancellationToken) =>
            {
                var result = await handler.Handle(new UpdateCategoryCommand(request), cancellationToken);

                return Results.Ok(result);

                // TODO: Implement Results class
                // return result.IsSuccess
                //     ? Results.Ok(result.Value)
                //     : ApiResults.Problem(result);
            })
            .WithName("UpdateCategory")
            .WithDescription("Update a category.")
            .Produces<UpdateCategoryResponse>(200)
            .ProducesProblem(400)
            .ProducesProblem(500)
            .ProducesProblem(502);
    }
}
