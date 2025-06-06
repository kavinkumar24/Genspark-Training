using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Notify.Hub;
using Notify.Interfaces;
using Notify.Misc;
using Notify.Models;

namespace Notify.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FileHandleController : ControllerBase
{
    private readonly IFileService _fileService;
    private readonly IHubContext<FileUploadNotification> _hubContext;

    public FileHandleController(IFileService fileService, IHubContext<FileUploadNotification> hubContext)
    {
            _fileService = fileService;
            _hubContext = hubContext;
    }

    [HttpPost("upload")]
    [Authorize(Roles = "HR")]
    [CustomExceptionFilter]
    public async Task<ActionResult<FileMetaData>> Upload(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded.");

        var result = await _fileService.UploadFileAsync(file);
        string downloadUrl = $"http://localhost:5146/api/FileHandle/download/{result.Id}";
        string message = $"<a href='{downloadUrl}' target='_blank'>Download here</a>";
        string time = DateTime.Now.ToString("g");

        var notification = new Notification
        {
            Message = message,
            DownloadUrl = downloadUrl,
            UserId = int.TryParse(User.FindFirst("id")?.Value, out var userId) ? userId : 0
        };
        await _fileService.SaveAsync(notification);
        await _hubContext.Clients.All.SendAsync("ReceiveMessage", "HR", message, time);

        return Ok("File uploaded successfully.");
    }


    [HttpGet("download/{id}")]
    [CustomExceptionFilter]
    [Authorize]
    public async Task<IActionResult> Download(int id)
    {
        var fileMetaData = await _fileService.DownloadFileAsync(id);
        if (fileMetaData == null)
            return NotFound("File not found.");

        return File(fileMetaData.Data, fileMetaData.ContentType, fileMetaData.FileName);
    }
}