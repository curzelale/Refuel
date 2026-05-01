using Mediator;

namespace Refuel.Application.GasStations.Commands.DeleteGasStation;

public record DeleteGasStationCommand(Guid Id) : IRequest<bool>;