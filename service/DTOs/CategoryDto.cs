namespace Stoxolio.Service.DTOs;

public class CategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Target { get; set; }
    public List<StockDto> Stocks { get; set; } = new();
}
