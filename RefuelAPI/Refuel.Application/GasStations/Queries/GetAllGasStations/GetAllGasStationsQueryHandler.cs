using Refuel.Application.GasStations.Dtos;
using Refuel.Application.Mediator;
using Refuel.Application.UnitOfWork;
using Refuel.Domain.Entities;

namespace Refuel.Application.GasStations.Queries.GetAllGasStations;

public class GetAllGasStationsQueryHandler : IRequestHandler<GetAllGasStationsQuery, IEnumerable<GasStationDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllGasStationsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<GasStationDto>> HandleAsync(GetAllGasStationsQuery request,
        CancellationToken cancellationToken = default)
    {
        var gasStations = await _unitOfWork.Repository<GasStation>().GetAllAsync();

        return gasStations.Select(gs => new GasStationDto(gs.Id, gs.Name, gs.Address, gs.Latitude, gs.Longitude));
    }
}