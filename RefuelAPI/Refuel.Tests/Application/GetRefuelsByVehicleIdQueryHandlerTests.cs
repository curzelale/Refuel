using NSubstitute;
using Refuel.Application.Refuels.Queries.GetRefuelsByVehicleId;
using Refuel.Domain.Repositories;
using RefuelEntity = Refuel.Domain.Entities.Refuel;

namespace Refuel.Tests.Application;

public class GetRefuelsByVehicleIdQueryHandlerTests
{
    private readonly IRefuelRepository _repository = Substitute.For<IRefuelRepository>();

    private GetRefuelsByVehicleIdQueryHandler CreateHandler() => new(_repository);

    private static RefuelEntity CreateRefuel(Guid vehicleId) =>
        new(vehicleId, Guid.NewGuid(), Guid.NewGuid(), 30, 50, DateTime.UtcNow.AddMinutes(-10), 1000, null);

    [Fact]
    public async Task Handle_WithRefuels_ReturnsDtos()
    {
        var vehicleId = Guid.NewGuid();
        _repository.GetByVehicleIdAsync(vehicleId).Returns([CreateRefuel(vehicleId), CreateRefuel(vehicleId)]);

        var result = await CreateHandler().Handle(new GetRefuelsByVehicleIdQuery(vehicleId), default);

        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task Handle_NoRefuels_ReturnsEmpty()
    {
        var vehicleId = Guid.NewGuid();
        _repository.GetByVehicleIdAsync(vehicleId).Returns([]);

        var result = await CreateHandler().Handle(new GetRefuelsByVehicleIdQuery(vehicleId), default);

        Assert.Empty(result);
    }

    [Fact]
    public async Task Handle_PassesVehicleIdToRepository()
    {
        var vehicleId = Guid.NewGuid();
        _repository.GetByVehicleIdAsync(vehicleId).Returns([]);

        await CreateHandler().Handle(new GetRefuelsByVehicleIdQuery(vehicleId), default);

        await _repository.Received(1).GetByVehicleIdAsync(vehicleId);
    }
}
