namespace Stoxolio.Service.Models;

public record Stock : BaseEntity
{
    public required string Name { get; set; }
    public required string Ticker { get; set; }
    public required string Exchange { get; set; }
    public required bool Sri { get; set; } = false;
    public required int Shares { get; set; } = 0;
    public required decimal Price { get; set; }
    public required bool Invest { get; set; } = true;
    public required long CategoryId { get; set; }
    public required decimal PrevPrice { get; set; } = 0;

    // Computed fields - calculated by backend
    public decimal Value => Shares * Price;
    public decimal PriceChange => Price - PrevPrice;
    public decimal ValueChange => Shares * PriceChange;

    // Navigation property
    public Category? Category { get; set; }
}
