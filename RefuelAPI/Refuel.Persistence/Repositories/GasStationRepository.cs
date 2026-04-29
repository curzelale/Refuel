using Microsoft.EntityFrameworkCore;
using Refuel.Domain.Entities;
using Refuel.Domain.Repositories;

namespace Refuel.Persistence.Repositories;

public class GasStationRepository : Repository<GasStation>, IGasStationRepository
{
    private readonly RefuelDbContext _context;

    public GasStationRepository(RefuelDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<GasStation?> GetByIdWithFuelsAsync(Guid id) =>
        await _context.GasStations
            .Include(gs => gs.Fuels)
            .FirstOrDefaultAsync(gs => gs.Id == id);

    public async Task<IEnumerable<GasStation>> GetAllWithFuelsAsync() =>
        await _context.GasStations
            .Include(gs => gs.Fuels)
            .ToListAsync();
}
