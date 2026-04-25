namespace Refuel.Application.GasStations.Dtos;

public record GasStationDto(
    Guid Id,
    string Name,
    string Address,
    double Latitude,
    double Longitude
);