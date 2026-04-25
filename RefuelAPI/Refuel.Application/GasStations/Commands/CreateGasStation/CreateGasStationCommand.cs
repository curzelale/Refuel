using Refuel.Application.GasStations.Dtos;
using Refuel.Application.Mediator;

namespace Refuel.Application.GasStations.Commands.CreateGasStation;

public record CreateGasStationCommand(
    string Name,
    string Address,
    double Latitude,
    double Longitude
) : IRequest<GasStationDto>;