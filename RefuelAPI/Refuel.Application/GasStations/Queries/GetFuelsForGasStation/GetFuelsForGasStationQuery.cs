using Refuel.Application.Fuels.Dtos;
using Refuel.Application.Mediator;

namespace Refuel.Application.GasStations.Queries.GetFuelsForGasStation;

public record GetFuelsForGasStationQuery(Guid GasStationId) : IRequest<IEnumerable<FuelDto>?>;
