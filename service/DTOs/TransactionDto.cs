namespace Stoxolio.Service.DTOs;

public class TransactionDto
{
    public DateOnly Date { get; init; }
    public TimeOnly Time { get; init; }
    public string Product { get; init; } = string.Empty;
    public string Isin { get; init; } = string.Empty;
    public string Exchange { get; init; } = string.Empty;
    public int Shares { get; init; }
    public decimal Price { get; init; }
    public decimal Value { get; init; }
    public decimal Fees { get; init; }
    public decimal Total { get; init; }
    public string OrderId { get; init; } = string.Empty;
}
