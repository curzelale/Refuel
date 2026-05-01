using Microsoft.EntityFrameworkCore;
using Refuel.Domain.Repositories;

namespace Refuel.Persistence.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly RefuelDbContext _context;
    private readonly DbSet<T> _dbSet;

    public Repository(RefuelDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public virtual async Task<T?> GetByIdAsync(Guid id) => await _dbSet.FindAsync(id);

    public virtual async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync();

    public async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);

    public void Update(T entity) => _dbSet.Update(entity);

    public void Delete(T entity) => _dbSet.Remove(entity);
}