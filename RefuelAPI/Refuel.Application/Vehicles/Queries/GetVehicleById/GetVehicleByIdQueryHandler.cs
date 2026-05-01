using Mediator;
using Refuel.Application.Fuels.Dtos;
using Refuel.Application.Vehicles.Dtos;
using Refuel.Domain.Repositories;

namespace Refuel.Application.Vehicles.Queries.GetVehicleById;

public class GetVehicleByIdQueryHandler : IRequestHandler<GetVehicleByIdQuery, VehicleDto?>
{
    private readonly IVehicleRepository _repository;

    public GetVehicleByIdQueryHandler(IVehicleRepository repository)
    {
        _repository = repository;
    }

    public async ValueTask<VehicleDto?> Handle(GetVehicleByIdQuery request, CancellationToken cancellationToken)
    {
        var vehicle = await _repository.GetByIdAsync(request.Id);
        
        return vehicle is null
            ? null
            : new VehicleDto(
                vehicle.Id, 
                vehicle.Brand, 
                vehicle.Model, 
                vehicle.Owner, 
                vehicle.Fuels.Select(f => new FuelDto(f.Id, f.Name)),
                vehicle.Nickname,
                vehicle.LicencesPlate);
    }
}