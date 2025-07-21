using Microsoft.EntityFrameworkCore;
using videoPortalAPI.Context;
using videoPortalAPI.Interfaces;
using videoPortalAPI.Models;
using videoPortalAPI.Models.Dto;

namespace videoPortalAPI.Repositories
{
    public class VideoRepository : IVideoRepository
    {
        private readonly AppDbContext _context;

        public VideoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<VideoData>> GetAllVideosAsync()
        {
            return await _context.Videos.ToListAsync();
        }

        public async Task<VideoData?> GetVideoByIdAsync(Guid id)
        {
            return await _context.Videos.FindAsync(id);
        }

        public async Task AddVideoAsync(VideoData video)
        {
            _context.Videos.Add(video);
            await _context.SaveChangesAsync();
        }
    }
}
