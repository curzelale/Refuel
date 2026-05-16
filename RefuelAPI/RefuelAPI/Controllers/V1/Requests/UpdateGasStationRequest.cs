using System.ComponentModel.DataAnnotations;

namespace RefuelAPI.Controllers.V1.Requests;

/// <summary>Request body for updating an existing gas station.</summary>
/// <param name="Name">New name for the gas station. Required.</param>
/// <param name="Address">New street address. Required.</param>
/// <param name="Latitude">New geographic latitude. Must be between -90 and 90.</param>
/// <param name="Longitude">New geographic longitude. Must be between -180 and 180.</param>
public record UpdateGasStationRequest(
    [Required] string Name,
    [Required] string Address,
    [Range(-90, 90)] double Latitude,
    [Range(-180, 180)] double Longitude
);
