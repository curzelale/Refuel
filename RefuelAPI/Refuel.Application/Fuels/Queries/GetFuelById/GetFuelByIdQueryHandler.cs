using Refuel.Application.Fuels.Dtos;
using Mediator;
using Refuel.Domain.Entities;
using Refuel.Domain.Repositories;

namespace Refuel.Application.Fuels.Queries.GetFuelById;

public class GetFuelByIdQueryHandler : IRequestHandler<GetFuelByIdQuery, FuelDto?>
{
    private readonly IRepository<Fuel> _repository;

    public GetFuelByIdQueryHandler(IRepository<Fuel> repository)
    {
        _repository = repository;
    }

    public async ValueTask<FuelDto?> Handle(GetFuelByIdQuery request, CancellationToken cancellationToken = default)
    {
        var fuel = await _repository.GetByIdAsync(request.Id);
        return fuel is null ? null : new FuelDto(fuel.Id, fuel.Name);
    }
}
