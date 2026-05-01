using System.ComponentModel.DataAnnotations;

namespace RefuelAPI.Controllers.V1.Requests;

public record CreateVehicleRequest(
    [Required] string Brand,
    [Required] string Model,
    [Required] string Owner,
    IEnumerable<Guid>? FuelIds,
    string? Nickname,
    string? LicencesPlate
);
