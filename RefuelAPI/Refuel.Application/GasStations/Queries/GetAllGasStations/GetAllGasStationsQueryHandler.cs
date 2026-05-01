using Refuel.Application.Fuels.Dtos;
using Refuel.Application.GasStations.Dtos;
using Mediator;
using Refuel.Domain.Repositories;

namespace Refuel.Application.GasStations.Queries.GetAllGasStations;

public class GetAllGasStationsQueryHandler : IRequestHandler<GetAllGasStationsQuery, IEnumerable<GasStationDto>>
{
    private readonly IGasStationRepository _repository;

    public GetAllGasStationsQueryHandler(IGasStationRepository repository)
    {
        _repository = repository;
    }

    public async ValueTask<IEnumerable<GasStationDto>> Handle(GetAllGasStationsQuery request,
        CancellationToken cancellationToken = default)
    {
        var gasStations = await _repository.GetAllWithFuelsAsync();

        return gasStations.Select(gs => new GasStationDto(
            gs.Id, gs.Name, gs.Address, gs.Latitude, gs.Longitude,
            gs.Fuels.Select(f => new FuelDto(f.Id, f.Name))));
    }
}
