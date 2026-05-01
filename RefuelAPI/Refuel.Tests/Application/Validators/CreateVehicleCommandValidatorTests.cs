using FluentValidation.TestHelper;
using Refuel.Application.Vehicles.Commands.CreateVehicle;

namespace Refuel.Tests.Application.Validators;

public class CreateVehicleCommandValidatorTests
{
    private readonly CreateVehicleCommandValidator _validator = new();

    [Fact]
    public void Validate_ValidCommand_NoErrors()
    {
        _validator.TestValidate(new CreateVehicleCommand("Alfa Romeo", "Giulia", "Ale", [], null, null))
            .ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Validate_BlankBrand_HasError(string? brand)
        => _validator.TestValidate(new CreateVehicleCommand(brand!, "Giulia", "Ale", [], null, null))
            .ShouldHaveValidationErrorFor(x => x.Brand);

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Validate_BlankModel_HasError(string? model)
        => _validator.TestValidate(new CreateVehicleCommand("Alfa Romeo", model!, "Ale", [], null, null))
            .ShouldHaveValidationErrorFor(x => x.Model);

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Validate_BlankOwner_HasError(string? owner)
        => _validator.TestValidate(new CreateVehicleCommand("Alfa Romeo", "Giulia", owner!, [], null, null))
            .ShouldHaveValidationErrorFor(x => x.Owner);
}
