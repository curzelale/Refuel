using NSubstitute;
using Refuel.Application.Vehicles.Queries.GetVehicleById;
using Refuel.Domain.Entities;
using Refuel.Domain.Repositories;

namespace Refuel.Tests.Application;

public class GetVehicleByIdQueryHandlerTests
{
    private readonly IVehicleRepository _repository = Substitute.For<IVehicleRepository>();

    private GetVehicleByIdQueryHandler CreateHandler() => new(_repository);

    [Fact]
    public async Task Handle_ExistingId_ReturnsVehicleDto()
    {
        var vehicle = new Vehicle("Alfa Romeo", "Giulia", "Ale", null, null);
        _repository.GetByIdAsync(vehicle.Id).Returns(vehicle);

        var result = await CreateHandler().Handle(new GetVehicleByIdQuery(vehicle.Id), default);

        Assert.NotNull(result);
        Assert.Equal(vehicle.Id, result.Id);
        Assert.Equal("Alfa Romeo", result.Brand);
        Assert.Equal("Giulia", result.Model);
        Assert.Equal("Ale", result.Owner);
        Assert.Null(result.Nickname);
        Assert.Null(result.LicencesPlate);
    }

    [Fact]
    public async Task Handle_ExistingId_MapsNicknameAndPlate()
    {
        var vehicle = new Vehicle("Alfa Romeo", "Giulia", "Ale", "Rosso", "AB123CD");
        _repository.GetByIdAsync(vehicle.Id).Returns(vehicle);

        var result = await CreateHandler().Handle(new GetVehicleByIdQuery(vehicle.Id), default);

        Assert.Equal("Rosso", result!.Nickname);
        Assert.Equal("AB123CD", result.LicencesPlate);
    }

    [Fact]
    public async Task Handle_UnknownId_ReturnsNull()
    {
        _repository.GetByIdAsync(Arg.Any<Guid>()).Returns((Vehicle?)null);

        var result = await CreateHandler().Handle(new GetVehicleByIdQuery(Guid.NewGuid()), default);

        Assert.Null(result);
    }

    [Fact]
    public async Task Handle_VehicleWithFuels_MapsFuelsToDto()
    {
        var vehicle = new Vehicle("Alfa Romeo", "Giulia", "Ale", null, null);
        vehicle.AddFuel(new Fuel("Diesel"));
        vehicle.AddFuel(new Fuel("Petrol"));
        _repository.GetByIdAsync(vehicle.Id).Returns(vehicle);

        var result = await CreateHandler().Handle(new GetVehicleByIdQuery(vehicle.Id), default);

        Assert.Equal(2, result!.Fuels.Count());
    }
}
