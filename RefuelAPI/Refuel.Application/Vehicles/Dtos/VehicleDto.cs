using Refuel.Application.Fuels.Dtos;

namespace Refuel.Application.Vehicles.Dtos;

public record VehicleDto(
    Guid Id,
    string Brand,
    string Model,
    string Owner,
    IEnumerable<FuelDto> Fuels,
    string? Nickname,
    string? LicencesPlate
);