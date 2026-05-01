using NSubstitute;
using Refuel.Application.UnitOfWork;
using Refuel.Application.Vehicles.Commands.CreateVehicle;
using Refuel.Domain.Entities;
using Refuel.Domain.Repositories;

namespace Refuel.Tests.Application;

public class CreateVehicleCommandHandlerTests
{
    private readonly IVehicleRepository _vehicleRepository = Substitute.For<IVehicleRepository>();
    private readonly IRepository<Fuel> _fuelRepository = Substitute.For<IRepository<Fuel>>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();

    private CreateVehicleCommandHandler CreateHandler() =>
        new(_vehicleRepository, _fuelRepository, _unitOfWork);

    [Fact]
    public async Task Handle_NoFuelIds_CreatesVehicleAndReturnsDto()
    {
        _fuelRepository.GetAllAsync().Returns([]);

        var result = await CreateHandler().Handle(new CreateVehicleCommand("Alfa Romeo", "Giulia", "Ale", []), default);

        Assert.Equal("Alfa Romeo", result.Brand);
        Assert.Equal("Giulia", result.Model);
        Assert.Equal("Ale", result.Owner);
        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Empty(result.Fuels);
    }

    [Fact]
    public async Task Handle_WithValidFuelIds_AddsFuelsToVehicle()
    {
        var diesel = new Fuel("Diesel");
        var petrol = new Fuel("Petrol");
        _fuelRepository.GetAllAsync().Returns([diesel, petrol]);

        var result = await CreateHandler().Handle(
            new CreateVehicleCommand("Alfa Romeo", "Giulia", "Ale", [diesel.Id, petrol.Id]), default);

        Assert.Equal(2, result.Fuels.Count());
    }

    [Fact]
    public async Task Handle_UnknownFuelId_ThrowsKeyNotFoundException()
    {
        _fuelRepository.GetAllAsync().Returns([]);

        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            CreateHandler().Handle(
                new CreateVehicleCommand("Alfa Romeo", "Giulia", "Ale", [Guid.NewGuid()]), default).AsTask());
    }

    [Fact]
    public async Task Handle_ValidCommand_CallsAddAsyncAndCommit()
    {
        _fuelRepository.GetAllAsync().Returns([]);

        await CreateHandler().Handle(new CreateVehicleCommand("Alfa Romeo", "Giulia", "Ale", []), default);

        await _vehicleRepository.Received(1).AddAsync(Arg.Any<Vehicle>());
        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WithFuelIds_FetchesFuelsInSingleQuery()
    {
        var diesel = new Fuel("Diesel");
        _fuelRepository.GetAllAsync().Returns([diesel]);

        await CreateHandler().Handle(
            new CreateVehicleCommand("Alfa Romeo", "Giulia", "Ale", [diesel.Id]), default);

        await _fuelRepository.Received(1).GetAllAsync();
    }
}
