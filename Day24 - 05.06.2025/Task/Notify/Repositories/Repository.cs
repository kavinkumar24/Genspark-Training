
using Notify.Contexts;

namespace Notify.Repositories;


public abstract class Repository<T> : IRepository<T> where T : class
{
    protected readonly NotifyContext _context;

    public Repository(NotifyContext context)
    {
        _context = context;
    }

    public abstract  Task<T?> GetByIdAsync(int id);

    public abstract  Task<IEnumerable<T>> GetAllAsync();

    public async Task AddAsync(T entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity), "Entity cannot be null");
        }
        if (_context.Set<T>().Local.Any(e => e == entity))
        {
            return;
        }
        _context.Add(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(T entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity), "Entity cannot be null");
        }
        _context.Set<T>().Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await GetByIdAsync(id);
        if (entity == null)
        {
            throw new ArgumentException($"Entity with id {id} not found");
        }
        _context.Set<T>().Remove(entity);
        await _context.SaveChangesAsync();
    }

}