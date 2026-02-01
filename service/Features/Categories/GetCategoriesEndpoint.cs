using Microsoft.EntityFrameworkCore;
using Stoxolio.Service.BuildingBlocks.CQRS;
using Stoxolio.Service.Data;
using Stoxolio.Service.Models;

namespace Stoxolio.Service.Features.Categories;

public sealed record GetCategoriesQuery : IQuery<GetCategoriesResponse>;

public sealed record GetCategoriesResponse
    {
        public required List<Category> Categories { get; init; }
    }

public class GetCategoriesHandler(StoxolioDbContext context) : IQueryHandler<GetCategoriesQuery, GetCategoriesResponse>
{
    public async Task<GetCategoriesResponse?> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = await context.Categories
            .Include(c => c.Stocks)
            .ToListAsync(cancellationToken);

        return new GetCategoriesResponse
        {
            Categories = categories.Select(c => new Category
            {
                Id = c.Id,
                Name = c.Name,
                Target = c.Target,
                Stocks = c.Stocks.Select(s => new Stock
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
                    PrevPrice = s.PrevPrice
                }).ToList()
            }).ToList()
        };
    }
}