namespace Stoxolio.Service.Models;

public record Transaction : BaseEntity
{
    public required DateOnly Date { get; init; }
    public required TimeOnly Time { get; init; }
    public required string Product { get; init; } = string.Empty;
    public required string Isin { get; init; } = string.Empty;
    public required string Exchange { get; init; } = string.Empty;
    public required int Shares { get; init; }
    public required decimal Price { get; init; }
    public required decimal Value { get; init; }
    public required decimal Fees { get; init; }
    public required decimal Total { get; init; }
    public required string OrderId { get; init; } = string.Empty;
}
