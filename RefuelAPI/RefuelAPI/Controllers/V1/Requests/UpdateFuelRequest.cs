using System.ComponentModel.DataAnnotations;

namespace RefuelAPI.Controllers.V1.Requests;

/// <summary>Request body for updating an existing fuel type.</summary>
/// <param name="Name">New display name for the fuel type. Required.</param>
public record UpdateFuelRequest([Required] string Name);
