using Notify.Models;
using Notify.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Notify.Repositories;

public class NotificationRepository : Repository<Notification>
{
    public NotificationRepository(NotifyContext context) : base(context)
    {
    }

    public override async Task<Notification?> GetByIdAsync(int id)
    {
        return await _context.Set<Notification>().FindAsync(id);
    }

    public override async Task<IEnumerable<Notification>> GetAllAsync()
    {
        return await _context.Set<Notification>().ToListAsync();
    }
}