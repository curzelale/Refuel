using Mediator;
using Refuel.Application.Vehicles.Dtos;

namespace Refuel.Application.Vehicles.Queries.GetAllVehicles;

public record GetAllVehiclesQuery : IRequest<IEnumerable<VehicleDto>>;
