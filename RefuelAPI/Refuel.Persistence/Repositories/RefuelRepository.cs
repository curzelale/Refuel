using Microsoft.EntityFrameworkCore;
using Refuel.Domain.Repositories;

namespace Refuel.Persistence.Repositories;

public class RefuelRepository : Repository<Domain.Entities.Refuel>, IRefuelRepository
{
    private readonly RefuelDbContext _context;

    public RefuelRepository(RefuelDbContext context) : base(context)
    {
        _context = context;
    }

    public override async Task<Domain.Entities.Refuel?> GetByIdAsync(Guid id) =>
        await _context.Refuels
            .Include(r => r.Vehicle).ThenInclude(v => v!.Fuels)
            .Include(r => r.GasStation)
            .Include(r => r.Fuel)
            .FirstOrDefaultAsync(r => r.Id == id);

    public override async Task<IEnumerable<Domain.Entities.Refuel>> GetAllAsync() =>
        await _context.Refuels
            .Include(r => r.Vehicle).ThenInclude(v => v!.Fuels)
            .Include(r => r.GasStation)
            .Include(r => r.Fuel)
            .ToListAsync();

    public async Task<IEnumerable<Domain.Entities.Refuel>> GetByVehicleIdAsync(Guid vehicleId) =>
        await _context.Refuels
            .Where(r => r.VehicleId == vehicleId)
            .Include(r => r.Vehicle).ThenInclude(v => v!.Fuels)
            .Include(r => r.GasStation)
            .Include(r => r.Fuel)
            .ToListAsync();
}
