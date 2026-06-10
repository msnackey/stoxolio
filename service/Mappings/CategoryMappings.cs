using Stoxolio.Service.DTOs;
using Stoxolio.Service.Models;

namespace Stoxolio.Service.Mappings;

public static class CategoryMappings
{
    public static CategoryDto ToDto(this Category category, List<Category> categories) => new()
    {
        Id = category.Id,
        Name = category.Name,
        Value = category.Value(),
        Target = category.Target,
        Actual = category.Actual(categories),
        Stocks = category.Stocks.Select(s => s.ToDto()).ToList()
    };

    private static decimal Value(this Category category) =>
        category.Stocks.Sum(s => s.Value);

    private static double Actual(this Category category, List<Category> categories)
    {
        var categoryValue = category.Value();
        var totalValue = categories.Sum(c => c.Value());

        return totalValue == 0 ? 0 : (double)(categoryValue / totalValue);
    }
}
