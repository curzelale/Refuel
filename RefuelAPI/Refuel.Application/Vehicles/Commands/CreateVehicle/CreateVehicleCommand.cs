using Mediator;
using Refuel.Application.Vehicles.Dtos;

namespace Refuel.Application.Vehicles.Commands.CreateVehicle;

public record CreateVehicleCommand(
    string Brand,
    string Model,
    string Owner,
    IEnumerable<Guid> FuelIds,
    string? Nickname,
    string? LicencesPlate
) : IRequest<VehicleDto>;