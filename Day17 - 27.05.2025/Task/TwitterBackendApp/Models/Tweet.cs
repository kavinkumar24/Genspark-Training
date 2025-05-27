using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TwitterBackendApp.Models;

public class Tweet
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int TweetId { get; set; }
    public string Content { get; set; } = string.Empty;
    public int UserId { get; set; }
    
    [ForeignKey("UserId")]
    public User? User { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public ICollection<TweetLike>? Likes { get; set; }
    public ICollection<TweetHashTag>? TweetHashtags { get; set; }
    public ICollection<Reply>? Replies { get; set; }

}
