using Refuel.Domain.Entities;
using Refuel.Domain.Exceptions;

namespace Refuel.Tests.Domain;

public class GasStationTests
{
    [Fact]
    public void Constructor_ValidData_CreatesGasStation()
    {
        var station = new GasStation("Shell", "Via Roma 1", 45.0, 11.0);

        Assert.Equal("Shell", station.Name);
        Assert.Equal("Via Roma 1", station.Address);
        Assert.Equal(45.0, station.Latitude);
        Assert.Equal(11.0, station.Longitude);
        Assert.NotEqual(Guid.Empty, station.Id);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Constructor_BlankName_ThrowsBusinessRuleException(string? name)
    {
        Assert.Throws<BusinessRuleException>(() => new GasStation(name!, "Via Roma 1", 45.0, 11.0));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Constructor_BlankAddress_ThrowsBusinessRuleException(string? address)
    {
        Assert.Throws<BusinessRuleException>(() => new GasStation("Shell", address!, 45.0, 11.0));
    }

    [Theory]
    [InlineData(-91.0)]
    [InlineData(91.0)]
    public void Constructor_LatitudeOutOfRange_ThrowsBusinessRuleException(double latitude)
    {
        Assert.Throws<BusinessRuleException>(() => new GasStation("Shell", "Via Roma 1", latitude, 11.0));
    }

    [Theory]
    [InlineData(-181.0)]
    [InlineData(181.0)]
    public void Constructor_LongitudeOutOfRange_ThrowsBusinessRuleException(double longitude)
    {
        Assert.Throws<BusinessRuleException>(() => new GasStation("Shell", "Via Roma 1", 45.0, longitude));
    }

    [Fact]
    public void Update_ValidData_UpdatesProperties()
    {
        var station = new GasStation("Shell", "Via Roma 1", 45.0, 11.0);

        station.Update("Eni", "Corso Italia 5", -90.0, 180.0);

        Assert.Equal("Eni", station.Name);
        Assert.Equal("Corso Italia 5", station.Address);
        Assert.Equal(-90.0, station.Latitude);
        Assert.Equal(180.0, station.Longitude);
    }

    [Fact]
    public void Update_BlankName_ThrowsBusinessRuleException()
    {
        var station = new GasStation("Shell", "Via Roma 1", 45.0, 11.0);

        Assert.Throws<BusinessRuleException>(() => station.Update("", "Corso Italia 5", 45.0, 11.0));
    }

    [Fact]
    public void Update_LatitudeOutOfRange_ThrowsBusinessRuleException()
    {
        var station = new GasStation("Shell", "Via Roma 1", 45.0, 11.0);

        Assert.Throws<BusinessRuleException>(() => station.Update("Shell", "Via Roma 1", 91.0, 11.0));
    }
}
