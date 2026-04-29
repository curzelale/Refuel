using FluentValidation;

namespace Refuel.Application.GasStations.Commands.RemoveFuelFromGasStation;

public class RemoveFuelFromGasStationCommandValidator : AbstractValidator<RemoveFuelFromGasStationCommand>
{
    public RemoveFuelFromGasStationCommandValidator()
    {
        RuleFor(x => x.GasStationId).NotEmpty().WithMessage("GasStationId is required.");
        RuleFor(x => x.FuelId).NotEmpty().WithMessage("FuelId is required.");
    }
}
