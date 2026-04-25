using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Refuel.Application.UnitOfWork;
using Refuel.Domain.Repositories;
using Refuel.Persistence.Repositories;

namespace Refuel.Persistence;

public static class RegisterPersistenceServices
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services)
    {
        services.AddDbContext<RefuelDbContext>(options =>
            options.UseSqlite("name=RefuelDb"));


        //Crea il db ed applica le migrazioni se necessario
        services.AddHostedService<DatabaseMigrationService>();
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();

        return services;
    }
}