using Notify.Models;
using Notify.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Notify.Repositories;

public class UserRepository : Repository<User>
{
    public UserRepository(NotifyContext context) : base(context)
    {
    }

    public override async Task<User?> GetByIdAsync(int id)
    {
        return await _context.Set<User>().FindAsync(id);
    }

    public override async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _context.Set<User>().ToListAsync();
    }
}