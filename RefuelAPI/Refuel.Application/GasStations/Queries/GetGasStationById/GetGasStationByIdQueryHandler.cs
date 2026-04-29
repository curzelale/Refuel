using Refuel.Application.Fuels.Dtos;
using Refuel.Application.GasStations.Dtos;
using Refuel.Application.Mediator;
using Refuel.Domain.Repositories;

namespace Refuel.Application.GasStations.Queries.GetGasStationById;

public class GetGasStationByIdQueryHandler : IRequestHandler<GetGasStationByIdQuery, GasStationDto?>
{
    private readonly IGasStationRepository _repository;

    public GetGasStationByIdQueryHandler(IGasStationRepository repository)
    {
        _repository = repository;
    }

    public async Task<GasStationDto?> HandleAsync(GetGasStationByIdQuery request,
        CancellationToken cancellationToken = default)
    {
        var gasStation = await _repository.GetByIdWithFuelsAsync(request.Id);

        return gasStation is null
            ? null
            : new GasStationDto(
                gasStation.Id, gasStation.Name, gasStation.Address,
                gasStation.Latitude, gasStation.Longitude,
                gasStation.Fuels.Select(f => new FuelDto(f.Id, f.Name)));
    }
}
