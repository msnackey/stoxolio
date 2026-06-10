namespace Stoxolio.Service.DTOs;

public class CategoryDto
{
    public long Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public decimal Value { get; init; }
    public double Target { get; init; }
    public double Actual { get; init; }
    public List<StockDto> Stocks { get; init; } = [];
}
