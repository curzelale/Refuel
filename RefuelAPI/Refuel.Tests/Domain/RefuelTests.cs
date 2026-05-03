using Refuel.Domain.Exceptions;
using RefuelEntity = Refuel.Domain.Entities.Refuel;

namespace Refuel.Tests.Domain;

public class RefuelTests
{
    private static readonly Guid ValidVehicleId = Guid.NewGuid();
    private static readonly Guid ValidGasStationId = Guid.NewGuid();
    private static readonly Guid ValidFuelId = Guid.NewGuid();
    private static readonly DateTime ValidDate = DateTime.UtcNow.AddMinutes(-10);

    private static RefuelEntity Valid() =>
        new(ValidVehicleId, ValidGasStationId, ValidFuelId, 30, 50, ValidDate, 1000, null);

    [Fact]
    public void Constructor_ValidData_CreatesRefuel()
    {
        var refuel = Valid();

        Assert.NotEqual(Guid.Empty, refuel.Id);
        Assert.Equal(ValidVehicleId, refuel.VehicleId);
        Assert.Equal(ValidGasStationId, refuel.GasStationId);
        Assert.Equal(ValidFuelId, refuel.FuelId);
        Assert.Equal(30, refuel.Quantity);
        Assert.Equal(50, refuel.TotalPrice);
        Assert.Equal(ValidDate, refuel.Date);
        Assert.Equal(1000, refuel.OdometerKm);
        Assert.Null(refuel.Note);
    }

    [Fact]
    public void Constructor_EmptyVehicleId_ThrowsBusinessRuleException()
    {
        Assert.Throws<BusinessRuleException>(() =>
            new RefuelEntity(Guid.Empty, ValidGasStationId, ValidFuelId, 30, 50, ValidDate, 1000, null));
    }

    [Fact]
    public void Constructor_EmptyGasStationId_ThrowsBusinessRuleException()
    {
        Assert.Throws<BusinessRuleException>(() =>
            new RefuelEntity(ValidVehicleId, Guid.Empty, ValidFuelId, 30, 50, ValidDate, 1000, null));
    }

    [Fact]
    public void Constructor_EmptyFuelId_ThrowsBusinessRuleException()
    {
        Assert.Throws<BusinessRuleException>(() =>
            new RefuelEntity(ValidVehicleId, ValidGasStationId, Guid.Empty, 30, 50, ValidDate, 1000, null));
    }

    [Fact]
    public void Constructor_ZeroQuantity_ThrowsBusinessRuleException()
    {
        Assert.Throws<BusinessRuleException>(() =>
            new RefuelEntity(ValidVehicleId, ValidGasStationId, ValidFuelId, 0, 50, ValidDate, 1000, null));
    }

    [Fact]
    public void Constructor_NegativeQuantity_ThrowsBusinessRuleException()
    {
        Assert.Throws<BusinessRuleException>(() =>
            new RefuelEntity(ValidVehicleId, ValidGasStationId, ValidFuelId, -1, 50, ValidDate, 1000, null));
    }

    [Fact]
    public void Constructor_ZeroTotalPrice_ThrowsBusinessRuleException()
    {
        Assert.Throws<BusinessRuleException>(() =>
            new RefuelEntity(ValidVehicleId, ValidGasStationId, ValidFuelId, 30, 0, ValidDate, 1000, null));
    }

    [Fact]
    public void Constructor_NegativeTotalPrice_ThrowsBusinessRuleException()
    {
        Assert.Throws<BusinessRuleException>(() =>
            new RefuelEntity(ValidVehicleId, ValidGasStationId, ValidFuelId, 30, -1, ValidDate, 1000, null));
    }

    [Fact]
    public void Constructor_FutureDateBeyondTolerance_ThrowsBusinessRuleException()
    {
        var futureDate = DateTime.UtcNow.AddMinutes(10);

        Assert.Throws<BusinessRuleException>(() =>
            new RefuelEntity(ValidVehicleId, ValidGasStationId, ValidFuelId, 30, 50, futureDate, 1000, null));
    }

    [Fact]
    public void Constructor_DateWithinFiveMinuteTolerance_DoesNotThrow()
    {
        var nearFutureDate = DateTime.UtcNow.AddMinutes(3);

        var exception = Record.Exception(() =>
            new RefuelEntity(ValidVehicleId, ValidGasStationId, ValidFuelId, 30, 50, nearFutureDate, 1000, null));

        Assert.Null(exception);
    }

    [Fact]
    public void Constructor_NegativeOdometerKm_ThrowsBusinessRuleException()
    {
        Assert.Throws<BusinessRuleException>(() =>
            new RefuelEntity(ValidVehicleId, ValidGasStationId, ValidFuelId, 30, 50, ValidDate, -1, null));
    }

    [Fact]
    public void Constructor_ZeroOdometerKm_DoesNotThrow()
    {
        var exception = Record.Exception(() =>
            new RefuelEntity(ValidVehicleId, ValidGasStationId, ValidFuelId, 30, 50, ValidDate, 0, null));

        Assert.Null(exception);
    }

    [Fact]
    public void Constructor_NullNote_DoesNotThrow()
    {
        var refuel = new RefuelEntity(ValidVehicleId, ValidGasStationId, ValidFuelId, 30, 50, ValidDate, 1000, null);

        Assert.Null(refuel.Note);
    }

    [Fact]
    public void Constructor_WithNote_SetsNote()
    {
        var refuel = new RefuelEntity(ValidVehicleId, ValidGasStationId, ValidFuelId, 30, 50, ValidDate, 1000, "Full tank");

        Assert.Equal("Full tank", refuel.Note);
    }
}
