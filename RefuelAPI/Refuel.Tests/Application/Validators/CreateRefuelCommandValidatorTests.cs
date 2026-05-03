using FluentValidation.TestHelper;
using Refuel.Application.Refuels.Commands.CreateRefuel;

namespace Refuel.Tests.Application.Validators;

public class CreateRefuelCommandValidatorTests
{
    private readonly CreateRefuelCommandValidator _validator = new();

    private static CreateRefuelCommand ValidCommand() =>
        new(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 30, 50, DateTime.UtcNow.AddMinutes(-10), 1000, null);

    [Fact]
    public void Validate_ValidCommand_NoErrors()
    {
        _validator.TestValidate(ValidCommand())
            .ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_EmptyVehicleId_HasError()
    {
        _validator.TestValidate(ValidCommand() with { VehicleId = Guid.Empty })
            .ShouldHaveValidationErrorFor(x => x.VehicleId);
    }

    [Fact]
    public void Validate_EmptyGasStationId_HasError()
    {
        _validator.TestValidate(ValidCommand() with { GasStationId = Guid.Empty })
            .ShouldHaveValidationErrorFor(x => x.GasStationId);
    }

    [Fact]
    public void Validate_EmptyFuelId_HasError()
    {
        _validator.TestValidate(ValidCommand() with { FuelId = Guid.Empty })
            .ShouldHaveValidationErrorFor(x => x.FuelId);
    }

    [Fact]
    public void Validate_ZeroQuantity_HasError()
    {
        _validator.TestValidate(ValidCommand() with { Quantity = 0 })
            .ShouldHaveValidationErrorFor(x => x.Quantity);
    }

    [Fact]
    public void Validate_NegativeQuantity_HasError()
    {
        _validator.TestValidate(ValidCommand() with { Quantity = -1 })
            .ShouldHaveValidationErrorFor(x => x.Quantity);
    }

    [Fact]
    public void Validate_ZeroTotalPrice_HasError()
    {
        _validator.TestValidate(ValidCommand() with { TotalPrice = 0 })
            .ShouldHaveValidationErrorFor(x => x.TotalPrice);
    }

    [Fact]
    public void Validate_NegativeTotalPrice_HasError()
    {
        _validator.TestValidate(ValidCommand() with { TotalPrice = -1 })
            .ShouldHaveValidationErrorFor(x => x.TotalPrice);
    }

    [Fact]
    public void Validate_NegativeOdometerKm_HasError()
    {
        _validator.TestValidate(ValidCommand() with { OdometerKm = -1 })
            .ShouldHaveValidationErrorFor(x => x.OdometerKm);
    }

    [Fact]
    public void Validate_ZeroOdometerKm_NoError()
    {
        _validator.TestValidate(ValidCommand() with { OdometerKm = 0 })
            .ShouldNotHaveValidationErrorFor(x => x.OdometerKm);
    }
}
