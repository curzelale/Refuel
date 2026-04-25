using Refuel.Application.UnitOfWork;

namespace Refuel.Persistence.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly RefuelDbContext _context;

    public UnitOfWork(RefuelDbContext context)
    {
        _context = context;
    }

    public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
        => await _context.SaveChangesAsync(cancellationToken);
}