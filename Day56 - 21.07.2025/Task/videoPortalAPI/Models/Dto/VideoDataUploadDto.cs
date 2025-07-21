namespace videoPortalAPI.Models.Dto
{
    public class VideoDataUploadDto
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public IFormFile File { get; set; } = null!;
        public IFormFile? Thumbnail { get; set; }
    }
}
