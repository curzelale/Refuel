using Refuel.Application.GasStations.Dtos;
using Refuel.Application.Mediator;

namespace Refuel.Application.GasStations.Commands.UpdateGasStation;

public record UpdateGasStationCommand(
    Guid Id,
    string Name,
    string Address,
    double Latitude,
    double Longitude
) : IRequest<GasStationDto>;