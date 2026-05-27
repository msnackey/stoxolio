using Stoxolio.Service.BuildingBlocks.Common;
using Stoxolio.Service.BuildingBlocks.CQRS;
using Stoxolio.Service.Data;
using Stoxolio.Service.DTOs;
using Stoxolio.Service.Mappings;

namespace Stoxolio.Service.Features.Categories;

public sealed record UpdateCategoryCommand(UpdateCategoryRequest Request) : ICommand<UpdateCategoryResponse>;

public sealed record UpdateCategoryRequest(int Id, string? Name = null, double? Target = null);

public sealed record UpdateCategoryResponse(CategoryDto Category);

public class UpdateCategoryHandler(StoxolioDbContext context)
    : ICommandHandler<UpdateCategoryCommand, UpdateCategoryResponse>
{
    public async Task<Result<UpdateCategoryResponse>> Handle(UpdateCategoryCommand command,
        CancellationToken cancellationToken)
    {
        try
        {
            var category =
                await context.Categories.FindAsync([command.Request.Id], cancellationToken);

            if (category == null)
                return Result.Failure<UpdateCategoryResponse>(
                    new Error(
                        "UpdateCategory.NotFound",
                        "Category not found.",
                        ErrorType.NotFound));

            category.Name = command.Request.Name ?? category.Name;
            category.Target = command.Request.Target ?? category.Target;

            context.Categories.Update(category);
            await context.SaveChangesAsync(cancellationToken);

            return Result.Success(new UpdateCategoryResponse(category.ToDto()));
        }
        catch
        {
            return Result.Failure<UpdateCategoryResponse>(
                new Error(
                    "UpdateCategory.Failed",
                    "Failed trying to update category.",
                    ErrorType.Problem));
        }
    }
}