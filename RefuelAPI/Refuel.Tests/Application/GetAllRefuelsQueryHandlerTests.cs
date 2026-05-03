using NSubstitute;
using Refuel.Application.Refuels.Queries.GetAllRefuels;
using Refuel.Domain.Repositories;
using RefuelEntity = Refuel.Domain.Entities.Refuel;

namespace Refuel.Tests.Application;

public class GetAllRefuelsQueryHandlerTests
{
    private readonly IRefuelRepository _repository = Substitute.For<IRefuelRepository>();

    private GetAllRefuelsQueryHandler CreateHandler() => new(_repository);

    private static RefuelEntity CreateRefuel() =>
        new(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 30, 50, DateTime.UtcNow.AddMinutes(-10), 1000, null);

    [Fact]
    public async Task Handle_WithRefuels_ReturnsDtos()
    {
        _repository.GetAllAsync().Returns([CreateRefuel(), CreateRefuel()]);

        var result = await CreateHandler().Handle(new GetAllRefuelsQuery(), default);

        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task Handle_NoRefuels_ReturnsEmpty()
    {
        _repository.GetAllAsync().Returns([]);

        var result = await CreateHandler().Handle(new GetAllRefuelsQuery(), default);

        Assert.Empty(result);
    }

    [Fact]
    public async Task Handle_MapsAllFieldsCorrectly()
    {
        var date = DateTime.UtcNow.AddMinutes(-10);
        var refuel = new RefuelEntity(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 42.5, 75.0, date, 12345, "Note");
        _repository.GetAllAsync().Returns([refuel]);

        var result = (await CreateHandler().Handle(new GetAllRefuelsQuery(), default)).Single();

        Assert.Equal(refuel.Id, result.Id);
        Assert.Equal(refuel.VehicleId, result.VehicleId);
        Assert.Equal(42.5, result.Quantity);
        Assert.Equal(75.0, result.TotalPrice);
        Assert.Equal(date, result.Date);
        Assert.Equal(12345, result.OdometerKm);
        Assert.Equal("Note", result.Note);
    }
}
