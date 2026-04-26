using Refuel.Application.Fuels.Dtos;
using Refuel.Application.Mediator;

namespace Refuel.Application.Fuels.Queries.GetFuelById;

public record GetFuelByIdQuery(Guid Id) : IRequest<FuelDto?>;
