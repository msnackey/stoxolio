using Stoxolio.Service.BuildingBlocks.Common;
using Stoxolio.Service.BuildingBlocks.CQRS;
using Stoxolio.Service.Data;
using Stoxolio.Service.DTOs;
using Stoxolio.Service.Mappings;

namespace Stoxolio.Service.Features.Categories;

public sealed record DeleteCategoryCommand(DeleteCategoryRequest Request) : ICommand<DeleteCategoryResponse>;

public sealed record DeleteCategoryRequest(long Id);

public sealed record DeleteCategoryResponse(CategoryDto Category);

public class DeleteCategoryHandler(StoxolioDbContext context)
    : ICommandHandler<DeleteCategoryCommand, DeleteCategoryResponse>
{
    public async Task<Result<DeleteCategoryResponse>> Handle(DeleteCategoryCommand command,
        CancellationToken cancellationToken)
    {
        try
        {
            var category =
                await context.Categories.FindAsync([command.Request.Id], cancellationToken);

            if (category == null)
                return Result.Failure<DeleteCategoryResponse>(
                    new Error(
                        "DeleteCategory.NotFound",
                        "Category not found.",
                        ErrorType.NotFound));

            context.Categories.Remove(category);
            await context.SaveChangesAsync(cancellationToken);

            return Result.Success(new DeleteCategoryResponse(category.ToDto([])));
        }
        catch
        {
            return Result.Failure<DeleteCategoryResponse>(
                new Error(
                    "DeleteCategory.Failed",
                    "Failed trying to delete category.",
                    ErrorType.Problem));
        }
    }
}