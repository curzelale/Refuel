using Mediator;
using Refuel.Application.UnitOfWork;
using Refuel.Application.Vehicles.Dtos;
using Refuel.Domain.Entities;
using Refuel.Domain.Repositories;

namespace Refuel.Application.Vehicles.Commands.CreateVehicle;

public class CreateVehicleCommandHandler : IRequestHandler<CreateVehicleCommand, VehicleDto>
{
    private readonly IVehicleRepository _vehicleRepository;
    private readonly IRepository<Fuel> _fuelRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateVehicleCommandHandler(
        IVehicleRepository vehicleRepository,
        IRepository<Fuel> fuelRepository,
        IUnitOfWork unitOfWork)
    {
        _vehicleRepository = vehicleRepository;
        _fuelRepository = fuelRepository;
        _unitOfWork = unitOfWork;
    }

    public async ValueTask<VehicleDto> Handle(CreateVehicleCommand request, CancellationToken cancellationToken)
    {
        var allFuels = (await _fuelRepository.GetAllAsync()).ToDictionary(f => f.Id);

        var missingId = request.FuelIds.FirstOrDefault(id => !allFuels.ContainsKey(id));
        if (missingId != default)
            throw new KeyNotFoundException($"Fuel with id '{missingId}' was not found.");

        var vehicle = new Vehicle(request.Brand, request.Model, request.Owner, request.Nickname, request.LicencesPlate);

        foreach (var fuelId in request.FuelIds)
            vehicle.AddFuel(allFuels[fuelId]);

        await _vehicleRepository.AddAsync(vehicle);
        await _unitOfWork.CommitAsync(cancellationToken);

        return new VehicleDto(vehicle.Id, vehicle.Brand, vehicle.Model, vehicle.Owner,
            vehicle.Fuels.Select(f => new Fuels.Dtos.FuelDto(f.Id, f.Name)), vehicle.Nickname, vehicle.LicencesPlate);
    }
}