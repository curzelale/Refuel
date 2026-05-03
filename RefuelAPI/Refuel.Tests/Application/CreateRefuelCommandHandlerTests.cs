using NSubstitute;
using Refuel.Application.Refuels.Commands.CreateRefuel;
using Refuel.Application.UnitOfWork;
using Refuel.Domain.Entities;
using Refuel.Domain.Repositories;
using RefuelEntity = Refuel.Domain.Entities.Refuel;

namespace Refuel.Tests.Application;

public class CreateRefuelCommandHandlerTests
{
    private readonly IRefuelRepository _refuelRepository = Substitute.For<IRefuelRepository>();
    private readonly IVehicleRepository _vehicleRepository = Substitute.For<IVehicleRepository>();
    private readonly IGasStationRepository _gasStationRepository = Substitute.For<IGasStationRepository>();
    private readonly IRepository<Fuel> _fuelRepository = Substitute.For<IRepository<Fuel>>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();

    private CreateRefuelCommandHandler CreateHandler() =>
        new(_refuelRepository, _vehicleRepository, _gasStationRepository, _fuelRepository, _unitOfWork);

    private static (Vehicle vehicle, GasStation gasStation, Fuel fuel) CreateEntities()
    {
        var fuel = new Fuel("Diesel");
        var vehicle = new Vehicle("Alfa Romeo", "Giulia", "Ale", null, null);
        vehicle.AddFuel(fuel);
        var gasStation = new GasStation("Shell", "Via Roma 1", 45.0, 9.0);
        return (vehicle, gasStation, fuel);
    }

    private static CreateRefuelCommand BasicCommand(Guid vehicleId, Guid gasStationId, Guid fuelId) =>
        new(vehicleId, gasStationId, fuelId, 30, 50, DateTime.UtcNow.AddMinutes(-10), 1000, null);

    private void SetupMocks(Vehicle vehicle, GasStation gasStation, Fuel fuel)
    {
        _vehicleRepository.GetByIdAsync(vehicle.Id).Returns(vehicle);
        _gasStationRepository.GetByIdAsync(gasStation.Id).Returns(gasStation);
        _fuelRepository.GetByIdAsync(fuel.Id).Returns(fuel);
    }

    [Fact]
    public async Task Handle_ValidCommand_ReturnsDto()
    {
        var (vehicle, gasStation, fuel) = CreateEntities();
        SetupMocks(vehicle, gasStation, fuel);

        var result = await CreateHandler().Handle(BasicCommand(vehicle.Id, gasStation.Id, fuel.Id), default);

        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal(vehicle.Id, result.VehicleId);
        Assert.Equal(gasStation.Id, result.GasStationId);
        Assert.Equal(fuel.Id, result.FuelId);
        Assert.Equal(30, result.Quantity);
        Assert.Equal(50, result.TotalPrice);
        Assert.Equal(1000, result.OdometerKm);
    }

    [Fact]
    public async Task Handle_ValidCommand_CallsAddAsyncAndCommit()
    {
        var (vehicle, gasStation, fuel) = CreateEntities();
        SetupMocks(vehicle, gasStation, fuel);

        await CreateHandler().Handle(BasicCommand(vehicle.Id, gasStation.Id, fuel.Id), default);

        await _refuelRepository.Received(1).AddAsync(Arg.Any<RefuelEntity>());
        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_UnknownVehicleId_ThrowsKeyNotFoundException()
    {
        _vehicleRepository.GetByIdAsync(Arg.Any<Guid>()).Returns((Vehicle?)null);

        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            CreateHandler().Handle(BasicCommand(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()), default).AsTask());
    }

    [Fact]
    public async Task Handle_UnknownGasStationId_ThrowsKeyNotFoundException()
    {
        var (vehicle, _, fuel) = CreateEntities();
        _vehicleRepository.GetByIdAsync(vehicle.Id).Returns(vehicle);
        _gasStationRepository.GetByIdAsync(Arg.Any<Guid>()).Returns((GasStation?)null);

        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            CreateHandler().Handle(BasicCommand(vehicle.Id, Guid.NewGuid(), fuel.Id), default).AsTask());
    }

    [Fact]
    public async Task Handle_UnknownFuelId_ThrowsKeyNotFoundException()
    {
        var (vehicle, gasStation, _) = CreateEntities();
        _vehicleRepository.GetByIdAsync(vehicle.Id).Returns(vehicle);
        _gasStationRepository.GetByIdAsync(gasStation.Id).Returns(gasStation);
        _fuelRepository.GetByIdAsync(Arg.Any<Guid>()).Returns((Fuel?)null);

        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            CreateHandler().Handle(BasicCommand(vehicle.Id, gasStation.Id, Guid.NewGuid()), default).AsTask());
    }

    [Fact]
    public async Task Handle_FuelNotCompatibleWithVehicle_ThrowsKeyNotFoundException()
    {
        var incompatibleFuel = new Fuel("Petrol");
        var vehicle = new Vehicle("Alfa Romeo", "Giulia", "Ale", null, null);
        var gasStation = new GasStation("Shell", "Via Roma 1", 45.0, 9.0);
        _vehicleRepository.GetByIdAsync(vehicle.Id).Returns(vehicle);
        _gasStationRepository.GetByIdAsync(gasStation.Id).Returns(gasStation);
        _fuelRepository.GetByIdAsync(incompatibleFuel.Id).Returns(incompatibleFuel);

        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            CreateHandler().Handle(BasicCommand(vehicle.Id, gasStation.Id, incompatibleFuel.Id), default).AsTask());
    }

    [Fact]
    public async Task Handle_ValidCommand_NestedVehicleDtoMapped()
    {
        var (vehicle, gasStation, fuel) = CreateEntities();
        SetupMocks(vehicle, gasStation, fuel);

        var result = await CreateHandler().Handle(BasicCommand(vehicle.Id, gasStation.Id, fuel.Id), default);

        Assert.NotNull(result.Vehicle);
        Assert.Equal(vehicle.Id, result.Vehicle.Id);
        Assert.Equal("Alfa Romeo", result.Vehicle.Brand);
        Assert.Equal("Giulia", result.Vehicle.Model);
    }

    [Fact]
    public async Task Handle_ValidCommand_NestedFuelDtoMapped()
    {
        var (vehicle, gasStation, fuel) = CreateEntities();
        SetupMocks(vehicle, gasStation, fuel);

        var result = await CreateHandler().Handle(BasicCommand(vehicle.Id, gasStation.Id, fuel.Id), default);

        Assert.NotNull(result.Fuel);
        Assert.Equal(fuel.Id, result.Fuel.Id);
        Assert.Equal("Diesel", result.Fuel.Name);
    }

    [Fact]
    public async Task Handle_ValidCommand_NoteIsNull_DtoNoteIsNull()
    {
        var (vehicle, gasStation, fuel) = CreateEntities();
        SetupMocks(vehicle, gasStation, fuel);

        var result = await CreateHandler().Handle(BasicCommand(vehicle.Id, gasStation.Id, fuel.Id), default);

        Assert.Null(result.Note);
    }

    [Fact]
    public async Task Handle_ValidCommand_NoteSet_DtoNoteSet()
    {
        var (vehicle, gasStation, fuel) = CreateEntities();
        SetupMocks(vehicle, gasStation, fuel);
        var command = BasicCommand(vehicle.Id, gasStation.Id, fuel.Id) with { Note = "Full tank" };

        var result = await CreateHandler().Handle(command, default);

        Assert.Equal("Full tank", result.Note);
    }
}
