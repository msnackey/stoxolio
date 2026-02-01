using Stoxolio.Service.BuildingBlocks.CQRS;
using Stoxolio.Service.Data;
using Stoxolio.Service.Models;

namespace Stoxolio.Service.Features.Categories;

public sealed record DeleteCategoryCommand(DeleteCategoryRequest Request) : ICommand<DeleteCategoryResponse>;

public sealed record DeleteCategoryRequest
{
    public required int Id { get; init; }
}

public sealed record DeleteCategoryResponse
{
    public required Category Category { get; init; }
};

public class DeleteCategoryHandler(StoxolioDbContext context) : ICommandHandler<DeleteCategoryCommand, DeleteCategoryResponse>
{
    public async Task<DeleteCategoryResponse> Handle(DeleteCategoryCommand command, CancellationToken cancellationToken)
    {
        var category = await context.Categories.FindAsync(command.Request.Id, cancellationToken);

        if (category == null)
            throw new InvalidOperationException("Category not found"); // TODO: Implement result pattern

        context.Categories.Remove(category);
        await context.SaveChangesAsync(cancellationToken);

        return new DeleteCategoryResponse { Category = category };
    }
}