using NSubstitute;
using Refuel.Application.GasStations.Commands.CreateGasStation;
using Refuel.Application.UnitOfWork;
using Refuel.Domain.Entities;
using Refuel.Domain.Repositories;

namespace Refuel.Tests.Application;

public class CreateGasStationCommandHandlerTests
{
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly IRepository<GasStation> _repository = Substitute.For<IRepository<GasStation>>();

    public CreateGasStationCommandHandlerTests()
    {
        _unitOfWork.Repository<GasStation>().Returns(_repository);
    }

    [Fact]
    public async Task HandleAsync_ValidCommand_ReturnsDto()
    {
        var command = new CreateGasStationCommand("Shell", "Via Roma 1", 45.0, 11.0);
        var handler = new CreateGasStationCommandHandler(_unitOfWork);

        var result = await handler.HandleAsync(command);

        Assert.Equal("Shell", result.Name);
        Assert.Equal("Via Roma 1", result.Address);
        Assert.Equal(45.0, result.Latitude);
        Assert.Equal(11.0, result.Longitude);
        Assert.NotEqual(Guid.Empty, result.Id);
    }

    [Fact]
    public async Task HandleAsync_ValidCommand_CallsAddAndCommit()
    {
        var command = new CreateGasStationCommand("Shell", "Via Roma 1", 45.0, 11.0);
        var handler = new CreateGasStationCommandHandler(_unitOfWork);

        await handler.HandleAsync(command);

        await _repository.Received(1).AddAsync(Arg.Any<GasStation>());
        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
    }
}