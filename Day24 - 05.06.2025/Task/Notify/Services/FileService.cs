using Microsoft.AspNetCore.Http;
using Notify.Interfaces;
using Notify.Contexts;
using Notify.Models;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace Notify.Services;

public class FileService : IFileService
{
    private readonly NotifyContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;


    public FileService(NotifyContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<FileMetaData> UploadFileAsync(IFormFile file)
    {
        using var memoryStream = new MemoryStream();
        await file.CopyToAsync(memoryStream);

        var httpContext = _httpContextAccessor.HttpContext;
        var email = httpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        int? userId = null;
        if (!string.IsNullOrEmpty(email))
        {
            var user = await _context.Set<User>().FirstOrDefaultAsync(u => u.Email == email);
            userId = user?.Id;
        }

        var fileMetaData = new FileMetaData
        {
            FileName = file.FileName,
            ContentType = file.ContentType,
            Data = memoryStream.ToArray(),
            UserId = userId ?? 0
        };
        _context.Set<FileMetaData>().Add(fileMetaData);
        await _context.SaveChangesAsync();
        return fileMetaData;
    }

    public async Task<FileMetaData?> DownloadFileAsync(int id)
    {
        return await _context.Set<FileMetaData>().FindAsync(id);
    }
    
    public async Task SaveAsync(Notification notification)
    {
        _context.Set<Notification>().Add(notification);
        await _context.SaveChangesAsync();
    }
}