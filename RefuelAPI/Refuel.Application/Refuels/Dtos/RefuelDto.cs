using Refuel.Application.Fuels.Dtos;
using Refuel.Application.GasStations.Dtos;
using Refuel.Application.Vehicles.Dtos;

namespace Refuel.Application.Refuels.Dtos;

/// <summary>Represents a recorded refueling session.</summary>
/// <param name="Id">Unique identifier of the refueling record.</param>
/// <param name="VehicleId">ID of the vehicle that was refueled.</param>
/// <param name="GasStationId">ID of the gas station where the refuel took place.</param>
/// <param name="FuelId">ID of the fuel type used.</param>
/// <param name="Quantity">Amount of fuel dispensed in liters.</param>
/// <param name="TotalPrice">Total cost paid in the local currency.</param>
/// <param name="Date">Date and time of the refueling session.</param>
/// <param name="OdometerKm">Odometer reading in kilometers at the time of refueling.</param>
/// <param name="Note">Optional free-text note about the refueling session.</param>
/// <param name="Vehicle">Full details of the refueled vehicle (eagerly loaded).</param>
/// <param name="GasStation">Full details of the gas station (eagerly loaded).</param>
/// <param name="Fuel">Full details of the fuel type used (eagerly loaded).</param>
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
