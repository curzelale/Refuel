using Refuel.Application.Fuels.Dtos;
using Mediator;

namespace Refuel.Application.Fuels.Commands.UpdateFuel;

public record UpdateFuelCommand(Guid Id, string Name) : IRequest<FuelDto>;
