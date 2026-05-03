using System.ComponentModel.DataAnnotations;

namespace RefuelAPI.Controllers.V1.Requests;

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
