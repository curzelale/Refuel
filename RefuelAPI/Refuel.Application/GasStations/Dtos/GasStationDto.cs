using Refuel.Application.Fuels.Dtos;

namespace Refuel.Application.GasStations.Dtos;

/// <summary>Represents a gas station.</summary>
/// <param name="Id">Unique identifier of the gas station.</param>
/// <param name="Name">Name of the gas station (e.g. "Shell - Via Roma").</param>
/// <param name="Address">Street address of the gas station.</param>
/// <param name="Latitude">Geographic latitude of the station (-90 to 90).</param>
/// <param name="Longitude">Geographic longitude of the station (-180 to 180).</param>
/// <param name="Fuels">Fuel types currently offered at this station.</param>
public record GasStationDto(
    Guid Id,
    string Name,
    string Address,
    double Latitude,
    double Longitude,
    IEnumerable<FuelDto> Fuels
);
