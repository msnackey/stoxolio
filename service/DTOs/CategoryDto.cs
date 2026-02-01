namespace Stoxolio.Service.DTOs;

public class CategoryDto
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public double Target { get; init; }
    public List<StockDto> Stocks { get; init; } = new();
}
