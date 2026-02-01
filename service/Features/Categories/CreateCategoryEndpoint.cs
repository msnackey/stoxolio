using Stoxolio.Service.BuildingBlocks.CQRS;
using Stoxolio.Service.Data;
using Stoxolio.Service.DTOs;
using Stoxolio.Service.Models;

namespace Stoxolio.Service.Features.Categories;

public sealed record CreateCategoryCommand(CategoryDto CategoryDto) : ICommand<CategoryDto>;

public class CreateCategoryEndpoint
{
    private static async Task<IResult> HandleCreateCategory(
        CategoryDto categoryDto,
        StoxolioDbContext context,
        CancellationToken cancellationToken)
    {
        var handler = new CreateCategoryHandler(context);
        var result = await handler.Handle(new CreateCategoryCommand(categoryDto), cancellationToken);
        return Results.CreatedAtRoute("GetCategory", new { id = result.Id }, result);
    }

    public static void MapEndpoint(RouteGroupBuilder group)
    {
        group.MapPost("/", HandleCreateCategory)
            .WithName("CreateCategory");
    }

    private class CreateCategoryHandler : ICommandHandler<CreateCategoryCommand, CategoryDto>
    {
        private readonly StoxolioDbContext _context;

        public CreateCategoryHandler(StoxolioDbContext context)
        {
            _context = context;
        }

        public async Task<CategoryDto> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = new Category
            {
                Name = request.CategoryDto.Name,
                Target = request.CategoryDto.Target
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync(cancellationToken);

            request.CategoryDto.Id = category.Id;
            return request.CategoryDto;
        }
    }
}
