using Microsoft.AspNetCore.Mvc;
using videoPortalAPI.Models.Dto;

using videoPortalAPI.Interfaces;

namespace videoPortalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideosController : ControllerBase
    {
        private readonly IVideoService _videoService;
        public VideosController(IVideoService videoService)
        {
            _videoService = videoService;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> AddVideo_Details([FromForm] VideoDataUploadDto dto)
        {
            if (dto.File == null || dto.File.Length == 0)
            {
                return BadRequest("No file provided");
            }
            var video = await _videoService.AddVideoAsync(dto);
            return Ok(video);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllVideos()
        {
            var videos = await _videoService.GetAllVideosAsync();
            return Ok(videos);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetVideoById(Guid id)
        {
            var video = await _videoService.GetVideoByIdAsync(id);
            if (video == null)
            {
                return NotFound();
            }
            return Ok(video);
        }
    }
}