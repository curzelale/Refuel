using Mediator;
using Microsoft.AspNetCore.Mvc;
using Refuel.Application.Refuels.Commands.CreateRefuel;
using Refuel.Application.Refuels.Dtos;
using Refuel.Application.Refuels.Queries.GetAllRefuels;
using Refuel.Application.Refuels.Queries.GetRefuelById;
using RefuelAPI.Authorization;
using RefuelAPI.Controllers.V1.Requests;
using RefuelAPI.Models;

namespace RefuelAPI.Controllers.V1;

//TODO: Alla creazione di un nuovo record controllare che i km non vandano indietro
//TODO: Migliorare la gestione dei codici http di risposta in caso di incompatibilità

/// <summary>
/// Record and retrieve refueling sessions. Each entry tracks the vehicle, gas station, fuel type,
/// quantity, price per liter, total cost, odometer reading, and optional notes.
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
[BearerAuthorize]
[Produces("application/json")]
public class RefuelsController : ControllerBase
{
    private readonly IMediator _mediator;

    public RefuelsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>Returns all refueling records.</summary>
    /// <response code="200">List of all refueling records.</response>
    /// <response code="401">Missing or invalid authentication token.</response>
    /// <response code="500">An unexpected server error occurred.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<RefuelDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllRefuelsQuery(), cancellationToken);
        return Ok(result);
    }

    /// <summary>Returns a single refueling record by its unique identifier.</summary>
    /// <param name="id">The refueling record's unique identifier.</param>
    /// <response code="200">The requested refueling record.</response>
    /// <response code="401">Missing or invalid authentication token.</response>
    /// <response code="404">No refueling record found with the given identifier.</response>
    /// <response code="500">An unexpected server error occurred.</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(RefuelDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetRefuelByIdQuery(id), cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>Records a new refueling session.</summary>
    /// <param name="request">Refueling session payload.</param>
    /// <response code="201">Refueling record created successfully. Returns the created resource.</response>
    /// <response code="400">Invalid request data (e.g. missing required fields, incompatible vehicle/fuel/station combination).</response>
    /// <response code="401">Missing or invalid authentication token.</response>
    /// <response code="500">An unexpected server error occurred.</response>
    [HttpPost]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(RefuelDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
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
