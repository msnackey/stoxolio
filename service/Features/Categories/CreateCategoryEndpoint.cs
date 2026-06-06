using Stoxolio.Service.BuildingBlocks.Common;
using Stoxolio.Service.BuildingBlocks.CQRS;
using Stoxolio.Service.Data;
using Stoxolio.Service.DTOs;
using Stoxolio.Service.Mappings;
using Stoxolio.Service.Models;

namespace Stoxolio.Service.Features.Categories;

public sealed record CreateCategoryCommand(CreateCategoryRequest Request) : ICommand<CreateCategoryResponse>;

public sealed record CreateCategoryRequest(string Name, double Target);

public sealed record CreateCategoryResponse(CategoryDto Category);

public class CreateCategoryHandler(StoxolioDbContext context)
    : ICommandHandler<CreateCategoryCommand, CreateCategoryResponse>
{
    public async Task<Result<CreateCategoryResponse>> Handle(CreateCategoryCommand command,
        CancellationToken cancellationToken)
    {
        try
        {
            var category = new Category
            {
                Name = command.Request.Name,
                Target = command.Request.Target
            };

            context.Categories.Add(category);
            await context.SaveChangesAsync(cancellationToken);

            return Result.Success(new CreateCategoryResponse(category.ToDto()));
        }
        catch
        {
            return Result.Failure<CreateCategoryResponse>(
                new Error(
                    "CreateCategory.Failed",
                    "Failed trying to create category.",
                    ErrorType.Problem));
        }
    }
}