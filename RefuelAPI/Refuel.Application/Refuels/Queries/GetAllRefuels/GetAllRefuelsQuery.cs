using Mediator;
using Refuel.Application.Refuels.Dtos;

namespace Refuel.Application.Refuels.Queries.GetAllRefuels;

public record GetAllRefuelsQuery : IRequest<IEnumerable<RefuelDto>>;
