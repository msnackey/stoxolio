using Stoxolio.Service.BuildingBlocks.CQRS;
using Stoxolio.Service.Data;
using Stoxolio.Service.DTOs;
using Stoxolio.Service.Models;

namespace Stoxolio.Service.Features.Categories;

public sealed record CreateCategoryCommand(CreateCategoryRequest Request) : ICommand<CreateCategoryResponse>;

public sealed record CreateCategoryRequest
{
    public required CategoryDto CategoryDto { get; init; }
}

public sealed record CreateCategoryResponse
{
    public required Category Category { get; init; }
};

public class CreateCategoryHandler(StoxolioDbContext context) : ICommandHandler<CreateCategoryCommand, CreateCategoryResponse>
{
    public async Task<CreateCategoryResponse> Handle(CreateCategoryCommand command, CancellationToken cancellationToken)
    {
        var category = new Category
        {
            Name = command.Request.CategoryDto.Name,
            Target = command.Request.CategoryDto.Target
        };

        context.Categories.Add(category);
        await context.SaveChangesAsync(cancellationToken);

        return new CreateCategoryResponse
        {
            Category = category
        };
    }
}