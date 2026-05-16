using System.ComponentModel.DataAnnotations;

namespace RefuelAPI.Controllers.V1.Requests;

/// <summary>Request body for registering a new vehicle.</summary>
/// <param name="Brand">Vehicle manufacturer (e.g. "Toyota", "Ford"). Required.</param>
/// <param name="Model">Vehicle model name (e.g. "Corolla", "Focus"). Required.</param>
/// <param name="Owner">Name of the vehicle owner. Required.</param>
/// <param name="FuelIds">Optional list of fuel type IDs the vehicle is compatible with.</param>
/// <param name="Nickname">Optional friendly name for the vehicle (e.g. "My Daily Driver").</param>
/// <param name="LicencesPlate">Optional licence plate number.</param>
public record CreateVehicleRequest(
    [Required] string Brand,
    [Required] string Model,
    [Required] string Owner,
    IEnumerable<Guid>? FuelIds,
    string? Nickname,
    string? LicencesPlate
);
