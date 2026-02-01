using Microsoft.EntityFrameworkCore;
using Stoxolio.Service.BuildingBlocks.CQRS;
using Stoxolio.Service.Data;
using Stoxolio.Service.DTOs;

namespace Stoxolio.Service.Features.Categories;

public sealed record GetCategoriesQuery : IQuery<List<CategoryDto>>;

public class GetCategoriesEndpoint
{
    private static async Task<IResult> HandleGetCategories(
        StoxolioDbContext context,
        CancellationToken cancellationToken)
    {
        var handler = new GetCategoriesHandler(context);
        var result = await handler.Handle(new GetCategoriesQuery(), cancellationToken);
        return Results.Ok(result);
    }

    public static void MapEndpoint(RouteGroupBuilder group)
    {
        group.MapGet("/", HandleGetCategories)
            .WithName("GetCategories");
    }

    private class GetCategoriesHandler : IQueryHandler<GetCategoriesQuery, List<CategoryDto>>
    {
        private readonly StoxolioDbContext _context;

        public GetCategoriesHandler(StoxolioDbContext context)
        {
            _context = context;
        }

        public async Task<List<CategoryDto>?> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
        {
            var categories = await _context.Categories
                .Include(c => c.Stocks)
                .ToListAsync(cancellationToken);

            return categories.Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                Target = c.Target,
                Stocks = c.Stocks.Select(s => new StockDto
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
            }).ToList();
        }
    }
}
