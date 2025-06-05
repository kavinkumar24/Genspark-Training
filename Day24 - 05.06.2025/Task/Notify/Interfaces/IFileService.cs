using Microsoft.AspNetCore.Http;
using Notify.Models;

namespace Notify.Interfaces;

public interface IFileService
{
    Task<FileMetaData> UploadFileAsync(IFormFile file);
    Task<FileMetaData?> DownloadFileAsync(int id);
}