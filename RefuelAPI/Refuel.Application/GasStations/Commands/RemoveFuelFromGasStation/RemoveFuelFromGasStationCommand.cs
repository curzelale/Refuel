using Refuel.Application.GasStations.Dtos;
using Refuel.Application.Mediator;

namespace Refuel.Application.GasStations.Commands.RemoveFuelFromGasStation;

public record RemoveFuelFromGasStationCommand(Guid GasStationId, Guid FuelId) : IRequest<GasStationDto>;
