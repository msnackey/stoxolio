using Microsoft.EntityFrameworkCore;
using Stoxolio.Service.BuildingBlocks.Common;
using Stoxolio.Service.BuildingBlocks.CQRS;
using Stoxolio.Service.Data;
using Stoxolio.Service.DTOs;
using Stoxolio.Service.Mappings;

namespace Stoxolio.Service.Features.Categories;

public sealed record GetCategoriesQuery : IQuery<GetCategoriesResponse>;

public sealed record GetCategoriesResponse(List<CategoryDto> Categories);

public class GetCategoriesHandler(StoxolioDbContext context) : IQueryHandler<GetCategoriesQuery, GetCategoriesResponse>
{
    public async Task<Result<GetCategoriesResponse>> Handle(GetCategoriesQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var categories = await context.Categories
                .Include(c => c.Stocks)
                .ToListAsync(cancellationToken);

            return Result.Success(new GetCategoriesResponse(categories.Select(c => c.ToDto(categories)).ToList()));
        }
        catch
        {
            return Result.Failure<GetCategoriesResponse>(
                new Error(
                    "GetCategories.Failed",
                    "Failed trying to get categories.",
                    ErrorType.Problem));
        }
    }
}