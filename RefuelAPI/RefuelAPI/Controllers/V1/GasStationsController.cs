using Mediator;
using Microsoft.AspNetCore.Mvc;
using Refuel.Application.Fuels.Dtos;
using Refuel.Application.GasStations.Commands.AddFuelToGasStation;
using Refuel.Application.GasStations.Commands.CreateGasStation;
using Refuel.Application.GasStations.Commands.DeleteGasStation;
using Refuel.Application.GasStations.Commands.RemoveFuelFromGasStation;
using Refuel.Application.GasStations.Commands.UpdateGasStation;
using Refuel.Application.GasStations.Dtos;
using Refuel.Application.GasStations.Queries.GetAllGasStations;
using Refuel.Application.GasStations.Queries.GetFuelsForGasStation;
using Refuel.Application.GasStations.Queries.GetGasStationById;
using RefuelAPI.Authorization;
using RefuelAPI.Controllers.V1.Requests;
using RefuelAPI.Models;

namespace RefuelAPI.Controllers.V1;

//TODO: Se l'indirizzo non viene compilato fare il geocoding in automatico
//TODO: Bloccare la cancellazione se ci sono rifornimenti collegati

/// <summary>
/// Manage gas stations and the fuel types they offer. Admins can create, update, and delete stations
/// and associate them with specific fuels.
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
[BearerAuthorize]
[Produces("application/json")]
public class GasStationsController : ControllerBase
{
    private readonly IMediator _mediator;

    public GasStationsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>Returns all registered gas stations.</summary>
    /// <response code="200">List of all gas stations.</response>
    /// <response code="401">Missing or invalid authentication token.</response>
    /// <response code="500">An unexpected server error occurred.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<GasStationDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllGasStationsQuery(), cancellationToken);
        return Ok(result);
    }

    /// <summary>Returns a single gas station by its unique identifier.</summary>
    /// <param name="id">The gas station's unique identifier.</param>
    /// <response code="200">The requested gas station.</response>
    /// <response code="401">Missing or invalid authentication token.</response>
    /// <response code="404">No gas station found with the given identifier.</response>
    /// <response code="500">An unexpected server error occurred.</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(GasStationDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetGasStationByIdQuery(id), cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>Registers a new gas station. Restricted to administrators.</summary>
    /// <param name="request">Gas station creation payload.</param>
    /// <response code="201">Gas station created successfully. Returns the created resource.</response>
    /// <response code="400">Invalid request data (e.g. missing required fields or invalid coordinates).</response>
    /// <response code="401">Missing or invalid authentication token.</response>
    /// <response code="403">Insufficient permissions — admin role required.</response>
    /// <response code="500">An unexpected server error occurred.</response>
    [HttpPost]
    [BearerAuthorize(Roles.Admin)]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(GasStationDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create([FromBody] CreateGasStationRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateGasStationCommand(request.Name, request.Address, request.Latitude, request.Longitude);
        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>Updates a gas station's details. Restricted to administrators.</summary>
    /// <param name="id">The gas station's unique identifier.</param>
    /// <param name="request">Updated gas station data.</param>
    /// <response code="200">Gas station updated successfully. Returns the updated resource.</response>
    /// <response code="400">Invalid request data (e.g. invalid coordinates).</response>
    /// <response code="401">Missing or invalid authentication token.</response>
    /// <response code="403">Insufficient permissions — admin role required.</response>
    /// <response code="404">No gas station found with the given identifier.</response>
    /// <response code="500">An unexpected server error occurred.</response>
    [HttpPut("{id:guid}")]
    [BearerAuthorize(Roles.Admin)]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(GasStationDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateGasStationRequest request,
        CancellationToken cancellationToken)
    {
        var command =
            new UpdateGasStationCommand(id, request.Name, request.Address, request.Latitude, request.Longitude);
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    /// <summary>Deletes a gas station. Restricted to administrators.</summary>
    /// <param name="id">The gas station's unique identifier.</param>
    /// <response code="204">Gas station deleted successfully.</response>
    /// <response code="401">Missing or invalid authentication token.</response>
    /// <response code="403">Insufficient permissions — admin role required.</response>
    /// <response code="404">No gas station found with the given identifier.</response>
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
        await _mediator.Send(new DeleteGasStationCommand(id), cancellationToken);
        return NoContent();
    }

    /// <summary>Returns the list of fuel types available at a specific gas station.</summary>
    /// <param name="id">The gas station's unique identifier.</param>
    /// <response code="200">List of fuel types offered at the station.</response>
    /// <response code="401">Missing or invalid authentication token.</response>
    /// <response code="404">No gas station found with the given identifier.</response>
    /// <response code="500">An unexpected server error occurred.</response>
    [HttpGet("{id:guid}/fuels")]
    [ProducesResponseType(typeof(IEnumerable<FuelDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetFuels(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetFuelsForGasStationQuery(id), cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>Associates a fuel type with a gas station. Restricted to administrators.</summary>
    /// <param name="id">The gas station's unique identifier.</param>
    /// <param name="fuelId">The fuel type's unique identifier.</param>
    /// <response code="200">Fuel type added successfully. Returns the updated gas station.</response>
    /// <response code="400">Business rule violation (e.g. fuel already associated with this station).</response>
    /// <response code="401">Missing or invalid authentication token.</response>
    /// <response code="403">Insufficient permissions — admin role required.</response>
    /// <response code="404">Gas station or fuel type not found.</response>
    /// <response code="500">An unexpected server error occurred.</response>
    [HttpPut("{id:guid}/fuels/{fuelId:guid}")]
    [BearerAuthorize(Roles.Admin)]
    [ProducesResponseType(typeof(GasStationDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddFuel(Guid id, Guid fuelId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new AddFuelToGasStationCommand(id, fuelId), cancellationToken);
        return Ok(result);
    }

    /// <summary>Removes a fuel type from a gas station. Restricted to administrators.</summary>
    /// <param name="id">The gas station's unique identifier.</param>
    /// <param name="fuelId">The fuel type's unique identifier.</param>
    /// <response code="200">Fuel type removed successfully. Returns the updated gas station.</response>
    /// <response code="401">Missing or invalid authentication token.</response>
    /// <response code="403">Insufficient permissions — admin role required.</response>
    /// <response code="404">Gas station or fuel type not found.</response>
    /// <response code="500">An unexpected server error occurred.</response>
    [HttpDelete("{id:guid}/fuels/{fuelId:guid}")]
    [BearerAuthorize(Roles.Admin)]
    [ProducesResponseType(typeof(GasStationDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RemoveFuel(Guid id, Guid fuelId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new RemoveFuelFromGasStationCommand(id, fuelId), cancellationToken);
        return Ok(result);
    }
}
