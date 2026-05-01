using Refuel.Application.GasStations.Dtos;
using Mediator;

namespace Refuel.Application.GasStations.Commands.RemoveFuelFromGasStation;

public record RemoveFuelFromGasStationCommand(Guid GasStationId, Guid FuelId) : IRequest<GasStationDto>;
