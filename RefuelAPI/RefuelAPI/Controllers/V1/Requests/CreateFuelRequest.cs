using System.ComponentModel.DataAnnotations;

namespace RefuelAPI.Controllers.V1.Requests;

/// <summary>Request body for creating a new fuel type.</summary>
/// <param name="Name">Display name of the fuel type (e.g. "Gasoline 95", "Diesel", "LPG"). Required.</param>
public record CreateFuelRequest([Required] string Name);
