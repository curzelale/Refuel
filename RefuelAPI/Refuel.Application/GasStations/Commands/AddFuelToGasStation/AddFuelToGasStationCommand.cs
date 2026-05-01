using Refuel.Application.GasStations.Dtos;
using Mediator;

namespace Refuel.Application.GasStations.Commands.AddFuelToGasStation;

public record AddFuelToGasStationCommand(Guid GasStationId, Guid FuelId) : IRequest<GasStationDto>;
