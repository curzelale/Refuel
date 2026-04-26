using FluentValidation;

namespace Refuel.Application.Fuels.Commands.DeleteFuel;

public class DeleteFuelCommandValidator : AbstractValidator<DeleteFuelCommand>
{
    public DeleteFuelCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required.");
    }
}
