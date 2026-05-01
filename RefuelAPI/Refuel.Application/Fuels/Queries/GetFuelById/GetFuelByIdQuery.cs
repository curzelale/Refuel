using Refuel.Application.Fuels.Dtos;
using Mediator;

namespace Refuel.Application.Fuels.Queries.GetFuelById;

public record GetFuelByIdQuery(Guid Id) : IRequest<FuelDto?>;
