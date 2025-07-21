using Microsoft.EntityFrameworkCore;
using videoPortalAPI.Models;

namespace videoPortalAPI.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<VideoData> Videos { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<VideoData>().HasKey(v => v.Id);
            modelBuilder.Entity<VideoData>().Property(v => v.Title).IsRequired();
            modelBuilder.Entity<VideoData>().Property(v => v.BlobUrl).IsRequired();
        }
    }
}
