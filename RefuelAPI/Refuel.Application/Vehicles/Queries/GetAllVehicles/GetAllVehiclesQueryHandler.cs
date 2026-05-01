using Mediator;
using Refuel.Application.Fuels.Dtos;
using Refuel.Application.Vehicles.Dtos;
using Refuel.Domain.Repositories;

namespace Refuel.Application.Vehicles.Queries.GetAllVehicles;

public class GetAllVehiclesQueryHandler : IRequestHandler<GetAllVehiclesQuery, IEnumerable<VehicleDto>>
{
    private readonly IVehicleRepository _repository;

    public GetAllVehiclesQueryHandler(IVehicleRepository repository)
    {
        _repository = repository;
    }

    public async ValueTask<IEnumerable<VehicleDto>> Handle(GetAllVehiclesQuery request, CancellationToken cancellationToken)
    {
        var vehicles = await _repository.GetAllAsync();

        return vehicles.Select(v => new VehicleDto(
            v.Id,
            v.Brand,
            v.Model,
            v.Owner,
            v.Fuels.Select(f => new FuelDto(f.Id, f.Name))));
    }
}
