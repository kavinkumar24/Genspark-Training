namespace Notify.Models;

public class Notification
{
    public int Id { get; set; }
    public string Message { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string? DownloadUrl { get; set; }
    public int UserId { get; set; }
    public User? User { get; set; }
}