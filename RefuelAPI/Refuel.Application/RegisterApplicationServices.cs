using FluentValidation;
using Mediator;
using Microsoft.Extensions.DependencyInjection;
using Refuel.Application.Pipeline;

namespace Refuel.Application;

public static class RegisterApplicationServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));
        services.AddValidatorsFromAssembly(typeof(RegisterApplicationServices).Assembly, ServiceLifetime.Scoped);
        return services;
    }
}
