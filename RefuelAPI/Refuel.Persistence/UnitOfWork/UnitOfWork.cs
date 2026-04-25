using Microsoft.Extensions.DependencyInjection;
using Refuel.Application.UnitOfWork;
using Refuel.Domain.Repositories;

namespace Refuel.Persistence.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly RefuelDbContext _context;
    private readonly IServiceProvider _serviceProvider;
    private readonly Dictionary<Type, object> _repositories = new();

    public UnitOfWork(RefuelDbContext context, IServiceProvider serviceProvider)
    {
        _context = context;
        _serviceProvider = serviceProvider;
    }

    public IRepository<T> Repository<T>() where T : class
    {
        var type = typeof(T);
        if (!_repositories.TryGetValue(type, out var repo))
        {
            repo = _serviceProvider.GetRequiredService<IRepository<T>>();
            _repositories[type] = repo;
        }

        return (IRepository<T>)repo;
    }

    public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
        => await _context.SaveChangesAsync(cancellationToken);
}