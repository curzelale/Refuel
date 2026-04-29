using Refuel.Application.Fuels.Dtos;
using Refuel.Application.Mediator;
using Refuel.Domain.Repositories;

namespace Refuel.Application.GasStations.Queries.GetFuelsForGasStation;

public class GetFuelsForGasStationQueryHandler : IRequestHandler<GetFuelsForGasStationQuery, IEnumerable<FuelDto>?>
{
    private readonly IGasStationRepository _repository;

    public GetFuelsForGasStationQueryHandler(IGasStationRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<FuelDto>?> HandleAsync(GetFuelsForGasStationQuery request,
        CancellationToken cancellationToken = default)
    {
        var gasStation = await _repository.GetByIdWithFuelsAsync(request.GasStationId);

        return gasStation is null
            ? null
            : gasStation.Fuels.Select(f => new FuelDto(f.Id, f.Name));
    }
}
