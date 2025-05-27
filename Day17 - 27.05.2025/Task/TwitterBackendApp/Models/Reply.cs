using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace TwitterBackendApp.Models;

public class Reply
{
    public int Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public int UserId { get; set; }
    [ForeignKey("UserId")]
    public User? User { get; set; }
    public int TweetId { get; set; }
    [ForeignKey("TweetId")]
    public Tweet? Tweet { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
