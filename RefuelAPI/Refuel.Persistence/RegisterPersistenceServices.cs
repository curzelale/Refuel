using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Refuel.Persistence;

public static class RegisterPersistenceServices
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services)
    {
        services.AddDbContext<RefuelDbContext>(options =>
            options.UseSqlite("name=RefuelDb"));

        services.AddHostedService<DatabaseMigrationService>();

        return services;
    }
}