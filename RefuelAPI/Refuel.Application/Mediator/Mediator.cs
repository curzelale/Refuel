using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Refuel.Application.Mediator;

public class MediatorService : IMediator
{
    private readonly IServiceProvider _serviceProvider;

    public MediatorService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request,
        CancellationToken cancellationToken = default)
    {
        var validatorType = typeof(IValidator<>).MakeGenericType(request.GetType());
        if (_serviceProvider.GetService(validatorType) is IValidator validator)
        {
            ValidateRequest((dynamic)request, validator);
        }

        var handlerType = typeof(IRequestHandler<,>).MakeGenericType(request.GetType(), typeof(TResponse));
        dynamic handler = _serviceProvider.GetRequiredService(handlerType);
        return await handler.HandleAsync((dynamic)request, cancellationToken);
    }

    private void ValidateRequest<TRequest>(TRequest request, IValidator validator)
    {
        var context = new ValidationContext<TRequest>(request);
        var result = validator.Validate(context);
        if (!result.IsValid)
        {
            var errors = result.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(g => g.Key, g => 
                    g.Select(e => e.ErrorMessage).ToArray());
            throw new Exceptions.ValidationException(errors);
        }
    }
}