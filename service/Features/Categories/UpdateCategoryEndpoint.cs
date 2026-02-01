using Stoxolio.Service.BuildingBlocks.CQRS;
using Stoxolio.Service.Data;
using Stoxolio.Service.Models;

namespace Stoxolio.Service.Features.Categories;

public sealed record UpdateCategoryCommand(UpdateCategoryRequest Request) : ICommand<UpdateCategoryResponse>;

public sealed record UpdateCategoryRequest
{
    public required int Id { get; init; }
    public string? Name { get; init; }
    public double? Target { get; init; }
}

public sealed record UpdateCategoryResponse
{
    public required Category Category { get; init; }
}

public class UpdateCategoryHandler(StoxolioDbContext context) : ICommandHandler<UpdateCategoryCommand, UpdateCategoryResponse>
{
    public async Task<UpdateCategoryResponse> Handle(UpdateCategoryCommand command, CancellationToken cancellationToken)
    {
        var category = await context.Categories.FindAsync(command.Request.Id, cancellationToken);

        if (category == null)
            throw new InvalidOperationException("Category not found"); // TODO: Implement result pattern

        category.Name = command.Request.Name ?? category.Name;
        category.Target = command.Request.Target ?? category.Target;

        context.Categories.Update(category);
        await context.SaveChangesAsync(cancellationToken);

        return new UpdateCategoryResponse { Category = category };
    }
}