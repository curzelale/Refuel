using Microsoft.AspNetCore.Mvc;
using Refuel.Application.GasStations.Commands.CreateGasStation;
using Refuel.Application.GasStations.Commands.DeleteGasStation;
using Refuel.Application.GasStations.Commands.UpdateGasStation;
using Refuel.Application.GasStations.Dtos;
using Refuel.Application.GasStations.Queries.GetAllGasStations;
using Refuel.Application.GasStations.Queries.GetGasStationById;
using Refuel.Application.Mediator;
using RefuelAPI.Controllers.V1.Requests;

namespace RefuelAPI.Controllers.V1;

//TODO: Gestire i carburanti
//TODO: Se l'indirizzo non viene compilato fare il geocoding in automatico
//TODO: Bloccare la cancellazione se ci sono rifornimenti collegati

[ApiController]
[Route("api/v1/{controller}")]
public class GasStationsController : ControllerBase
{
    private readonly IMediator _mediator;

    public GasStationsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result =
            await _mediator.SendAsync<IEnumerable<GasStationDto>>(new GetAllGasStationsQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.SendAsync<GasStationDto?>(new GetGasStationByIdQuery(id), cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateGasStationRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateGasStationCommand(request.Name, request.Address, request.Latitude, request.Longitude);
        var result = await _mediator.SendAsync<GasStationDto>(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateGasStationRequest request,
        CancellationToken cancellationToken)
    {
        var command =
            new UpdateGasStationCommand(id, request.Name, request.Address, request.Latitude, request.Longitude);
        var result = await _mediator.SendAsync<GasStationDto>(command, cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await _mediator.SendAsync<bool>(new DeleteGasStationCommand(id), cancellationToken);
        return NoContent();
    }
}