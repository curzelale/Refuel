using FluentValidation;

namespace Refuel.Application.GasStations.Commands.CreateGasStation;

public class CreateGasStationCommandValidator : AbstractValidator<CreateGasStationCommand>
{
    public CreateGasStationCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.");
        RuleFor(x => x.Address).NotEmpty().WithMessage("Address is required.");
        RuleFor(x => x.Latitude).InclusiveBetween(-90, 90).WithMessage("Latitude must be between -90 and 90.");
        RuleFor(x => x.Longitude).InclusiveBetween(-180, 180).WithMessage("Longitude must be between -180 and 180.");
    }
}