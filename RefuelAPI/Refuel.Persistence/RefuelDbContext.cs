using Microsoft.EntityFrameworkCore;
using Refuel.Domain.Entities;

namespace Refuel.Persistence;

public class RefuelDbContext : DbContext
{
    public RefuelDbContext(DbContextOptions<RefuelDbContext> options) : base(options)
    {
    }

    public RefuelDbContext()
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        //Questo permette di individuare e applicare in automatico le classi che estendono IEntityTypeConfiguration
        builder.ApplyConfigurationsFromAssembly(typeof(RefuelDbContext).Assembly);
    }

    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<GasStation> GasStations { get; set; }
    public DbSet<Fuel> Fuels { get; set; }
    public DbSet<Domain.Entities.Refuel> Refuels { get; set; }
}