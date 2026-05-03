using Refuel.Application.Fuels.Dtos;
using Refuel.Application.GasStations.Dtos;
using Refuel.Application.Vehicles.Dtos;

namespace Refuel.Application.Refuels.Dtos;

public record RefuelDto(
    Guid Id,
    Guid VehicleId,
    Guid GasStationId,
    Guid FuelId,
    double Quantity,
    double TotalPrice,
    DateTime Date,
    float OdometerKm,
    string? Note,
    VehicleDto? Vehicle,
    GasStationDto? GasStation,
    FuelDto? Fuel
);
