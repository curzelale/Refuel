using Refuel.Application.Fuels.Dtos;
using Refuel.Application.Mediator;

namespace Refuel.Application.Fuels.Commands.CreateFuel;

public record CreateFuelCommand(string Name) : IRequest<FuelDto>;
