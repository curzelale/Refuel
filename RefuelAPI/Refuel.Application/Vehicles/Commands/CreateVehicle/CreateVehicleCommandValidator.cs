using FluentValidation;

namespace Refuel.Application.Vehicles.Commands.CreateVehicle;

public class CreateVehicleCommandValidator: AbstractValidator<CreateVehicleCommand>
{
    public CreateVehicleCommandValidator()
    {
        RuleFor(v => v.Model).NotEmpty().WithMessage("Model cannot be empty");
        RuleFor(v => v.Brand).NotEmpty().WithMessage("Brand cannot be empty");
        RuleFor(v => v.Owner).NotEmpty().WithMessage("Owner cannot be empty");
    }
    
}