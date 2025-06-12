namespace OnlineAuctionAPI.Models;

public class RefreshToken
{
    public Guid Id { get; set; }
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsRevoked { get; set; }
    public DateTime RevokedAt{ get; set; }
    public Guid UserId { get; set; }
    public User? User { get; set; }
    public bool IsValid => !IsRevoked && DateTime.UtcNow < ExpiresAt;
}