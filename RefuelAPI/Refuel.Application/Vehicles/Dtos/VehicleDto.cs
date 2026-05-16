using Refuel.Application.Fuels.Dtos;

namespace Refuel.Application.Vehicles.Dtos;

/// <summary>Represents a registered vehicle.</summary>
/// <param name="Id">Unique identifier of the vehicle.</param>
/// <param name="Brand">Vehicle manufacturer (e.g. "Toyota", "Ford").</param>
/// <param name="Model">Vehicle model name (e.g. "Corolla", "Focus").</param>
/// <param name="Owner">Name of the vehicle owner.</param>
/// <param name="Fuels">Fuel types the vehicle is compatible with.</param>
/// <param name="Nickname">Optional friendly name for the vehicle.</param>
/// <param name="LicencesPlate">Optional licence plate number.</param>
public record VehicleDto(
    Guid Id,
    string Brand,
    string Model,
    string Owner,
    IEnumerable<FuelDto> Fuels,
    string? Nickname,
    string? LicencesPlate
);
