using FluentValidation;
using Mediator;
using NSubstitute;
using Refuel.Application.GasStations.Commands.CreateGasStation;
using Refuel.Application.GasStations.Dtos;
using Refuel.Application.Pipeline;

namespace Refuel.Tests.Application;

public class ValidationPipelineBehaviorTests
{
    private readonly MessageHandlerDelegate<CreateGasStationCommand, GasStationDto> _next;
    private readonly GasStationDto _expectedDto = new(Guid.NewGuid(), "Shell", "Via Roma 1", 45.0, 11.0, []);

    public ValidationPipelineBehaviorTests()
    {
        _next = Substitute.For<MessageHandlerDelegate<CreateGasStationCommand, GasStationDto>>();
        _next.Invoke(Arg.Any<CreateGasStationCommand>(), Arg.Any<CancellationToken>())
             .Returns(new ValueTask<GasStationDto>(_expectedDto));
    }

    [Fact]
    public async Task Handle_NoValidators_CallsNext()
    {
        var behavior = new ValidationPipelineBehavior<CreateGasStationCommand, GasStationDto>([]);
        var command = new CreateGasStationCommand("Shell", "Via Roma 1", 45.0, 11.0);

        var result = await behavior.Handle(command, _next, CancellationToken.None);

        Assert.Equal(_expectedDto, result);
        await _next.Received(1).Invoke(command, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ValidCommand_CallsNext()
    {
        var validators = new IValidator<CreateGasStationCommand>[] { new CreateGasStationCommandValidator() };
        var behavior = new ValidationPipelineBehavior<CreateGasStationCommand, GasStationDto>(validators);
        var command = new CreateGasStationCommand("Shell", "Via Roma 1", 45.0, 11.0);

        var result = await behavior.Handle(command, _next, CancellationToken.None);

        Assert.Equal(_expectedDto, result);
        await _next.Received(1).Invoke(command, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_InvalidCommand_ThrowsValidationException_NextNotCalled()
    {
        var validators = new IValidator<CreateGasStationCommand>[] { new CreateGasStationCommandValidator() };
        var behavior = new ValidationPipelineBehavior<CreateGasStationCommand, GasStationDto>(validators);
        var invalid = new CreateGasStationCommand("", "Via Roma 1", 45.0, 11.0);

        await Assert.ThrowsAsync<Refuel.Application.Exceptions.ValidationException>(
            () => behavior.Handle(invalid, _next, CancellationToken.None).AsTask());

        await _next.DidNotReceive().Invoke(Arg.Any<CreateGasStationCommand>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ValidCommandWithValidator_ReturnsResult()
    {
        var validators = new IValidator<CreateGasStationCommand>[] { new CreateGasStationCommandValidator() };
        var behavior = new ValidationPipelineBehavior<CreateGasStationCommand, GasStationDto>(validators);
        var command = new CreateGasStationCommand("Shell", "Via Roma 1", 45.0, 11.0);

        var result = await behavior.Handle(command, _next, CancellationToken.None);

        Assert.Equal(_expectedDto, result);
    }
}
