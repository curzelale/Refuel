using Refuel.Application.Fuels.Dtos;
using Refuel.Application.Mediator;

namespace Refuel.Application.Fuels.Queries.GetAllFuels;

public record GetAllFuelsQuery : IRequest<IEnumerable<FuelDto>>;
