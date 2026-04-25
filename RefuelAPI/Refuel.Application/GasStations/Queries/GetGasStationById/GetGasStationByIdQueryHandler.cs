using Refuel.Application.GasStations.Dtos;
using Refuel.Application.Mediator;
using Refuel.Application.UnitOfWork;
using Refuel.Domain.Entities;

namespace Refuel.Application.GasStations.Queries.GetGasStationById;

public class GetGasStationByIdQueryHandler : IRequestHandler<GetGasStationByIdQuery, GasStationDto?>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetGasStationByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<GasStationDto?> HandleAsync(GetGasStationByIdQuery request,
        CancellationToken cancellationToken = default)
    {
        var gasStation = await _unitOfWork.Repository<GasStation>().GetByIdAsync(request.Id);

        return gasStation is null
            ? null
            : new GasStationDto(gasStation.Id, gasStation.Name, gasStation.Address, gasStation.Latitude,
                gasStation.Longitude);
    }
}