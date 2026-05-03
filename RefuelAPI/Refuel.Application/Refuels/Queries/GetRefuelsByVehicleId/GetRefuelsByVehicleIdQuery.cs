using Mediator;
using Refuel.Application.Refuels.Dtos;

namespace Refuel.Application.Refuels.Queries.GetRefuelsByVehicleId;

public record GetRefuelsByVehicleIdQuery(Guid VehicleId) : IRequest<IEnumerable<RefuelDto>>;
