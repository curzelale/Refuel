using Refuel.Domain.Entities;
using Refuel.Domain.Exceptions;

namespace Refuel.Tests.Domain;

public class VehicleTests
{
    [Fact]
    public void Constructor_ValidData_CreatesVehicle()
    {
        var vehicle = new Vehicle("Alfa Romeo", "Giulia", "Ale", null, null);

        Assert.Equal("Alfa Romeo", vehicle.Brand);
        Assert.Equal("Giulia", vehicle.Model);
        Assert.Equal("Ale", vehicle.Owner);
        Assert.NotEqual(Guid.Empty, vehicle.Id);
        Assert.Empty(vehicle.Fuels);
    }

    [Fact]
    public void Constructor_WithNicknameAndPlate_SetsOptionalFields()
    {
        var vehicle = new Vehicle("Alfa Romeo", "Giulia", "Ale", "Rosso", "AB123CD");

        Assert.Equal("Rosso", vehicle.Nickname);
        Assert.Equal("AB123CD", vehicle.LicencesPlate);
    }

    [Fact]
    public void Constructor_NullOptionalFields_LeavesThemNull()
    {
        var vehicle = new Vehicle("Alfa Romeo", "Giulia", "Ale", null, null);

        Assert.Null(vehicle.Nickname);
        Assert.Null(vehicle.LicencesPlate);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Constructor_BlankBrand_ThrowsBusinessRuleException(string? brand)
    {
        Assert.Throws<BusinessRuleException>(() => new Vehicle(brand!, "Giulia", "Ale", null, null));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Constructor_BlankModel_ThrowsBusinessRuleException(string? model)
    {
        Assert.Throws<BusinessRuleException>(() => new Vehicle("Alfa Romeo", model!, "Ale", null, null));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Constructor_BlankOwner_ThrowsBusinessRuleException(string? owner)
    {
        Assert.Throws<BusinessRuleException>(() => new Vehicle("Alfa Romeo", "Giulia", owner!, null, null));
    }

    [Fact]
    public void AddFuel_NewFuel_AddsFuelToCollection()
    {
        var vehicle = new Vehicle("Alfa Romeo", "Giulia", "Ale", null, null);
        var fuel = new Fuel("Diesel");

        vehicle.AddFuel(fuel);

        Assert.Single(vehicle.Fuels);
        Assert.Contains(fuel, vehicle.Fuels);
    }

    [Fact]
    public void AddFuel_DuplicateFuel_DoesNotAddTwice()
    {
        var vehicle = new Vehicle("Alfa Romeo", "Giulia", "Ale", null, null);
        var fuel = new Fuel("Diesel");

        vehicle.AddFuel(fuel);
        vehicle.AddFuel(fuel);

        Assert.Single(vehicle.Fuels);
    }
}
