namespace Refuel.Application.Fuels.Dtos;

/// <summary>Represents a fuel type.</summary>
/// <param name="Id">Unique identifier of the fuel type.</param>
/// <param name="Name">Display name of the fuel type (e.g. "Gasoline 95", "Diesel", "LPG").</param>
public record FuelDto(Guid Id, string Name);
