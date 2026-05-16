using Mediator;
using Microsoft.AspNetCore.Mvc;
using Refuel.Application.Fuels.Commands.CreateFuel;
using Refuel.Application.Fuels.Commands.DeleteFuel;
using Refuel.Application.Fuels.Commands.UpdateFuel;
using Refuel.Application.Fuels.Dtos;
using Refuel.Application.Fuels.Queries.GetAllFuels;
using Refuel.Application.Fuels.Queries.GetFuelById;
using RefuelAPI.Authorization;
using RefuelAPI.Controllers.V1.Requests;
using RefuelAPI.Models;

namespace RefuelAPI.Controllers.V1;

//TODO: Bloccare la cancellazione se è collegato a qualcosa

/// <summary>
/// Manage fuel types available in the system (e.g. gasoline, diesel, LPG).
/// Creation, update, and deletion are restricted to admins.
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
[BearerAuthorize]
[Produces("application/json")]
public class FuelsController : ControllerBase
{
    private readonly IMediator _mediator;

    public FuelsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>Returns all fuel types registered in the system.</summary>
    /// <response code="200">List of all fuel types.</response>
    /// <response code="401">Missing or invalid authentication token.</response>
    /// <response code="500">An unexpected server error occurred.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<FuelDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllFuelsQuery(), cancellationToken);
        return Ok(result);
    }

    /// <summary>Returns a single fuel type by its unique identifier.</summary>
    /// <param name="id">The fuel type's unique identifier.</param>
    /// <response code="200">The requested fuel type.</response>
    /// <response code="401">Missing or invalid authentication token.</response>
    /// <response code="404">No fuel type found with the given identifier.</response>
    /// <response code="500">An unexpected server error occurred.</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(FuelDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetFuelByIdQuery(id), cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>Creates a new fuel type. Restricted to administrators.</summary>
    /// <param name="request">Fuel type creation payload.</param>
    /// <response code="201">Fuel type created successfully. Returns the created resource.</response>
    /// <response code="400">Invalid request data (e.g. missing required fields).</response>
    /// <response code="401">Missing or invalid authentication token.</response>
    /// <response code="403">Insufficient permissions — admin role required.</response>
    /// <response code="500">An unexpected server error occurred.</response>
    [HttpPost]
    [BearerAuthorize(Roles.Admin)]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(FuelDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create([FromBody] CreateFuelRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateFuelCommand(request.Name);
        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>Updates an existing fuel type. Restricted to administrators.</summary>
    /// <param name="id">The fuel type's unique identifier.</param>
    /// <param name="request">Updated fuel type data.</param>
    /// <response code="200">Fuel type updated successfully. Returns the updated resource.</response>
    /// <response code="400">Invalid request data.</response>
    /// <response code="401">Missing or invalid authentication token.</response>
    /// <response code="403">Insufficient permissions — admin role required.</response>
    /// <response code="404">No fuel type found with the given identifier.</response>
    /// <response code="500">An unexpected server error occurred.</response>
    [HttpPut("{id:guid}")]
    [BearerAuthorize(Roles.Admin)]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(FuelDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateFuelRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateFuelCommand(id, request.Name);
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    /// <summary>Deletes a fuel type. Restricted to administrators.</summary>
    /// <param name="id">The fuel type's unique identifier.</param>
    /// <response code="204">Fuel type deleted successfully.</response>
    /// <response code="401">Missing or invalid authentication token.</response>
    /// <response code="403">Insufficient permissions — admin role required.</response>
    /// <response code="404">No fuel type found with the given identifier.</response>
    /// <response code="500">An unexpected server error occurred.</response>
    [HttpDelete("{id:guid}")]
    [BearerAuthorize(Roles.Admin)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteFuelCommand(id), cancellationToken);
        return NoContent();
    }
}
