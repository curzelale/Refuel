using Refuel.Application.GasStations.Dtos;
using Mediator;
using Refuel.Application.UnitOfWork;
using Refuel.Domain.Entities;
using Refuel.Domain.Repositories;

namespace Refuel.Application.GasStations.Commands.AddFuelToGasStation;

public class AddFuelToGasStationCommandHandler : IRequestHandler<AddFuelToGasStationCommand, GasStationDto>
{
    private readonly IGasStationRepository _gasStationRepository;
    private readonly IRepository<Fuel> _fuelRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddFuelToGasStationCommandHandler(
        IGasStationRepository gasStationRepository,
        IRepository<Fuel> fuelRepository,
        IUnitOfWork unitOfWork)
    {
        _gasStationRepository = gasStationRepository;
        _fuelRepository = fuelRepository;
        _unitOfWork = unitOfWork;
    }

    public async ValueTask<GasStationDto> Handle(AddFuelToGasStationCommand request,
        CancellationToken cancellationToken = default)
    {
        var gasStation = await _gasStationRepository.GetByIdWithFuelsAsync(request.GasStationId)
            ?? throw new KeyNotFoundException($"GasStation with id '{request.GasStationId}' was not found.");

        var fuel = await _fuelRepository.GetByIdAsync(request.FuelId)
            ?? throw new KeyNotFoundException($"Fuel with id '{request.FuelId}' was not found.");

        gasStation.AddFuel(fuel);

        _gasStationRepository.Update(gasStation);
        await _unitOfWork.CommitAsync(cancellationToken);

        return new GasStationDto(
            gasStation.Id, gasStation.Name, gasStation.Address,
            gasStation.Latitude, gasStation.Longitude,
            gasStation.Fuels.Select(f => new Fuels.Dtos.FuelDto(f.Id, f.Name)));
    }
}
