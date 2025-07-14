using BlobAPI.Models.DTO;
using BlobAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlobAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly BlobStorageService _blobStorageService;

        public FilesController(BlobStorageService blobStorageService)
        {
            _blobStorageService = blobStorageService;
        }

        [HttpGet]
        public async Task<ActionResult<Stream>> Download(string fileName)
        {
            var stream = await _blobStorageService.DownloadFile(fileName);
            if (stream == null)
                return NotFound();
            return File(stream, "application/octet-stream", fileName);
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Upload([FromForm] FileUploadDto filerequest)
        {
            if (filerequest?.File == null || filerequest.File.Length == 0)
                return BadRequest("No file to upload");
            using var stream = filerequest.File.OpenReadStream();
            await _blobStorageService.UploadFile(stream, filerequest.File.FileName);
            return Ok("File uploaded");
        }
    }
}
