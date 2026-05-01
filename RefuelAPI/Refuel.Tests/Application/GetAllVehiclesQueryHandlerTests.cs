using NSubstitute;
using Refuel.Application.Vehicles.Queries.GetAllVehicles;
using Refuel.Domain.Entities;
using Refuel.Domain.Repositories;

namespace Refuel.Tests.Application;

public class GetAllVehiclesQueryHandlerTests
{
    private readonly IVehicleRepository _repository = Substitute.For<IVehicleRepository>();

    private GetAllVehiclesQueryHandler CreateHandler() => new(_repository);

    [Fact]
    public async Task Handle_WithVehicles_ReturnsDtos()
    {
        var vehicles = new[]
        {
            new Vehicle("Alfa Romeo", "Giulia", "Ale"),
            new Vehicle("BMW", "M3", "Bob")
        };
        _repository.GetAllAsync().Returns(vehicles);

        var result = await CreateHandler().Handle(new GetAllVehiclesQuery(), default);

        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task Handle_NoVehicles_ReturnsEmpty()
    {
        _repository.GetAllAsync().Returns([]);

        var result = await CreateHandler().Handle(new GetAllVehiclesQuery(), default);

        Assert.Empty(result);
    }

    [Fact]
    public async Task Handle_MapsAllFieldsCorrectly()
    {
        var fuel = new Fuel("Diesel");
        var vehicle = new Vehicle("Alfa Romeo", "Giulia", "Ale");
        vehicle.AddFuel(fuel);
        _repository.GetAllAsync().Returns([vehicle]);

        var result = (await CreateHandler().Handle(new GetAllVehiclesQuery(), default)).Single();

        Assert.Equal(vehicle.Id, result.Id);
        Assert.Equal("Alfa Romeo", result.Brand);
        Assert.Equal("Giulia", result.Model);
        Assert.Equal("Ale", result.Owner);
        Assert.Single(result.Fuels);
        Assert.Equal(fuel.Name, result.Fuels.First().Name);
    }
}
