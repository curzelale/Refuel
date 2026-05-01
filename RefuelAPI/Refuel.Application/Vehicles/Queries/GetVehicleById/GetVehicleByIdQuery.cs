using Mediator;
using Refuel.Application.Vehicles.Dtos;

namespace Refuel.Application.Vehicles.Queries.GetVehicleById;

public record GetVehicleByIdQuery(
    Guid Id
) : IRequest<VehicleDto?>;