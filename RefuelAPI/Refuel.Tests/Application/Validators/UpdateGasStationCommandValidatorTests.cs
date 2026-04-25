using FluentValidation.TestHelper;
using Refuel.Application.GasStations.Commands.UpdateGasStation;

namespace Refuel.Tests.Application.Validators;

public class UpdateGasStationCommandValidatorTests
{
    private readonly UpdateGasStationCommandValidator _validator = new();

    [Fact]
    public void Validate_ValidCommand_NoErrors()
    {
        _validator.TestValidate(new UpdateGasStationCommand(Guid.NewGuid(), "Shell", "Via Roma 1", 45.0, 11.0))
            .ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_EmptyId_HasError()
        => _validator.TestValidate(new UpdateGasStationCommand(Guid.Empty, "Shell", "Via Roma 1", 45.0, 11.0))
            .ShouldHaveValidationErrorFor(x => x.Id);

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Validate_BlankName_HasError(string? name)
        => _validator.TestValidate(new UpdateGasStationCommand(Guid.NewGuid(), name!, "Via Roma 1", 45.0, 11.0))
            .ShouldHaveValidationErrorFor(x => x.Name);

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Validate_BlankAddress_HasError(string? address)
        => _validator.TestValidate(new UpdateGasStationCommand(Guid.NewGuid(), "Shell", address!, 45.0, 11.0))
            .ShouldHaveValidationErrorFor(x => x.Address);

    [Theory]
    [InlineData(-91.0)]
    [InlineData(91.0)]
    public void Validate_LatitudeOutOfRange_HasError(double lat)
        => _validator.TestValidate(new UpdateGasStationCommand(Guid.NewGuid(), "Shell", "Via Roma 1", lat, 11.0))
            .ShouldHaveValidationErrorFor(x => x.Latitude);

    [Theory]
    [InlineData(-181.0)]
    [InlineData(181.0)]
    public void Validate_LongitudeOutOfRange_HasError(double lon)
        => _validator.TestValidate(new UpdateGasStationCommand(Guid.NewGuid(), "Shell", "Via Roma 1", 45.0, lon))
            .ShouldHaveValidationErrorFor(x => x.Longitude);
}