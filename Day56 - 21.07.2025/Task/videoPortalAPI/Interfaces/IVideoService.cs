using videoPortalAPI.Models;
using videoPortalAPI.Models.Dto;

namespace videoPortalAPI.Interfaces
{
    public interface IVideoService
    {
        Task<IEnumerable<VideoData>> GetAllVideosAsync();
        Task<VideoData?> GetVideoByIdAsync(Guid id);
        Task<VideoData> AddVideoAsync(VideoDataUploadDto videoUploadDto);
    }
}