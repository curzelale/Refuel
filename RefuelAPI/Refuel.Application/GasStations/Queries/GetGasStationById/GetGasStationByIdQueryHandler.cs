using Refuel.Application.GasStations.Dtos;
using Refuel.Application.Mediator;
using Refuel.Domain.Entities;
using Refuel.Domain.Repositories;

namespace Refuel.Application.GasStations.Queries.GetGasStationById;

public class GetGasStationByIdQueryHandler : IRequestHandler<GetGasStationByIdQuery, GasStationDto?>
{
    private readonly IRepository<GasStation> _repository;

    public GetGasStationByIdQueryHandler(IRepository<GasStation> repository)
    {
        _repository = repository;
    }

    public async Task<GasStationDto?> HandleAsync(GetGasStationByIdQuery request,
        CancellationToken cancellationToken = default)
    {
        var gasStation = await _repository.GetByIdAsync(request.Id);

        return gasStation is null
            ? null
            : new GasStationDto(gasStation.Id, gasStation.Name, gasStation.Address, gasStation.Latitude,
                gasStation.Longitude);
    }
}