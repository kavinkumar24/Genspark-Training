using Notify.Models;
using Notify.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Notify.Repositories;

public class FileRepository : Repository<FileMetaData>
{
    public FileRepository(NotifyContext context) : base(context)
    {
    }

    public override async Task<FileMetaData?> GetByIdAsync(int id)
    {
        return await _context.Set<FileMetaData>().FindAsync(id);
    }

    public override async Task<IEnumerable<FileMetaData>> GetAllAsync()
    {
        return await _context.Set<FileMetaData>().ToListAsync();
    }
}