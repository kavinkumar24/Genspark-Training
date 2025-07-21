using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using videoPortalAPI.Interfaces;
using videoPortalAPI.Models;
using videoPortalAPI.Models.Dto;

namespace videoPortalAPI.Services
{
    public class VideoService : IVideoService
    {
        private readonly IVideoRepository _videoRepositoty;
        private readonly IConfiguration _configuration;

        public VideoService(IVideoRepository videoRepository, IConfiguration configuration)
        {
            _videoRepositoty = videoRepository;
            _configuration = configuration;
        }

        public async Task<VideoData> AddVideoAsync(VideoDataUploadDto videoUploadDto)
        {
            var uniqueFileName = Guid.NewGuid() + Path.GetExtension(videoUploadDto.File.FileName);
            var connectionString = _configuration["AzureBlob:ConnectionString"];
            var containerName = _configuration["AzureBlob:ContainerName"];

            var blobClient = new BlobContainerClient(connectionString, containerName);

            var blob = blobClient.GetBlobClient(uniqueFileName);
            await blob.UploadAsync(
                videoUploadDto.File.OpenReadStream(),
                new BlobUploadOptions
                {
                    HttpHeaders = new BlobHttpHeaders
                    {
                        ContentType = videoUploadDto.File.ContentType,
                    },
                }
            );

            string? thumbnailUrl = null;
            if (videoUploadDto.Thumbnail != null)
            {
                var thumbFileName =
                    "thumb_"
                    + Guid.NewGuid()
                    + Path.GetExtension(videoUploadDto.Thumbnail.FileName);
                var thumbBlob = blobClient.GetBlobClient(thumbFileName);

                await thumbBlob.UploadAsync(
                    videoUploadDto.Thumbnail.OpenReadStream(),
                    new BlobUploadOptions
                    {
                        HttpHeaders = new BlobHttpHeaders
                        {
                            ContentType = videoUploadDto.Thumbnail.ContentType,
                        },
                    }
                );
                thumbnailUrl = thumbBlob.Uri.ToString();
            }

            var videoData = new VideoData
            {
                Id = Guid.NewGuid(),
                Title = videoUploadDto.Title,
                Description = videoUploadDto.Description,
                BlobUrl = blob.Uri.ToString(),
                ThumbnailUrl = thumbnailUrl,
                UploadedDate = DateTime.UtcNow,
            };
            await _videoRepositoty.AddVideoAsync(videoData);
            return videoData;
        }

        public Task<IEnumerable<VideoData>> GetAllVideosAsync()
        {
            var videos = _videoRepositoty.GetAllVideosAsync();
            return videos;
        }

        public Task<VideoData?> GetVideoByIdAsync(Guid id)
        {
            var video = _videoRepositoty.GetVideoByIdAsync(id);
            return video;
        }
    }
}
