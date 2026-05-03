using Mediator;
using Refuel.Application.Refuels.Dtos;

namespace Refuel.Application.Refuels.Commands.CreateRefuel;

public record CreateRefuelCommand(
    Guid VehicleId,
    Guid GasStationId,
    Guid FuelId,
    double Quantity,
    double TotalPrice,
    DateTime Date,
    float OdometerKm,
    string? Note
) : IRequest<RefuelDto>;
