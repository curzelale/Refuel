using Mediator;
using Refuel.Application.Refuels.Dtos;

namespace Refuel.Application.Refuels.Queries.GetRefuelById;

public record GetRefuelByIdQuery(Guid Id) : IRequest<RefuelDto?>;
