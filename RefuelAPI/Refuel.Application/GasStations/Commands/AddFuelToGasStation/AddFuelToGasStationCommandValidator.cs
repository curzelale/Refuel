using FluentValidation;

namespace Refuel.Application.GasStations.Commands.AddFuelToGasStation;

public class AddFuelToGasStationCommandValidator : AbstractValidator<AddFuelToGasStationCommand>
{
    public AddFuelToGasStationCommandValidator()
    {
        RuleFor(x => x.GasStationId).NotEmpty().WithMessage("GasStationId is required.");
        RuleFor(x => x.FuelId).NotEmpty().WithMessage("FuelId is required.");
    }
}
