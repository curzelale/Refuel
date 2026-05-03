using Mediator;
using Microsoft.AspNetCore.Mvc;
using Refuel.Application.Refuels.Commands.CreateRefuel;
using Refuel.Application.Refuels.Queries.GetAllRefuels;
using Refuel.Application.Refuels.Queries.GetRefuelById;
using RefuelAPI.Controllers.V1.Requests;

namespace RefuelAPI.Controllers.V1;

//TODO: Alla creazione di un nuovo record controllare che i km non vandano indietro

[ApiController]
[Route("api/v1/{controller}")]
public class RefuelsController : ControllerBase
{
    private readonly IMediator _mediator;

    public RefuelsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllRefuelsQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetRefuelByIdQuery(id), cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateRefuelRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateRefuelCommand(
            request.VehicleId,
            request.GasStationId,
            request.FuelId,
            request.Quantity,
            request.TotalPrice,
            request.Date,
            request.OdometerKm,
            request.Note);
        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }
}
