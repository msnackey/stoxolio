namespace Stoxolio.Service.Models;

public record RefreshToken : BaseEntity
{
    public required long UserId { get; set; }
    public required string Token { get; set; }
    public required DateTime Expires { get; set; }
    public bool IsRevoked { get; set; }
    public User? User { get; set; }
}
