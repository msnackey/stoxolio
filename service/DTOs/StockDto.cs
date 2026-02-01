namespace Stoxolio.Service.DTOs;

public class StockDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Ticker { get; set; } = string.Empty;
    public string Exchange { get; set; } = string.Empty;
    public bool Sri { get; set; }
    public int Shares { get; set; }
    public decimal Price { get; set; }
    public bool Invest { get; set; }
    public int CategoryId { get; set; }
    public decimal PrevPrice { get; set; }
    
    // Computed fields
    public decimal Value { get; set; }
    public decimal PriceChange { get; set; }
    public decimal ValueChange { get; set; }
}
