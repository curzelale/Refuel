using Refuel.Domain.Repositories;

namespace Refuel.Application.UnitOfWork;

public interface IUnitOfWork
{
    IRepository<T> Repository<T>() where T : class;
    Task<int> CommitAsync(CancellationToken cancellationToken = default);
}
