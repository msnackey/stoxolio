namespace Stoxolio.Service.Models;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public double Target { get; set; }
    
    public ICollection<Stock> Stocks { get; set; } = new List<Stock>();
}
