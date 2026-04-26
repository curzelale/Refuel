using Refuel.Application.Mediator;

namespace Refuel.Application.Fuels.Commands.DeleteFuel;

public record DeleteFuelCommand(Guid Id) : IRequest<bool>;
