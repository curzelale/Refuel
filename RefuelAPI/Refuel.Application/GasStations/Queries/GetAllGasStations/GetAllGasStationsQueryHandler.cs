using Refuel.Application.GasStations.Dtos;
using Refuel.Application.Mediator;
using Refuel.Domain.Entities;
using Refuel.Domain.Repositories;

namespace Refuel.Application.GasStations.Queries.GetAllGasStations;

public class GetAllGasStationsQueryHandler : IRequestHandler<GetAllGasStationsQuery, IEnumerable<GasStationDto>>
{
    private readonly IRepository<GasStation> _repository;

    public GetAllGasStationsQueryHandler(IRepository<GasStation> repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<GasStationDto>> HandleAsync(GetAllGasStationsQuery request,
        CancellationToken cancellationToken = default)
    {
        var gasStations = await _repository.GetAllAsync();

        return gasStations.Select(gs => new GasStationDto(gs.Id, gs.Name, gs.Address, gs.Latitude, gs.Longitude));
    }
}