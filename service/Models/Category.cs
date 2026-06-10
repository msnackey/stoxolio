namespace Stoxolio.Service.Models;

public record Category : BaseEntity
{
    public required string Name { get; set; }
    public required double Target { get; set; }

    public ICollection<Stock> Stocks { get; set; } = [];
}
