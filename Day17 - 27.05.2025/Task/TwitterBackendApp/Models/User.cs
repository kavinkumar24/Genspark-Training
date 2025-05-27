using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TwitterBackendApp.Models;

public class User
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [Required]
    public string Username { get; set; } = string.Empty;
    [Required]
    [EmailAddress(ErrorMessage = "Invalid Email Address")]
    public string Email { get; set; } = string.Empty;
    [Required]
    public string Password { get; set; } = string.Empty;
    public string? Bio { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public ICollection<Tweet>? Tweets { get; set; }
    public ICollection<TweetLike>? Likes { get; set; }
    [InverseProperty(nameof(Follow.Following))]
    public ICollection<Follow>? Followers { get; set; }
    [InverseProperty(nameof(Follow.Follower))]
    public ICollection<Follow>? Following { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
