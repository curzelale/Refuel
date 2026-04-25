using Refuel.Application.GasStations.Dtos;
using Refuel.Application.Mediator;
using Refuel.Application.UnitOfWork;
using Refuel.Domain.Entities;

namespace Refuel.Application.GasStations.Commands.CreateGasStation;

public class CreateGasStationCommandHandler : IRequestHandler<CreateGasStationCommand, GasStationDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateGasStationCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<GasStationDto> HandleAsync(CreateGasStationCommand request,
        CancellationToken cancellationToken = default)
    {
        var gasStation = new GasStation(request.Name, request.Address, request.Latitude, request.Longitude);

        await _unitOfWork.Repository<GasStation>().AddAsync(gasStation);
        await _unitOfWork.CommitAsync(cancellationToken);

        return new GasStationDto(gasStation.Id, gasStation.Name, gasStation.Address, gasStation.Latitude,
            gasStation.Longitude);
    }
}