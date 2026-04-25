using NSubstitute;
using Refuel.Application.GasStations.Commands.DeleteGasStation;
using Refuel.Application.UnitOfWork;
using Refuel.Domain.Entities;
using Refuel.Domain.Repositories;

namespace Refuel.Tests.Application;

public class DeleteGasStationCommandHandlerTests
{
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly IRepository<GasStation> _repository = Substitute.For<IRepository<GasStation>>();

    public DeleteGasStationCommandHandlerTests()
    {
        _unitOfWork.Repository<GasStation>().Returns(_repository);
    }

    [Fact]
    public async Task HandleAsync_ExistingId_DeletesAndCommits()
    {
        var id = Guid.NewGuid();
        var existing = new GasStation("Shell", "Via Roma 1", 45.0, 11.0);
        _repository.GetByIdAsync(id).Returns(existing);
        var handler = new DeleteGasStationCommandHandler(_unitOfWork);

        var result = await handler.HandleAsync(new DeleteGasStationCommand(id));

        Assert.True(result);
        _repository.Received(1).Delete(existing);
        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task HandleAsync_UnknownId_ThrowsKeyNotFoundException()
    {
        var id = Guid.NewGuid();
        _repository.GetByIdAsync(id).Returns((GasStation?)null);
        var handler = new DeleteGasStationCommandHandler(_unitOfWork);

        await Assert.ThrowsAsync<KeyNotFoundException>(() => handler.HandleAsync(new DeleteGasStationCommand(id)));
    }
}