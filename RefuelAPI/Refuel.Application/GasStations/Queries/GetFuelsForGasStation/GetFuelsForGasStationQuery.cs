using Refuel.Application.Fuels.Dtos;
using Mediator;

namespace Refuel.Application.GasStations.Queries.GetFuelsForGasStation;

public record GetFuelsForGasStationQuery(Guid GasStationId) : IRequest<IEnumerable<FuelDto>?>;
