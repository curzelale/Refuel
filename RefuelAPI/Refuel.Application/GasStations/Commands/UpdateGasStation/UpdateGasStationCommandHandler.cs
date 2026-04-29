using Refuel.Application.Fuels.Dtos;
using Refuel.Application.GasStations.Dtos;
using Refuel.Application.Mediator;
using Refuel.Application.UnitOfWork;
using Refuel.Domain.Repositories;

namespace Refuel.Application.GasStations.Commands.UpdateGasStation;

public class UpdateGasStationCommandHandler : IRequestHandler<UpdateGasStationCommand, GasStationDto>
{
    private readonly IGasStationRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateGasStationCommandHandler(IGasStationRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<GasStationDto> HandleAsync(UpdateGasStationCommand request,
        CancellationToken cancellationToken = default)
    {
        var gasStation = await _repository.GetByIdWithFuelsAsync(request.Id)
                         ?? throw new KeyNotFoundException($"GasStation with id '{request.Id}' was not found.");

        gasStation.Update(request.Name, request.Address, request.Latitude, request.Longitude);

        _repository.Update(gasStation);
        await _unitOfWork.CommitAsync(cancellationToken);

        return new GasStationDto(
            gasStation.Id, gasStation.Name, gasStation.Address,
            gasStation.Latitude, gasStation.Longitude,
            gasStation.Fuels.Select(f => new FuelDto(f.Id, f.Name)));
    }
}
