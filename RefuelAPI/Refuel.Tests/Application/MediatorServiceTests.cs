using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Refuel.Application.GasStations.Commands.CreateGasStation;
using Refuel.Application.GasStations.Dtos;
using Refuel.Application.GasStations.Queries.GetGasStationById;
using Refuel.Application.Mediator;

namespace Refuel.Tests.Application;

public class MediatorServiceTests
{
    [Fact]
    public async Task SendAsync_RegisteredHandler_InvokesHandlerAndReturnsResult()
    {
        var expectedDto = new GasStationDto(Guid.NewGuid(), "Shell", "Via Roma 1", 45.0, 11.0);
        var handler = Substitute.For<IRequestHandler<GetGasStationByIdQuery, GasStationDto?>>();
        handler.HandleAsync(Arg.Any<GetGasStationByIdQuery>(), Arg.Any<CancellationToken>())
            .Returns(expectedDto);

        var services = new ServiceCollection();
        services.AddSingleton<IRequestHandler<GetGasStationByIdQuery, GasStationDto?>>(handler);
        var provider = services.BuildServiceProvider();

        var mediator = new MediatorService(provider);
        var query = new GetGasStationByIdQuery(expectedDto.Id);

        var result = await mediator.SendAsync<GasStationDto?>(query);

        Assert.Equal(expectedDto, result);
        await handler.Received(1).HandleAsync(query, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task SendAsync_UnregisteredHandler_ThrowsInvalidOperationException()
    {
        var provider = new ServiceCollection().BuildServiceProvider();
        var mediator = new MediatorService(provider);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            mediator.SendAsync<GasStationDto?>(new GetGasStationByIdQuery(Guid.NewGuid())));
    }

    [Fact]
    public async Task SendAsync_InvalidCommand_ThrowsValidationException_HandlerNotInvoked()
    {
        var handler = Substitute.For<IRequestHandler<CreateGasStationCommand, GasStationDto>>();
        var services = new ServiceCollection();
        services.AddSingleton(handler);
        services.AddScoped<IValidator<CreateGasStationCommand>, CreateGasStationCommandValidator>();
        var mediator = new MediatorService(services.BuildServiceProvider());

        await Assert.ThrowsAsync<Refuel.Application.Exceptions.ValidationException>(() =>
            mediator.SendAsync<GasStationDto>(new CreateGasStationCommand("", "Via Roma 1", 45.0, 11.0)));

        await handler.DidNotReceive().HandleAsync(Arg.Any<CreateGasStationCommand>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task SendAsync_ValidCommandWithValidator_InvokesHandler()
    {
        var dto = new GasStationDto(Guid.NewGuid(), "Shell", "Via Roma 1", 45.0, 11.0);
        var handler = Substitute.For<IRequestHandler<CreateGasStationCommand, GasStationDto>>();
        handler.HandleAsync(Arg.Any<CreateGasStationCommand>(), Arg.Any<CancellationToken>()).Returns(dto);
        var services = new ServiceCollection();
        services.AddSingleton(handler);
        services.AddScoped<IValidator<CreateGasStationCommand>, CreateGasStationCommandValidator>();
        var mediator = new MediatorService(services.BuildServiceProvider());

        var result =
            await mediator.SendAsync<GasStationDto>(new CreateGasStationCommand("Shell", "Via Roma 1", 45.0, 11.0));

        Assert.Equal(dto, result);
    }
}