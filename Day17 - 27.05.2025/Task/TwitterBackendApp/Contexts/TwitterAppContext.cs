using System;
namespace TwitterBackendApp.Contexts;

using Microsoft.EntityFrameworkCore;
using TwitterBackendApp.Models;

public class TwitterAppContext : DbContext
{
    public TwitterAppContext(DbContextOptions options) : base(options)
    {

    }
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Tweet> Tweets { get; set; } = null!;
    public DbSet<Reply> Replies { get; set; } = null!;
    public DbSet<TweetLike> TweetLikes { get; set; } = null!;
    public DbSet<TweetHashTag> TweetHashTags { get; set; } = null!;
    public DbSet<HashTag> HashTags { get; set; } = null!;
    public DbSet<Follow> Follows { get; set; } = null!;
    

}
