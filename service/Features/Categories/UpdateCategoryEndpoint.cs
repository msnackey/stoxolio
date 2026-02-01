using Stoxolio.Service.BuildingBlocks.CQRS;
using Stoxolio.Service.Data;
using Stoxolio.Service.DTOs;

namespace Stoxolio.Service.Features.Categories;

public sealed record UpdateCategoryCommand(int Id, CategoryDto CategoryDto) : ICommand<bool>;

public class UpdateCategoryEndpoint
{
    private static async Task<IResult> HandleUpdateCategory(
        int id,
        CategoryDto categoryDto,
        StoxolioDbContext context,
        CancellationToken cancellationToken)
    {
        var handler = new UpdateCategoryHandler(context);
        var success = await handler.Handle(new UpdateCategoryCommand(id, categoryDto), cancellationToken);
        return success ? Results.NoContent() : Results.NotFound();
    }

    public static void MapEndpoint(RouteGroupBuilder group)
    {
        group.MapPut("/{id}", HandleUpdateCategory)
            .WithName("UpdateCategory");
    }

    private class UpdateCategoryHandler : ICommandHandler<UpdateCategoryCommand, bool>
    {
        private readonly StoxolioDbContext _context;

        public UpdateCategoryHandler(StoxolioDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _context.Categories.FindAsync(new object[] { request.Id }, cancellationToken);

            if (category == null)
                return false;

            category.Name = request.CategoryDto.Name;
            category.Target = request.CategoryDto.Target;

            _context.Categories.Update(category);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
