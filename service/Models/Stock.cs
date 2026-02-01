namespace Stoxolio.Service.Models;

public class Stock
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
    
    // Computed fields - calculated by backend
    public decimal Value => Shares * Price;
    public decimal PriceChange => Price - PrevPrice;
    public decimal ValueChange => Shares * PriceChange;
    
    // Navigation property
    public Category? Category { get; set; }
}
