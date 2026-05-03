using NSubstitute;
using Refuel.Application.Refuels.Queries.GetRefuelById;
using Refuel.Domain.Repositories;
using RefuelEntity = Refuel.Domain.Entities.Refuel;

namespace Refuel.Tests.Application;

public class GetRefuelByIdQueryHandlerTests
{
    private readonly IRefuelRepository _repository = Substitute.For<IRefuelRepository>();

    private GetRefuelByIdQueryHandler CreateHandler() => new(_repository);

    private static RefuelEntity CreateRefuel() =>
        new(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 30, 50, DateTime.UtcNow.AddMinutes(-10), 1000, null);

    [Fact]
    public async Task Handle_ExistingId_ReturnsDto()
    {
        var refuel = CreateRefuel();
        _repository.GetByIdAsync(refuel.Id).Returns(refuel);

        var result = await CreateHandler().Handle(new GetRefuelByIdQuery(refuel.Id), default);

        Assert.NotNull(result);
        Assert.Equal(refuel.Id, result.Id);
    }

    [Fact]
    public async Task Handle_NonExistingId_ReturnsNull()
    {
        _repository.GetByIdAsync(Arg.Any<Guid>()).Returns((RefuelEntity?)null);

        var result = await CreateHandler().Handle(new GetRefuelByIdQuery(Guid.NewGuid()), default);

        Assert.Null(result);
    }

    [Fact]
    public async Task Handle_MapsAllScalarFieldsCorrectly()
    {
        var date = DateTime.UtcNow.AddMinutes(-10);
        var refuel = new RefuelEntity(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 42.5, 75.0, date, 12345, "Test note");
        _repository.GetByIdAsync(refuel.Id).Returns(refuel);

        var result = await CreateHandler().Handle(new GetRefuelByIdQuery(refuel.Id), default);

        Assert.NotNull(result);
        Assert.Equal(refuel.VehicleId, result.VehicleId);
        Assert.Equal(refuel.GasStationId, result.GasStationId);
        Assert.Equal(refuel.FuelId, result.FuelId);
        Assert.Equal(42.5, result.Quantity);
        Assert.Equal(75.0, result.TotalPrice);
        Assert.Equal(date, result.Date);
        Assert.Equal(12345, result.OdometerKm);
        Assert.Equal("Test note", result.Note);
    }
}
