using NSubstitute;
using Refuel.Application.GasStations.Commands.UpdateGasStation;
using Refuel.Application.UnitOfWork;
using Refuel.Domain.Entities;
using Refuel.Domain.Repositories;

namespace Refuel.Tests.Application;

public class UpdateGasStationCommandHandlerTests
{
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly IRepository<GasStation> _repository = Substitute.For<IRepository<GasStation>>();

    public UpdateGasStationCommandHandlerTests()
    {
        _unitOfWork.Repository<GasStation>().Returns(_repository);
    }

    [Fact]
    public async Task HandleAsync_ExistingId_ReturnsUpdatedDto()
    {
        var id = Guid.NewGuid();
        var existing = new GasStation("Shell", "Via Roma 1", 45.0, 11.0);
        _repository.GetByIdAsync(id).Returns(existing);
        var command = new UpdateGasStationCommand(id, "Eni", "Corso Italia 5", 46.0, 12.0);
        var handler = new UpdateGasStationCommandHandler(_unitOfWork);

        var result = await handler.HandleAsync(command);

        Assert.Equal("Eni", result.Name);
        Assert.Equal("Corso Italia 5", result.Address);
    }

    [Fact]
    public async Task HandleAsync_ExistingId_CallsUpdateAndCommit()
    {
        var id = Guid.NewGuid();
        var existing = new GasStation("Shell", "Via Roma 1", 45.0, 11.0);
        _repository.GetByIdAsync(id).Returns(existing);
        var command = new UpdateGasStationCommand(id, "Eni", "Corso Italia 5", 46.0, 12.0);
        var handler = new UpdateGasStationCommandHandler(_unitOfWork);

        await handler.HandleAsync(command);

        _repository.Received(1).Update(existing);
        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task HandleAsync_UnknownId_ThrowsKeyNotFoundException()
    {
        var id = Guid.NewGuid();
        _repository.GetByIdAsync(id).Returns((GasStation?)null);
        var command = new UpdateGasStationCommand(id, "Eni", "Corso Italia 5", 46.0, 12.0);
        var handler = new UpdateGasStationCommandHandler(_unitOfWork);

        await Assert.ThrowsAsync<KeyNotFoundException>(() => handler.HandleAsync(command));
    }
}