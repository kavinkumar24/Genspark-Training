using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace TwitterBackendApp.Models;

public class TweetLike
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public int UserId { get; set; }
    [ForeignKey("UserId")]
    public User? User { get; set; }
    public int TweetId { get; set; }
    [ForeignKey("TweetId")]
    public Tweet? Tweet { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

}
