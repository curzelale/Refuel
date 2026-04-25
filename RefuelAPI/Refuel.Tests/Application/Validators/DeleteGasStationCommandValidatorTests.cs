using FluentValidation.TestHelper;
using Refuel.Application.GasStations.Commands.DeleteGasStation;

namespace Refuel.Tests.Application.Validators;

public class DeleteGasStationCommandValidatorTests
{
    private readonly DeleteGasStationCommandValidator _validator = new();

    [Fact]
    public void Validate_ValidCommand_NoErrors()
        => _validator.TestValidate(new DeleteGasStationCommand(Guid.NewGuid()))
            .ShouldNotHaveAnyValidationErrors();

    [Fact]
    public void Validate_EmptyId_HasError()
        => _validator.TestValidate(new DeleteGasStationCommand(Guid.Empty))
            .ShouldHaveValidationErrorFor(x => x.Id);
}