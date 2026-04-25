using FluentValidation;

namespace Refuel.Application.GasStations.Commands.DeleteGasStation;

public class DeleteGasStationCommandValidator : AbstractValidator<DeleteGasStationCommand>
{
    public DeleteGasStationCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required.");
    }
}