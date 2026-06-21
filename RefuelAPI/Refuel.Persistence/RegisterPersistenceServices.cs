using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refuel.Application.UnitOfWork;
using Refuel.Domain.Repositories;
using Refuel.Persistence.Repositories;

namespace Refuel.Persistence;

public static class RegisterPersistenceServices
{
    public static IServiceCollection AddPersistenceServices(
        this IServiceCollection services, IConfiguration configuration)
    {
        var provider = configuration["DatabaseProvider"] ?? "Sqlite";
        var connectionString = configuration.GetConnectionString("RefuelDb");

        services.AddDbContext<RefuelDbContext>(options =>
        {
            switch (provider.ToLowerInvariant())
            {
                case "postgres":
                    options.UseNpgsql(connectionString, npgsql =>
                        npgsql.MigrationsAssembly("Refuel.Persistence.Migrations.Postgres"));
                    break;

                case "sqlite":
                    options.UseSqlite(connectionString, sqlite =>
                        sqlite.MigrationsAssembly("Refuel.Persistence.Migrations.Sqlite"));
                    break;

                default:
                    throw new InvalidOperationException(
                        $"Unsupported DatabaseProvider '{provider}'. Use 'Sqlite' or 'Postgres'.");
            }
        });


        //Crea il db ed applica le migrazioni se necessario
        services.AddHostedService<DatabaseMigrationService>();
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IGasStationRepository, GasStationRepository>();
        services.AddScoped<IVehicleRepository, VehicleRepository>();
        services.AddScoped<IRefuelRepository, RefuelRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();

        return services;
    }
}
