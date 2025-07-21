namespace videoPortalAPI.Models
{
    public class VideoData
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string BlobUrl { get; set; } = string.Empty;
        public string? ThumbnailUrl { get; set; }
        public DateTime UploadedDate { get; set; } = DateTime.UtcNow;
    }
}
