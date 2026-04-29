using Refuel.Application.GasStations.Dtos;
using Refuel.Application.Mediator;
using Refuel.Application.UnitOfWork;
using Refuel.Domain.Repositories;

namespace Refuel.Application.GasStations.Commands.RemoveFuelFromGasStation;

public class RemoveFuelFromGasStationCommandHandler : IRequestHandler<RemoveFuelFromGasStationCommand, GasStationDto>
{
    private readonly IGasStationRepository _gasStationRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RemoveFuelFromGasStationCommandHandler(IGasStationRepository gasStationRepository, IUnitOfWork unitOfWork)
    {
        _gasStationRepository = gasStationRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<GasStationDto> HandleAsync(RemoveFuelFromGasStationCommand request,
        CancellationToken cancellationToken = default)
    {
        var gasStation = await _gasStationRepository.GetByIdWithFuelsAsync(request.GasStationId)
            ?? throw new KeyNotFoundException($"GasStation with id '{request.GasStationId}' was not found.");

        gasStation.RemoveFuel(request.FuelId);

        _gasStationRepository.Update(gasStation);
        await _unitOfWork.CommitAsync(cancellationToken);

        return new GasStationDto(
            gasStation.Id, gasStation.Name, gasStation.Address,
            gasStation.Latitude, gasStation.Longitude,
            gasStation.Fuels.Select(f => new Fuels.Dtos.FuelDto(f.Id, f.Name)));
    }
}
