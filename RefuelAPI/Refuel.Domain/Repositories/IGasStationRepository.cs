using Refuel.Domain.Entities;

namespace Refuel.Domain.Repositories;

public interface IGasStationRepository : IRepository<GasStation>
{
    Task<GasStation?> GetByIdWithFuelsAsync(Guid id);
    Task<IEnumerable<GasStation>> GetAllWithFuelsAsync();
}
