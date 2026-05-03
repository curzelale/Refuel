using Mediator;
using Refuel.Application.Fuels.Dtos;
using Refuel.Application.GasStations.Dtos;
using Refuel.Application.Refuels.Dtos;
using Refuel.Application.UnitOfWork;
using Refuel.Application.Vehicles.Dtos;
using Refuel.Domain.Entities;
using Refuel.Domain.Repositories;

namespace Refuel.Application.Refuels.Commands.CreateRefuel;

public class CreateRefuelCommandHandler : IRequestHandler<CreateRefuelCommand, RefuelDto>
{
    private readonly IRefuelRepository _refuelRepository;
    private readonly IVehicleRepository _vehicleRepository;
    private readonly IGasStationRepository _gasStationRepository;
    private readonly IRepository<Fuel> _fuelRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateRefuelCommandHandler(
        IRefuelRepository refuelRepository,
        IVehicleRepository vehicleRepository,
        IGasStationRepository gasStationRepository,
        IRepository<Fuel> fuelRepository,
        IUnitOfWork unitOfWork)
    {
        _refuelRepository = refuelRepository;
        _vehicleRepository = vehicleRepository;
        _gasStationRepository = gasStationRepository;
        _fuelRepository = fuelRepository;
        _unitOfWork = unitOfWork;
    }

    public async ValueTask<RefuelDto> Handle(CreateRefuelCommand request, CancellationToken cancellationToken)
    {
        var vehicle = await _vehicleRepository.GetByIdAsync(request.VehicleId);
        if (vehicle is null)
            throw new KeyNotFoundException($"Vehicle with id '{request.VehicleId}' was not found.");

        var gasStation = await _gasStationRepository.GetByIdAsync(request.GasStationId);
        if (gasStation is null)
            throw new KeyNotFoundException($"GasStation with id '{request.GasStationId}' was not found.");

        var fuel = await _fuelRepository.GetByIdAsync(request.FuelId);
        if (fuel is null)
            throw new KeyNotFoundException($"Fuel with id '{request.FuelId}' was not found.");

        if (!vehicle.Fuels.Any(f => f.Id == request.FuelId))
            throw new KeyNotFoundException(
                $"Fuel with id '{request.FuelId}' is not compatible with vehicle '{request.VehicleId}'.");

        var refuel = new Domain.Entities.Refuel(
            request.VehicleId,
            request.GasStationId,
            request.FuelId,
            request.Quantity,
            request.TotalPrice,
            request.Date,
            request.OdometerKm,
            request.Note);

        await _refuelRepository.AddAsync(refuel);
        await _unitOfWork.CommitAsync(cancellationToken);

        var vehicleDto = new VehicleDto(
            vehicle.Id, vehicle.Brand, vehicle.Model, vehicle.Owner,
            vehicle.Fuels.Select(f => new FuelDto(f.Id, f.Name)),
            vehicle.Nickname, vehicle.LicencesPlate);

        var gasStationDto = new GasStationDto(
            gasStation.Id, gasStation.Name, gasStation.Address,
            gasStation.Latitude, gasStation.Longitude,
            Enumerable.Empty<FuelDto>());

        var fuelDto = new FuelDto(fuel.Id, fuel.Name);

        return new RefuelDto(
            refuel.Id,
            refuel.VehicleId,
            refuel.GasStationId,
            refuel.FuelId,
            refuel.Quantity,
            refuel.TotalPrice,
            refuel.Date,
            refuel.OdometerKm,
            refuel.Note,
            vehicleDto,
            gasStationDto,
            fuelDto);
    }
}
