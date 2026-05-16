using System.ComponentModel.DataAnnotations;

namespace RefuelAPI.Controllers.V1.Requests;

/// <summary>Request body for recording a new refueling session.</summary>
/// <param name="VehicleId">ID of the vehicle that was refueled. Required.</param>
/// <param name="GasStationId">ID of the gas station where the refuel took place. Required.</param>
/// <param name="FuelId">ID of the fuel type used. Required.</param>
/// <param name="Quantity">Amount of fuel dispensed in liters. Must be greater than 0.</param>
/// <param name="TotalPrice">Total cost paid in the local currency. Must be greater than 0.</param>
/// <param name="Date">Date and time of the refueling session. Required.</param>
/// <param name="OdometerKm">Current odometer reading in kilometers at the time of refueling. Must be 0 or greater.</param>
/// <param name="Note">Optional free-text note about this refueling session.</param>
public record CreateRefuelRequest(
    [Required] Guid VehicleId,
    [Required] Guid GasStationId,
    [Required] Guid FuelId,
    [Range(0.001, double.MaxValue)] double Quantity,
    [Range(0.001, double.MaxValue)] double TotalPrice,
    [Required] DateTime Date,
    [Range(0, float.MaxValue)] float OdometerKm,
    string? Note
);
