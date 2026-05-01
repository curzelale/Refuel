using Refuel.Application.Fuels.Dtos;
using Mediator;

namespace Refuel.Application.Fuels.Queries.GetAllFuels;

public record GetAllFuelsQuery : IRequest<IEnumerable<FuelDto>>;
