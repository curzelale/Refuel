using System.ComponentModel.DataAnnotations;

namespace RefuelAPI.Controllers.V1.Requests;

public record CreateGasStationRequest(
    [Required] string Name,
    [Required] string Address,
    [Range(-90, 90)] double Latitude,
    [Range(-180, 180)] double Longitude
);