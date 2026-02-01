namespace Stoxolio.Service.DTOs;

public class StockDto
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Ticker { get; init; } = string.Empty;
    public string Exchange { get; init; } = string.Empty;
    public bool Sri { get; init; }
    public int Shares { get; init; }
    public decimal Price { get; init; }
    public bool Invest { get; init; }
    public int CategoryId { get; init; }
    public decimal PrevPrice { get; init; }
    
    // Computed fields
    public decimal Value { get; init; }
    public decimal PriceChange { get; init; }
    public decimal ValueChange { get; init; }
}
