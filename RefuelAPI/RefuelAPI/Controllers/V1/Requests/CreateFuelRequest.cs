using System.ComponentModel.DataAnnotations;

namespace RefuelAPI.Controllers.V1.Requests;

public record CreateFuelRequest([Required] string Name);
