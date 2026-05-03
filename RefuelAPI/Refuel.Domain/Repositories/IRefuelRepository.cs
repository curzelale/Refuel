namespace Refuel.Domain.Repositories;

public interface IRefuelRepository : IRepository<Domain.Entities.Refuel>
{
    Task<IEnumerable<Domain.Entities.Refuel>> GetByVehicleIdAsync(Guid vehicleId);
}
