using FluentValidation;

namespace Refuel.Application.Refuels.Commands.CreateRefuel;

public class CreateRefuelCommandValidator : AbstractValidator<CreateRefuelCommand>
{
    public CreateRefuelCommandValidator()
    {
        RuleFor(x => x.VehicleId).NotEmpty().WithMessage("VehicleId cannot be empty.");
        RuleFor(x => x.GasStationId).NotEmpty().WithMessage("GasStationId cannot be empty.");
        RuleFor(x => x.FuelId).NotEmpty().WithMessage("FuelId cannot be empty.");
        RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("Quantity must be greater than zero.");
        RuleFor(x => x.TotalPrice).GreaterThan(0).WithMessage("TotalPrice must be greater than zero.");
        RuleFor(x => x.OdometerKm).GreaterThanOrEqualTo(0).WithMessage("OdometerKm cannot be negative.");
    }
}
