using Stoxolio.Service.BuildingBlocks.CQRS;
using Stoxolio.Service.Data;

namespace Stoxolio.Service.Features.Categories;

public sealed record DeleteCategoryCommand(int Id) : ICommand<bool>;

public class DeleteCategoryEndpoint
{
    private static async Task<IResult> HandleDeleteCategory(
        int id,
        StoxolioDbContext context,
        CancellationToken cancellationToken)
    {
        var handler = new DeleteCategoryHandler(context);
        var success = await handler.Handle(new DeleteCategoryCommand(id), cancellationToken);
        return success ? Results.NoContent() : Results.NotFound();
    }

    public static void MapEndpoint(RouteGroupBuilder group)
    {
        group.MapDelete("/{id}", HandleDeleteCategory)
            .WithName("DeleteCategory");
    }

    private class DeleteCategoryHandler : ICommandHandler<DeleteCategoryCommand, bool>
    {
        private readonly StoxolioDbContext _context;

        public DeleteCategoryHandler(StoxolioDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _context.Categories.FindAsync(new object[] { request.Id }, cancellationToken);

            if (category == null)
                return false;

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
