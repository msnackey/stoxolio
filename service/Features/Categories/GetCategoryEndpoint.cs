using Microsoft.EntityFrameworkCore;
using Stoxolio.Service.BuildingBlocks.CQRS;
using Stoxolio.Service.Data;
using Stoxolio.Service.DTOs;

namespace Stoxolio.Service.Features.Categories;

public sealed record GetCategoryQuery(int Id) : IQuery<CategoryDto>;

public class GetCategoryEndpoint
{
    private static async Task<IResult> HandleGetCategory(
        int id,
        StoxolioDbContext context,
        CancellationToken cancellationToken)
    {
        var handler = new GetCategoryHandler(context);
        var result = await handler.Handle(new GetCategoryQuery(id), cancellationToken);
        return result is not null ? Results.Ok(result) : Results.NotFound();
    }

    public static void MapEndpoint(RouteGroupBuilder group)
    {
        group.MapGet("/{id}", HandleGetCategory)
            .WithName("GetCategory");
    }

    private class GetCategoryHandler : IQueryHandler<GetCategoryQuery, CategoryDto>
    {
        private readonly StoxolioDbContext _context;

        public GetCategoryHandler(StoxolioDbContext context)
        {
            _context = context;
        }

        public async Task<CategoryDto?> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
        {
            var category = await _context.Categories
                .Include(c => c.Stocks)
                .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

            if (category == null)
                return null;

            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Target = category.Target,
                Stocks = category.Stocks.Select(s => new StockDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Ticker = s.Ticker,
                    Exchange = s.Exchange,
                    Sri = s.Sri,
                    Shares = s.Shares,
                    Price = s.Price,
                    Invest = s.Invest,
                    CategoryId = s.CategoryId,
                    PrevPrice = s.PrevPrice,
                    Value = s.Value,
                    PriceChange = s.PriceChange,
                    ValueChange = s.ValueChange
                }).ToList()
            };
        }
    }
}
