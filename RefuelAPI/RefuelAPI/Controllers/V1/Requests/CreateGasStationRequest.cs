using System.ComponentModel.DataAnnotations;

namespace RefuelAPI.Controllers.V1.Requests;

/// <summary>Request body for registering a new gas station.</summary>
/// <param name="Name">Name of the gas station (e.g. "Shell - Via Roma"). Required.</param>
/// <param name="Address">Street address of the gas station. Required.</param>
/// <param name="Latitude">Geographic latitude of the station. Must be between -90 and 90.</param>
/// <param name="Longitude">Geographic longitude of the station. Must be between -180 and 180.</param>
public record CreateGasStationRequest(
    [Required] string Name,
    [Required] string Address,
    [Range(-90, 90)] double Latitude,
    [Range(-180, 180)] double Longitude
);
