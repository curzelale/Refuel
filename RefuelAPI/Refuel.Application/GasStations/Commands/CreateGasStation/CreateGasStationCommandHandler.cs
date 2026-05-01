using Refuel.Application.GasStations.Dtos;
using Mediator;
using Refuel.Application.UnitOfWork;
using Refuel.Domain.Entities;
using Refuel.Domain.Repositories;

namespace Refuel.Application.GasStations.Commands.CreateGasStation;

public class CreateGasStationCommandHandler : IRequestHandler<CreateGasStationCommand, GasStationDto>
{
    private readonly IRepository<GasStation> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateGasStationCommandHandler(IRepository<GasStation> repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async ValueTask<GasStationDto> Handle(CreateGasStationCommand request,
        CancellationToken cancellationToken = default)
    {
        var gasStation = new GasStation(request.Name, request.Address, request.Latitude, request.Longitude);

        await _repository.AddAsync(gasStation);
        await _unitOfWork.CommitAsync(cancellationToken);

        return new GasStationDto(gasStation.Id, gasStation.Name, gasStation.Address, gasStation.Latitude,
            gasStation.Longitude, Enumerable.Empty<Fuels.Dtos.FuelDto>());
    }
}