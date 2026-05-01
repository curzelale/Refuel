using Refuel.Application.Fuels.Dtos;
using Mediator;

namespace Refuel.Application.Fuels.Commands.CreateFuel;

public record CreateFuelCommand(string Name) : IRequest<FuelDto>;
