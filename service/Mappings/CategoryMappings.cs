using Stoxolio.Service.DTOs;
using Stoxolio.Service.Models;

namespace Stoxolio.Service.Mappings;

public static class CategoryMappings
{
    public static CategoryDto ToDto(this Category category) => new()
    {
        Id = category.Id,
        Name = category.Name,
        Target = category.Target,
        Stocks = category.Stocks.Select(s => s.ToDto()).ToList()
    };
}
