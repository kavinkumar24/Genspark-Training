using videoPortalAPI.Models;

namespace videoPortalAPI.Interfaces
{
    public interface IVideoRepository
    {
        Task<IEnumerable<VideoData>> GetAllVideosAsync();
        Task<VideoData?> GetVideoByIdAsync(Guid id);
        Task AddVideoAsync(VideoData video);
    }
}
