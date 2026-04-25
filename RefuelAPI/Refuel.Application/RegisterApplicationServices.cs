using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Refuel.Application.Mediator;

namespace Refuel.Application;

public static class RegisterApplicationServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IMediator, Mediator.MediatorService>();

        var assembly = typeof(RegisterApplicationServices).Assembly;

        var handlerRegistrations = assembly.GetTypes()
            .Where(t => t is { IsAbstract: false, IsInterface: false })
            .SelectMany(t => t.GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>))
                .Select(i => (Implementation: t, Interface: i)));

        foreach (var (implementation, @interface) in handlerRegistrations)
        {
            services.AddScoped(@interface, implementation);
        }

        services.AddValidatorsFromAssembly(assembly, ServiceLifetime.Scoped);
        
        
        return services;
    }
    
}