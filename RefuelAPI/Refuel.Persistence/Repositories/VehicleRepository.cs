using Microsoft.EntityFrameworkCore;
using Refuel.Domain.Entities;
using Refuel.Domain.Repositories;

namespace Refuel.Persistence.Repositories;

public class VehicleRepository: Repository<Vehicle>, IVehicleRepository
{
    private readonly RefuelDbContext _context;

    public VehicleRepository(RefuelDbContext context) : base(context)
    {
        _context = context;
    }
    
    public override async Task<Vehicle?> GetByIdAsync(Guid id) =>
        await _context.Vehicles
            .Include(v => v.Fuels)
            .FirstOrDefaultAsync(v => v.Id == id);

    public override async Task<IEnumerable<Vehicle>> GetAllAsync() =>
        await _context.Vehicles
            .Include(v => v.Fuels)
            .ToListAsync();
}