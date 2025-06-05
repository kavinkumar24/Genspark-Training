using Microsoft.EntityFrameworkCore;
using Notify.Models;
namespace Notify.Contexts;

public class NotifyContext : DbContext
{
    public NotifyContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Notification> Notifications { get; set; } = null!;
    public DbSet<FileMetaData> FileMetaDatas { get; set; } = null!;

   protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasKey(u => u.Id);

        modelBuilder.Entity<Notification>().HasKey(n => n.Id);
        modelBuilder.Entity<Notification>()
            .HasOne(n => n.User)
            .WithMany()
            .HasForeignKey(n => n.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<FileMetaData>()
            .HasKey(f => f.Id);

        modelBuilder.Entity<FileMetaData>()
            .HasOne(f => f.User)
            .WithMany()
            .HasForeignKey(f => f.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}