using Microsoft.AspNetCore.Mvc;
using Refuel.Application.Fuels.Commands.CreateFuel;
using Refuel.Application.Fuels.Commands.DeleteFuel;
using Refuel.Application.Fuels.Commands.UpdateFuel;
using Refuel.Application.Fuels.Dtos;
using Refuel.Application.Fuels.Queries.GetAllFuels;
using Refuel.Application.Fuels.Queries.GetFuelById;
using Refuel.Application.Mediator;
using RefuelAPI.Controllers.V1.Requests;

namespace RefuelAPI.Controllers.V1;

//TODO: Bloccare la cancellazione se è collegato a qualcosa

[ApiController]
[Route("api/v1/{controller}")]
public class FuelsController : ControllerBase
{
    private readonly IMediator _mediator;

    public FuelsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await _mediator.SendAsync<IEnumerable<FuelDto>>(new GetAllFuelsQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.SendAsync<FuelDto?>(new GetFuelByIdQuery(id), cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateFuelRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateFuelCommand(request.Name);
        var result = await _mediator.SendAsync<FuelDto>(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateFuelRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateFuelCommand(id, request.Name);
        var result = await _mediator.SendAsync<FuelDto>(command, cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await _mediator.SendAsync<bool>(new DeleteFuelCommand(id), cancellationToken);
        return NoContent();
    }
}
